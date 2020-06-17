using System;
using System.Collections.Generic;
using System.Text;

namespace SamuraiApp.Domain
{
	public class Samurai
	{
		public Samurai()
		{
			Quotes = new List<Quote>();
		}

		public int Id { get; set; }

		public string Name { get; set; }
		public List<Quote> Quotes { get; set; }
		public Clan Clan { get; set; } // EF core figure out it needed the foerign key, 

		//have to create an object instance  over property to connect the samuai to a clan. 


		public Horse Horse { get; set; }

		public List<SamuraiBattle> SamuriaBattles { get; set; }
	}



}
