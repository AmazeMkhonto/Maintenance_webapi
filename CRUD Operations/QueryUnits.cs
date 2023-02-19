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
                            UnitType = reader.GetString(3)
                        
                        });
                    }
                }
            }
            connection.Close();

            return units;
        }


        public static List<Unit> GetUnitById(NpgsqlConnection connection, int BuildingID)

        {
            List<Unit> units = new List<Unit>();

            using (NpgsqlCommand command = new NpgsqlCommand(
                "SELECT * FROM Units WHERE BuildingID = @BuildingID",
                connection))
            {
                command.Parameters.AddWithValue("BuildingID", BuildingID);
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        units.Add(new Unit
                        {
                            UnitID = reader.GetInt32(0),
                            BuildingID = reader.GetInt32(1),
                            UnitNumber = reader.GetInt32(2),
                            UnitType = reader.GetString(3)
                        });
                    }
                }
            }
            connection.Close();
            return units;
        }

    }
}
