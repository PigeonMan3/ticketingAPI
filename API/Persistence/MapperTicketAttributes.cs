using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Business;

namespace API.Persistence
{
    internal class MapperTicketAttributes
    {
        string _conString;
        string _cString;
        string _sql;

        public MapperTicketAttributes()
        {
            //_conString = "server=localhost;userid=root;database=ticketing;port=3306;password=1234";
            _conString = "Server=sqlserverkobe.mysql.database.azure.com;UserID=Kobe;Password=TicketingAzure!;Database=ticketing;";
        }

        public List<Priority> GetPriors()
        {
            List<Priority> list = new List<Priority>();
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "SELECT * FROM priority";
            cmd = new MySqlCommand(_sql, con);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Priority p = new Priority(Convert.ToInt16(rdr["PriorityID"]), rdr["PriorityName"].ToString());
                list.Add(p);
            }
            rdr.Close();
            con.Close();

            return list;
        }
        public List<Categories> GetCats(int UID)
        {
            List<Categories> list = new List<Categories>();
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "SELECT categories.category_id, categories.categoryName FROM categories inner JOIN categories_has_company ON categories.Category_id = categories_has_company.categories_Category_id WHERE categories_has_company.company_CompanyID = (SELECT company_CompanyID FROM users where User_id = @ID);";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@ID", UID));
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Categories c = new Categories(Convert.ToInt16(rdr["category_id"]), rdr["CategoryName"].ToString());
                list.Add(c);
            }
            rdr.Close();
            con.Close();

            return list;
        }
        public List<Status> GetStatuses()
        {
            List<Status> list = new List<Status>();
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "SELECT * FROM status";
            cmd = new MySqlCommand(_sql, con);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Status s = new Status(Convert.ToInt16(rdr["Status_id"]), rdr["StatusName"].ToString());
                list.Add(s);
            }
            rdr.Close();
            con.Close();

            return list;
        }


        public void ChangeStatus(string Status, int TID)
        {
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "UPDATE tickets SET status_Status_id = (SELECT Status_id from status where StatusName = @STATUS) WHERE tickets.Ticket_id = @TID;";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@TID", TID));
            cmd.Parameters.Add(new MySqlParameter("@STATUS", Status));
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void AddCategory(string Cat)
        {
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "INSERT INTO categories (CategoryName) SELECT @CAT FROM dual WHERE NOT EXISTS (SELECT 1    FROM categories    WHERE CategoryName = @CAT);";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@CAT", Cat));
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public void PermanentlyDelete_DeleteComments(int UID)
        {
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "DELETE comments FROM comments INNER JOIN tickets ON comments.tickets_Ticket_id = tickets.Ticket_id INNER JOIN users ON tickets.ByUser = users.User_id WHERE users.company_CompanyID = (SELECT company_CompanyID FROM users where User_id =@UID) and tickets.Archived = 1;";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@UID", UID));
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void PermanentlyDelete_DeleteAssignments(int UID)
        {
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "DELETE assignedtable FROM assignedtable INNER JOIN tickets ON assignedtable.tickets_Ticket_id = tickets.Ticket_id INNER JOIN users ON tickets.ByUser = users.User_id WHERE users.company_CompanyID = (SELECT company_CompanyID FROM users where User_id = @UID) and tickets.Archived = 1;";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@UID", UID));
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void DeleteAssignmentsOfTicket(int TicketID)
        {
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "DELETE assignedtable FROM assignedtable INNER JOIN tickets ON assignedtable.tickets_Ticket_id = tickets.Ticket_id WHERE tickets_Ticket_id = @TID";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@TID", TicketID));
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void DeleteCommentsOfTicket(int TicketID)
        {
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "DELETE comments FROM comments INNER JOIN tickets ON comments.tickets_Ticket_id = tickets.Ticket_id WHERE tickets_Ticket_id = @TID";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@TID", TicketID));
            cmd.ExecuteNonQuery();
            con.Close();
        }
    }
}
