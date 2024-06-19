using API.Business;
using API.Persistence;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Crypto.Agreement.JPake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json; //using json serializer
using System.Threading.Tasks;

namespace API.Business
{
    public class BusinessController
    {
        private Persistence.PersistenceController _persController;

        public BusinessController()
        {
            _persController = new Persistence.PersistenceController();
        }

        public string ValidateCredentials(string email, string password)
        {
            int UID = _persController.GetUID(email); //get UID
            string Seskey = "";
            User u = new User(UID, _persController.GetPassword(UID), _persController.GetVerification(UID));//create object with UID, 
            string auth = u.authentication(password); //can the user be logged in or not ? // (correct password & verified acc)
            if (auth == "OK") { Seskey = CreateSession(UID); }
            Console.WriteLine("Created " + Seskey + "  |  " + DateTime.Now.ToString());
            var Data = new
            {
                Auth = auth,
                UID = UID,
                SESKEY = Seskey
            };

            return JsonSerializer.Serialize(Data);
        }
        public string CreateSession(int UserID) 
        {
            Session S = new Session();
            string SESKEY = S.GenerateSessionID();
            if (_persController.GetSession(UserID) == "") //if the user doesnt have an active session -> create new
            {
                _persController.AddSession(UserID, SESKEY);
                return SESKEY;
            }
            else
            {
                _persController.UpdateSession(UserID, SESKEY); //incase of an active session overwrite the key
                return SESKEY;
            }

        }
        public bool ValidateSessionID(int UserID, string skey) 
        {
            Console.WriteLine("received " + skey + "  |  " + DateTime.Now.ToString());
            Console.WriteLine("read DB" + _persController.GetSession(UserID) + "  |  " + DateTime.Now.ToString());
            User u = new User(UserID, _persController.GetSession(UserID)); //make object with actual key
            return u.validateSession(skey); //compare instance var against parameter
        }
        public void DeleteSessionID(int UserID)
        {
            _persController.DeleteSession(UserID);
        }
        public string GetUserPermission(int UserID)
        {
            string jsonString = JsonSerializer.Serialize(_persController.GetUserPerms(UserID));
            return jsonString;
        }
        public string GetUserData(int UserID)
        {
            string jsonString = JsonSerializer.Serialize(_persController.GetUserData(UserID));
            return jsonString;
        }
        public void AddTicket(JsonElement root, int UserID)
        {
            DateTime? deadline = null;
            Console.WriteLine(root);
            if (root.GetProperty("TicketData").GetProperty("Deadline").GetString() != null) //deadline = null when no deadline is assigned
            {
                deadline = root.GetProperty("TicketData").GetProperty("Deadline").GetDateTime();

            }
            Ticket t = new Ticket(root.GetProperty("TicketData").GetProperty("Title").GetString(), root.GetProperty("TicketData").GetProperty("Description").GetString(), root.GetProperty("TicketData").GetProperty("Priority").GetString(), root.GetProperty("TicketData").GetProperty("Status").GetString(), root.GetProperty("TicketData").GetProperty("Created").GetDateTime(), deadline, root.GetProperty("TicketData").GetProperty("Category").GetString(), root.GetProperty("TicketData").GetProperty("Removed").GetBoolean(), UserID, root.GetProperty("TicketData").GetProperty("Location").GetString());
            _persController.CreateTicket(t);
        }
        public string GetTickets(int UserID)
        {
            string jsonstring;
            if (_persController.GetUserPerms(UserID) == "Admin" || _persController.GetUserPerms(UserID) == "Master") //if user has administrator perms
            {
                Console.WriteLine("1");
                jsonstring = JsonSerializer.Serialize(_persController.GetAllTickets(UserID)); //admins can see all tickets within the company
            }
            else
            {
                Console.WriteLine("2");
                jsonstring = JsonSerializer.Serialize(_persController.GetOwnTickets(UserID)); //normal users can only see their own tickets
            }
            return jsonstring; //return json
        }
        public string GetArchivedTickets(int UserID)
        {
            string jsonstring;
            jsonstring = JsonSerializer.Serialize(_persController.GetArchivedTickets(UserID));          
            return jsonstring; //return json
        }
        public string GetTicketByID(int TicketID)
        {
            return JsonSerializer.Serialize(_persController.GetTicketByID(TicketID));
        }
        public void CheckTicketStatusNew(int ID, int UID) //WhenTicket gets opened --> change status to open instead of new
        {
            List<Ticket> ticket = _persController.GetTicketByID(ID);
            string status = ticket[0].Status;
            if (status == "New" && _persController.GetUserPerms(UID) != "Normal")
            {
                _persController.ChangeStatus("Open", ID);
            }
        }
        public void DeleteTicket(int TicketID)
        {
            _persController.DeleteAssignmentsOfTicket(TicketID);
            _persController.DeleteCommentsOfTicket(TicketID);
            _persController.DeleteTicket(TicketID);
        }

