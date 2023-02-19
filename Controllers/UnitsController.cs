using Buildings;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Xml.Linq;
using TodoApi.DBConnection;
using Units;

namespace TodoApi.Controllers
{

    [ApiController]
        [Route("[controller]")]
        public class UnitsController : Controller
        {
            NpgsqlConnection con = config.getConnection();

            [HttpGet(Name = "GetAllUnits")]
            public IActionResult Get()
            {

                return new ObjectResult(QueryUnits.GetAllUnits(con));

            }



            [HttpGet("{UnitID}", Name = "GetUnitsById")]
            public IActionResult Get(int UnitID)
            {
                return new ObjectResult(QueryUnits.GetUnitById(con, UnitID));
            }



        [HttpPost(Name = "AddUnit")]
        public IActionResult Add(int BuildingID, int UnitNumber, string UnitType)

        {
            return new ObjectResult(QueryUnits.AddUnit(con, BuildingID, UnitNumber, UnitType));

        }



        [HttpDelete("{UnitID}", Name = "DeleteUnitById")]
        public IActionResult Delete(int UnitID)
        {
            try
            {
                bool result = QueryUnits.DeleteUnit(con, UnitID);
                if (result)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("Could not delete unit");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Could not delete unit: " + ex.Message);
            }
        }




        [HttpPut("SoftDelete/{UnitID}", Name = "SoftDeleteUnitById")]
        public IActionResult SoftDelete(int UnitID)
        {
            try
            {
                int result = QueryUnits.SoftDeleteUnit(con, UnitID);
                if (result > 0)
                {
                    return Ok($"{result} recored affected\nUnit with id {UnitID} is deleted");
                }
                else
                {
                    return BadRequest("Could not delete unit");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Could not delete unit: " + ex.Message);
            }
        }




    }
    }

