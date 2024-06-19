using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace API.Business
{
	public class Session
	{
		private Random random = new Random();
		private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789$*&!@";

		public string GenerateSessionID()
		{		
			string sessionID = "";
			for (int i = 0; i < 30; i++)
			{
				sessionID += chars[random.Next(chars.Length)];
			}
			return sessionID;
		}
	}
}
