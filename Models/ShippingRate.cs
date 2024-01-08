using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Xml.Linq;

namespace P02_WebApr2023_Assg1_Team6.Models
{
    public class ShippingRate
    {
        [Display(Name = "Shipping Rate ID")]
        public int ShippingRateID { get; set; }

        [Display(Name = "Source City")]
        public string FromCity { get; set; }

        [Display(Name = "Source Country")]
        public string FromCountry { get; set; }

        [Display(Name = "Destination City")]
        public string ToCity { get; set; }

        [Display(Name = "Destination Country")]
        public string ToCountry { get; set; }

        [Display(Name = "Shipping Rate")]
        public decimal ShippingRates { get; set; }

        [Display(Name = "Currency")]
        public string Currency { get; set; }

        [Display(Name = "Transit Time")]
        public int TransitTime { get; set; }

        [Display(Name = "Last updated by")]
        public int Lastupdatedby { get; set; }

        private IConfiguration Configuration { get; }
        private SqlConnection conn;

    }
}
