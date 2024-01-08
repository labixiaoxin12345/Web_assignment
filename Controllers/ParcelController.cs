using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Common;
using P02_WebApr2023_Assg1_Team6.DAL;
using P02_WebApr2023_Assg1_Team6.Models;
using System;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;
using DeepEqual.Syntax;


namespace P02_WebApr2023_Assg1_Team6.Controllers
{
    public class ParcelController : Controller
    {
        private ParcelDAL parcelContext = new ParcelDAL();
        private StaffDAL staffContext = new StaffDAL();
        private DeliveryHistoryDAL deliveryHistoryContext = new DeliveryHistoryDAL();
        private ShippingRateDAL shippingRateContext = new ShippingRateDAL();
        private List<string> country = new List<string> { "Singapore", "Malaysia", "Indonesia", "China", "USA", "Japan", "France", "UK", "Australia" };
        private List<string> ft = new List<string> { "Receiver not found", "Wrong delivery addresss", "Parcel damaged", "Other" };



        // GET: ParcelController
        public ActionResult Index()
        {

            //List<Parcel> parcelList = parcelContext.GetAllParcel();
            //return View(parcelList);
            return View();
        }


        // GET: ParcelController/CurrencyConversion
        public ActionResult CurrencyConversion()
        {
            return View();
        }
        //Package 1
        public List<SelectListItem> GetAllCountries()
        {
            List<SelectListItem> c = new List<SelectListItem>();
            c.Add(new SelectListItem
            {
                Value = null,
                Text = "--- Select Country ---"
            });
            c.Add(new SelectListItem
            {
                Value = "Australia",
                Text = "Australia"
            });
            c.Add(new SelectListItem
            {
                Value = "China",
                Text = "China"
            });
            c.Add(new SelectListItem
            {
                Value = "France",
                Text = "France"
            });
            c.Add(new SelectListItem
            {
                Value = "Indonesia",
                Text = "Indonesia"
            });
            c.Add(new SelectListItem
            {
                Value = "Japan",
                Text = "Japan"
            });
            c.Add(new SelectListItem
            {
                Value = "Malaysia",
                Text = "Malaysia"
            });
            c.Add(new SelectListItem
            {
                Value = "Singapore",
                Text = "Singapore"
            });
            c.Add(new SelectListItem
            {
                Value = "UK",
                Text = "UK"
            });
            c.Add(new SelectListItem
            {
                Value = "USA",
                Text = "USA"
            });
            return c;
        }
        public ActionResult Create()
        {  
            
            ViewData["Countries"] = GetAllCountries();
            Parcel p = new Parcel //Setting default values
            {
                Currency = "SGD",
                ParcelWeight = 0.0,
                DeliveryStatus = "0"
            };
            return View(p);
            return RedirectToAction("Create");
            /* //creating a list to store parcels
             List<SelectListItem> selectListItems = new List<SelectListItem>();
             List<Parcel> parcellist = parcelContext.GetAllCity();

             //creating a list to store transit time 
             List<SelectListItem> selectEachItem = new List<SelectListItem>();
             List<ShippingRate> TimeList = parcelContext.GetAllTime();

             for (int i = 0; i < parcellist.Count; i++)
             {
                 selectListItems.Add(new SelectListItem
                 {
                     Value = parcellist[i].ToCity.ToString(),
                     Text = parcellist[i].ToCity.ToString()
                 });

             }
             Console.WriteLine(GetAllCity()[0].ToCity);
             for (int a = 0; a < TimeList.Count; a++)
             {
                 selectEachItem.Add(new SelectListItem
                 {
                     Value = TimeList[a].TransitTime.ToString(),
                     Text= TimeList[a].TransitTime.ToString()
                 });
             }
             ViewData["CityList"] = selectListItems;
             ViewData["TimeList"] = selectEachItem;
             return View();*/


        }

