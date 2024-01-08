namespace P02_WebApr2023_Assg1_Team6.Models
{
    public class StaffViewModel
    {
        public List<Parcel> parcelList { get; set; }
        public List<Staff> staffList { get; set; }

        public StaffViewModel()
        {
            parcelList = new List<Parcel>();
            staffList = new List<Staff>();
        }
    }
}
