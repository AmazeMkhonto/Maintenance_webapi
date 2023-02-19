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

            [HttpGet("{id}", Name = "GetUnitsById")]
            public IActionResult Get(int id)
            {
                return new ObjectResult(QueryUnits.GetUnitById(con, id));
            }
        }
    }

