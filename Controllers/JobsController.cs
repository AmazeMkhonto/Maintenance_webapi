using Microsoft.AspNetCore.Mvc;
using Npgsql;
using Jobs;
using Units;
using TodoApi.DBConnection;

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

    }


}
