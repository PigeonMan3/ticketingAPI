using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;

namespace API.Persistence
{
	public class Ticket
	{
		private int _id;
		private string _title;
		private string _description;
		private string _priority;
		private string _status;
		private DateTime _created;
		private DateTime? _deadline;
		private string _resolvedNotes;
		private string _category;
		private string _location;
		private bool _archived;
		private int _byUser;
		private string _username;


		public int ID { get { return _id; } set { _id = value; } }
		public string Title { get { return _title; } set { _title = value; } }
		public string Description { get { return _description; } set { _description = value; } }
		public string Priority { get { return _priority; } set { _priority = value; } }
		public string Status { get { return _status; } set { _status = value; } }
		public DateTime Created { get { return _created; } set { _created = value; } }
		public DateTime? Deadline { get { return _deadline; } set { _deadline = value; } }
		public string ResolvedNotes { get { return _resolvedNotes; } set { _resolvedNotes = value; } }
		public string Category { get { return _category; } set { _category = value; } }
		public string Location { get { return _location; } set { _location = value; } }
		public bool Archived { get { return _archived; } set { _archived = value; } }
		public int ByUser { get { return _byUser; } set { _byUser = value; } }
		public string Username { get { return _username; } set { _username = value; } }

		public Ticket()
		{

			_id = 0;
			_title = string.Empty;
			_description = string.Empty;
			_priority = string.Empty;
			_status = string.Empty;
			_created = DateTime.MinValue;
			_deadline = null;
			_resolvedNotes = string.Empty;
			_category = string.Empty;
			_location = string.Empty;
			_archived = false;
			_byUser = 0;

		}

		public Ticket(int id, string title, string description, string Priority, string Status, DateTime Created, DateTime? Deadline, string Category, string Location, int byUser, string Username) //for gathering partial ticket data
		{
			_id = id;
			_title = title;
			_description = description;
			_priority = Priority;
			_status = Status;
			_created = Created;
			_deadline = Deadline;
			_category = Category;
			_location = Location;
			_byUser = byUser;
			_username = Username;
		}

		public Ticket(int id, string title, string description, string priority, string status, DateTime created, DateTime? Deadline, string resolvedNotes, string category,string location, bool Archived, int byUser, string Username) //for gathering full ticket data
		{
			_id = id;
			_title = title;
			_description = description;
			_priority = priority;
			_status = status;
			_created = created;
			_deadline = Deadline;
			_resolvedNotes = resolvedNotes;
			_category = category;
			_location = location;
			_archived = Archived;
			_byUser = byUser;
			_username = Username;
		}
		public Ticket(string title, string description, string priority, string status, DateTime created, DateTime? Deadline, string category, bool Archived, int byUser, string Location) //for creating a new ticket
		{
			_title = title;
			_description = description;
			_priority = priority;
			_status = status;
			_created = created;
			_deadline = Deadline;
			_category = category;
			_archived = Archived;
			_byUser = byUser;
			_location = Location;
		}

        public Ticket(string title, string description, string priority, string category, string Location, DateTime? Deadline) //for updating a new ticket
        {
            _title = title;
            _description = description;
            _priority = priority;
            _deadline = Deadline;
            _category = category;
            _location = Location;
        }



    }
}
