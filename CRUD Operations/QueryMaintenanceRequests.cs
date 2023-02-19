
using Npgsql;

namespace Requests
{
    public class QueryMaintenanceRequests
    {
        public static List<MaintenanceRequest> GetRequests(NpgsqlConnection connection)
        {
            List<MaintenanceRequest> requests = new List<MaintenanceRequest>();


            using (NpgsqlCommand command = new NpgsqlCommand(
                "SELECT RequestID, TenantID, Description, RequestDate, is_deleted FROM MaintenanceRequests ",
                connection))
            {

                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        requests.Add(new MaintenanceRequest
                        {
                            RequestID = reader.GetInt32(0),
                            TenantID = reader.GetInt32(1),
                            Description = reader.GetString(2),
                            RequestDate = reader.GetDateTime(3),
                            is_deleted= reader.GetBoolean(4)

                        });
                    }
                }
            }
            connection.Close();

            return requests;
        }



        public static List<MaintenanceRequest> GetRequestById(NpgsqlConnection connection, int RequestID)
        {
            List<MaintenanceRequest> requests = new List<MaintenanceRequest>();


            using (NpgsqlCommand command = new NpgsqlCommand(
                "SELECT RequestID, TenantID, Description, RequestDate, is_deleted FROM MaintenanceRequests  WHERE RequestID = @RequestID",
                connection))
            {
                command.Parameters.AddWithValue("RequestID", RequestID);

                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        requests.Add(new MaintenanceRequest
                        {
                            RequestID = reader.GetInt32(0),
                            TenantID = reader.GetInt32(1),
                            Description = reader.GetString(2),
                            RequestDate = reader.GetDateTime(3),
                            is_deleted= reader.GetBoolean(4)

                        });
                    }
                }
            }
            connection.Close();

            return requests;
        }



        public static bool DeleteRequest(NpgsqlConnection connection, int RequestID)
        {
            using (NpgsqlCommand command = new NpgsqlCommand(
                "DELETE FROM MaintenanceRequests WHERE RequestID = @RequestID; ",
                connection))
            {
                command.Parameters.AddWithValue("RequestID", RequestID);

                int affectedRows = command.ExecuteNonQuery();

                connection.Close();
                return affectedRows > 0;
            }

        }


        public static MaintenanceRequest[] AddRequest(NpgsqlConnection connection, int TenantID, string Description, DateTime RequestDate)
        {
            List<MaintenanceRequest> requests = new();

            using (NpgsqlCommand command = new NpgsqlCommand(
                "INSERT INTO MaintenanceRequests (TenantID,  Description,  RequestDate) VALUES (@TenantID, @Description,  @RequestDate)",
                connection))
            {
                command.Parameters.AddWithValue("TenantID", TenantID);
                command.Parameters.AddWithValue("Description", Description);
                command.Parameters.AddWithValue("RequestDate", RequestDate);
               

                command.ExecuteScalar();

                MaintenanceRequest request = new MaintenanceRequest();

                request.TenantID = TenantID;
                request.Description = Description;
                request.RequestDate = RequestDate;
                        
                
            }
            connection.Close();

            return requests.ToArray();
        }



        public static int SoftDeleteRequest(NpgsqlConnection connection, int RequestID)
        {
            int n = 0;

            try
            {
                using (NpgsqlCommand command = new NpgsqlCommand(
                    "UPDATE MaintenanceRequests set is_deleted=cast(1 as bit) where RequestID=@RequestID",
                    connection))
                {
                    command.Parameters.AddWithValue("RequestID", RequestID);
                    n = command.ExecuteNonQuery();
                }

                connection.Close();
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine("Could not delete request: " + ex.Message);
            }

            return n;
        }






    }
}
