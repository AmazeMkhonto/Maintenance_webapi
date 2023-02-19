using Microsoft.AspNetCore.Mvc;
using Npgsql;
using Jobs;
using Units;
using TodoApi.DBConnection;
using Contacts;
using Buildings;

namespace TodoApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class JobsController : Controller
    {
        NpgsqlConnection con = config.getConnection();



        [HttpGet(Name = "GetJobs")]
        public IActionResult Get()
        {
            return new ObjectResult(QueryJobs.GetJobs(con));
        }



        [HttpGet("{JobsId}", Name = "GetJobsById")]
        public IActionResult Get(int JobsId)
        {
            return new ObjectResult(QueryJobs.GetJobById(con, JobsId));
        }



        [HttpPut("SoftDelete/{JobsId}", Name = "SoftDeleteJobById")]
        public IActionResult SoftDelete(int JobsId)
        {
            try
            {
                int result = QueryJobs.SoftDeleteJob(con, JobsId);
                if (result > 0)
                {
                    return Ok($"{result} recored affected\nJob with id {JobsId} is deleted");
                }
                else
                {
                    return BadRequest("Could not delete Job");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Could not delete job: " + ex.Message);
            }
        }



        [HttpDelete("{JobsId}", Name = "DeleteJobById")]
        public IActionResult Delete(int JobsId)
        {
            try
            {
                bool result = QueryJobs.DeleteJob(con, JobsId);
                if (result)
                {
                    return Ok($"Job with ID={JobsId} deleted.");
                }
                else
                {
                    return BadRequest("Could not delete job");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Could not delete job: " + ex.Message);
            }
        }


        [HttpPost(Name = "AddJobs")]
        public IActionResult Add(string status, int TechnicianID, int RequestId, DateTime CompletionDate)

        {
            return new ObjectResult(QueryJobs.AddJob(con, status, TechnicianID, RequestId, CompletionDate));

        }

    }


}
