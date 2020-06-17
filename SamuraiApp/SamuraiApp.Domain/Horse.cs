using System;
using System.Collections.Generic;
using System.Text;

namespace SamuraiApp.Domain
{
	public class Horse
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public int SamuraiId { get; set; } //This is used to create a foreign key. EF will understand the one-to-one relationship.

		//note currently a horse can't exisit without a samuria, must have SamuraiId populated to be valid.


	}
}
