using Npgsql;
using System.Linq.Expressions;

namespace Buildings
{
    public class QueryBuildings
    {
        public static List<Building> GetBuildings(NpgsqlConnection connection)
        {
            List<Building> buildings = new List<Building>();

            string query = "SELECT * FROM Buildings ";
            using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
            {
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        buildings.Add(new Building
                        {
                            BuildingID = reader.GetInt32(0),
                            BuildingName = reader.GetString(1),
                            Address = reader.GetString(2),
                            is_deleted= reader.GetBoolean(3),
                           
                        });
                    }
                }
            }
            connection.Close();

            return buildings;
        }


        public static Building GetBuildingById(NpgsqlConnection connection, int BuildingID)
        {
            Building building = new();

            try
            {
                using (NpgsqlCommand command = new NpgsqlCommand(
                    "SELECT * FROM Buildings WHERE BuildingID = @BuildingID",
                    connection))
                {
                    command.Parameters.AddWithValue("BuildingID", BuildingID);

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            building = new Building
                            {
                                BuildingID = reader.GetInt32(0),
                                BuildingName = reader.GetString(1),
                                Address = reader.GetString(2),
                                is_deleted = reader.GetBoolean(3),

                            };
                        }
                    }
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally { 
                connection.Close();
            }
           
            return building;
        }





        public static Building[] AddBuilding(NpgsqlConnection connection, string BuildingName, string Address)
        {
            List<Building> buildings = new();
            try
            {

                using (NpgsqlCommand command = new NpgsqlCommand(
                    "INSERT INTO Buildings (BuildingName, Address) VALUES (@BuildingName, @Address) RETURNING BuildingID",
                    connection))
                {
                    command.Parameters.AddWithValue("BuildingName", BuildingName);
                    command.Parameters.AddWithValue("Address", Address);
                    command.Parameters.AddWithValue("is_deleted", false);


                    int buildingID = (int)command.ExecuteScalar();

                    Building building = new Building();
                    building.BuildingID = buildingID;
                    building.BuildingName = BuildingName;
                    building.Address = Address;
                    
                    buildings.Add(building);
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return buildings.ToArray();
        }







        public static bool DeleteBuilding(NpgsqlConnection connection, int BuildingID)
        {
            try
            {

                using (NpgsqlCommand command = new NpgsqlCommand("DELETE FROM Buildings WHERE BuildingID = @BuildingID", connection))
                {
                    command.Parameters.AddWithValue("BuildingID", BuildingID);

                    int affectedRows = command.ExecuteNonQuery();

                    return affectedRows > 0;
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            finally { connection.Close(); }

        }




        public static int SoftDeleteBuilding(NpgsqlConnection connection, int BuildingID)
        {
            int n = 0;

            try
            {
                using (NpgsqlCommand command = new NpgsqlCommand(
                    "UPDATE Buildings set is_deleted=cast(1 as bit) where BuildingID=@BuildingID",
                    connection))
                {
                    command.Parameters.AddWithValue("BuildingID", BuildingID);
                    n = command.ExecuteNonQuery();
                }

                connection.Close();
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine("Could not delete building: " + ex.Message);
            }

            return n;
        }



        //public static bool UpdateBuilding(NpgsqlConnection connection, int BuildingID, string buildingName)
        //{
        //    using (NpgsqlCommand command = new NpgsqlCommand(
        //        "UPDATE Buildings SET buildingName = @buildingName WHERE BuildingID = @BuildingID",
        //        connection))
        //    {
        //        command.Parameters.AddWithValue("buildingName", buildingName);

        //        command.Parameters.AddWithValue("BuildingID", BuildingID);

        //        int affectedRows = command.ExecuteNonQuery();

        //        connection.Close();

        //        return affectedRows > 0;
        //    }
        //}


        public static bool UpdateBuilding(NpgsqlConnection connection, int buildingID, string buildingName = null, string address = null, bool? isDeleted = null)
        {
            using (NpgsqlCommand command = new NpgsqlCommand(
                "UPDATE Buildings SET " +
                (buildingName != null ? "buildingName = @buildingName, " : "") +
                (address != null ? "address = @address, " : "") +
                (isDeleted.HasValue ? "is_deleted = CASE WHEN @isDeleted THEN CAST(1 AS bit) ELSE CAST(0 AS bit) END " : "") +
                "WHERE buildingID = @buildingID",
                connection))
            {
                if (buildingName != null)
                    command.Parameters.AddWithValue("buildingName", buildingName);

                if (address != null)
                    command.Parameters.AddWithValue("address", address);

                if (isDeleted.HasValue)
                    command.Parameters.AddWithValue("isDeleted", isDeleted.Value);

                command.Parameters.AddWithValue("buildingID", buildingID);

                int affectedRows = command.ExecuteNonQuery();

                connection.Close();

                return affectedRows > 0;
            }
        }


      



    }
}

