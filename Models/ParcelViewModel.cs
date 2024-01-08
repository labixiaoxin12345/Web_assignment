using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace P02_WebApr2023_Assg1_Team6.Models
{
    public class ParcelViewModel
    {

        [Display(Name = "Parcel ID")]
        public int ParcelId { get; set; }

        [Display(Name = "Item Descirption")]
        public string? ItemDescription { get; set; }

        [Display(Name = "Delivery address")]
        public string DeliveryAddress { get; set; }

        [Display(Name = "Target delivery date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime? TargetDeliveryDate { get; set; }

		[Display(Name = "Destination Country")]
		public string ToCountry { get; set; }

		[Display(Name = "Delivery status")]
        public int? DeliveryStatus { get; set; }


        [Display(Name = "Staff ID")]
        public int StaffId { get; set; }

        [Display(Name = "Staff Name")]
        public string StaffName { get; set; }


    }
}
