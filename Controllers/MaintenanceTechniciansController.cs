using Buildings;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using Requests;
using Technicians;
using TodoApi.DBConnection;

namespace TodoApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class TechniciansController : Controller
    {
        NpgsqlConnection con = config.getConnection();

        [HttpGet(Name = "GetTechnicians")]
        public IActionResult Get()

        {
            return new ObjectResult(QueryMaintenanceTechnicians.GetTechnicians(con));
        }


        [HttpGet("{TechnicianId}", Name = "GetTechnicianBy")]
        public IActionResult Get(int TechnicianId)
        {
            return new ObjectResult(QueryMaintenanceTechnicians.GetTechnicianById(con, TechnicianId)); 
        }



        [HttpPost(Name = "AddTechnician")]
        public IActionResult Add(string FirstName, string LastName, string Speciality, int ContactID)
        {
            return new ObjectResult(QueryMaintenanceTechnicians.AddTechnician(con, FirstName, LastName, Speciality, ContactID));
        }



        [HttpDelete("{TechnicianId}", Name = "DeleteTechnicianById")]
        public IActionResult Delete(int TechnicianId)
        {
            try
            {
                bool result = QueryMaintenanceTechnicians.DeleteTechnician(con, TechnicianId);
                if (result)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("Could not delete Technician");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Could not delete Technician: "+ex );
            }
        }



        [HttpPut("SoftDelete/{TechnicianId}", Name = "SoftDeleteTechnicianById")]
        public IActionResult SoftDelete(int TechnicianId)
        {
            try
            {
                int result = QueryMaintenanceTechnicians.SoftDeleteTechnician(con, TechnicianId);
                if (result > 0)
                {
                    return Ok($"{result} recored affected\nTechnician with id {TechnicianId} is deleted");
                }
                else
                {
                    return BadRequest("Could not delete technician");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Could not delete technician: " + ex.Message);
            }
        }






    }


}
