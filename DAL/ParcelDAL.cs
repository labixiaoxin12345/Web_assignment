using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using P02_WebApr2023_Assg1_Team6.Models;
using System.Collections;
using System.Security.Cryptography;

namespace P02_WebApr2023_Assg1_Team6.DAL
{
    public class ParcelDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;
        //Constructor
        public ParcelDAL()
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


        public Parcel GetDetails(int parcelId)
        {
            Parcel parcel = new Parcel();
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement that
            //retrieves all attributes of a parcel record.
            cmd.CommandText = @"SELECT * FROM Parcel
			WHERE ParcelID = @selectedParcelID";
            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “parcelId”.
            cmd.Parameters.AddWithValue("@selectedParcelID", parcelId);
            //Open a database connection
            conn.Open();
            //Execute SELCT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                //Read the record from database
                while (reader.Read())
                {
                    // Fill parcel object with values from the data reader
                    parcel.ParcelId = reader.GetInt32(0);
                    parcel.ItemDescription = reader.GetString(1);
                    parcel.SenderName = reader.GetString(2);
                    parcel.SenderTelNo = reader.GetString(3);
                    parcel.ReceiverName = reader.GetString(4);
                    parcel.ReceiverTelNo = reader.GetString(5);
                    parcel.DeliveryAddress = reader.GetString(6);
                    parcel.FromCity = reader.GetString(7);
                    parcel.FromCountry = reader.GetString(8);
                    parcel.ToCity = reader.GetString(9);
                    parcel.ToCountry = reader.GetString(10);
                    parcel.ParcelWeight = reader.GetDouble(11);
                    parcel.DeliveryCharge = reader.GetDecimal(12);
                    parcel.Currency = reader.GetString(13);
                    parcel.TargetDeliveryDate = !reader.IsDBNull(14) ? reader.GetDateTime(14) : (DateTime?)null;
                    parcel.DeliveryStatus = reader.GetString(15);
                    parcel.DeliveryManID = !reader.IsDBNull(16) ? reader.GetInt32(16) : (int?)null;
                }
            }
            //Close data reader
            reader.Close();
            //Close database connection
            conn.Close();
            return parcel;
        }


        public int Update(Parcel parcel)
        {

            // Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            // Specify an UPDATE SQL statement
            cmd.CommandText = @"UPDATE Parcel SET DeliveryManID=@deliveryManID, DeliveryStatus=@deliveryStatus WHERE ParcelID = @selectedParcelID";
            // Define the parameters used in the SQL statement, value for each parameter is retrieved from the respective class's property
            cmd.Parameters.AddWithValue("@deliveryManID", parcel.DeliveryManID);
            cmd.Parameters.AddWithValue("@deliveryStatus", 1);
            cmd.Parameters.AddWithValue("@selectedParcelID", parcel.ParcelId);
            // Open a database connection
            conn.Open();
            // ExecuteNonQuery is used for UPDATE and DELETE
            int count = cmd.ExecuteNonQuery();
            // Close the database connection
            conn.Close();
            return count;
        }



        public int UpdateLocalStatus(Parcel parcel)
        {
            // Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            // Specify an UPDATE SQL statement
            cmd.CommandText = @"UPDATE Parcel SET DeliveryStatus=@deliveryStatus WHERE ParcelID = @selectedParcelID";
            // Define the parameters used in the SQL statement, value for each parameter is retrieved from the respective class's property
            cmd.Parameters.AddWithValue("@deliveryStatus", parcel.DeliveryStatus);
            cmd.Parameters.AddWithValue("@selectedParcelID", parcel.ParcelId);
            // Open a database connection
            conn.Open();
            // ExecuteNonQuery is used for UPDATE and DELETE
            int count = cmd.ExecuteNonQuery();
            // Close the database connection
            conn.Close();
            return count;
        }

        public int UpdateOverseaStatus(Parcel parcel)
        {
            // Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            // Specify an UPDATE SQL statement
            cmd.CommandText = @"UPDATE Parcel SET DeliveryStatus=@deliveryStatus WHERE ParcelID = @selectedParcelID";
            // Define the parameters used in the SQL statement, value for each parameter is retrieved from the respective class's property
            cmd.Parameters.AddWithValue("@deliveryStatus", parcel.DeliveryStatus);
            cmd.Parameters.AddWithValue("@selectedParcelID", parcel.ParcelId);
            // Open a database connection
            conn.Open();
            // ExecuteNonQuery is used for UPDATE and DELETE
            int count = cmd.ExecuteNonQuery();
            // Close the database connection
            conn.Close();
            return count;
        }




        public int Add(Parcel parcel)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated ParcelID after insertion
            cmd.CommandText = @"INSERT INTO Parcel (ItemDescription, SenderName, SenderTelNo, ReceiverName, 
            ReceiverTelNo, DeliveryAddress, FromCity, FromCountry, ToCity, ToCountry, ParcelWeight, DeliveryCharge, 
            Currency, TargetDeliveryDate, DeliveryStatus)
            OUTPUT INSERTED.ParcelID 
            VALUES(@description, @sendername, @sendertelno, @receivername, @receivertelno, @address, @fromcity, @fromcountry, 
            @tocity, @tocountry, @parcelweight, @deliverycharge, @currency, @deliverydate, @deliveryStatus)";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@description", parcel.ItemDescription);
            cmd.Parameters.AddWithValue("@sendername", parcel.SenderName);
            cmd.Parameters.AddWithValue("@sendertelno", parcel.SenderTelNo);
            cmd.Parameters.AddWithValue("@receivername", parcel.ReceiverName);
            cmd.Parameters.AddWithValue("@receivertelno", parcel.ReceiverTelNo);
            cmd.Parameters.AddWithValue("@address", parcel.DeliveryAddress);
            cmd.Parameters.AddWithValue("@fromcity", parcel.FromCity);
            cmd.Parameters.AddWithValue("@fromcountry", parcel.FromCountry);
            cmd.Parameters.AddWithValue("@tocity", parcel.ToCity);
            cmd.Parameters.AddWithValue("@tocountry", parcel.ToCountry);
            cmd.Parameters.AddWithValue("@parcelweight", parcel.ParcelWeight);
            cmd.Parameters.AddWithValue("@deliverycharge", parcel.DeliveryCharge);
            cmd.Parameters.AddWithValue("@currency", "SGD");
            cmd.Parameters.AddWithValue("@deliverydate", parcel.TargetDeliveryDate);
            cmd.Parameters.AddWithValue("@deliveryStatus", 0);

            /*            cmd.Parameters.AddWithValue("@deliverydate", parcel.TargetDeliveryDate);
                        cmd.Parameters.AddWithValue("@status", parcel.DeliveryStatus);*/


            //A connection to database must be opened before any operations made.
            conn.Open();
            //ExecuteScalar is used to retrieve the auto-generated
            //ParcelID after executing the INSERT SQL statement
            parcel.ParcelId = (int)cmd.ExecuteScalar();

            //A connection should be closed after operations.
            conn.Close();
            //Return id when no error occurs.
            return parcel.ParcelId;
        }

        /*//adding into delivery history table
        public int History(DeliveryHistory history)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated RecordID after insertion
            cmd.CommandText = @"INSERT INTO DeliveryHistory (ParcelID, Description)
            OUTPUT INSERTED.RecordID 
            VALUES(@parcelid, @description)";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@parcelid", history.ParcelID);
            cmd.Parameters.AddWithValue("@description", "Received parcel by FrontOffSG1 on " + DateTime.Now.ToString("'dd'-'MM'-'yyyy'hh:mm tt"));

            conn.Open();
            //ExecuteScalar is used to retrieve the auto-generated
            //ParcelID after executing the INSERT SQL statement
            history.RecordID = (int)cmd.ExecuteScalar();

            //A connection should be closed after operations.
            conn.Close();
            //Return id when no error occurs.
            return history.RecordID;



        }*/
