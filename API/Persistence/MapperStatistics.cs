using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Business;

namespace API.Persistence
{
    internal class MapperStatistics
    {
        string _conString;
        string _cString;
        string _sql;

        public MapperStatistics()
        {
            //_conString = "server=localhost;userid=root;database=ticketing;port=3306;password=1234";
            _conString = "Server=sqlserverkobe.mysql.database.azure.com;UserID=Kobe;Password=TicketingAzure!;Database=ticketing;";
        }

        public int GetUnresolvedTicketsStats(int UID)
        {
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "SELECT count(*) FROM tickets LEFT JOIN status ON tickets.status_Status_id = status.Status_id INNER JOIN users on tickets.ByUser = users.User_id WHERE status.statusName != 'Open' and status.statusName != 'New' and tickets.ByUser  and users.company_CompanyID = (select company_CompanyID from users where User_id = @UID)";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@UID", UID));
            try
            {
                int i = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                con.Close();
                return i;
            }
            catch
            {
                throw new Exception();
            }
        }
        public int GetNewTicketsStats(int UID)
        {
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "SELECT count(*) FROM tickets LEFT JOIN status ON tickets.status_Status_id = status.Status_id INNER JOIN users on tickets.ByUser = users.User_id WHERE status.statusName = 'New' and tickets.ByUser  and users.company_CompanyID = (select company_CompanyID from users where User_id = @UID)";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@UID", UID));
            try
            {
                int i = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                con.Close();
                return i;
            }
            catch
            {
                throw new Exception();
            }
        }

        public int GetCriticalTicketsStats(int UID)
        {
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "SELECT count(*) FROM tickets LEFT JOIN priority ON tickets.priority_PriorityID = priority.PriorityID INNER JOIN users on tickets.ByUser = users.User_id WHERE priority.PriorityName = 'Critical' and tickets.ByUser  and users.company_CompanyID = (select company_CompanyID from users where User_id = 63)";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@UID", UID));
            try
            {
                int i = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                con.Close();
                return i;
            }
            catch
            {
                throw new Exception();
            }
        }
        public string GetBestResolver(int UID)
        {
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "select u.Username from assignedtable a inner join users u on a.users_User_id = u.User_id where u.company_CompanyID = (select company_CompanyID from users where User_id = @UID) group by u.User_id, u.Username order by count(*) desc";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@UID", UID));
            if (cmd.ExecuteScalar() == null)
            {
                return "";
            }
            else
            {
                string i = cmd.ExecuteScalar().ToString();
                con.Close();
                return i;
            }
        }

        public int GetUpcomingDeadlines(int UID)
        {
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "SELECT count(Deadline) FROM tickets INNER JOIN users on tickets.ByUser = users.User_id WHERE users.company_CompanyID = (select company_CompanyID from users where User_id = @UID) and DATEDIFF(tickets.Deadline, CURDATE()) <= 5 and DATEDIFF(tickets.Deadline, CURDATE()) > 0";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@UID", UID));
            try
            {
                int i = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                con.Close();
                return i;
            }
            catch
            {
                throw new Exception();
            }
        }

        public Tuple<string, int> GetMostCommonTicketCategory(int UID)
        {
            Tuple<string, int> data = new Tuple<string, int>("NoTickets", 0);
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "SELECT categories.CategoryName, COUNT(tickets.categories_Category_id) AS ticket_count\r\nFROM tickets\r\nJOIN categories ON tickets.categories_Category_id = categories.Category_id\r\nJOIN users ON tickets.ByUser = users.User_id\r\nWHERE users.company_CompanyID = (SELECT company_CompanyID FROM users WHERE User_id = @UID) and tickets.Archived = 0\r\nGROUP BY categories.CategoryName\r\nORDER BY ticket_count DESC\r\nlimit 1;";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@UID", UID));
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                string categoryName = rdr.GetString(0);
                int ticketCount = rdr.GetInt32(1);

                data = new Tuple<string, int>(categoryName, ticketCount);
            }
            else
            {
                Console.WriteLine("No data found.");

            }
            rdr.Close();
            con.Close();
            return data;
        }

