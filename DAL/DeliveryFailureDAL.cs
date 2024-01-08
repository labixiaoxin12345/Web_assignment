using P02_WebApr2023_Assg1_Team6.Models;
using System.Data.SqlClient;

namespace P02_WebApr2023_Assg1_Team6.DAL
{
    public class DeliveryFailureDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;
        //Constructor
        public DeliveryFailureDAL()
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


        public List<DeliveryFailure> GetAllDeliveryFailureReport()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM DeliveryFailure ORDER BY ReportID";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a deliveryFailureList list
            List<DeliveryFailure> deliveryFailureList = new List<DeliveryFailure>();
            while (reader.Read())
            {
                deliveryFailureList.Add(
                new DeliveryFailure
                {
                    ReportID = reader.GetInt32(0), 
                    ParcelID = reader.GetInt32(1), 
                    DeliveryManID = reader.GetInt32(2), 
                    FailureType = reader.GetString(3), 
                    Description = reader.GetString(4), 
                    StationMgrID = !reader.IsDBNull(5) ? reader.GetInt32(5) : (int?)null, 
                    FollowUpAction = !reader.IsDBNull(6) ?
 reader.GetString(6) : string.Empty, 
                    DateCreated = reader.GetDateTime(7), 

                }
                );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return deliveryFailureList;
        }


        public int Add(DeliveryFailure deliveryFailure)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated ReportID after insertion
            cmd.CommandText = @"INSERT INTO DeliveryFailure (ParcelID, DeliveryManID, FailureType, Description,
StationMgrID, FollowUpAction, DateCreated)
OUTPUT INSERTED.ReportID
VALUES(@parcelID, @deliveryManID, @failureType, @description,
@stationMgrID, @followUpAction, @dateCreated)";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@parcelID", deliveryFailure.ParcelID);
            cmd.Parameters.AddWithValue("@deliveryManID", deliveryFailure.DeliveryManID);
            cmd.Parameters.AddWithValue("@failureType",deliveryFailure.FailureType);
            cmd.Parameters.AddWithValue("@description", deliveryFailure.Description);
            cmd.Parameters.AddWithValue("@stationMgrID", deliveryFailure.StationMgrID);
            cmd.Parameters.AddWithValue("@followUpAction", deliveryFailure.FollowUpAction);
            cmd.Parameters.AddWithValue("@dateCreated", DateTime.Now);
            //A connection to database must be opened before any operations made.
            conn.Open();
            //ExecuteScalar is used to retrieve the auto-generated
            //ReportID after executing the INSERT SQL statement
            deliveryFailure.ReportID = (int)cmd.ExecuteScalar();
            //A connection should be closed after operations.
            conn.Close();
            //Return id when no error occurs.
            return deliveryFailure.ReportID;
        }
    }
}
