using System.Data.SqlClient;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using NuGet.Protocol.Plugins;
using P02_WebApr2023_Assg1_Team6.Models;
namespace P02_WebApr2023_Assg1_Team6.DAL
{
	public class StaffDAL
	{
		private IConfiguration Configuration { get; }
		private SqlConnection conn;
		//Constructor
		public StaffDAL()
		{
			//Read ConnectionString from appsettings.json file
			var builder = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json");
			Configuration = builder.Build();
			string strConn = Configuration.GetConnectionString(
			"NPCSConnectionString");
			//Instantiate a SqlConnection object with the
			//Connection String read.
			conn = new SqlConnection(strConn);
		}

        public bool Login(string loginId, string password, HttpContext httpContext)
        {
           bool authenticated = false;
           //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM Staff ";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
           SqlDataReader reader = cmd.ExecuteReader();
           //Read all records until the end
            while (reader.Read())
            {
                // Convert loginID to lowercase for comparison
                // Password comparison is case-sensitive
                if ((reader.GetString(2).ToLower() == loginId) &&
               (reader.GetString(3) == password))
				{
					if (reader.GetString(4) == "Front Office Staff")
					{
						httpContext.Session.SetInt32("ID", reader.GetInt32(0));
						httpContext.Session.SetString("Name", reader.GetString(1));
						httpContext.Session.SetString("LoginID", reader.GetString(2));
						httpContext.Session.SetString("Role", reader.GetString(4));
                    }
					else if (reader.GetString(4) == "Station Manager") 
					{
						httpContext.Session.SetInt32("ID", reader.GetInt32(0));
						httpContext.Session.SetString("Name", reader.GetString(1));
						httpContext.Session.SetString("LoginID", reader.GetString(2));
						httpContext.Session.SetString("Role", reader.GetString(4));
					}
					else if (reader.GetString(4) == "Delivery Man")
					{
                        httpContext.Session.SetInt32("ID", reader.GetInt32(0));
                        httpContext.Session.SetString("Name", reader.GetString(1));
						httpContext.Session.SetString("LoginID", reader.GetString(2));
						httpContext.Session.SetString("Role", reader.GetString(4));
					}

					else if (reader.GetString(4) == "Admin Manager")
					{
						httpContext.Session.SetInt32("ID", reader.GetInt32(0));
						httpContext.Session.SetString("Name", reader.GetString(1));
						httpContext.Session.SetString("LoginID", reader.GetString(2));
						httpContext.Session.SetString("Role", reader.GetString(4));
					}

					else
					{
						break;
					}

					authenticated = true;
					break; // Exit the while loop
				}
			}
			//Close DataReader
			reader.Close();
			//Close the database connection
			conn.Close();
			return authenticated;
        }

        public List<Staff> GetAllDeliveryMan()
		{
			//Create a SqlCommand object from connection object
			SqlCommand cmd = conn.CreateCommand();
			//Specify the SELECT SQL statement
			cmd.CommandText = @"SELECT * FROM Staff WHERE Appointment='Delivery Man' ORDER BY StaffID";
			//Open a database connection
			conn.Open();
			//Execute the SELECT SQL through a DataReader
			SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a deliverymanList
            List<Staff> deliverymanList = new List<Staff>();
			while (reader.Read())
			{
				deliverymanList.Add(
				new Staff
				{
					StaffId = reader.GetInt32(0), //0: 1st column
					StaffName = reader.GetString(1),
				}
				);
			}
			//Close DataReader
			reader.Close();
			//Close the database connection
			conn.Close();

			return deliverymanList;
		}

        public List<Staff> GetAllStationManager()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM Staff WHERE Appointment='Station Manager' ORDER BY StaffID";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a stationManagerList
            List<Staff> stationManagerList = new List<Staff>();
            while (reader.Read())
            {
                stationManagerList.Add(
                new Staff
                {
                    StaffId = reader.GetInt32(0), //0: 1st column
                    StaffName = reader.GetString(1),
                }
                );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return stationManagerList;
        }

