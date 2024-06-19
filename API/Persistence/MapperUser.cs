using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Security;
using System.Security;
using System.Security.Permissions;
using MySql.Data;
using MySql.Data.MySqlClient;
using API.Business;

namespace API.Persistence
{
    internal class MapperUser
    {
        string _conString;
        string _cString;
        string _sql;

        public MapperUser()
        {
            //_conString = "server=localhost;userid=root;database=ticketing;port=3306;password=1234";
            _conString = "Server=sqlserverkobe.mysql.database.azure.com;UserID=Kobe;Password=TicketingAzure!;Database=ticketing;";
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////CRUD///////////////////////////////////////////////////////////////////////////////////////////////
        public List<User> GetUsers(int ID) 
        {
            List<User> list = new List<User>();
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "select * from users where (select company_CompanyID from users where User_id = @ID) = company_CompanyID and Removed = 0 ";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@ID", ID));

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
        public List<User> GetAdminUsers(int UID)
        {
            List<User> list = new List<User>();
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "SELECT * FROM users WHERE (Type = 'Admin' || Type = 'Master') AND (select company_CompanyID from users where User_id = @ID) = company_CompanyID AND Removed = 0 ";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@@ID", UID));
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
        public List<User> GetUserData(int UserID)
        {
            List<User> lijst = new List<User>();
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "SELECT User_id, Name, FirstName, Username, Email, Type, Jobtitle, company.companyname, Verified FROM ticketing.users inner join company on users.company_CompanyID = company.CompanyID where User_id = @ID";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@ID", UserID));
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                User user = new User(rdr.GetInt32("User_id"), rdr.GetString("Name"), rdr.GetString("FirstName"), rdr.GetString("Username"), rdr.GetString("Email"), rdr.GetString("Type"), (rdr.IsDBNull(rdr.GetOrdinal("JobTitle")) ? null : rdr.GetString("JobTitle")), rdr.GetString("companyname"), rdr.GetBoolean("Verified"));
                lijst.Add(user);

            }
            rdr.Close();
            con.Close();
            return lijst;
        }

        public void CreateUser(User u)
        {
            Console.WriteLine(u.ToString());
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "INSERT INTO users (users.Name, FirstName, Username, Email, Type, JobTitle, company_CompanyID, verified, Removed) VALUES (@NAME, @FNAME, @UNAME, @EMAIL, @TYPE, @JOB, @CID, @VERIFIED, false)";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@NAME", u.Name));
            cmd.Parameters.Add(new MySqlParameter("@FNAME", u.FirstName));
            cmd.Parameters.Add(new MySqlParameter("@UNAME", u.Username));
            cmd.Parameters.Add(new MySqlParameter("@EMAIL", u.Email));
            cmd.Parameters.Add(new MySqlParameter("@CID", u.CID));
            cmd.Parameters.Add(new MySqlParameter("@TYPE", u.Type));
            cmd.Parameters.Add(new MySqlParameter("@JOB", u.JobTitle));
            cmd.Parameters.Add(new MySqlParameter("@VERIFIED", u.Verified));
            cmd.ExecuteNonQuery();


            con.Close();

        }

        public void DeleteUser(int ID)
        {
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "update users set Removed = 1 where users.User_id = @ID";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@ID", ID));
            cmd.ExecuteNonQuery();
            con.Close();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////others//////////////////////////////////////////////////////////////////////////////////////
        public string GetPassword(int UserID)
        {
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "select Password from passwords where users_User_id = @ID";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@ID", UserID));
            if (cmd.ExecuteScalar() == null)
            {
                return null;
            }
            else
            {
                string ww = cmd.ExecuteScalar()?.ToString();
                return ww;
            }
        }
        public void CreatePassword(string pass, int UID)
        {
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "INSERT INTO passwords (password, users_User_id) values (@PASS, @UID)";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@PASS", pass));
            cmd.Parameters.Add(new MySqlParameter("@UID", UID));
            cmd.ExecuteNonQuery();
            con.Close();

        }
        public void DeletePassword(int ID)
        {
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "delete from passwords where passwords.users_User_id = @ID";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@ID", ID));
            cmd.ExecuteNonQuery();
            con.Close();
        }

        //////////////////////////////////////////
        public int GetUID(string email)
        {
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "select User_id from users where Email=@EMAIL";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@EMAIL", email));
            int ID = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            return ID;

        }

        public bool CheckVerification(int UID)
        {
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "SELECT Verified FROM users WHERE User_id = @UID";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@UID", UID));
            bool b = Convert.ToBoolean(cmd.ExecuteScalar());
            con.Close();
            return b;
        }

        public string GetUserPermission(int UserID)
        {
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "select Type from users where User_id=@ID";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@ID", UserID));
            string Type = cmd.ExecuteScalar().ToString();
            con.Close();
            return Type;
        }

        public bool CheckUsernameAvailability(string username)
        {
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "SELECT count(Username) FROM users WHERE Username = @USERNAME";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@USERNAME", username));

            if (Convert.ToInt16(cmd.ExecuteScalar()) == 0)
            {
                con.Close();
                return true;
            }
            else { con.Close(); return false; }
        }

        public void VerifyUser(int ID)
        {
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "UPDATE users SET Verified = 1 WHERE users.User_id = @ID;";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@ID", ID));
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void ChangeUserType(int UID, string goal)
        {
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "UPDATE users SET Type = @GOAL WHERE users.User_id = @ID;";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@ID", UID));
            cmd.Parameters.Add(new MySqlParameter("@GOAL", goal));
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void PermDeleteU_DeleteAssignments(int UID)
        {
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "DELETE assignedtable FROM assignedtable INNER JOIN users ON assignedtable.users_User_id = users.User_id WHERE users.company_CompanyID = (SELECT company_CompanyID FROM users where User_id = @UID) and users.Removed = 1;";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@UID", UID));
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void PermDeleteU_DeleteComments(int UID)
        {
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "DELETE comments FROM comments INNER JOIN users ON comments.users_User_id = users.User_id WHERE users.company_CompanyID = (SELECT company_CompanyID FROM users where User_id =@UID) and users.Removed = 1;";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@UID", UID));
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void PermDeleteU_DeletePasswords(int UID)
        {
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "DELETE passwords FROM passwords INNER JOIN users ON passwords.users_User_id = users.User_id WHERE users.company_CompanyID = (SELECT company_CompanyID FROM users where User_id =@UID) and users.Removed = 1;";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@UID", UID));
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void PermDeleteU_DeleteTickets(int UID)
        {
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "DELETE tickets FROM tickets INNER JOIN users ON tickets.ByUser = users.User_id WHERE users.company_CompanyID = (SELECT company_CompanyID FROM users where User_id =@UID) and users.Removed = 1;";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@UID", UID));
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void PermDeleteU_DeleteSessions(int UID)
        {
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "DELETE sessions FROM sessions INNER JOIN users ON sessions.users_User_id = users.User_id WHERE users.company_CompanyID = (SELECT company_CompanyID FROM users where User_id =@UID) and users.Removed = 1;";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@UID", UID));
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void PermanentlyDeleteUsers(int UID)
        {
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "DELETE FROM users u WHERE u.company_CompanyID = (SELECT company_CompanyID FROM (SELECT * FROM users) AS u WHERE User_id = @UID) and u.Removed = 1;";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@UID", UID));
            cmd.ExecuteNonQuery();
            con.Close();
        }
    }
}
