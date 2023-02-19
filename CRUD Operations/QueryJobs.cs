using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Npgsql;
using System.Data;

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
                            CompletionDate= reader.GetDateTime(4),
                            is_deleted=reader.GetBoolean(5)

                        });
                    }
                }
            }
            connection.Close();

            return jobs;
        }



        public static List<Job> GetJobById(NpgsqlConnection connection, int JobID)
        {
            List<Job> jobs = new List<Job>();


            using (NpgsqlCommand command = new NpgsqlCommand(
                "SELECT * FROM Jobs WHERE JobID = @JobID",
                connection))
            {
                command.Parameters.AddWithValue("JobID", JobID);

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
                            CompletionDate = reader.GetDateTime(4),
                            is_deleted=reader.GetBoolean(5)

                        });
                    }
                }
            }
            connection.Close();

            return jobs;
        }


        public static int SoftDeleteJob(NpgsqlConnection connection, int JobID)
        {
            int n = 0;

            try
            {
                using (NpgsqlCommand command = new NpgsqlCommand(
                    "UPDATE Jobs set is_deleted=cast(1 as bit) where JobID=@JobID",
                    connection))
                {
                    command.Parameters.AddWithValue("JobID", JobID);
                    n = command.ExecuteNonQuery();
                }

                connection.Close();
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine("Could not delete job: " + ex.Message);
            }

            return n;
        }


        public static bool DeleteJob(NpgsqlConnection connection, int JobID)
        {
            try
            {

                using (NpgsqlCommand command = new NpgsqlCommand("DELETE FROM Jobs WHERE JobID = @JobID", connection))
                {
                    command.Parameters.AddWithValue("JobID", JobID);

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


        public static Job[] AddJob(NpgsqlConnection connection, string status, int TechnicianID, int RequestId, DateTime CompletionDate)
        {
            List<Job> jobs = new();

            using (NpgsqlCommand command = new NpgsqlCommand(
                "INSERT INTO Jobs (status, TechnicianID, RequestId, CompletionDate) VALUES (@status, @TechnicianID, @RequestId, @CompletionDate)",
                connection))
            {
                command.Parameters.AddWithValue("status", status);
                command.Parameters.AddWithValue("TechnicianID", TechnicianID);
                command.Parameters.AddWithValue("RequestId", RequestId);
                command.Parameters.AddWithValue("CompletionDate", CompletionDate);

                command.ExecuteNonQuery();

                Job job = new Job();

                job.Status = status;
                job.TechnicianID= TechnicianID;
                job.RequestID = RequestId;
                job.CompletionDate = CompletionDate;

                jobs.Add(job);
            }
            connection.Close();

            return jobs.ToArray();
        }




    }
}
