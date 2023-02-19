using Npgsql;

namespace Units
{
    public class QueryUnits
    {
        public static List<Unit> GetAllUnits(NpgsqlConnection connection)
        {
            List<Unit> units = new List<Unit>();


            using (NpgsqlCommand command = new NpgsqlCommand(
                "SELECT * FROM Units",
                connection))
            {

                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        units.Add(new Unit
                        {
                            UnitID = reader.GetInt32(0),
                            BuildingID = reader.GetInt32(1),
                            UnitNumber = reader.GetInt32(2),
                            UnitType = reader.GetString(3),
                            is_deleted= reader.GetBoolean(4)
                        
                        });
                    }
                }
            }
            connection.Close();

            return units;
        }


        public static List<Unit> GetUnitById(NpgsqlConnection connection, int UnitID)

        {
            List<Unit> units = new List<Unit>();

            using (NpgsqlCommand command = new NpgsqlCommand(
                "SELECT * FROM Units WHERE UnitID = @UnitID",
                connection))
            {
                command.Parameters.AddWithValue("UnitID", UnitID);
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        units.Add(new Unit
                        {
                            UnitID = reader.GetInt32(0),
                            BuildingID = reader.GetInt32(1),
                            UnitNumber = reader.GetInt32(2),
                            UnitType = reader.GetString(3),
                            is_deleted= reader.GetBoolean(4)
                        });
                    }
                }
            }
            connection.Close();
            return units;
        }




        public static Unit[] AddUnit(NpgsqlConnection connection, int BuildingID, int UnitNumber, string UnitType)
        {
            List<Unit> units = new();
            try
            {

                using (NpgsqlCommand command = new NpgsqlCommand(
                    "INSERT INTO Buildings (BuildingID, UnitNumber, UnitType) VALUES (@BuildingID, @UnitNumber, @UnitType)",
                    connection))
                {
                    command.Parameters.AddWithValue("BuildingID", BuildingID);
                    command.Parameters.AddWithValue("UnitNumber", UnitNumber);
                    command.Parameters.AddWithValue("UnitType", UnitType);
                    command.Parameters.AddWithValue("is_deleted", false);


                    int UnitID = (int)command.ExecuteScalar();

                    Unit unit = new Unit();

                    unit.BuildingID = BuildingID;
                    unit.UnitNumber = UnitNumber;
                    unit.UnitType= UnitType;
                  
                    units.Add(unit);
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
            return units.ToArray();
        }







        public static bool DeleteUnit(NpgsqlConnection connection, int UnitID)
        {
            try
            {

                using (NpgsqlCommand command = new NpgsqlCommand("DELETE FROM Units WHERE UnitID = @UnitID", connection))
                {
                    command.Parameters.AddWithValue("UnitID", UnitID);

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




        public static int SoftDeleteUnit(NpgsqlConnection connection, int UnitID)
        {
            int n = 0;

            try
            {
                using (NpgsqlCommand command = new NpgsqlCommand(
                    "UPDATE Units set is_deleted=cast(1 as bit) where UnitID=@UnitID",
                    connection))
                {
                    command.Parameters.AddWithValue("UnitID", UnitID);
                    n = command.ExecuteNonQuery();
                }

                connection.Close();
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine("Could not delete unit: " + ex.Message);
            }

            return n;
        }




    }
}
