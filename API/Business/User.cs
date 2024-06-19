using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace API.Persistence
{
	public class User
	{
		private int _id;
		private string _name;
		private string _firstName;
		private string _username;
		private string _email;
		private string _type;
		private string _jobTitle;
		private string _company;
		private int _cid;
		private string _password;
		private string _sessionID;
		private bool _verified;
		
		public int ID { get { return _id; } set { _id = value; } }
		public string Name { get { return _name; } set { _name = value; } }
		public string FirstName { get { return _firstName; } set { _firstName = value; } }
		public string Username { get { return _username; } set { _username = value; } }
		public string Email { get { return _email; } set { _email = value; } }
		public string Type { get { return _type; } set { _type = value; } }
		public string JobTitle { get { return _jobTitle; } set { _jobTitle = value; } }
		public string Company { get { return _company; } set { _company = value; } }
		public int CID { get { return _cid; } set { _cid = value; } }
		private string Password { get { return _password; } set { _password = value; } }
		private string SessionID { get { return _sessionID; } set { _sessionID = value; } }

		public bool Verified { get { return _verified; } set { _verified = value; } }

		public User() 
		{
			_name = "";
			_firstName = "";
			_username = "";
			_email = "";
			_type = "normal";
			_jobTitle = "";
		}

		public User(string Fname, string Lname)
		{
			_name = Lname;
			_firstName = Fname;
			_username = "";
			_email = "";
			_type = "normal";
			_jobTitle = "";
		}

		public User(int ID, string Name, string FirstName, string Username, string Email, string Type, string JobTitle, string company, bool Verified) //gatherData
		{
			_id = ID;
			_name = Name;
			_firstName = FirstName;
			_username = Username;
			_email = Email;
			_type = Type;
			_jobTitle = JobTitle;
			_company = company;
			_verified = Verified;
		}
		public User(string Name, string FirstName, string Username, string Email, string JobTitle, int CID,string Type, bool Verified) //CreateData
		{
			_name = Name;
			_firstName = FirstName;
			_username = Username;
			_email = Email;
			_jobTitle= JobTitle;
			_cid = CID;
			_type = Type;
			_verified= Verified;
		}

		public User(int UserID, string ww, bool verified) //auth
		{
			_id = UserID;
			_email = "";
			_password = ww;
			_name = "";
			_firstName = "";
			_email = "";
			_type = "normal";
			_jobTitle = "";
			_verified = verified;
		}

		public User(int UserID, string sKey) //validateSession
		{
			_id = UserID;	
			_email = "";
			_sessionID = sKey;
			_name = "";
			_firstName = "";
			_email = "";
			_type = "";
			_jobTitle = "";
		}

		public string authentication(string code)
		{
			Console.WriteLine(code + " " + _password);
			if (code != _password)
			{
				return "wrong password";
			}
			else if(_verified != true) { return "not verified"; }
			else { return "OK"; }
		}

		public bool validateSession(string sessionID)
		{
            if (sessionID == _sessionID)
			{
				return true;
			}
			else { return false; }
		}

		public string GenerateUsername(int Number)
		{
			string BaseUsername; //first name + last name
			if (Number == 0) { 
				BaseUsername = _firstName.ToLower() + _name.ToLower(); //if username doesnt exist
			}
			else {
				BaseUsername = _firstName.ToLower() + _name.ToLower() + Number.ToString(); //if username exists, add a suffix
			}
			return BaseUsername;
			
		}

	}
}
