using Npgsql;

namespace Jobs
{
    public class QueryJobs
    {
        public static List<Job> GetJobs(NpgsqlConnection connection)
        {
            List<Job> jobs = new List<Job>();


            using (NpgsqlCommand command = new NpgsqlCommand(
                "SELECT * FROM Jobs ",
                connection))
            {

                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        jobs.Add(new Job
                        {
                            JobID = reader.GetInt32(0),
                            Status = reader.GetString(1),
                            TechnicianID = reader.GetInt32(2),
                            RequestID= reader.GetInt32(3),
                            CompletionDate= reader.GetDateTime(4)

                        });
                    }
                }
            }
            connection.Close();

            return jobs;
        }



        public static List<Job> GetJobById(NpgsqlConnection connection, int RequestID)
        {
            List<Job> jobs = new List<Job>();


            using (NpgsqlCommand command = new NpgsqlCommand(
                "SELECT * FROM Jobs WHERE RequestID = @RequestID",
                connection))
            {
                command.Parameters.AddWithValue("RequestID", RequestID);

                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        jobs.Add(new Job
                        {
                            JobID = reader.GetInt32(0),
                            Status = reader.GetString(1),
                            TechnicianID = reader.GetInt32(2),
                            RequestID = reader.GetInt32(3),
                            CompletionDate = reader.GetDateTime(4)

                        });
                    }
                }
            }
            connection.Close();

            return jobs;
        }

    }
}
