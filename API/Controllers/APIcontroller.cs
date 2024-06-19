using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Diagnostics.Eventing.Reader;
using System.Text;
using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;
using API.Business;


namespace API.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class APIcontroller : Controller
    {
        API.Business.BusinessController _ctrl = new API.Business.BusinessController();

        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("api/items/auth")]
        public IActionResult ValidateUser(string data)
        {
            byte[] bytes = Convert.FromBase64String(data);
            string jsonData = Encoding.UTF8.GetString(bytes);
            JsonDocument jsonDocument = JsonDocument.Parse(jsonData);
            JsonElement Payload = jsonDocument.RootElement ;
            /*try
            {*/
                var Item = _ctrl.ValidateCredentials(Payload.GetProperty("Email").GetString(), Payload.GetProperty("Password").GetString());

                return Ok(Item);
            /*}
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.Message);  //server error
            }*/
            
        }

        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("api/items/ValidateSession")]
        public IActionResult ValidateSession(string data)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(data);
                string jsonData = Encoding.UTF8.GetString(bytes);
                JsonDocument jsonDocument = JsonDocument.Parse(jsonData);
                JsonElement Payload = jsonDocument.RootElement;

                int UID = Convert.ToInt32(Payload.GetProperty("UID").GetString());
                string SESID = Payload.GetProperty("SESID").GetString();

                bool Item = _ctrl.ValidateSessionID(UID, SESID);
                if (Item == true)
                {
                    return Ok(); //authorized
                }
                else
                {
                    return StatusCode(501, "Unauthorized");
                }
                
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message); //server error
            }

        }
            

        [Microsoft.AspNetCore.Mvc.HttpDelete]
        [Microsoft.AspNetCore.Mvc.Route("api/items/LogOut")]
        public IActionResult DeleteSessionID([Microsoft.AspNetCore.Mvc.FromBody] JsonElement Payload)
        {
            try{
                int UID = Convert.ToInt32(Payload.GetProperty("UID").GetString());
                string SESID = Payload.GetProperty("SESID").GetString();
            
                if (_ctrl.ValidateSessionID(UID, SESID))
                {
                    _ctrl.DeleteSessionID(UID);
                    return Ok(); //succesfull
                }
                else
                {
                    return StatusCode(401, "Unauthorized"); //session not correct
                }
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); //server error
            }
            
        }

        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("api/items/GetUserPerms")]
        public IActionResult GetUserPermission(string data)
        {
            try {
                byte[] bytes = Convert.FromBase64String(data);
                string jsonData = Encoding.UTF8.GetString(bytes);
                JsonDocument jsonDocument = JsonDocument.Parse(jsonData);
                JsonElement Payload = jsonDocument.RootElement;

                int UID = Convert.ToInt32(Payload.GetProperty("UID").GetString());
                string SESID = Payload.GetProperty("SESID").GetString();

                if (_ctrl.ValidateSessionID(UID, SESID))
                {
                    string Item = _ctrl.GetUserPermission(UID);
                    return Ok(Item);
                }
                else
                {
                    return StatusCode(401, "Unauthorized");
                }
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("api/items/GetUserData")]
        public IActionResult GetUserData(string data)
        {
            /*try
            {*/
                byte[] bytes = Convert.FromBase64String(data);
                string jsonData = Encoding.UTF8.GetString(bytes);
                JsonDocument jsonDocument = JsonDocument.Parse(jsonData);
                JsonElement Payload = jsonDocument.RootElement;

                int IDtargetUser = Payload.GetProperty("IDtargetUser").GetInt32();
                int UID = Convert.ToInt32(Payload.GetProperty("UID").GetString());
                string SESID = Payload.GetProperty("SESID").GetString();

                if (_ctrl.ValidateSessionID(UID, SESID))
                {
                    string Item = _ctrl.GetUserData(IDtargetUser);
                    return Ok(Item);
                }
                else
                {
                    Console.WriteLine("SessionError: " + UID + "  |  " + DateTime.Now);
                    return StatusCode(401, "Unauthorized");
                }
            /*}catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }*/
            
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("api/items/AddTicket")]
        public IActionResult AddTicket([Microsoft.AspNetCore.Mvc.FromBody] JsonElement Payload)
        {
            try
            {
                int UID = Convert.ToInt32(Payload.GetProperty("UID").GetString());
                string SESID = Payload.GetProperty("SESID").GetString();

                if (_ctrl.ValidateSessionID(UID, SESID))
                {
                    _ctrl.AddTicket(Payload.GetProperty("Ticket"), UID);
                    return Ok();
                }
                else
                {
                    Console.WriteLine("SessionError: " + UID + "  |  " + DateTime.Now);
                    return StatusCode(401, "Unauthorized");
                }
            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            
        }

        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("api/items/GetTickets")]
        public IActionResult GetTickets(string data)
        {
            /*try
            {*/
                byte[] bytes = Convert.FromBase64String(data);
                string jsonData = Encoding.UTF8.GetString(bytes);
                JsonDocument jsonDocument = JsonDocument.Parse(jsonData);
                JsonElement Payload = jsonDocument.RootElement;

                int UID = Convert.ToInt32(Payload.GetProperty("UID").GetString());
                string SESID = Payload.GetProperty("SESID").GetString();

                if (_ctrl.ValidateSessionID(UID, SESID))
                {
                    string Item = _ctrl.GetTickets(UID);
                    return Ok(Item);
                }
                else
                {
                    Console.WriteLine("SessionError: " + UID + "  |  " + DateTime.Now);
                    return StatusCode(401, "Unauthorized");
                }
            /*}catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }*/
            
        }
        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("api/items/GetArchivedTickets")]
        public IActionResult GetArchivedTickets(string data)
        {
            /*try
            {*/
                byte[] bytes = Convert.FromBase64String(data);
                string jsonData = Encoding.UTF8.GetString(bytes);
                JsonDocument jsonDocument = JsonDocument.Parse(jsonData);
                JsonElement Payload = jsonDocument.RootElement;

                int UID = Convert.ToInt32(Payload.GetProperty("UID").GetString());
                string SESID = Payload.GetProperty("SESID").GetString();

                if (_ctrl.ValidateSessionID(UID, SESID))
                {
                    string Item = _ctrl.GetArchivedTickets(UID);
                    return Ok(Item);
                }
                else
                {
                    Console.WriteLine("SessionError: " + UID + "  |  " + DateTime.Now);
                    return StatusCode(401, "Unauthorized");
                }
            /*}catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }*/

        }

        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("api/items/GetTicketByID")]
        public IActionResult GetTicketByID(string data)
        {
            /*try
            {*/
                byte[] bytes = Convert.FromBase64String(data);
                string jsonData = Encoding.UTF8.GetString(bytes);
                JsonDocument jsonDocument = JsonDocument.Parse(jsonData);
                JsonElement Payload = jsonDocument.RootElement;

                int UID = Convert.ToInt32(Payload.GetProperty("UID").GetString());
                string SESID = Payload.GetProperty("SESID").GetString();
                int TicketID = Payload.GetProperty("TicketID").GetInt32();

                if (_ctrl.ValidateSessionID(UID, SESID))
                {
                    _ctrl.CheckTicketStatusNew(TicketID, UID);
                    string Item = _ctrl.GetTicketByID(TicketID);
                    return Ok(Item);
                }
                else
                {
                    Console.WriteLine("SessionError: " + UID + "  |  " + DateTime.Now);
                    return StatusCode(401, "Unauthorized");
                }
            /*}
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }*/

        }

        [Microsoft.AspNetCore.Mvc.HttpDelete]
        [Microsoft.AspNetCore.Mvc.Route("api/items/DeleteTicket")]
        public IActionResult DeleteTicket([Microsoft.AspNetCore.Mvc.FromBody] JsonElement Payload)
        {
            try
            {
                int UID = Convert.ToInt32(Payload.GetProperty("UID").GetString());
                string SESID = Payload.GetProperty("SESID").GetString();
                int TicketID = Payload.GetProperty("TicketID").GetInt32();
                if (_ctrl.ValidateSessionID(UID, SESID))
                {
                    _ctrl.DeleteTicket(TicketID);
                    return Ok();
                }
                else
                {
                    Console.WriteLine("SessionError: " + UID + "  |  " + DateTime.Now);
                    return StatusCode(401, "Unauthorized");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [Microsoft.AspNetCore.Mvc.HttpPut]
        [Microsoft.AspNetCore.Mvc.Route("api/items/ArchiveTicket")]
        public IActionResult ArchiveTicket([Microsoft.AspNetCore.Mvc.FromBody] JsonElement Payload)
        {
            try
            {
                int UID = Convert.ToInt32(Payload.GetProperty("UID").GetString());
                string SESID = Payload.GetProperty("SESID").GetString();
                int TicketID = Payload.GetProperty("TicketID").GetInt32();
                if (_ctrl.ValidateSessionID(UID, SESID))
                {
                    _ctrl.ArchiveTicket(TicketID);
                    return Ok();
                }
                else
                {
                    Console.WriteLine("SessionError: " + UID + "  |  " + DateTime.Now);
                    return StatusCode(401, "Unauthorized");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [Microsoft.AspNetCore.Mvc.HttpPut]
        [Microsoft.AspNetCore.Mvc.Route("api/items/UnarchiveTicket")]
        public IActionResult UnarchiveTicket([Microsoft.AspNetCore.Mvc.FromBody] JsonElement Payload)
        {
            try
            {
                int UID = Convert.ToInt32(Payload.GetProperty("UID").GetString());
                string SESID = Payload.GetProperty("SESID").GetString();
                int TicketID = Payload.GetProperty("TicketID").GetInt32();
                if (_ctrl.ValidateSessionID(UID, SESID))
                {
                    _ctrl.UnarchiveTicket(TicketID);
                    return Ok();
                }
                else
                {
                    Console.WriteLine("SessionError: " + UID + "  |  " + DateTime.Now);
                    return StatusCode(401, "Unauthorized");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("api/items/GetComments")]
        public IActionResult GetComments(string data)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(data);
                string jsonData = Encoding.UTF8.GetString(bytes);
                JsonDocument jsonDocument = JsonDocument.Parse(jsonData);
                JsonElement Payload = jsonDocument.RootElement;

                int UID = Convert.ToInt32(Payload.GetProperty("UID").GetString());
                string SESID = Payload.GetProperty("SESID").GetString();
                int TicketID = Payload.GetProperty("TicketID").GetInt32();
                bool IsAdminOnly = Payload.GetProperty("IsAdminOnly").GetBoolean();

                if (_ctrl.ValidateSessionID(UID, SESID))
                {
                    string Item =  _ctrl.GetComments(TicketID, IsAdminOnly);
                    return Ok(Item);
                }
                else
                {
                    Console.WriteLine("SessionError: " + UID + "  |  " + DateTime.Now);
                    return StatusCode(401, "Unauthorized");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("api/items/PostComment")]
        public IActionResult PostComment([Microsoft.AspNetCore.Mvc.FromBody] JsonElement Payload)
        {
            try
            {
                int UID = Convert.ToInt32(Payload.GetProperty("UID").GetString());
                string SESID = Payload.GetProperty("SESID").GetString();

                if (_ctrl.ValidateSessionID(UID, SESID))
                {
                    _ctrl.PostComment(Payload.GetProperty("CommentData"), UID);
                    return Ok();
                }
                else
                {
                    Console.WriteLine("SessionError: " + UID + "  |  " + DateTime.Now);
                    return StatusCode(401, "Unauthorized");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("api/items/GetPriors")]
        public IActionResult GetPriors(string data)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(data);
                string jsonData = Encoding.UTF8.GetString(bytes);
                JsonDocument jsonDocument = JsonDocument.Parse(jsonData);
                JsonElement Payload = jsonDocument.RootElement;

                int UID = Convert.ToInt32(Payload.GetProperty("UID").GetString());
                string SESID = Payload.GetProperty("SESID").GetString();

                if (_ctrl.ValidateSessionID(UID, SESID))
                {
                    string Item = _ctrl.GetPriors();
                    return Ok(Item);
                }
                else
                {
                    Console.WriteLine("SessionError: " + UID + "  |  " + DateTime.Now);
                    return StatusCode(401, "Unauthorized");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("api/items/GetCats")]
        public IActionResult GetCats(string data)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(data);
                string jsonData = Encoding.UTF8.GetString(bytes);
                JsonDocument jsonDocument = JsonDocument.Parse(jsonData);
                JsonElement Payload = jsonDocument.RootElement;

                int UID = Convert.ToInt32(Payload.GetProperty("UID").GetString());
                string SESID = Payload.GetProperty("SESID").GetString();

                if (_ctrl.ValidateSessionID(UID, SESID))
                {
                    string Item = _ctrl.GetCats(UID);
                    return Ok(Item);
                }
                else
                {
                    Console.WriteLine("SessionError: " + UID + "  |  " + DateTime.Now);
                    return StatusCode(401, "Unauthorized");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("api/items/SignUp")]
        public IActionResult SignUp([Microsoft.AspNetCore.Mvc.FromBody] JsonElement jsonPayload)
        {
            try
            {
                if (!_ctrl.CheckIfUserExists(jsonPayload.GetProperty("Email").ToString()))
                {
                        

                    int CID = _ctrl.CheckDomain(jsonPayload.GetProperty("Email").ToString());
                    Console.WriteLine(CID);
                    if (CID == 0) { return StatusCode(404, "Non existing Domain"); }
                    _ctrl.CreateUser(jsonPayload, CID, false);
                    return Ok();
                }
                else
                {
                    return Conflict("User already exists");
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("api/items/GetUsers")]
        public IActionResult GetUsers(string data)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(data);
                string jsonData = Encoding.UTF8.GetString(bytes);
                JsonDocument jsonDocument = JsonDocument.Parse(jsonData);
                JsonElement Payload = jsonDocument.RootElement;

                int UID = Convert.ToInt32(Payload.GetProperty("UID").GetString());
                string SESID = Payload.GetProperty("SESID").GetString();

                if (_ctrl.ValidateSessionID(UID, SESID))
                {
                    string Item = _ctrl.GetUsers(UID);
                    return Ok(Item);
                }
                else
                {
                    Console.WriteLine("SessionError: " + UID + "  |  " + DateTime.Now);
                    return StatusCode(401, "Unauthorized");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("api/items/AllowUser")]
        public IActionResult VerifyUser([Microsoft.AspNetCore.Mvc.FromBody] JsonElement Payload)
        {
            try
            {
                int UID = Convert.ToInt32(Payload.GetProperty("UID").GetString());
                string SESID = Payload.GetProperty("SESID").GetString();
                int IDtargetUser = Payload.GetProperty("IDtargetUser").GetInt32();

                if (_ctrl.ValidateSessionID(UID, SESID) && _ctrl.GetUserPermission(UID) != "Normal")
                {
                    _ctrl.VerifyUser(IDtargetUser);
                    return Ok();
                }
                else
                {
                    Console.WriteLine("SessionError: " + UID + "  |  " + DateTime.Now);
                    return StatusCode(401, "Unauthorized");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("api/items/GetStatuses")]
        public IActionResult GetStat(string data)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(data);
                string jsonData = Encoding.UTF8.GetString(bytes);
                JsonDocument jsonDocument = JsonDocument.Parse(jsonData);
                JsonElement Payload = jsonDocument.RootElement;

                int UID = Convert.ToInt32(Payload.GetProperty("UID").GetString());
                string SESID = Payload.GetProperty("SESID").GetString();

                if (_ctrl.ValidateSessionID(UID, SESID))
                {
                    string Item = _ctrl.GetStatuses();
                    return Ok(Item);
                }
                else
                {
                    Console.WriteLine("SessionError: " + UID + "  |  " + DateTime.Now);
                    return StatusCode(401, "Unauthorized");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [Microsoft.AspNetCore.Mvc.HttpDelete]
        [Microsoft.AspNetCore.Mvc.Route("api/items/DeleteUser")]
        public IActionResult DeleteUser([Microsoft.AspNetCore.Mvc.FromBody] JsonElement Payload)
        {
            try
            {
                int UID = Convert.ToInt32(Payload.GetProperty("UID").GetString());
                string SESID = Payload.GetProperty("SESID").GetString();
                int IDtargetUser = Payload.GetProperty("IDtargetUser").GetInt32();

                if (_ctrl.ValidateSessionID(UID, SESID) && _ctrl.GetUserPermission(UID) != "Normal")
                {
                    _ctrl.DeleteUser(IDtargetUser);
                    return Ok();
                }
                else
                {
                    Console.WriteLine("SessionError: " + UID + "  |  " + DateTime.Now);
                    return StatusCode(401, "Unauthorized");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("api/items/GetDashStats")]
        public IActionResult GetDashStatistics(string data)
        {
            /*try
            {*/
                byte[] bytes = Convert.FromBase64String(data);
                string jsonData = Encoding.UTF8.GetString(bytes);
                JsonDocument jsonDocument = JsonDocument.Parse(jsonData);
                JsonElement Payload = jsonDocument.RootElement;

                int UID = Convert.ToInt32(Payload.GetProperty("UID").GetString());
                string SESID = Payload.GetProperty("SESID").GetString();

                if (_ctrl.ValidateSessionID(UID, SESID))
                {
                    string Item = _ctrl.GetDashStats(UID);
                    return Ok(Item);
                }
                else
                {
                    Console.WriteLine("SessionError: " + UID + "  |  " + DateTime.Now);
                    return StatusCode(401, "Unauthorized");
                }
            /*}
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }*/

        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("api/items/CloseTicket")]
        public IActionResult CloseTicket([Microsoft.AspNetCore.Mvc.FromBody] JsonElement Payload)
        {

            try
            {
                int UID = Convert.ToInt32(Payload.GetProperty("UID").GetString());
                string SESID = Payload.GetProperty("SESID").GetString();

                if (_ctrl.ValidateSessionID(UID, SESID) && _ctrl.GetUserPermission(UID) != "Normal")
                {
                    _ctrl.CloseTicket(Payload, UID);
                    return Ok();
                }
                else
                {
                    Console.WriteLine("SessionError: " + UID + "  |  " + DateTime.Now);
                    return StatusCode(401, "Unauthorized");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("api/items/PromoteUser")]
        public IActionResult PromoteUser([Microsoft.AspNetCore.Mvc.FromBody] JsonElement Payload)
        {
            try
            {
                int UID = Convert.ToInt32(Payload.GetProperty("UID").GetString());
                string SESID = Payload.GetProperty("SESID").GetString();
                int IDtargetUser = Payload.GetProperty("IDtargetUser").GetInt32();
                string Goal = Payload.GetProperty("Goal").GetString();

                if (_ctrl.ValidateSessionID(UID, SESID) && _ctrl.GetUserPermission(UID) != "Normal")
                {
                    _ctrl.PromoteUser(IDtargetUser, Goal, UID);
                    return Ok();
                }
                else
                {
                    Console.WriteLine("SessionError: " + UID + "  |  " + DateTime.Now);
                    return StatusCode(401, "Unauthorized");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("api/items/AssignUser")]
        public IActionResult AssignUsers([Microsoft.AspNetCore.Mvc.FromBody] JsonElement Payload)
        {
            try
            {
                int UID = Convert.ToInt32(Payload.GetProperty("UID").GetString());
                string SESID = Payload.GetProperty("SESID").GetString();
                int TID = Payload.GetProperty("TicketID").GetInt32();

                if (_ctrl.ValidateSessionID(UID, SESID) && _ctrl.GetUserPermission(UID) != "Normal")
                {
                    _ctrl.AssignUsers(UID, TID);
                    return Ok();
                }
                else
                {
                    Console.WriteLine("SessionError: " + UID + "  |  " + DateTime.Now);
                    return StatusCode(401, "Unauthorized");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("api/items/GetAssignedUsers")]
        public IActionResult GetAssignedUsers(string data)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(data);
                string jsonData = Encoding.UTF8.GetString(bytes);
                JsonDocument jsonDocument = JsonDocument.Parse(jsonData);
                JsonElement Payload = jsonDocument.RootElement;

                int UID = Convert.ToInt32(Payload.GetProperty("UID").GetString());
                string SESID = Payload.GetProperty("SESID").GetString();
                int TID = Payload.GetProperty("TicketID").GetInt32();

                if (_ctrl.ValidateSessionID(UID, SESID))
                {
                    string Item = _ctrl.GetAssignedUsers(TID);
                    return Ok(Item);
                }
                else
                {
                    Console.WriteLine("SessionError: " + UID + "  |  " + DateTime.Now);
                    return StatusCode(401, "Unauthorized");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [Microsoft.AspNetCore.Mvc.HttpDelete]
        [Microsoft.AspNetCore.Mvc.Route("api/items/UnassignUser")]
        public IActionResult UnassignUser([Microsoft.AspNetCore.Mvc.FromBody] JsonElement Payload)
        {
            try
            {
                int UID = Convert.ToInt32(Payload.GetProperty("UID").GetString());
                string SESID = Payload.GetProperty("SESID").GetString();
                int TID = Payload.GetProperty("TicketID").GetInt32();
                int IDtargetUser = Convert.ToInt32(Payload.GetProperty("IDtargetUser").GetString());

                if (_ctrl.ValidateSessionID(UID, SESID) && _ctrl.GetUserPermission(UID) != "Normal")
                {
                    _ctrl.UnassignUser(IDtargetUser, TID);
                    return Ok();
                }
                else
                {
                    Console.WriteLine("SessionError: " + UID + "  |  " + DateTime.Now);
                    return StatusCode(401, "Unauthorized");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("api/items/ChangeStatus")]
        public IActionResult ChangeStatus([Microsoft.AspNetCore.Mvc.FromBody] JsonElement Payload)
        {
            try
            {
                int UID = Convert.ToInt32(Payload.GetProperty("UID").GetString());
                string SESID = Payload.GetProperty("SESID").GetString();
                int TID = Payload.GetProperty("TicketID").GetInt32();
                string Status = Payload.GetProperty("Status").GetString();

                if (_ctrl.ValidateSessionID(UID, SESID) && _ctrl.GetUserPermission(UID) != "Normal")
                {
                    _ctrl.ChangeStatus(Status, TID);
                    return Ok();
                }
                else
                {
                    Console.WriteLine("SessionError: " + UID + "  |  " + DateTime.Now);
                    return StatusCode(401, "Unauthorized");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("api/items/UpdateTicket")]
        public IActionResult UpdateTicket([Microsoft.AspNetCore.Mvc.FromBody] JsonElement Payload)
        {

            /*try
            {*/
                int UID = Convert.ToInt32(Payload.GetProperty("UID").GetString());
                string SESID = Payload.GetProperty("SESID").GetString();

                if (_ctrl.ValidateSessionID(UID, SESID))
                {
                    _ctrl.UpdateTicket(Payload.GetProperty("Ticket"));
                    return Ok();
                }
                else
                {
                    Console.WriteLine("SessionError: " + UID + "  |  " + DateTime.Now);
                    return StatusCode(401, "Unauthorized");
                }
            /*}
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }*/
        }

        [Microsoft.AspNetCore.Mvc.HttpDelete]
        [Microsoft.AspNetCore.Mvc.Route("api/items/DeleteCategory")]
        public IActionResult DeleteCategory([Microsoft.AspNetCore.Mvc.FromBody] JsonElement Payload)
        {
            /*try
            {*/
                int UID = Convert.ToInt32(Payload.GetProperty("UID").GetString());
                string SESID = Payload.GetProperty("SESID").GetString();
                string Category = Payload.GetProperty("Category").GetString();

                if (_ctrl.ValidateSessionID(UID, SESID) && _ctrl.GetUserPermission(UID) != "Normal")
                {
                    _ctrl.DeleteCategory(Category, UID);
                    return Ok();
                }
                else
                {
                    Console.WriteLine("SessionError: " + UID + "  |  " + DateTime.Now);
                    return StatusCode(401, "Unauthorized");
                }
            /*}
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }*/

        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("api/items/AddCategory")]
        public IActionResult AddCategory([Microsoft.AspNetCore.Mvc.FromBody] JsonElement Payload)
        {
            try
            {
                int UID = Convert.ToInt32(Payload.GetProperty("UID").GetString());
                string SESID = Payload.GetProperty("SESID").GetString();
                string Category = Payload.GetProperty("Category").GetString();

                if (_ctrl.ValidateSessionID(UID, SESID) && _ctrl.GetUserPermission(UID) != "Normal")
                {
                    _ctrl.AddCategory(Category, UID);
                    return Ok();
                }
                else
                {
                    Console.WriteLine("SessionError: " + UID + "  |  " + DateTime.Now);
                    return StatusCode(401, "Unauthorized");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("api/items/Register")]
        public IActionResult Register([Microsoft.AspNetCore.Mvc.FromBody] JsonElement jsonPayload)
        {
            try
            {
                Console.WriteLine(_ctrl.CheckDomain(jsonPayload.GetProperty("Domain").ToString()));
                if (_ctrl.CheckDomain(jsonPayload.GetProperty("Domain").ToString()) == 0)
                {
                    Console.WriteLine(jsonPayload);
                    _ctrl.Register(jsonPayload);

                    return Ok();
                }
                else
                {
                    return Conflict("Company already exists");
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [Microsoft.AspNetCore.Mvc.HttpDelete]
        [Microsoft.AspNetCore.Mvc.Route("api/items/PermDeleteTickets")]
        public IActionResult PermDeleteTickets([Microsoft.AspNetCore.Mvc.FromBody] JsonElement Payload)
        {
           try
            {
                int UID = Convert.ToInt32(Payload.GetProperty("UID").GetString());
                string SESID = Payload.GetProperty("SESID").GetString();

                if (_ctrl.ValidateSessionID(UID, SESID) && _ctrl.GetUserPermission(UID) != "Normal")
                {
                    _ctrl.PermDeleteTickets(UID);
                    return Ok();
                }
                else
                {
                    Console.WriteLine("SessionError: " + UID + "  |  " + DateTime.Now);
                    return StatusCode(401, "Unauthorized");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpDelete]
        [Microsoft.AspNetCore.Mvc.Route("api/items/PermDeleteUsers")]
        public IActionResult PermDeleteUsers([Microsoft.AspNetCore.Mvc.FromBody] JsonElement Payload)
        {
            try
            {
                int UID = Convert.ToInt32(Payload.GetProperty("UID").GetString());
                string SESID = Payload.GetProperty("SESID").GetString();

                if (_ctrl.ValidateSessionID(UID, SESID) && _ctrl.GetUserPermission(UID) != "Normal")
                {
                    _ctrl.PermDeleteUsers(UID);
                    return Ok();
                }
                else
                {
                    Console.WriteLine("SessionError: " + UID + "  |  " + DateTime.Now);
                    return StatusCode(401, "Unauthorized");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}

    

