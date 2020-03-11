using Dapper;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using W23_PastorWebAPI.Models;

namespace W23_PastorWebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/Player")]
    public class PlayerController : ApiController
    {

         
        //GET api/Player/GetPlayerInfo
        [HttpGet]
        [Route("GetPlayerInfo/{id}")]
        public Player GetPlayerInfo(string Id)
        {
            string authenticatedAspNetUserId = RequestContext.Principal.Identity.GetUserId();
            using (IDbConnection cnn = new ApplicationDbContext().Database.Connection)
            {
                string sql = $"SELECT * FROM dbo.Player WHERE Id LIKE '{Id}'";
                var player = cnn.Query<Player>(sql).FirstOrDefault();
                return player;
            }
        }
         


        //POST api/Player/InsertNewPlayer
        [HttpPost]
        [Route("InsertNewPlayer")]
        public IHttpActionResult InsertNewPlayer(Player player)
        {
            using (IDbConnection cnn = new ApplicationDbContext().Database.Connection)
            {
                //string sql = $"INSERT INTO dbo.Players(Id, Firstname, LastName, NickName, City) VALUES ('{player.Id}','{player.FirstName}','{player.LastName}','{player.NickName}','{player.City}')";

                string sql = $"INSERT INTO dbo.Player(Id,Email,NickName)" +
                    $" VALUES ('{player.Id}','{player.Email}','{player.NickName}')";

                try
                {
                    cnn.Execute(sql);

                }
                catch (Exception e)
                {
                    return BadRequest("Error inserting player in database, " + e.Message);
                }
                finally
                {
                    cnn.Close();
                }

                return Ok();
            }
        }


    }
}