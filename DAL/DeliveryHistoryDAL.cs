using Microsoft.AspNetCore.Http;
using P02_WebApr2023_Assg1_Team6.Models;
using System.Data.SqlClient;
using System.Net.Http;


namespace P02_WebApr2023_Assg1_Team6.DAL
{
    public class DeliveryHistoryDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;
        //Constructor
        public DeliveryHistoryDAL()
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

		public List<DeliveryHistory> GetAllDeliveryHistory()
		{
			//Create a SqlCommand object from connection object
			SqlCommand cmd = conn.CreateCommand();
			//Specify the SELECT SQL statement
			cmd.CommandText = @"SELECT * FROM DeliveryHistory ORDER BY RecordID";
			//Open a database connection
			conn.Open();
			//Execute the SELECT SQL through a DataReader
			SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a deliveryHistoryList
            List<DeliveryHistory> deliveryHistoryList = new List<DeliveryHistory>();
			while (reader.Read())
			{
				deliveryHistoryList.Add(
				new DeliveryHistory
				{
					RecordID = reader.GetInt32(0), //0: 1st column
					ParcelID = reader.GetInt32(1), //0: 2nd column
					Description = reader.GetString(2) //0: 3rd column

				}
				);
			}
			//Close DataReader
			reader.Close();
			//Close the database connection
			conn.Close();
			return deliveryHistoryList;
		}


		public int AddDeliveryHistory(DeliveryHistory deliveryHistory)
        {
            //Specify an INSERT SQL statement which will
            //return the auto-generated RecordID after insertion
            string sql = @"INSERT INTO DeliveryHistory (ParcelID,Description)
                OUTPUT INSERTED.RecordID
                VALUES(@parcelID, @description)";

            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
			//Define the parameters used in SQL statement, value for each parameter
			//is retrieved from respective class's property.
			cmd.Parameters.AddWithValue("@parcelID", deliveryHistory.ParcelID);
            cmd.Parameters.AddWithValue("@description", deliveryHistory.Description);
            //A connection to database must be opened before any operations made.
            conn.Open();
            //ExecuteScalar is used to retrieve the auto-generated
            //RecordID after executing the INSERT SQL statement
            deliveryHistory.RecordID = (int)cmd.ExecuteScalar();
            //A connection should be closed after operations.
            conn.Close();
            //Return id when no error occurs.
            return deliveryHistory.RecordID;
        }

        public int AddDeliveryHistoryForDeliveryMan(DeliveryHistory deliveryHistory)
        {
            //Specify an INSERT SQL statement which will
            //return the auto-generated RecordID after insertion
            string sql = @"INSERT INTO DeliveryHistory (ParcelID,Description)
OUTPUT INSERTED.RecordID
VALUES(@parcelID, @description)";

            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@parcelID", deliveryHistory.ParcelID);
            cmd.Parameters.AddWithValue("@description",deliveryHistory.Description);
            //A connection to database must be opened before any operations made.
            conn.Open();
            //ExecuteScalar is used to retrieve the auto-generated
            //RecordID after executing the INSERT SQL statement
            deliveryHistory.RecordID = (int)cmd.ExecuteScalar();
            //A connection should be closed after operations.
            conn.Close();
            //Return id when no error occurs.
            return deliveryHistory.RecordID;
        }



    }
    }
