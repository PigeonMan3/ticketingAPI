using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Business;

namespace API.Persistence
{
    internal class MapperComments
    {
        string _conString;
        string _cString;
        string _sql;

        public MapperComments()
        {
            //_conString = "server=localhost;userid=root;database=ticketing;port=3306;password=1234";
            _conString = "Server=sqlserverkobe.mysql.database.azure.com;UserID=Kobe;Password=TicketingAzure!;Database=ticketing;";
        }

        public List<Comment> GetComments(int TicketID, bool IsAdminOnly) //completed
        {
            List<Comment> list = new List<Comment>();
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "SELECT Comment_id, TimeOfComment, CommentContent, users.Username, tickets_Ticket_id, AdminOnly, users_User_id FROM ticketing.comments inner join users on User_id = users_User_id where tickets_Ticket_id = @TID and AdminOnly = @IAO";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@TID", TicketID));
            cmd.Parameters.Add(new MySqlParameter("@IAO", IsAdminOnly));
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Comment c = new Comment(Convert.ToInt32(rdr["Comment_id"]), Convert.ToDateTime(rdr["TimeOfComment"]), rdr["CommentContent"].ToString(), Convert.ToInt32(rdr["users_User_id"]), rdr["Username"].ToString(), Convert.ToInt32(rdr["tickets_Ticket_id"]), Convert.ToBoolean(rdr["AdminOnly"]));
                list.Add(c);
            }
            rdr.Close();
            con.Close();

            return list;
        }

        public void AddComment(Comment c)
        {
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "insert into comments (TimeOfComment, CommentContent, users_User_id, tickets_Ticket_id, AdminOnly) values (@TOC, @CONTENT, @UID, @TID, @IAO)";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@TOC", c.Created));
            cmd.Parameters.Add(new MySqlParameter("@CONTENT", c.Content));
            cmd.Parameters.Add(new MySqlParameter("@UID", c.UID));
            cmd.Parameters.Add(new MySqlParameter("@TID", c.TID));
            cmd.Parameters.Add(new MySqlParameter("@IAO", c.AdminOnly));
            cmd.ExecuteNonQuery();
            con.Close();
        }
    }
}