        // POST: ParcelController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Parcel parcel)
        //public ActionResult Create(string fromcountry, string fromcity, string tocountry, string tocity)
        {

            if (!ModelState.IsValid)
            {
                ShippingRate ccObject = shippingRateContext.GetShippingRatebyCC(parcel.ToCity, parcel.ToCountry); //Creating a shipping rate object to inherit a shipping rate object that has the same tocity & tocountry

                decimal deliveryCharge = 0;
                decimal rdelCharge = 0;
                decimal shipRate = 0;
                int transitTime = 0;

                //error condition
                if (ccObject.IsDeepEqual(new ShippingRate())) // Checks if the ccObject equals to a new shippingrate that has empty values
                {
                    TempData["ErrorMessage"] = $"Parcel creation failed. <br><br>------------------------------------------------------------ <br><br> Invalid ToCity & ToCountry, please try again with the correct city & country names. <br><br> ------------------------------------------------------------";
                    return RedirectToAction("Create");
                }
                //input is equal to 
                else if ((parcel.ToCity.ToLower() == ccObject.ToCity.ToLower()) && (parcel.ToCountry.ToLower() == ccObject.ToCountry.ToLower())) //Checks if the city & country matches the records in shipping rate 
                {
                    //Advanced Feature 3 - Parcel Receiving, compute delivery charge
                    shipRate = ccObject.ShippingRates; //Store shiprate into shipRate, to be printed out later as tempData
                    deliveryCharge = Convert.ToDecimal(parcel.ParcelWeight) * ccObject.ShippingRates; //Delivery Charge = parcel weight * ship rate
                                                                                                      //Basic Feature 2 - Parcel Receiving, calculating target delivery date
                    parcel.ToCity = ccObject.ToCity; //Added to replace value entered by staff. E.g. if staff enter pAriS, it will be replaced to Paris from shipping rate.
                    parcel.ToCountry = ccObject.ToCountry; //Added to replace value entered by staff. E.g. if staff enter frAnCe, it will be replaced to France from shipping rate.
                    transitTime = ccObject.TransitTime;
                }

                //Advanced Feature 3 - Parcel Receiving, compute delivery charge
                rdelCharge = Math.Round(deliveryCharge, MidpointRounding.AwayFromZero); //Rounding the delivery charge to the nearest dollar
                if (rdelCharge >= 5) //Checks if delivery charge is more than 5
                {
                    parcel.DeliveryCharge = rdelCharge;
                }
                else //If delivery charge is less than 5, the minimum delivery charge is 5 dollars  
                {
                    parcel.DeliveryCharge = 5;
                }

                //Basic Feature 2 - Parcel Receiving, calculating target delivery date
                DateTime receiveParcel = DateTime.Now;
                DateTime tdd = receiveParcel.AddDays(transitTime); //Target delivery date = receive parcel datetime + transit datetime
                parcel.TargetDeliveryDate = tdd;


                //Basic Feature 1 - Parcel Receiving, adding parcel delivery record
                string desc = $"Recieved parcel by {HttpContext.Session.GetString("LoginID")} on {DateTime.Now.ToString("dd MMM yyyy hh:mm tt")}.";

                DeliveryHistory dh = new DeliveryHistory
                {
                    ParcelID = parcelContext.Add(parcel), //Obtaining parcel ID by adding details to parcel
                    Description = desc,
                };
                deliveryHistoryContext.AddDeliveryHistory(dh); //Adding parcel ID & description into delivery history

                TempData["InsertMessage"] = $"Parcel Added to Database! <br><br> -------------------- Parcel Delivery Order -------------------- <br><br> Parcel ID:  {parcel.ParcelId} <br> Parcel Weight:  {parcel.ParcelWeight} kg <br> From City and Country:  {parcel.FromCity}, {parcel.FromCountry} <br> To City and Country:  {parcel.ToCity}, {parcel.ToCountry} <br> Shipping Rate:  {String.Format("{0:0.##}", shipRate)}/kg <br> Delivery Charge (Raw):  ({String.Format("{0:0.##}", shipRate)} x {parcel.ParcelWeight}) = ${String.Format("{0:0.##}", deliveryCharge)} <br> Delivery Charge (Rounded):  ${String.Format("{0:0.##}", rdelCharge)} <br> Delivery Charge (Final):  ${String.Format("{0:0.##}", parcel.DeliveryCharge)} (Note: Minimum delivery charge is S$5.00) <br><br> ------------------------------";
                return View("Index");
            }
            else
            {
                return View("Create");
            }

            /*ViewData["CityList"] = GetAllCity();
            ViewData["TimeList"] = GetAllTime();
            
            if (!ModelState.IsValid)
            {
                decimal shipr = 0;
                decimal charge = 0;
                ShippingRate sr = parcelContext.GetShippingRate(parcel.ToCity, parcel.ToCountry);
                if ((parcel.ToCity.ToLower() == sr.ToCity.ToLower()) && parcel.ToCountry.ToLower() == sr.ToCountry.ToLower())
                {
                    shipr = sr.ShippingRates;
                    charge = Convert.ToDecimal(parcel.ParcelWeight) * shipr;




                }
                //ShippingRate sr = parcelContext.GetShippingRate();

                //parcel.TargetDeliveryDate = DateTime.Now.AddDays(parcel.TransitTime);

                //create get shipping rate in shipping rate DAL
                //get list of shipping rates
                //for loop for shipping rates
                //if parcel.tocity == shipping.tocity { parcel.deliverydate=datetime.now.addDays(shipping.transit)}

                Console.WriteLine(GetAllCity()[0].ToCity);
                if (parcel.ToCity == GetAllCity()[0].ToCity)
                {

                    //deivery charge calculation
                parcel.TargetDeliveryDate = DateTime.Now.AddDays(GetAllTime()[0].TransitTime);
                //add parcel record to database
                parcel.ParcelId = parcelContext.Add(parcel);
                //ViewData["ShowResult"] = true;
                    //Redirect user to Index view
                    return RedirectToAction("Index") ;
                }
            }
            else
            {
                //Input validation fails, return to the Create view
                //to display error message
                return RedirectToAction("Create");
            }
            return View();*/
        }
        /* private List<Parcel> GetAllCity()
         {
             // Get a list of cities from database
             List<Parcel> CityList = parcelContext.GetAllCity();
             return CityList;
         }
         private List<ShippingRate> GetAllTime()
         {
             // Get a list of cities from database
             List<ShippingRate> TimeList = parcelContext.GetAllTime();
             return TimeList;
         }

         //Package 1
         [HttpPost]
         public ActionResult Index(DeliveryHistory history)
         {
             ViewData["CityList"] = GetAllCity();
             ViewData["TimeList"] = GetAllTime();
             if (ModelState.IsValid)
             {
                 var date = DateTime.Now;
                 history.RecordID = parcelContext.History(history);
                 //date.ParcelId = parcelContext.Add(date);
                 //Redirect user to the view
                 return RedirectToAction("Success");
             }
             else
             {
                 //Input validation fails, return to the Create view
                 //to display error message
                 return RedirectToAction("Create");
             }

         }*/
        public ActionResult ShippingRCalculator()
        {

            ViewData["Countries"] = GetAllCountries();
            return View();
        }

        // POST: Shipping Calculator
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShippingRCalculator(ShippingCalculatorViewModel ShipCalc)
        {

            ShippingRate ccObject = shippingRateContext.GetShippingRatebyCC(ShipCalc.ToCity, ShipCalc.ToCountry); //Creating a shipping rate object to inherit a shipping rate object that has the same toCity & toCountry

            decimal deliveryCharge = 0;
            decimal rdelCharge = 0;
            decimal shipRate = 0;

            if (ccObject.IsDeepEqual(new ShippingRate())) // Checks if the ccObject equals to a new shippingrate that has empty values
            {
                TempData["ErrorMessage"] = $"Shipping Calculation failed. <br><br>-------------------------------------------------------------------------------------------- <br><br> Invalid ToCity name, please try again with the correct city name. <br><br> --------------------------------------------------------------------------------------------";
                return RedirectToAction("ShippingRCalculator");
            }
            else if ((ShipCalc.ToCity.ToLower() == ccObject.ToCity.ToLower()) && (ShipCalc.ToCountry.ToLower() == ccObject.ToCountry.ToLower())) //Checks if the city & country matches the records in shipping rate 
            {
                //Computing delivery charge
                shipRate = ccObject.ShippingRates; //Store shiprate into shipRate, to be printed out later as tempData
                deliveryCharge = Convert.ToDecimal(ShipCalc.ParcelWeight) * ccObject.ShippingRates; //Delivery Charge = parcel weight * ship rate
            }

            //Computing rounded delivery charge
            rdelCharge = Math.Round(deliveryCharge, MidpointRounding.AwayFromZero); //Rounding the delivery charge to the nearest dollar
            if (rdelCharge >= 5) //Checks if delivery charge is more than 5
            {
                ShipCalc.DeliveryCharge = rdelCharge;
            }
            else //If delivery charge is less than 5, the minimum delivery charge is 5 dollars  
            {
                ShipCalc.DeliveryCharge = 5;
            }
            //Display Results
            TempData["Message"] = $"--------------- Calculated Delivery Charge --------------- <br> Parcel Weight: {ShipCalc.ParcelWeight} KG <br> From city, country: {ShipCalc.FromCity}, {ShipCalc.FromCountry} <br> To city, country: {ShipCalc.ToCity}, {ShipCalc.ToCountry} <br> Shipping Rate: ${String.Format("{0:0.##}", shipRate)} <br> Delivery Charge Cost (Shipping Rate x Parcel Weight): ${String.Format("{0:0.##}", ShipCalc.DeliveryCharge)} <br>(Note: Minimum delivery charge is S$5.00) <br> ------------------------------";
            return RedirectToAction("ShippingRCalculator");
        }

        //PACKAGE 2
        private List<Staff> GetAllDeliveryMan()
        {
            // Get a list of deliveryman from database
            List<Staff> deliverymanList = staffContext.GetAllDeliveryMan();
            return deliverymanList;
        }

        private List<Parcel> GetNumParcelAssign()
        {
            // Get a list of deliveryman from database
            List<Parcel> numParcelAssignList = parcelContext.GetNumParcelAssign();
            return numParcelAssignList;
        }


        public ActionResult AssignDeliveryMan()
        {
            // Stop accessing the action if not logged in
            // or account not in the "Station manager" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Station Manager"))
            {
                return RedirectToAction("Index","Home");
            }

            List<Parcel> parcelList = parcelContext.GetAllParcel();
            return View(parcelList);
        }

        // GET: Parcel/AssignParcel/5
        public ActionResult AssignParcel(int? id)
        {

            // Stop accessing the action if not logged in
            // or account not in the "Station manager" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Station Manager"))
            {
            	return RedirectToAction("AssignDeliveryMan");
            }

            if (id == null)
            { //Query string parameter not provided
              //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            ViewData["DeliveryManList"] = GetAllDeliveryMan();
            Parcel parcel = parcelContext.GetDetails(id.Value);
            if (parcel == null)
            {
                //Return to listing page, not allowed to assign delivery man
                return RedirectToAction("AssignDeliveryMan");
            }
            return View(parcel);
        }



        // GET: ParcelController/Details/5
        public ActionResult Details(int id)
        {
            // Stop accessing the action if not logged in
            // or account not in the "Staff" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Delivery Man"))
            {
                return RedirectToAction("Index", "Home");
            }

            Parcel parcel = parcelContext.GetDetails(id);
            ParcelViewModel parcelVM = MapToParcelVM(parcel);
            return View(parcelVM);
        }

        public ParcelViewModel MapToParcelVM(Parcel parcel)
        {
            string staffName = "";
            if (parcel.DeliveryManID != null)
            {
                List<Staff> staffList = staffContext.GetAllDeliveryMan();
                foreach (Staff staff in staffList)
                {
                    if (staff.StaffId == parcel.DeliveryManID.Value)
                    {
                        staffName = staff.StaffName;
                        //Exit the foreach loop once the name is found
                        break;
                    }
                }
            }
        

        ParcelViewModel parcelVM = new ParcelViewModel
        {
            ParcelId = parcel.ParcelId,
            StaffName = staffName,
            ItemDescription = parcel.ItemDescription,
            DeliveryAddress = parcel.DeliveryAddress,
            ToCountry = parcel.ToCountry,
            TargetDeliveryDate = parcel.TargetDeliveryDate,
    

        };

        return parcelVM;
        }

         


        [HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult AssignParcel(Parcel parcel)
		{
			//Get deliverymanid list for drop-down list
			ViewData["DeliveryManList"] = GetAllDeliveryMan();

            List<Parcel> numParcelAssignList = parcelContext.GetNumParcelAssign();

            int Deliveryman = 0;

            foreach (var p in numParcelAssignList)
            {
                //check if the deliveryman have less than 5 parcel assigned to him/her
                if ( p.DeliveryManID == parcel.DeliveryManID)
                {
                    Deliveryman = Convert.ToInt32(p.DeliveryManID);
                }

                
            }

            if (parcel.DeliveryManID == Deliveryman)
            {
                string id = HttpContext.Session.GetString("LoginID");
                DeliveryHistory deliveryHistory = new DeliveryHistory();
                deliveryHistory.ParcelID = parcel.ParcelId;
                deliveryHistory.Description = "Received parcel by " + id + " on " + DateTime.Now.ToString("d MMM yyyy h:mmtt");
                //Update parcel record to database
                parcelContext.Update(parcel);
                deliveryHistory.RecordID = deliveryHistoryContext.AddDeliveryHistory(deliveryHistory);

                return RedirectToAction("AssignDeliveryMan");
            }
            else
            {
                //Input validation fails, return to the view
                //to display error message
                TempData["ErrorMessage"] = "The deliveryman has reached its maximum delivery. Please select another delivery man! ";
                return View(parcel);
            }

        }
	}
}

