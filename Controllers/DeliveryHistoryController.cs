using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P02_WebApr2023_Assg1_Team6.DAL;
using P02_WebApr2023_Assg1_Team6.Models;

namespace P02_WebApr2023_Assg1_Team6.Controllers
{
    public class DeliveryHistoryController : Controller
    {
        private DeliveryHistoryDAL deliveryHistoryContext = new DeliveryHistoryDAL();
        private ParcelDAL parcelContext = new ParcelDAL();
        private StaffDAL staffContext = new StaffDAL();

        public ActionResult ViewDeliveryHistory()
        {

			List<DeliveryHistory> deliveryHistoryList = deliveryHistoryContext.GetAllDeliveryHistory();
			return View(deliveryHistoryList);
		}



        private List<DeliveryHistory> GetAllDeliveryHistory()
        {
            // Get a list of deliveryman from database
            List<DeliveryHistory> deliveryHistoryList = deliveryHistoryContext.GetAllDeliveryHistory();
            return deliveryHistoryList;
        }

        
    }
}