        public Dictionary<string, int> GetTicketsByCategory(int UID)
        {
            Dictionary<string, int> Tickets = new Dictionary<string, int>();
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "SELECT c.CategoryName, COUNT(t.Ticket_id) AS ticket_count FROM tickets t INNER JOIN categories c ON t.categories_Category_id = c.Category_id INNER JOIN users u ON t.ByUser = u.User_id WHERE u.company_CompanyID = (SELECT company_CompanyID FROM users WHERE User_id = @UID) and t.Archived = 0 GROUP BY c.CategoryName;";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@UID", UID));
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                string categoryName = rdr["CategoryName"].ToString();
                int ticketCount = Convert.ToInt32(rdr["ticket_count"]);
                Tickets.Add(categoryName, ticketCount);
            }
            rdr.Close();
            con.Close();

            return Tickets;

        }
        public Dictionary<string, int> GetTicketsByStatus(int UID)
        {
            Dictionary<string, int> TicketsByStatus = new Dictionary<string, int>();
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            string _sql = "SELECT s.StatusName, COUNT(t.Ticket_id) AS ticket_count FROM tickets t inner join status s on t.status_Status_id = s.Status_id JOIN users u ON t.ByUser = u.User_id WHERE u.company_CompanyID = (SELECT company_CompanyID FROM users WHERE User_id = @UID) group by s.StatusName;";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@UID", UID));
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                string status = rdr["StatusName"].ToString();
                int ticketCount = Convert.ToInt32(rdr["ticket_count"]);
                TicketsByStatus.Add(status, ticketCount);
            }
            rdr.Close();
            con.Close();

            return TicketsByStatus;

        }

        ///////////////////////////////////////////////////////////////////////////

        public int GetOwnUnresolvedTicketsStats(int UID)
        {
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "SELECT count(*) FROM tickets LEFT JOIN status ON tickets.status_Status_id = status.Status_id INNER JOIN users on tickets.ByUser = users.User_id WHERE status.statusName != 'Open' and status.statusName != 'New' and tickets.ByUser  and users.User_id = @UID";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@UID", UID));
            try
            {
                int i = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                con.Close();
                return i;
            }
            catch
            {
                throw new Exception();
            }
        }

        public Dictionary<string, int> GetOwnTicketsByCategory(int UID)
        {
            Dictionary<string, int> Tickets = new Dictionary<string, int>();
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "SELECT c.CategoryName, COUNT(t.Ticket_id) AS ticket_count FROM tickets t INNER JOIN categories c ON t.categories_Category_id = c.Category_id INNER JOIN users u ON t.ByUser = u.User_id WHERE u.User_id = @UID GROUP BY c.CategoryName;";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@UID", UID));
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                string categoryName = rdr["CategoryName"].ToString();
                int ticketCount = Convert.ToInt32(rdr["ticket_count"]);
                Tickets.Add(categoryName, ticketCount);
            }
            rdr.Close();
            con.Close();

            return Tickets;

        }

        public Dictionary<string, int> GetOwnTicketsByStatus(int UID)
        {
            Dictionary<string, int> TicketsByStatus = new Dictionary<string, int>();
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            string _sql = "SELECT s.StatusName, COUNT(t.Ticket_id) AS ticket_count FROM tickets t inner join status s on t.status_Status_id = s.Status_id JOIN users u ON t.ByUser = u.User_id WHERE u.User_id = @UID group by s.StatusName;";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@UID", UID));
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                string status = rdr["StatusName"].ToString();
                int ticketCount = Convert.ToInt32(rdr["ticket_count"]);
                TicketsByStatus.Add(status, ticketCount);
            }
            rdr.Close();
            con.Close();

            return TicketsByStatus;

        }
    }
}
