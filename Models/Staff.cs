using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
namespace P02_WebApr2023_Assg1_Team6.Models
{
	public class Staff
	{
		[Display(Name = "Staff ID")]
		public int StaffId { get; set; }

		[Display(Name = "Staff Name")]
		public string StaffName { get; set; }

		[Display(Name = "Login ID")]
		public string LoginId { get; set; }

		[Display(Name = "Password")]
		public string Password { get; set; }

		[Display(Name = "Appointment")]
		public string? Appointment { get; set; }

		[Display(Name = "OfficeTelNo")]
		public string? OfficeTelNo { get; set; }


		[Display(Name = "Location")]
		public string? Location { get; set; }
	}
}
