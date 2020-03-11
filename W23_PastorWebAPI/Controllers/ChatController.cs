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
    [RoutePrefix("api/Chat")]
    public class ChatController : ApiController
    {


        //GET api/chat/Getmessages
        [HttpPost]
        [Route("getMessages")]
        public List<ChatModel> getMessages(W23_PastorWebAPI.Models.Time time)
        {
            using (IDbConnection cnn = new ApplicationDbContext().Database.Connection)
            {
                string sql = $"SELECT * FROM dbo.ChatPlayer WHERE time > '{time.time}'";
                var player = cnn.Query<ChatModel>(sql).ToList();
                return player;
            }
        }
        //POST api/chat/InsertNewmessage
        [HttpPost]
        [Route("InsertNewMessage")]
        public IHttpActionResult InsertNewMessage(ChatModel player)
        {
            using (IDbConnection cnn = new ApplicationDbContext().Database.Connection)
            {
                string sql = $"INSERT INTO dbo.ChatPlayer(Id,message,time)" +
                    $" VALUES ('{player.Id}','{player.message}','{player.time}')";

                try
                {
                    cnn.Execute(sql);

                }
                catch (Exception e)
                {
                    return BadRequest("Error inserting chat message in database, " + e.Message);
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