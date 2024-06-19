using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Business
{
	public class Comment
	{
		private int _id;
		private DateTime _created;
		private string _content;
		private int _uid; //userID
		private string _username;
		private int _tid; //TicketID
		private bool _AdminOnly;

		public int ID { get { return _id; } set { _id = value; } }
		public DateTime Created { get { return _created; } set { _created = value; } }
		public string Content { get { return _content; } set { _content = value; } }
		public int UID { get { return _uid; } set { _uid = value; } }
		public string Username { get { return _username; } set { _username = value; } }
		public int TID { get { return _tid; } set { _tid = value; } }
		public bool AdminOnly { get { return _AdminOnly; } set { _AdminOnly = value; } }

		public Comment () //empty
		{ 
			_id = 0;
			_created = DateTime.Now;
			_content = string.Empty;
			_uid = 0;
			_tid = 0;
			_AdminOnly = false;
		}

		public Comment (DateTime Created, string Content, int UID, int TID, bool AdminOnly) //for creating data
		{
			_created = Created;
			_content = Content;
			_uid = UID;
			_tid = TID;
			_AdminOnly = AdminOnly;
		}
		public Comment(int ID, DateTime Created, string Content, int UID, string Username, int TID, bool AdminOnly) //for gathering data
		{
			_id = ID;
			_created = Created;
			_content = Content;
			_uid= UID;
			_username = Username;
			_tid = TID;
			_AdminOnly = AdminOnly;
		}



	}
}
