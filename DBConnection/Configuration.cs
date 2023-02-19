using Npgsql;


namespace TodoApi.DBConnection
{


    public class config
    {

        public const string connectionString = $"Server=localhost;Port=5432;User Id=postgres;Password=F1234;Database=demoDB";

        public static NpgsqlConnection connection = new NpgsqlConnection(connectionString);

        public static NpgsqlConnection getConnection()
        {
            connection.Open();
            return connection;
        }
    }





}
