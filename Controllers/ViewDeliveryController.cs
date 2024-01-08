using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using P02_WebApr2023_Assg1_Team6.DAL;
using P02_WebApr2023_Assg1_Team6.Models;

namespace P02_WebApr2023_Assg1_Team6.Controllers
{
    public class ViewDeliveryController : Controller
    {
		private ParcelDAL parcelContext = new ParcelDAL();
		private StaffDAL staffContext = new StaffDAL();
		private DeliveryHistoryDAL deliveryHistoryContext = new DeliveryHistoryDAL();
		public ActionResult VDelivery(int? id)
        {
            // Stop accessing the action if not logged in
            // or account not in the "delivery man" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Delivery Man"))
           {
               return RedirectToAction("Index", "Home");
            }
            StaffViewModel staffVM = new StaffViewModel();
            staffVM.staffList = staffContext.GetAllDeliveryMan();

            // Check if delieryman (id) presents in the query string
            if (id != null)
            {
                ViewData["selectedDeliveryManId"] = id.Value;
				// Get list of parcel under deliveryman
                staffVM.parcelList = staffContext.GetLocalStaffParcel(id.Value);
                //staffVM.parcelList = staffContext.GetOverseaStaffParcel(id.Value);

			}
            else
            {
                ViewData["selectedDeliveryManId"] = "";
            }

            return View(staffVM);
        }


        public ActionResult VOverseaDelivery(int? id)
        {
            // Stop accessing the action if not logged in
            // or account not in the "delivery man" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Delivery Man"))
            {
                return RedirectToAction("Index", "Home");
            }
            StaffViewModel staffVM = new StaffViewModel();
            staffVM.staffList = staffContext.GetAllDeliveryMan();

            // Check if delieryman (id) presents in the query string
            if (id != null)
            {
                ViewData["selectedDeliveryManId"] = id.Value;
                // Get list of parcel under deliveryman
                staffVM.parcelList = staffContext.GetOverseaStaffParcel(id.Value);
                //staffVM.parcelList = staffContext.GetOverseaStaffParcel(id.Value);

            }
            else
            {
                ViewData["selectedDeliveryManId"] = "";
            }

            return View(staffVM);
        }

        private List<SelectListItem> GetDeliveryStatus()
        {
            List<SelectListItem> status = new List<SelectListItem>();
            status.Add(new SelectListItem
            {
                Value = "2",
                Text = "2 - delivery to airport in progress"
            });
            status.Add(new SelectListItem
            {
                Value = "3",
                Text = "3 - delivery completed"
            });
            status.Add(new SelectListItem
            {
                Value = "4",
                Text = "4 - delivery failed"
            });

            return status;
        }



        // GET: ViewDeliveryController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ViewDeliveryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ViewDeliveryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ViewDeliveryController/UpdateLocalStatus/5
        public ActionResult UpdateLocalStatus(int? id)
        {
            // Stop accessing the action if not logged in
            // or account not in the "Deliveryman" role
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
            ViewData["status"] = GetDeliveryStatus();

            Parcel parcel = parcelContext.GetDetails(id.Value) ;

            if (parcel == null)
            {
                //Return to listing page, not allowed to assign delivery man
                return RedirectToAction("VDelivery");
            }
            return View(parcel);
        }

        // GET: ViewDeliveryController/UpdateOverseaStatus/5
        public ActionResult UpdateOverseaStatus(int? id)
        {
            // Stop accessing the action if not logged in
            // or account not in the "Staff" role
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
            ViewData["status"] = GetDeliveryStatus();

            Parcel parcel = parcelContext.GetDetails(id.Value);

            if (parcel == null)
            {
                //Return to listing page, not allowed to assign delivery man
                return RedirectToAction("VOverseaDelivery");
            }
            return View(parcel);
        }


        // POST: ViewDeliveryController/UpdateOverseatatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateOverseaStatus(Parcel parcel)
        {
            //Get deliverymanid list for drop-down list
            //in case of the need to return to Edit.cshtml view
            ViewData["status"] = GetDeliveryStatus();

            if (parcel.DeliveryStatus == "2")
            {
                string id = HttpContext.Session.GetString("LoginID");
                DeliveryHistory deliveryHistory = new DeliveryHistory();
                deliveryHistory.ParcelID = parcel.ParcelId;
                deliveryHistory.Description = "Parcel delivered to airport by " + id + " on "   + DateTime.Now.ToString("d MMM yyyy h:mmtt");
                //Update parcel record to database
                parcelContext.UpdateOverseaStatus(parcel);
                deliveryHistory.RecordID = deliveryHistoryContext.AddDeliveryHistoryForDeliveryMan(deliveryHistory);

                return RedirectToAction("VOverseaDelivery");
            }

            else if (parcel.DeliveryStatus == "4")
            {

                //Update deliverystatus record to database
                parcelContext.UpdateLocalStatus(parcel);
                return RedirectToAction("VDelivery");
            }

            else
            {
                //Input validation fails, return to the view
                //to display error message
                TempData["ErrorMessage"] = "This is oversea delivery! Are you sure? Please select again!";
                return View(parcel);
            }
        }

        // POST: ViewDeliveryController/UpdateLocalStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateLocalStatus(Parcel parcel)
        {
            //Get deliverymanid list for drop-down list
            //in case of the need to return to Edit.cshtml view
            ViewData["status"] = GetDeliveryStatus();

            if (parcel.DeliveryStatus == "3")
            {
                string id = HttpContext.Session.GetString("LoginID");
                DeliveryHistory deliveryHistory = new DeliveryHistory();
                deliveryHistory.ParcelID = parcel.ParcelId;
                deliveryHistory.Description = "Parcel delivered successfully by " + id + " on " + DateTime.Now.ToString("d MMM yyyy h:mmtt");
                //Update parcel record to database
                parcelContext.UpdateLocalStatus(parcel);
                deliveryHistory.RecordID = deliveryHistoryContext.AddDeliveryHistoryForDeliveryMan(deliveryHistory);

                return RedirectToAction("VDelivery");
            }
            else if(parcel.DeliveryStatus == "4")
            {
                
                //Update deliverystatus record to database
                parcelContext.UpdateLocalStatus(parcel);
                return RedirectToAction("VDelivery");
            }

            else
            {
                //Input validation fails, return to the view
                //to display error message
                TempData["ErrorMessage"] = "This is Local delivery! Are you sure? Please select again!";
                return View(parcel);
            }
        }

        // GET: ViewDeliveryController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ViewDeliveryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
