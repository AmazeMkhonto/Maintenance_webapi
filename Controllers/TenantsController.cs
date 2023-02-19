using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Xml.Linq;
using Npgsql;
using Tenants;
using TodoApi.DBConnection;

namespace TodoApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class TenantsController : Controller
    {
        NpgsqlConnection con = config.getConnection();

        [HttpGet(Name = "GetAllTenants")]
        public IActionResult Get()

        {
            return new ObjectResult(QueryTenants.GetAllTenants(con));
        }




        [HttpGet("{TenantID}", Name = "GetTenantById")]
        public IActionResult Get(int TenantID)
        {
            return new ObjectResult(QueryTenants.GetTenantById(con, TenantID));
        }




        [HttpPost(Name = "AddTenant")]
        public IActionResult Add(int UnitID, string name, string surname, int ContactID)
        {
            return new ObjectResult(QueryTenants.AddTenants(con, UnitID, name, surname, ContactID));
        }




        [HttpDelete("{TenantId}", Name = "DeleteTenantById")]
        public IActionResult Delete(int TenantId)
        {
            QueryTenants.DeleteTenant(con, TenantId);
            return Ok();
        }


        [HttpPut("SoftDelete/{TenantId}", Name = "SoftDeleteTenantById")]
        public IActionResult Deletes(int TenantId)
        {
            return new ObjectResult(QueryTenants.SoftDeleteTenants(con, TenantId));

        }


        [HttpPut("Update",Name = "UpdateTenant")]
        public IActionResult UpdateTenant(int tenantId, string firstName, string lastName)
        {
            var updated = QueryTenants.UpdateTenant(con, tenantId, firstName, lastName);

            if (updated)
            {
                return Ok($"Tenant with ID {tenantId} successfully updated.");
            }
            else
            {
                return BadRequest($"Failed to update tenant with ID {tenantId}.");
            }
        }



    }
}
