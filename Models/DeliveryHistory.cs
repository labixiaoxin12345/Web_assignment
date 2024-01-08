using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace P02_WebApr2023_Assg1_Team6.Models
{
    public class DeliveryHistory
    {
        [Display(Name = "Record ID")]
        public int RecordID { get; set; }

        [Display(Name = "Parcel ID")]
        public int ParcelID { get; set; }

        public string Description { get; set; }

     
    }
}
