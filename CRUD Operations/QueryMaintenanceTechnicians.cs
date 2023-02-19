

using Npgsql;

namespace Technicians
{
    public class QueryMaintenanceTechnicians
    {
        public static List<MaintenanceTechnician> GetTechnicians(NpgsqlConnection connection)
        {
            List<MaintenanceTechnician> technicians = new List<MaintenanceTechnician>();


            using (NpgsqlCommand command = new NpgsqlCommand(
                "select * from MaintenanceTechnicians; ",
                connection))
            {

                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        technicians.Add(new MaintenanceTechnician
                        {
                            TechnicianID = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            Speciality = reader.GetString(3),
                            ContactID= reader.GetInt32(4)

                        });
                    }
                }
            }
            connection.Close();

            return technicians;
        }





        public static List<MaintenanceTechnician> GetTechnicianById(NpgsqlConnection connection, int TechnicianID)
        {
            List<MaintenanceTechnician> technicians = new List<MaintenanceTechnician>();


            using (NpgsqlCommand command = new NpgsqlCommand(
                "select * from MaintenanceTechnicians where TechnicianID = @TechnicianID ; ",
                connection))
            {

                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        technicians.Add(new MaintenanceTechnician
                        {
                            TechnicianID = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            Speciality = reader.GetString(3),
                            ContactID = reader.GetInt32(4)

                        });
                    }
                }
            }
            connection.Close();

            return technicians;
        }




        public static bool DeleteTechnician(NpgsqlConnection connection, int TechnicianID)
        {
            using (NpgsqlCommand command = new NpgsqlCommand(
                "DELETE FROM MaintenanceTechnicians WHERE TechnicianID = @TechnicianID; ",
                connection))
            {
                command.Parameters.AddWithValue("TechnicianID", TechnicianID);

                int affectedRows = command.ExecuteNonQuery();

                connection.Close();
                return affectedRows > 0;
            }
     
        }


        public static MaintenanceTechnician[] AddTechnician(NpgsqlConnection connection, string FirstName, string LastName, string Speciality, int ContactID)
        {
            List<MaintenanceTechnician> techs = new();

            using (NpgsqlCommand command = new NpgsqlCommand(
                "INSERT INTO MaintenanceTechnicians (FirstName, LastName, Speciality, ContactID) VALUES (@FirstName, @LastName, @Speciality, @ContactID)",
                connection))
            {
                command.Parameters.AddWithValue("FirstName", FirstName);
                command.Parameters.AddWithValue("LastName", LastName);
                command.Parameters.AddWithValue("Speciality", Speciality);
                command.Parameters.AddWithValue("ContactID", ContactID);

                command.ExecuteScalar();

                MaintenanceTechnician tech = new MaintenanceTechnician();
                
                tech.FirstName = FirstName;
                tech.LastName = LastName;
                tech.Speciality = Speciality;
                tech.ContactID = ContactID;
                techs.Add(tech);
            }
            connection.Close();

            return techs.ToArray();
        }




    }
}

