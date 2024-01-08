using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Xml.Linq;

namespace P02_WebApr2023_Assg1_Team6.Models
{
    public class Parcel
    {
        [Display(Name = "Parcel ID")]
        public int ParcelId { get; set; }

        [Display(Name = "Description of item")]
        public string? ItemDescription { get; set; }

        [Display(Name = "Sender's name")]
        public string SenderName { get; set; }

        [Display(Name = "Sender's telephone no.")]
        public string SenderTelNo { get; set; }

        [Display(Name = "Receiver's name")]
        public string ReceiverName { get; set; }

        [Display(Name = "Receiver's telephone no.")]
        public string ReceiverTelNo { get; set; }

        [Display(Name = "Delivery address")]
        public string DeliveryAddress { get; set; }

        [Display(Name = "Source city")]
        public string FromCity { get; set; }

        [Display(Name = "Source Country")]
        public string FromCountry { get; set; }

        [Display(Name = "Destination city")]
        public string ToCity { get; set; }

        [Display(Name = "Destination country")]
        public string ToCountry { get; set; }

        [Display(Name = "Parcel weight(KG)")]
        public double ParcelWeight { get; set; }

        [Display(Name = "Delivery charge($)")]
        public decimal DeliveryCharge { get; set; }

        [Display(Name = "Currency")]
        public string Currency { get; set; }

        [Display(Name = "Target delivery date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime? TargetDeliveryDate { get; set; }

        [Required(ErrorMessage = "Delivery Status is required. <br> Values: <br> 0 - pending delivery <br> 1 - delivery to destination in progress <br> 2 - delivery to airport in progress <br> 3 - delivery completed <br> 4 - delivery failed")]
        [Display(Name = "Delivery Status")]
        [RegularExpression("^[0-4]{1}$", ErrorMessage = "Delivery Status accept values between 0 and 4.")] //Validates if value entered is within 0 - 4
        [StringLength(1, ErrorMessage = "Invalid Status. <br> Values: <br> 0 - pending delivery <br> 1 - delivery to destination in progress; <br> 2 - delivery to airport in progress; <br> 3 - delivery completed; <br> 4 - delivery failed")]
        public string DeliveryStatus { get; set; }

        [Display(Name = "Deliveryman ID")]
        public int? DeliveryManID { get; set; }

/*        public decimal ShippingRate { get; set; } 

        public int TransitTime { get; set; }*/
        private IConfiguration Configuration { get; }
        private SqlConnection conn;
    }
}
