using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Business;

namespace API.Persistence
{
    internal class MapperTickets
    {
        string _conString;
        string _cString;
        string _sql;

        public MapperTickets()
        {
            //_conString = "server=localhost;userid=root;database=ticketing;port=3306;password=1234";
            _conString = "Server=sqlserverkobe.mysql.database.azure.com;UserID=Kobe;Password=TicketingAzure!;Database=ticketing;";
        }




        ////////////////////////////////////////////////////////////////////////////////////////////////////////CRUD/////////////////////////////////////////////////////////////////////////////////////
        public void AddTicket(Ticket ticket) //complete
        {
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "insert into tickets (Title, Description, Created_at, Deadline, categories_Category_id, Location, Archived, ByUser, priority_PriorityID, status_Status_ID) values (@TITLE, @DESCRIPTION, @CREATED, @DEADLINE, (select Category_id from categories where CategoryName=@CATEGORY), @LOCATION, @ARCHIVED, @ID, (select PriorityID from priority where PriorityName=@PRIORITY), (select Status_id from status where StatusName=@STATUS))";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@TITLE", ticket.Title));
            cmd.Parameters.Add(new MySqlParameter("@DESCRIPTION", ticket.Description));
            cmd.Parameters.Add(new MySqlParameter("@PRIORITY", ticket.Priority));
            cmd.Parameters.Add(new MySqlParameter("@STATUS", ticket.Status));
            cmd.Parameters.Add(new MySqlParameter("@CREATED", ticket.Created));
            cmd.Parameters.Add(new MySqlParameter("@DEADLINE", ticket.Deadline));
            cmd.Parameters.Add(new MySqlParameter("@CATEGORY", ticket.Category));
            cmd.Parameters.Add(new MySqlParameter("@LOCATION", ticket.Location));
            cmd.Parameters.Add(new MySqlParameter("@ARCHIVED", ticket.Archived));
            cmd.Parameters.Add(new MySqlParameter("@ID", ticket.ByUser));
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public List<Ticket> GetAllTickets(int UserID) //completed
        {
            List<Ticket> list = new List<Ticket>();
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "SELECT Description, Ticket_id, Title, Created_at, Deadline, CategoryName, Location, ByUser, Username, PriorityName, StatusName FROM ticketing.tickets inner join categories on categories_Category_id = Category_id inner join users on ByUser = User_id inner join priority on priority_PriorityID = PriorityID inner join ticketing.status on status_Status_id = Status_id where company_CompanyID = (select company_CompanyID from users where User_id = @ID) AND tickets.Archived = 0";
            cmd = new MySqlCommand(_sql, con);
            Console.WriteLine("UID: " +  UserID);
            cmd.Parameters.Add(new MySqlParameter("@ID", UserID));
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                DateTime? deadline = null;
                if (rdr["Deadline"] != DBNull.Value)
                {
                    deadline = Convert.ToDateTime(rdr["Deadline"]);
                }
                Ticket t = new Ticket(Convert.ToInt32(rdr["Ticket_id"]), rdr["title"].ToString(), rdr["Description"].ToString(), rdr["priorityName"].ToString(), rdr["statusName"].ToString(), Convert.ToDateTime(rdr["Created_at"]), deadline, rdr["categoryName"].ToString(), rdr["location"].ToString(), Convert.ToInt16(rdr["byUser"]), rdr["Username"].ToString());
                list.Add(t);
            }
            rdr.Close();
            con.Close();

            return list;
        }

        public List<Ticket> GetArchivedTickets(int UserID) //completed
        {
            List<Ticket> list = new List<Ticket>();
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "SELECT Description, Ticket_id, Title, Created_at, Deadline, CategoryName, Location, ByUser, Username, PriorityName, StatusName FROM ticketing.tickets inner join categories on categories_Category_id = Category_id inner join users on ByUser = User_id inner join priority on priority_PriorityID = PriorityID inner join ticketing.status on status_Status_id = Status_id where company_CompanyID = (select company_CompanyID from users where User_id = @ID) AND tickets.Archived = 1";
            cmd = new MySqlCommand(_sql, con);
            Console.WriteLine("UID: " + UserID);
            cmd.Parameters.Add(new MySqlParameter("@ID", UserID));
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                DateTime? deadline = null;
                if (rdr["Deadline"] != DBNull.Value)
                {
                    deadline = Convert.ToDateTime(rdr["Deadline"]);
                }
                Ticket t = new Ticket(Convert.ToInt32(rdr["Ticket_id"]), rdr["title"].ToString(), rdr["Description"].ToString(), rdr["priorityName"].ToString(), rdr["statusName"].ToString(), Convert.ToDateTime(rdr["Created_at"]), deadline, rdr["categoryName"].ToString(), rdr["location"].ToString(), Convert.ToInt16(rdr["byUser"]), rdr["Username"].ToString());
                list.Add(t);
            }
            rdr.Close();
            con.Close();

            return list;
        }