/*        public List<Parcel> GetAllCity()
        {
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement 
            cmd.CommandText = @"SELECT * FROM ShippingRate ORDER BY ShippingRateID";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a parcelList
            List<Parcel> CityList = new List<Parcel>();
            while (reader.Read())
            {
                CityList.Add(
                new Parcel
                {
                    ToCity = reader.GetString(3),
                });
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            Console.WriteLine(CityList);

            return CityList;
        }*/

        public List<Parcel> GetAllParcel()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
			//Specify the SELECT SQL statement 
			cmd.CommandText = @"SELECT * FROM Parcel WHERE DeliveryManID IS NULL ORDER BY ParcelID";
			//Open a database connection
			conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a parcelList
            List<Parcel> parcelList = new List<Parcel>();
            while (reader.Read())
            {
                parcelList.Add(
                new Parcel
                {
                    ParcelId = reader.GetInt32(0),              //0: 1st column
                    //ItemDescription = reader.GetString(1),      //1: 2nd column
                    //SenderName = reader.GetString(2),           //2: 3rd column
                    //SenderTelNo = reader.GetString(3),           //3: 4th column
                    ReceiverName = reader.GetString(4),         //4: 5th column
                    ReceiverTelNo = reader.GetString(5),         //5: 6th column
                    DeliveryAddress = reader.GetString(6),      //6: 7th column
                    //FromCity = reader.GetString(7),             //7: 8th column
                    //FromCountry = reader.GetString(8),          //8: 9th column
                    //ToCity = reader.GetString(9),               //9: 10th column
                    //ToCountry = reader.GetString(10),            //10: 11th column
                    //ParcelWeight = reader.GetFloat(11),         //11: 12th column
                    //DeliveryCharge = reader.GetDecimal(12),      //12: 13th column
                    //Currency = reader.GetString(13),            //13: 14th column
                     TargetDeliveryDate = !reader.IsDBNull(14) ?
reader.GetDateTime(14) : (DateTime?)null, //14: 14th column
                    DeliveryStatus = reader.GetString(15),      //15: 16th column
					//DeliveryManID = !reader.IsDBNull(16) ? reader.GetInt32(16) : (int?)null,
				}
                );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return parcelList;

        }

        public List<Parcel> GetAllFailedParcel()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement 
            cmd.CommandText = @"SELECT * FROM Parcel WHERE DeliveryStatus = 4 ORDER BY ParcelID";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a failedParcelList
            List<Parcel> failedParcelList = new List<Parcel>();
            while (reader.Read())
            {
                failedParcelList.Add(
                new Parcel
                {
                    ParcelId = reader.GetInt32(0),              //0: 1st column
                    ItemDescription = reader.GetString(1),      //1: 2nd column
                    DeliveryAddress = reader.GetString(6),      //6: 7th column
                    DeliveryStatus = reader.GetString(15),      //15: 16th column
                    DeliveryManID = !reader.IsDBNull(16) ? reader.GetInt32(16) : (int?)null,
                }
                );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return failedParcelList;

        }

        public List<Parcel> GetAllFailedParcelID()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement 
            cmd.CommandText = @"SELECT * FROM Parcel WHERE DeliveryStatus = 4 ORDER BY ParcelID";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a failedParcelIdList
            List<Parcel> failedParcelIdList = new List<Parcel>();
            while (reader.Read())
            {
                failedParcelIdList.Add(
                new Parcel
                {
                    ParcelId = reader.GetInt32(0),              //0: 1st column
                }
                );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return failedParcelIdList;

        }

        public List<Parcel> GetNumParcelAssign()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement 
            cmd.CommandText = @"select DeliveryManID,count(DeliveryStatus)
From Parcel
Where DeliveryStatus=1 AND DeliveryManID IS NOT NULL
Group by DeliveryManID
Having COUNT(DeliveryStatus)<6";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a numParcelAssignList
            List<Parcel> numParcelAssignList = new List<Parcel>();
            while (reader.Read())
            {
                numParcelAssignList.Add(
                new Parcel
                {
                    //ParcelId = reader.GetInt32(0),              //0: 1st column
                    DeliveryManID = !reader.IsDBNull(0) ? reader.GetInt32(0) : (int?)null,
                }
                );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return numParcelAssignList;

        }

    

    }


}
