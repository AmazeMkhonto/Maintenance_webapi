
using Npgsql;


namespace Tenants
{
    public class QueryTenants
    {



        public static List<Tenant> GetAllTenants(NpgsqlConnection connection)
        {
            List<Tenant> tenants = new List<Tenant>();


            using (NpgsqlCommand command = new NpgsqlCommand(
                "SELECT * FROM Tenants ",
                connection))
            {

                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tenants.Add(new Tenant
                        {
                            TenantID = reader.GetInt32(0),
                            UnitID = reader.GetInt32(1),
                            FirstName = reader.GetString(2),
                            LastName = reader.GetString(3),
                            ContactID = reader.GetInt32(4),
                            is_deleted = reader.GetBoolean(5)
                        });
                    }
                }
            }
            connection.Close();

            return tenants;
        }


        public static List<Tenant> GetTenantById(NpgsqlConnection connection, int TenantID)

        {
            List<Tenant> tenants = new List<Tenant>();

            using (NpgsqlCommand command = new NpgsqlCommand(
                "SELECT * FROM Tenants WHERE TenantID = @TenantID",
                connection))
            {
                command.Parameters.AddWithValue("TenantID", TenantID);
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tenants.Add(new Tenant
                        {
                            TenantID = reader.GetInt32(0),
                            UnitID = reader.GetInt32(1),
                            FirstName = reader.GetString(2),
                            LastName = reader.GetString(3),
                            ContactID = reader.GetInt32(4),
                            is_deleted = reader.GetBoolean(5)
                        });
                    }
                }
            }
            connection.Close();
            return tenants;
        }



        public static Tenant[] AddTenants(NpgsqlConnection connection, int UnitID, string FirstName, string LastName, int ContactID)
        {
            List<Tenant> tenants = new();

            try
            {
                using (NpgsqlCommand command = new NpgsqlCommand(
                    "INSERT INTO Tenants (UnitID, FirstName, LastName, ContactID) VALUES (@UnitID, @FirstName, @LastName, @ContactID)",
                    connection))
                {
                    command.Parameters.AddWithValue("UnitID", UnitID);
                    command.Parameters.AddWithValue("FirstName", FirstName);
                    command.Parameters.AddWithValue("LastName", LastName);
                    command.Parameters.AddWithValue("ContactID", ContactID);

                    command.ExecuteNonQuery();

                    Tenant tenant = new Tenant();

                    tenant.UnitID = UnitID;
                    tenant.FirstName = FirstName;
                    tenant.LastName = LastName;
                    tenant.ContactID = ContactID;
                    tenant.is_deleted = false;

                    tenants.Add(tenant);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return tenants.ToArray();
        }




        public static bool DeleteTenant(NpgsqlConnection connection, int tenantID)
        {
            using (NpgsqlCommand command = new NpgsqlCommand("DELETE FROM Tenants WHERE TenantID = @tenantID", connection))
            {
                command.Parameters.AddWithValue("tenantID", tenantID);

                int affectedRows = command.ExecuteNonQuery(); 
                
                connection.Close();
                return affectedRows > 0;
            }
        
        }



        public static int SoftDeleteTenants(NpgsqlConnection connection, int TenantID)
        {
            int numberOfRowsAffected = 0;
            using (NpgsqlCommand command = new NpgsqlCommand(
                "UPDATE Tenants set is_deleted=cast(1 as bit) where TenantID=@TenantID",
                connection))
            {
                command.Parameters.AddWithValue("TenantID", TenantID);
                numberOfRowsAffected = command.ExecuteNonQuery();
            }
                connection.Close();
                return numberOfRowsAffected;

        }



        public static bool UpdateTenant(NpgsqlConnection connection, int tenantID, string firstName, string lastName)
        {
            using (NpgsqlCommand command = new NpgsqlCommand(
                "UPDATE Tenants SET FirstName = @FirstName, LastName = @LastName WHERE TenantID = @TenantID",
                connection))
            {
                command.Parameters.AddWithValue("FirstName", firstName);
                command.Parameters.AddWithValue("LastName", lastName);
                command.Parameters.AddWithValue("TenantID", tenantID);

                int affectedRows = command.ExecuteNonQuery();

                connection.Close();

                return affectedRows > 0;
            }
        }




    }


}
    




