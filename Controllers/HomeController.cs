using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using P02_WebApr2023_Assg1_Team6.DAL;
using P02_WebApr2023_Assg1_Team6.Models;
using System.Data.SqlClient;
using System.Diagnostics;

namespace P02_WebApr2023_Assg1_Team6.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private StaffDAL staffContext = new StaffDAL();



        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


		private const string ApiUrl = "https://zenquotes.io";

		[HttpGet]
		public async Task<ActionResult> AdminManagerMain() 
		{
			HttpClient client = new HttpClient();

			client.BaseAddress = new Uri(ApiUrl);
			//client.DefaultRequestHeaders.Add("X-RapidAPI-Key", "HuANDw8O/Q4YeTJ6p38EnQ==TQnRLUJvDBlvqKA1");

			HttpResponseMessage response = await client.GetAsync("/api/quotes/");

			if (response.IsSuccessStatusCode)
			{
				string data = await response.Content.ReadAsStringAsync();


				List<Quote> quotes = JsonConvert.DeserializeObject<List<Quote>>(data); // Deserialize as a list of Quote objects
				if (quotes.Count > 0)
				{
					Random random = new Random();
					int randomIndex = random.Next(0, quotes.Count); // Generate a random index

					Quote randomQuote = quotes[randomIndex]; // Get the random quote
					return View(randomQuote);
				}


			}
			// Handle error
			return View();

		}

		[HttpGet]
		public async Task<ActionResult> FrontOfficeMain()
		{
			HttpClient client = new HttpClient();

			client.BaseAddress = new Uri(ApiUrl);
			//client.DefaultRequestHeaders.Add("X-RapidAPI-Key", "HuANDw8O/Q4YeTJ6p38EnQ==TQnRLUJvDBlvqKA1");

			HttpResponseMessage response = await client.GetAsync("/api/quotes/");

			if (response.IsSuccessStatusCode)
			{
				string data = await response.Content.ReadAsStringAsync();


				List<Quote> quotes = JsonConvert.DeserializeObject<List<Quote>>(data); // Deserialize as a list of Quote objects
				if (quotes.Count > 0)
				{
					Random random = new Random();
					int randomIndex = random.Next(0, quotes.Count); // Generate a random index

					Quote randomQuote = quotes[randomIndex]; // Get the random quote
					return View(randomQuote);
				}


			}
			// Handle error
			return View();

		}

		[HttpGet]
		public async Task<ActionResult> StationManagerMain()
		{
			HttpClient client = new HttpClient();

			client.BaseAddress = new Uri(ApiUrl);
			//client.DefaultRequestHeaders.Add("X-RapidAPI-Key", "HuANDw8O/Q4YeTJ6p38EnQ==TQnRLUJvDBlvqKA1");

			HttpResponseMessage response = await client.GetAsync("/api/quotes/");

			if (response.IsSuccessStatusCode)
			{
				string data = await response.Content.ReadAsStringAsync();


				List<Quote> quotes = JsonConvert.DeserializeObject<List<Quote>>(data); // Deserialize as a list of Quote objects
				if (quotes.Count > 0)
				{
					Random random = new Random();
					int randomIndex = random.Next(0, quotes.Count); // Generate a random index

					Quote randomQuote = quotes[randomIndex]; // Get the random quote
					return View(randomQuote);
				}


			}
			// Handle error
			return View();

		}

		[HttpGet]
		public async Task<ActionResult> DeliveryManMain()
		{
			HttpClient client = new HttpClient();
			
			client.BaseAddress = new Uri(ApiUrl);
			//client.DefaultRequestHeaders.Add("X-RapidAPI-Key", "HuANDw8O/Q4YeTJ6p38EnQ==TQnRLUJvDBlvqKA1");

			HttpResponseMessage response = await client.GetAsync("/api/quotes/");

			if (response.IsSuccessStatusCode)
			{
				string data = await response.Content.ReadAsStringAsync();


				List<Quote> quotes = JsonConvert.DeserializeObject<List<Quote>>(data); // Deserialize as a list of Quote objects
				if (quotes.Count > 0)
				{
					Random random = new Random();
					int randomIndex = random.Next(0, quotes.Count); // Generate a random index

					Quote randomQuote = quotes[randomIndex]; // Get the random quote
					return View(randomQuote);
				}


			}
			// Handle error
			return View();
			
		}

		public IActionResult Index()
        {
            return View();
        }


		public ActionResult LogOut()
        {
            // Clear all key-values pairs stored in session state
            HttpContext.Session.Clear();
            // Call the Index action of Home controller
            return RedirectToAction("Index");
        }



        public IActionResult Privacy()
        {
            return View();
        }


		[HttpPost]
		public ActionResult StaffLogin(IFormCollection formData)
		{
			// Read inputs from textboxes
			
			string loginID = formData["txtLoginID"].ToString().ToLower();

			string password = formData["txtPassword"].ToString();

			if (staffContext.Login(loginID, password, HttpContext) && (HttpContext.Session.GetString("Role") == "Front Office Staff"))
			{
				// Redirect user to the "StationManagerMain" view through an action
				return RedirectToAction("FrontOfficeMain");
			}


			else if (staffContext.Login(loginID, password, HttpContext) && (HttpContext.Session.GetString("Role") == "Station Manager"))
			{
				// Redirect user to the "StationManagerMain" view through an action
				return RedirectToAction("StationManagerMain");
			}

			else if (staffContext.Login(loginID, password, HttpContext) && (HttpContext.Session.GetString("Role") == "Delivery Man"))
			{
				
				// Redirect user to the "StationManagerMain" view through an action
				return RedirectToAction("DeliveryManMain");
			}

			else if (staffContext.Login(loginID, password, HttpContext) && (HttpContext.Session.GetString("Role") == "Admin Manager"))
			{
				// Redirect user to the "StationManagerMain" view through an action
				return RedirectToAction("AdminManagerMain");
			}


			else
			{
				// Store an error message in TempData for display at the index view
				TempData["Message"] = "Invalid Login Credentials!";
				// Redirect user back to the index view through an action
				return RedirectToAction("Index");
			}

		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}