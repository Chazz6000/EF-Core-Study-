using System;
using System.Collections.Generic;
using System.Text;

namespace SamuraiApp.Domain
{
	public class Clan
	{
		public int Id { get; set; }
		public int ClanName { get; set; }

		//without a list of samruias in this class we can't add a samuria to clan this way. without a foreign key property pointing back to clan in samuia class can't set the value of the foreign key. 

	}
}