        public List<Staff> GetAllStaff()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM Staff ORDER BY StaffID";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a staff list
            List<Staff> staffList = new List<Staff>();
            while (reader.Read())
            {
                staffList.Add(
                new Staff
                {
                    StaffId = reader.GetInt32(0), //0: 1st column
                    StaffName = reader.GetString(1),
                    LoginId = reader.GetString(2),
                    Password = reader.GetString(3),
                    Appointment = !reader.IsDBNull(4) ?
 reader.GetString(4) : string.Empty,
                    OfficeTelNo = !reader.IsDBNull(5) ?
 reader.GetString(5) : string.Empty,

                Location = !reader.IsDBNull(6) ?
 reader.GetString(6) : string.Empty

            }
                );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return staffList;
        }


        public List<Parcel> GetLocalStaffParcel(int DeliveryManId)
		{
			//Create a SqlCommand object from connection object
			SqlCommand cmd = conn.CreateCommand();
			//Specify the SQL statement that select all delivery man
			cmd.CommandText = @"SELECT * FROM Parcel WHERE DeliveryManId = @selectedDeliveryManId AND DeliveryStatus=1 AND ToCountry='Singapore'";
			//Define the parameter used in SQL statement, value for the
			//parameter is retrieved from the method parameter “Deliverymanid
			cmd.Parameters.AddWithValue("@selectedDeliveryManId", DeliveryManId);

			//Open a database connection
			conn.Open();
			//Execute SELCT SQL through a DataReader
			SqlDataReader reader = cmd.ExecuteReader();
			List<Parcel> localParcelList = new List<Parcel>();
			while (reader.Read())
			{
				localParcelList.Add(
				new Parcel
				{
					ParcelId = reader.GetInt32(0), //0: 1st column
					ItemDescription = reader.GetString(1), //1: 2nd column									   
					DeliveryAddress = reader.GetString(6), //2: 3rd column
					ToCountry=reader.GetString(10),
					TargetDeliveryDate = !reader.IsDBNull(14) ? reader.GetDateTime(14) : (DateTime?)null,
					DeliveryStatus = reader.GetString(15), //5: 6th column
					DeliveryManID = !reader.IsDBNull(16) ? reader.GetInt32(16) : (int?)null
					
				}
				);
			}
			//Close DataReader
			reader.Close();
			//Close database connection
			conn.Close();

			return localParcelList;
		}


		public List<Parcel> GetOverseaStaffParcel(int DeliveryManId)
		{
			//Create a SqlCommand object from connection object
			SqlCommand cmd = conn.CreateCommand();
			//Specify the SQL statement that select all delivery man
			cmd.CommandText = @"SELECT * FROM Parcel WHERE DeliveryManId = @selectedDeliveryManId AND DeliveryStatus=1 AND ToCountry!='Singapore'";
			//Define the parameter used in SQL statement, value for the
			//parameter is retrieved from the method parameter “deliverymanid”.
			cmd.Parameters.AddWithValue("@selectedDeliveryManId", DeliveryManId);

			//Open a database connection
			conn.Open();
			//Execute SELCT SQL through a DataReader
			SqlDataReader reader = cmd.ExecuteReader();
			List<Parcel> overseaParcelList = new List<Parcel>();
			while (reader.Read())
			{
				overseaParcelList.Add(
				new Parcel
				{
					ParcelId = reader.GetInt32(0), //0: 1st column
					ItemDescription = reader.GetString(1), //1: 2nd column									   
					DeliveryAddress = reader.GetString(6), //2: 3rd column
					ToCountry = reader.GetString(10),
					TargetDeliveryDate = !reader.IsDBNull(14) ? reader.GetDateTime(14) : (DateTime?)null,
					DeliveryStatus = reader.GetString(15), //5: 6th column
					DeliveryManID = !reader.IsDBNull(16) ? reader.GetInt32(16) : (int?)null

				}
				);
			}
			//Close DataReader
			reader.Close();
			//Close database connection
			conn.Close();

			return overseaParcelList;
		}
	}
}