        public void ArchiveTicket(int TicketID)
        {
            _persController.ArchiveTicket(TicketID);
        }

        public void UnarchiveTicket(int TicketID)
        {
            _persController.UnarchiveTicket(TicketID);
        }

        public string GetComments(int TicketID, bool IsAdminOnly)
        {
            return JsonSerializer.Serialize(_persController.GetComments(TicketID, IsAdminOnly));
        }
        public void PostComment(JsonElement root, int UID)
        {
            Console.WriteLine(root);
            Comment c = new Comment(root.GetProperty("Commentdata").GetProperty("Created").GetDateTime(), root.GetProperty("Commentdata").GetProperty("Content").GetString(), UID, root.GetProperty("Commentdata").GetProperty("TID").GetInt32(), root.GetProperty("Commentdata").GetProperty("IsAdminOnly").GetBoolean());
            _persController.AddComment(c);
        }
        public string GetPriors()
        {
            return JsonSerializer.Serialize(_persController.GetPriors());
        }
        public string GetCats(int UID)
        {
            return JsonSerializer.Serialize(_persController.GetCats(UID));
        }
        public string GetStatuses()
        {
            return JsonSerializer.Serialize(_persController.GetStatuses());
        }
        public int CheckDomain(string Email)
        {
            string Domain = Email.ToString();
            Domain = Domain.Substring(Domain.IndexOf('@')); //extract domain from email

            return _persController.GetCompanyByDomainAndGetCID(Domain);
        }
        public void CreateUser(JsonElement root, int CID, bool Master)
        {
            string Type = "Normal";
            bool Verified = false;
            if (Master)
            {
                Type = "Master";
                Verified = true;
            }
            User user = new User(root.GetProperty("FirstName").ToString(), root.GetProperty("LastName").ToString());
            int suffix = 0;

            string Username = user.GenerateUsername(suffix);
            while (!_persController.CheckUsernameAvailability(Username)) //while username is not available, try again with higher suffix
            {
                suffix++;
                Username = user.GenerateUsername(suffix);
            }

            User u = new User(root.GetProperty("LastName").ToString(), root.GetProperty("FirstName").ToString(), Username, root.GetProperty("Email").ToString(), root.GetProperty("JobTitle").ToString(), CID, Type, Verified);
            _persController.CreateUser(u);
            string pass = root.GetProperty("Password").ToString();
            _persController.CreatePassword(pass, _persController.GetUID(root.GetProperty("Email").ToString()));
        }
        public bool CheckIfUserExists(string email)
        {
            if(_persController.GetUID(email) == 0) 
            {
                return false;
            }
            else{
                return true;
            }
           
           
            
        }
        public string GetUsers(int ID)
        {
            return JsonSerializer.Serialize(_persController.GetUsers(ID));
        }
        public void VerifyUser(int ID)
        {
            _persController.VerifyUser(ID);
        }
        public void DeleteUser(int ID)
        {
            _persController.DeletePassword(ID);
            _persController.DeleteSession(ID);
            _persController.DeleteUser(ID);
        }
        public string GetDashStats(int UID)
        {
            if (_persController.GetUserPerms(UID) == "Admin" || _persController.GetUserPerms(UID) == "Master") //stats for admin only
            {
                Dictionary<string, int> ticketCountsCategory = _persController.GetTicketsByCategory(UID); //doughnut data
                Dictionary<string, int> ticketCountsStatus = _persController.GetTicketsByStatus(UID); //doughnut data

                int newTickets = _persController.GetNewTicketsStats(UID); //small widgets
                int criticalTickets = _persController.GetCriticalTicketsStats(UID);
                int unresolvedTickets = _persController.GetUnresolvedTicketsStats(UID);

                Tuple<string, int> mostUsedCategory = _persController.GetMostCommonTicketCategory(UID);
                string MostUsedCategory = mostUsedCategory.Item1 + ": " + mostUsedCategory.Item2.ToString();

                string BestResolver = _persController.GetBestResolver(UID);
                int upcomingDeadlines = _persController.GetUpcomingDeadlines(UID);

                var data = new 
                {
                    CategoryStats = ticketCountsCategory,
                    StatusStats = ticketCountsStatus,
                    NewTicketsStats = newTickets,
                    CriticalTicketsStats = criticalTickets,
                    UnresolvedTicketsStats = unresolvedTickets,
                    MostUsedCategory = MostUsedCategory,
                    BestResolver = BestResolver,
                    UpcomingDeadlines = upcomingDeadlines
                };
                string jsonData = JsonSerializer.Serialize(data);
                return jsonData;
            }
            else
            {
                Dictionary<string, int> ticketCountsCategory = _persController.GetOwnTicketsByCategory(UID); //doughnut data
                Dictionary<string, int> ticketCountsStatus = _persController.GetOwnTicketsByStatus(UID); //doughnut data

                int unresolvedTickets = _persController.GetOwnUnresolvedTicketsStats(UID);
                var data = new
                {
                    CategoryStats = ticketCountsCategory,
                    StatusStats = ticketCountsStatus,
                    UnresolvedTicketsStats = unresolvedTickets,
                };
                string jsonData = JsonSerializer.Serialize(data);
                return jsonData;
            }
        }
        public void CloseTicket(JsonElement root, int UID)
        {
            Console.WriteLine("ID: " + root.ToString());
            _persController.ChangeStatus("Closed", root.GetProperty("TicketID").GetInt32());
            _persController.CloseTicket(root.GetProperty("TicketID").GetInt32(), UID, root.GetProperty("ResolvedNotes").GetString());
            _persController.AddResolver(root.GetProperty("TicketID").GetInt32(), UID);
        }
        public void PromoteUser(int ID, string Goal, int UID)
        {
            Console.WriteLine(Goal);
            if (Goal == "Master") //only 1 master per company --> demote previous master
            {
                _persController.ChangeUserType(UID, "Admin");
                _persController.ChangeUserType(ID, Goal);
            }
            else
            {
                _persController.ChangeUserType(ID, Goal);
            }
        }
        public void AssignUsers(int UID, int TID)
        {
            _persController.AssignUser(UID, TID);
        }
        public string GetAssignedUsers(int TID)
        {
            return JsonSerializer.Serialize(_persController.GetAssignedUsers(TID));
        }
        public void UnassignUser(int UID, int TID)
        {
            _persController.UnassignUser(UID, TID);
        }
        public void ChangeStatus(string Status, int TID)
        {
            _persController.ChangeStatus(Status, TID);
        }
        public void UpdateTicket(JsonElement root) //same as create, calling update method instead
        {
            DateTime? deadline = null;
            if (root.GetProperty("TicketData").GetProperty("Deadline").GetString() != null)
            {
                deadline = root.GetProperty("TicketData").GetProperty("Deadline").GetDateTime();

            }
            Ticket t = new Ticket(root.GetProperty("TicketData").GetProperty("Title").GetString(), root.GetProperty("TicketData").GetProperty("Description").GetString(), root.GetProperty("TicketData").GetProperty("Priority").GetString(), root.GetProperty("TicketData").GetProperty("Category").GetString(), root.GetProperty("TicketData").GetProperty("Location").GetString(), deadline);
            _persController.UpdateTicket(t, Convert.ToInt16(root.GetProperty("TicketData").GetProperty("TicketID").GetInt32()));
        }
        public void DeleteCategory(string Category, int UID)
        {
            _persController.DeleteCategory(Category, UID);
        }
        public void AddCategory(string Category, int UID)
        {
            _persController.AddCategory(Category); //add category to all categories
            _persController.AddCategoryToCompany(UID, Category); //assing category for company to use
        }

        public void Register(JsonElement Payload)
        {
            Company c = new Company(Payload.GetProperty("OrgName").GetString(), Payload.GetProperty("Domain").GetString(), Payload.GetProperty("Sector").GetString());
            _persController.AddCompany(c);
            int CID = _persController.GetCompanyByDomainAndGetCID(Payload.GetProperty("Domain").GetString());
            CreateUser(Payload, CID, true);
            AddCategory("Others", _persController.GetUID(Payload.GetProperty("Email").ToString()));
        }

        public void PermDeleteTickets(int UID)
        {
            _persController.PermDeleteT_DeleteAssignments(UID);
            _persController.PermDeleteT_DeleteComments(UID);
            _persController.PermanentlyDeleteTickets(UID);
        }


        public void PermDeleteUsers(int UID)
        {
            _persController.PermDeleteU_DeleteAssignments(UID);
            _persController.PermDeleteU_DeleteComments(UID);
            _persController.PermDeleteU_DeletePasswords(UID);
            _persController.PermDeleteU_DeleteTickets(UID);
            _persController.PermDeleteU_DeleteSessions(UID);
            _persController.PermanentlyDeleteUsers(UID);
        }
    }
}
