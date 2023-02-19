using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Xml.Linq;
using Contacts;
using Npgsql;
using Units;
using Tenants;
using TodoApi.DBConnection;
using Buildings;

namespace TodoApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ContactsController : Controller
    {
        NpgsqlConnection con = config.getConnection();

        [HttpGet(Name = "GetContacts")]
        public IActionResult Get()

        {
            return new ObjectResult(QueryContacts.GetContacts(con));
        }



        [HttpGet("{ContactId}", Name = "GetContactsById")]
        public IActionResult Get(int ContactId)

        {
            return new ObjectResult(QueryContacts.GetContactById(con, ContactId));
        }



        [HttpPost(Name = "AddContact")]
        public IActionResult Add(string Email, string CellNumber, string AlternativeNumber)
        {
            return new ObjectResult(QueryContacts.AddContacts(con, Email, CellNumber, AlternativeNumber));
        }


        [HttpDelete("{ContactId}", Name = "DeleteContactById")]
        public IActionResult Delete(int ContactId)
        {
            try
            {
                bool result = QueryContacts.DeleteContact(con, ContactId);
                if (result)
                {
                    return Ok($"Contact with ID={ContactId} deleted.");
                }
                else
                {
                    return BadRequest("Could not delete contact");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Could not delete contact: " + ex.Message);
            }
        }


        [HttpPut("SoftDelete/{ContactId}", Name = "SoftDeleteContactById")]
        public IActionResult SoftDelete(int ContactId)
        {
            try
            {
                int result = QueryContacts.SoftDeleteContact(con, ContactId);
                if (result > 0)
                {
                    return Ok($"{result} recored affected\nContact with id {ContactId} is deleted");
                }
                else
                {
                    return BadRequest("Could not delete Contact");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Could not delete building: " + ex.Message);
            }
        }




    }


}
