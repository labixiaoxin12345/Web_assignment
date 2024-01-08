using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace P02_WebApr2023_Assg1_Team6.Models
{
    public class DeliveryFailure
    {
        [Display(Name = "Report ID")]
        public int ReportID { get; set; }

        [Display(Name = "Parcel ID")]
        public int ParcelID { get; set; }

        [Display(Name = "Delivery Man ID")]
        public int DeliveryManID { get; set; }

        [Display(Name = "Failure Type")]
        public string FailureType { get; set; }

        [Required(ErrorMessage = "Please enter an explanation for delivery failure")]
        [StringLength(225, ErrorMessage ="Description cannot exceed 225 characters")]
        public string Description { get; set; }

        [Display(Name = "Station Manager ID")]
        public int? StationMgrID { get; set; }

        [Display(Name = "Follow Up Action")]
        [StringLength(225, ErrorMessage = "Description cannot exceed 225 characters")]
        public string? FollowUpAction { get; set; }

        [Display(Name = "Date Created")]
        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }



    }
}
