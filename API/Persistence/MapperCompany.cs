using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Business;

namespace API.Persistence
{
    internal class MapperCompany
    {
        string _conString;
        string _cString;
        string _sql;

        public MapperCompany()
        {
            //_conString = "server=localhost;userid=root;database=ticketing;port=3306;password=1234";
            _conString = "Server=sqlserverkobe.mysql.database.azure.com;UserID=Kobe;Password=TicketingAzure!;Database=ticketing;";
        }

        public void addCompany(Company c)
        {
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "insert into company (CompanyName, Domain, Sector) values (@NAME, @DOMAIN, @SECTOR)";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@NAME", c.Name));
            cmd.Parameters.Add(new MySqlParameter("@DOMAIN", c.Domain));
            cmd.Parameters.Add(new MySqlParameter("@SECTOR", c.Sector));
            cmd.ExecuteNonQuery();
        }
        public void AddCategoryToCompany(string Cat, int UID)
        {
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "insert into categories_has_company (categories_Category_id, company_CompanyID) values ((select Category_id from categories where CategoryName = @CAT), (select company_CompanyID from users where User_id = @UID))";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@CAT", Cat));
            cmd.Parameters.Add(new MySqlParameter("@UID", UID));
            cmd.ExecuteNonQuery();
        }
        public void DeleteCategory(string Cat, int UID)
        {
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "Delete from categories_has_company chc where company_CompanyID = (select company_CompanyID from users where User_id = @UID) and categories_Category_id = (select Category_id from categories where CategoryName = @CAT)";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@UID", UID));
            cmd.Parameters.Add(new MySqlParameter("@CAT", Cat));
            cmd.ExecuteNonQuery();
        }

        public int GetCompanyByDomainAndGetCID(string domain)
        {
            MySqlConnection con = new MySqlConnection(_conString);
            con.Open();
            MySqlCommand cmd;
            _sql = "SELECT CompanyID from company where domain = @DOMAIN";
            cmd = new MySqlCommand(_sql, con);
            cmd.Parameters.Add(new MySqlParameter("@DOMAIN", domain));
            int i = Convert.ToInt16(cmd.ExecuteScalar());
            con.Close();
            return i;


        }
    }
}