        public List<Ticket> GetOwnTickets(int UserID) //complete
        {
            List<Ticket> list = new List<Ticket>();
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "SELECT Description, Ticket_id, Title, Created_at, Deadline, CategoryName, Location, ByUser, Username, PriorityName, StatusName FROM ticketing.tickets inner join categories on categories_Category_id = Category_id inner join users on ByUser = User_id inner join priority on priority_PriorityID = PriorityID inner join ticketing.status on status_Status_id = Status_id where ByUser = @ID";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@ID", UserID));
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                DateTime? deadline = null;
                if (rdr["Deadline"] != DBNull.Value)
                {
                    deadline = Convert.ToDateTime(rdr["Deadline"]);
                }
                Ticket t = new Ticket(Convert.ToInt32(rdr["Ticket_id"]), rdr["title"].ToString(), rdr["Description"].ToString(), rdr["priorityName"].ToString(), rdr["statusName"].ToString(), Convert.ToDateTime(rdr["Created_at"]), deadline, rdr["categoryName"].ToString(), rdr["location"].ToString(), Convert.ToInt16(rdr["byUser"]), rdr["Username"].ToString());
                list.Add(t);
            }
            rdr.Close();
            con.Close();
            return list;

        }

        public List<Ticket> GetTicketById(int ID)//completed
        {
            List<Ticket> list = new List<Ticket>();
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "SELECT Ticket_id, Title, Description, Created_at, Deadline, Resolved_notes, Location, tickets.Archived, CategoryName, PriorityName, StatusName, ByUser, users.Username FROM ticketing.tickets  inner join categories on categories_Category_id = Category_id inner join users on ByUser = User_id inner join priority on priority_PriorityID = PriorityID inner join ticketing.status on status_Status_id = Status_id where Ticket_id = @ID";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@ID", ID));
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                DateTime? deadline = null;
                if (rdr["Deadline"] != DBNull.Value)
                {
                    deadline = Convert.ToDateTime(rdr["Deadline"]);
                }
                Ticket t = new Ticket(Convert.ToInt32(rdr["Ticket_id"]), rdr["title"].ToString(), rdr["description"].ToString(), rdr["priorityName"].ToString(), rdr["statusName"].ToString(), Convert.ToDateTime(rdr["Created_at"]), deadline, rdr["Resolved_notes"].ToString(), rdr["categoryName"].ToString(), rdr["location"].ToString(), Convert.ToBoolean(rdr["Archived"]), Convert.ToInt16(rdr["byUser"]), rdr["Username"].ToString());
                list.Add(t);
            }
            rdr.Close();
            con.Close();

            return list;
        }

        public void ArchiveTicket(int TicketID)
        {
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "UPDATE tickets SET Archived = 1 WHERE ticket_id = @ID";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@ID", TicketID));
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void UnarchiveTicket(int TicketID)
        {
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "UPDATE tickets SET Archived = 0 WHERE ticket_id = @ID";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@ID", TicketID));
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void DeleteTicket(int TicketID)
        {
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "DELETE tickets FROM tickets WHERE Ticket_id = @ID";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@ID", TicketID));
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void PermanentlyDelete(int UID)
        {
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "DELETE tickets FROM tickets INNER JOIN users ON tickets.ByUser = users.User_id WHERE users.company_CompanyID = (SELECT company_CompanyID FROM users where User_id = @UID) and tickets.Archived = 1;";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@UID", UID));
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void UpdateTicket(Ticket t, int TID)
        {
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "UPDATE tickets SET Title = @TITLE, Description = @DESC, Deadline = @DEADLINE, categories_Category_id = (select Category_id from categories where CategoryName=@CATEGORY), priority_PriorityID = (select PriorityID from priority where PriorityName=@PRIORITY)  WHERE Ticket_id = @TID";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@TID", TID));
            cmd.Parameters.Add(new MySqlParameter("@TITLE", t.Title));
            cmd.Parameters.Add(new MySqlParameter("@DESC", t.Description));
            cmd.Parameters.Add(new MySqlParameter("@DEADLINE", t.Deadline));
            cmd.Parameters.Add(new MySqlParameter("@CATEGORY", t.Category));
            cmd.Parameters.Add(new MySqlParameter("@PRIORITY", t.Priority));

            cmd.ExecuteNonQuery();
            con.Close();
        }

        ///////////////////////////////////////////////////////////////////////////Others////////////////////////

        public void CloseTicket(int TID, int UID, string RN)
        {
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "UPDATE tickets SET Resolved_notes = @RN WHERE tickets.Ticket_id = @ID;";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@ID", TID));
            cmd.Parameters.Add(new MySqlParameter("@RN", RN));
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void AddResolver(int TID, int UID)
        {
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "UPDATE assignedtable SET Resolver = 1 WHERE tickets_Ticket_id = @TID AND users_User_id = @UID;";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@TID", TID));
            cmd.Parameters.Add(new MySqlParameter("@UID", UID));
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void AssignUser(int UID, int TID)
        {
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "INSERT INTO assignedtable (tickets_Ticket_id, users_User_id, Resolver) values (@TID, @UID, 0)";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@UID", UID));
            cmd.Parameters.Add(new MySqlParameter("@TID", TID));
            Console.WriteLine(UID + " " + TID);
            cmd.ExecuteNonQuery();
        }
        public void UnassignUser(int UID, int TID)
        {
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "DELETE FROM assignedtable WHERE users_User_id = @UID AND tickets_Ticket_id = @TID";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@UID", UID));
            cmd.Parameters.Add(new MySqlParameter("@TID", TID));
            cmd.ExecuteNonQuery();
        }

        public List<User> GetAssignedUsers(int TID)
        {
            List<User> list = new List<User>();
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "SELECT * FROM assignedtable inner join users on assignedtable.users_User_id = users.User_id WHERE tickets_Ticket_id = @TID";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@TID", TID));
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                User u = new User(Convert.ToInt32(rdr["User_id"]), rdr["Name"].ToString(), rdr["FirstName"].ToString(), rdr["Username"].ToString(), rdr["Email"].ToString(), rdr["Type"].ToString(), rdr["JobTitle"].ToString(), rdr["company_CompanyID"].ToString(), Convert.ToBoolean(rdr["Verified"]));
                list.Add(u);
            }
            rdr.Close();
            con.Close();

            return list;

        }
    }
}
