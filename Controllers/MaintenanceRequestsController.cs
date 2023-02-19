using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Xml.Linq;
using Requests;
using Npgsql;
using Jobs;
using TodoApi.DBConnection;
using Technicians;

namespace TodoApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class RequestsController : Controller
    {
        NpgsqlConnection con = config.getConnection();


        [HttpGet(Name = "GetRequests")]
        public IActionResult Get()
        {
            return new ObjectResult(QueryMaintenanceRequests.GetRequests(con));
        }



        [HttpGet("{RequestId}", Name = "GetRequestById")]
        public IActionResult Get(int RequestId)
        {
            return new ObjectResult(QueryMaintenanceRequests.GetRequestById(con, RequestId));
        }



        [HttpPost(Name = "AddRequest")]
        public IActionResult Add(int TenantID, string Description, DateTime RequestDate)
        {
            return new ObjectResult(QueryMaintenanceRequests.AddRequest(con, TenantID, Description, RequestDate));
        }



        [HttpDelete(Name = "DeleteRequest")]
        public IActionResult Delete(int RequestId)
        {
            try
            {
                bool result = QueryMaintenanceRequests.DeleteRequest(con, RequestId);
                if (result)
                {
                    return Ok("Request deleted");
                }
                else
                {
                    return BadRequest("Could not delete request");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Could not delete request: ");
            }
        }



        //[HttpPut(Name = "UpdateRequest")]
        //public IActionResult Update()
        //{
        //    return View();
        //}













    }


}

