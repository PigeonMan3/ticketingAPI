using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Business;

namespace API.Persistence
{
    internal class MapperSessions
    {
        string _conString;
        string _cString;
        string _sql;

        public MapperSessions()
        {
            //_conString = "server=localhost;userid=root;database=ticketing;port=3306;password=1234";
            _conString = "Server=sqlserverkobe.mysql.database.azure.com;UserID=Kobe;Password=TicketingAzure!;Database=ticketing;";
        }

        public string GetSession(int UserID)
        {
            string key;
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "select sessions.key from sessions where users_User_id = @ID";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@ID", UserID));
            try
            {
                key = cmd.ExecuteScalar()?.ToString() ?? "";
                con.Close();
                return key;
            }
            catch (Exception ex)
            {
                return "";
            }

        }

        public void AddSession(string sessionID, int UserID) 
        {
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "insert into sessions (users_User_id, sessions.key) values (@ID, @KEY)";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@ID", UserID));
            cmd.Parameters.Add(new MySqlParameter("@KEY", sessionID));
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void DeleteSession(int UserID)
        {
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "delete from sessions where users_User_id = @ID";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@ID", UserID));
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void UpdateSession(int UserID, string Key)
        {
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "UPDATE sessions SET sessions.key = @KEY  WHERE users_User_id = @ID";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@ID", UserID));
            cmd.Parameters.Add(new MySqlParameter("@KEY", Key));
            cmd.ExecuteNonQuery();
            con.Close();
        }
    }
}
