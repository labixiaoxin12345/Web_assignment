using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using P02_WebApr2023_Assg1_Team6.DAL;
using P02_WebApr2023_Assg1_Team6.Models;

namespace P02_WebApr2023_Assg1_Team6.Controllers
{
    public class CreateReportController : Controller
    {

        private ParcelDAL parcelContext = new ParcelDAL();
        private StaffDAL staffContext = new StaffDAL();
        private DeliveryHistoryDAL deliveryHistoryContext = new DeliveryHistoryDAL();
        private DeliveryFailureDAL deliveryFailureContext = new DeliveryFailureDAL();


        // GET: CreateReportController
        public ActionResult FailedParcelReport()
        {
            int id = Convert.ToInt32(HttpContext.Session.GetInt32("ID"));
            // Stop accessing the action if not logged in
            // or account not in the "Delivery man" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Delivery Man"))
            {
                return RedirectToAction("Index", "Home");
            }
            List<Parcel> failedParcelList = parcelContext.GetAllFailedParcel();

			return View(failedParcelList);

        }

		// GET: DeliveryHistoryController
		public ActionResult GetAllDeliveryFailureReport()
		{

			List<DeliveryFailure> deliveryFailureList = deliveryFailureContext.GetAllDeliveryFailureReport();
			return View(deliveryFailureList);
		}

		private List<Staff> GetAllStationManager()
        {
            // Get a list of stationManagerList from database
            List<Staff> stationManagerList = staffContext.GetAllStationManager();
            return stationManagerList;
        }

        private List<Parcel> GetAllFailedParcelID()
        {
            // Get a list of failedParcelIdList from database
            List<Parcel> failedParcelIdList = parcelContext.GetAllFailedParcelID();
            return failedParcelIdList;
        }

        private List<Staff> GetAllDeliveryMan()
        {
            // Get a list of deliverymanList from database
            List<Staff> deliverymanList = staffContext.GetAllDeliveryMan();
            return deliverymanList;
        }


        //create a list for deliverystatus
        private List<SelectListItem> GetDeliveryFailureStatus()
        {
            List<SelectListItem> failureStatus = new List<SelectListItem>();

            failureStatus.Add(new SelectListItem
            {
                Value = "1",
                Text = "1 - receiver not found"
            });
            failureStatus.Add(new SelectListItem
            {
                Value = "2",
                Text = "2 - wrong delivery address"
            });
            failureStatus.Add(new SelectListItem
            {
                Value = "3",
                Text = "3 - parcel damaged"
            });
            failureStatus.Add(new SelectListItem
            {
                Value = "4",
                Text = "4 - other"
            });

            return failureStatus;
        }

      
        public ActionResult ViewDeliveryFailureReport()
        {
            // Stop accessing the action if not logged in
            // or account not in the "Delivery man" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Delivery Man"))
            {
            return RedirectToAction("Index","Home");
            }

            List<DeliveryFailure> deliveryFailureList = deliveryFailureContext.GetAllDeliveryFailureReport();
            return View(deliveryFailureList);
        }


        // GET: CreateReportController/CreateFailureReport/5
        public ActionResult CreateFailureReport(int? id)
        {
            // Stop accessing the action if not logged in
           // or account not in the "Delivery man" role
            if ((HttpContext.Session.GetString("Role") == null) ||
           (HttpContext.Session.GetString("Role") != "Delivery Man"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            { //Query string parameter not provided
              //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            ViewData["failureStatus"] = GetDeliveryFailureStatus();
            ViewData["stationManagerList"] = GetAllStationManager();
            ViewData["failedParcelIdList"] = GetAllFailedParcelID();
            ViewData["deliverymanList"] = GetAllDeliveryMan();
            Parcel parcel = parcelContext.GetDetails(id.Value);
            return View();

        }

       

        // POST: CreateReportController/CreateFailureReport
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFailureReport(DeliveryFailure deliveryFailure)
        {
            //Get all the list for drop-down list
            //in case of the need to return to CreateFailureReport.cshtml view
            ViewData["failureStatus"] = GetDeliveryFailureStatus();
            ViewData["stationManagerList"] = GetAllStationManager();
            ViewData["failedParcelIdList"] = GetAllFailedParcelID();
            ViewData["deliverymanList"] = GetAllDeliveryMan();

            int id = 0;
            List<DeliveryFailure> deliveryFailureList = deliveryFailureContext.GetAllDeliveryFailureReport();

            int staffId = Convert.ToInt32(HttpContext.Session.GetInt32("ID"));

            foreach (var r in deliveryFailureList)
            {
                id = r.ParcelID;  
            }


            //check whether if report is existed
            if (ModelState.IsValid && deliveryFailure.ParcelID!=id )
            {
                //make sure delivery man can only create failure report for their own parcel
                if (deliveryFailure.DeliveryManID == staffId)
                {
                    //Add delivery failure record to database
                    deliveryFailure.ReportID = deliveryFailureContext.Add(deliveryFailure);
                    //Redirect user to CreateReport/FailedParcelReport view
                    return RedirectToAction("FailedParcelReport");
                }

                else
                {
                    //Input validation fails, return to the CreateFailureReport view
                    //to display error message
                    TempData["ErrorMessage"] = "You can only create delivery failure report for your own parcel. Please select the correct parcel. ";
                    return View(deliveryFailure);
                }
               
            }
            else
            {
                if(deliveryFailure.DeliveryManID == staffId)
                {
                    //Input validation fails, return to the CreateFailureReport view
                    //to display error message
                    TempData["ErrorMessage"] = "Delivery failure report has already been created. Please select another Parcel. ";
                    return View(deliveryFailure);
                }

                else
                {
                    //Input validation fails, return to the CreateFailureReport view
                    //to display error message
                    TempData["ErrorMessage"] = "You can only create delivery failure report for your own parcel. Please select the correct parcel. ";
                    return View(deliveryFailure);
                }
            }
                
        }

    }
}
