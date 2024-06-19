using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Business
{
	public class Priority
	{
		private int _id;
		private string _name;

		public int ID { get { return _id; } set { _id = value; } }
		public string Name { get { return _name; } set { _name = value; } }

		public Priority() 
		{
			_id = 0;
			_name = string.Empty;
		}

		public Priority(int id, string name)
		{
			_id = id;
			_name = name;
		}
	}
}
