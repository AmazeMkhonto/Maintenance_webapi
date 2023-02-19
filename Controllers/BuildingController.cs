using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Xml.Linq;
using Buildings;
using Npgsql;
using TodoApi.DBConnection;
using Tenants;
using System.Collections;

namespace TodoApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class BuildingsController : Controller
    {

        NpgsqlConnection con = config.getConnection();



        [HttpGet(Name = "GetBuildings")]
        public IActionResult Get()

        {
            return new ObjectResult(QueryBuildings.GetBuildings(con));
        }



        [HttpGet("{BuildingId}", Name = "GetBuildingById")]
        public IActionResult Get(int BuildingId)
        {
            try
            {
                Building result = QueryBuildings.GetBuildingById(con, BuildingId);

                if (result.BuildingID != 0)
                {
                    return new ObjectResult(result);
                }
                else
                {
                    return BadRequest($"There is no building with id: {BuildingId} ");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPost(Name = "AddBuildings")]
        public IActionResult Add( string name, string address)

        {
           return new ObjectResult(QueryBuildings.AddBuilding(con, name, address));
            
        }




        [HttpDelete("{BuildingId}", Name = "DeleteBuildingById")]
        public IActionResult Delete(int BuildingId)
        {
            try
            {
                bool result = QueryBuildings.DeleteBuilding(con, BuildingId);
                if (result)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("Could not delete building");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Could not delete building: " + ex.Message);
            }
        }


        [HttpPut("SoftDelete/{BuildingId}", Name = "SoftDeleteBuildingById")]
        public IActionResult SoftDelete(int BuildingId)
        {
            try
            {
                int result = QueryBuildings.SoftDeleteBuilding(con, BuildingId);
                if (result>0)
                {
                    return Ok($"{result} recored affected\nBuilding with id {BuildingId} is deleted");
                }
                else
                {
                    return BadRequest("Could not delete building");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Could not delete building: " + ex.Message);
            }
        }



        


        [HttpPut("Update/{buildingId}", Name = "UpdateBuildingById")]
        public IActionResult Update(int buildingId, string buildingName=null, string address = null, bool is_deleted = false)
        {
            bool updated = QueryBuildings.UpdateBuilding(con, buildingId, buildingName, address, is_deleted);

            if (updated)
            {
                return Ok($"Building with ID {buildingId} successfully updated.");
            }
            else
            {
                return BadRequest($"Failed to update building with ID {buildingId}.");
            }
        }






    }



}



