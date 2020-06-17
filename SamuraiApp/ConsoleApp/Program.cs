using Microsoft.EntityFrameworkCore;
using SamuraiApp.Domain;
using SamuriaApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace ConsoleApp
{
	class Program
	{
		private static SamuraiContext context = new SamuraiContext();

		static void Main(string[] args)
		{



			AddSamurai();
			GetSamurais("After Add:");
			InsertMultipleSamurias();
			Console.WriteLine("Press Any key...");
			Console.ReadKey();



		}


		private static void InsertMultipleSamurias()
		{
			var samurai1 = new Samurai { Name = "Naruto" };
			var samurai2 = new Samurai { Name = "Ichigo" };
			context.Samurais.AddRange(samurai1, samurai2);
			context.SaveChanges();
		}

		private static void AddSamurai()
		{
			var samurai = new Samurai { Name = "Charlie" };
			context.Add(samurai);
			context.SaveChanges();

			Console.ReadKey();
		}

		private static void GetSamurais(string text)
		{
			var samurais = context.Samurais.ToList();
			Console.WriteLine($"{text} : Samurai count is {samurais.Count}");
			foreach (var samuria in samurais)
			{
				Console.WriteLine(samuria.Name);
			}
		}

		private static void GetMultpleSamurais()
		{
			var samurais = context.Samurais.Skip(1).Take(3).ToList();
			samurais.ForEach(s => s.Name += "San");
			context.SaveChanges();
		}



		private static void DeleteSamuria(string text)
		{

			var samurai = context.Samurais.FirstOrDefault(s => s.Name == text);
			Console.WriteLine($" Removing.. {text}");
			context.Remove(samurai);
			Console.WriteLine($" Removed {text}");
			context.SaveChanges();

		}

		public static void AddAndCheck()
		{

			GetSamurais("before add:");
			AddSamurai();
			GetSamurais("after Add:");
			Console.WriteLine("Press any Key");
			Console.ReadKey();

		}

		public static void Check()
		{
			var samurais = context.Samurais.ToList();
			if (samurais != null)
			{

				foreach (var samuria in samurais)
				{
					Console.WriteLine(samuria.Name);
				}

			}
			else
			{
				Console.WriteLine("no Data");
			}

		}

		private static void Text(String text)
		{
			Console.WriteLine(text);
		}

		private static void InsertNewSamuraiWithAquote()
		{
			var samurai = new Samurai
			{
				Name = "Kambei Shimada",
				Quotes = new List<Quote>
				{
					new Quote {Text ="I've come to save you"}
				}
			};
			context.Samurais.Add(samurai);
			context.SaveChanges();

		}

		private static void AddQuoteToExistingSamuria()
		{
			var samuria = context.Samurais.FirstOrDefault();
			samuria.Quotes.Add(new Quote { Text = "I've come to save you" });
			context.SaveChanges();

		}

		private static void ProjectSamuraisWithQuotes()
		{
			var PropWithQuotes = context.Samurais.Select(s => new { s.Id, s.Name, HappyQuotes = s.Quotes.Where(q => q.Text.Contains("happy")) });
		}

		private static void JoinBattleAndSamurai()
		{
			//Samurai and battle already exist and we have their ids.
			//can't use context.SamuraiBattles.Add(), as SamuraiBattles dones't have a DbSet]
			//You can use add,attach, remove so forth on enties thay don't have a DbSet.
			var sbJoin = new SamuraiBattle { SamuraiId = 1, BattleId = 3 };
			context.Add(sbJoin);
			context.SaveChanges();
		}

		private static void EnlistSamuraiIntoABattle()
		{
			//I want to enligh a samuria into that battle. 
			var battle = context.Battles.Find(1);
			//Add a battle to the samuriabattle property
			battle.SamuraiBattles
				.Add(new SamuraiBattle { SamuraiId = 21 });
			context.SaveChanges();
		}

		private static void GetSamuraiWithBattles()
		{

			//this is an annoyig way to get all the battles a samuria has been in.
			var samuraiWithBattle = context.Samurais
				.Include(s => s.SamuriaBattles) //navigation property
				.ThenInclude(sb => sb.Battle)   //navigation property
				.FirstOrDefault(samurai => samurai.Id == 2);    //retuen for the samuria with id of 2.


			//use a projection.
			var samuraiWithBattlesCleaner = context.Samurais.Where(s => s.Id == 2) //we grab the samuria with the id of two.
				.Select(s => new    //annonymous type
				{
					Samurai = s,
					Battles = s.SamuriaBattles.Select(sb => sb.Battle)
				})
				.FirstOrDefault();


		}

		private static void AddNewSamuraiWithHorse()
		{

			//we create a new samurai and give him a Horse.

			var samurai = new Samurai { Name = "Jina Ujichika" };
			samurai.Horse = new Horse { Name = "Silver" };
			context.Samurais.Add(samurai);
			context.SaveChanges();




		}

		private static void AddNewHorseToSamuraiUsingId()
		{
			var horse = new Horse { Name = "Scout", SamuraiId = 2 };
			context.Add(horse);
			context.SaveChanges();
		}

		private static void AddNewHorseToSamuraiObject()
		{
			//Add horse to Samuria already in memory.
			var samurai = context.Samurais.Find(22); //Find Samurai.
			samurai.Horse = new Horse { Name = "Black Beauty" }; //create horse object. and assing to horse property in samuria type. Job Done. 
			context.SaveChanges();
		}

		private static void AddNewHorseToDisconnetedSamuraiObject()
		{
			var samurai = context.Samurais.AsNoTracking().FirstOrDefault(s => s.Id == 23);
			samurai.Horse = new Horse { Name = "mr. Ed" };
			using (var newContext = new SamuraiContext())
			{
				newContext.Attach(samurai);
				newContext.SaveChanges();
			}


		}

		private static void ReplaceHorse()
		{
			var samurai = context.Samurais.Include(s => s.Horse).FirstOrDefault(s => s.Id == 23); //include the current Horse and look for Samurai id 23.
			samurai.Horse = new Horse { Name = "Jeffery" };
			context.SaveChanges();
		}

		private static void GetSamruaiWithHorse()
		{
			var samurai = context.Samurais.Include(h => h.Horse).ToList();
		}

		private static void GetHorseWithSamurai()
		{
			var horseWithoutSamuria = context.Set<Horse>().Find(3); //when the DbSet doesn't exisit you can use Set method to query.

			var horseWithSamurai = context.Samurais.Include(s => s.Horse) //drilling through the relation ship to get a horse ID
				.FirstOrDefault(s => s.Horse.Id == 3);

			var horsesWithSamurais = context.Samurais
				.Where(s => s.Horse != null) //Grab every samuria that has a horse
				.Select(s => new { Horse = s.Horse, Samruai = s })  //Projection that determines, samruias that have a horse and samuria.
				.ToList();  //the result will horse and samurai in each object. 

		}

		private static void GetSmauraiWithClan()
		{

			var samurai = context.Samurais.Include(s => s.Clan).FirstOrDefault();
		}

		private static void GetClanWithSamurias()
		{
			//Get a clan with list of samurais 
			var clan = context.Clans.Find(3);
			var samuraisForClan = context.Samurais.Where(s => s.Clan.Id == 3).ToList();

		}

		private static void QuerySamuraiBattleStats()
		{
			var firstStat = context.SamuraiBattleStats.FirstOrDefault();
			var sampsonStat = context.SamuraiBattleStats.Where(s => s.Name == "SampsonSan").FirstOrDefault();
		}

		private static void QueryUsingRawSQL()
		{
			var samurais = context.Samurais.FromSqlRaw("Select * from Samurias").ToList();
		}

		private static void QueryUsingRawSQLWithInterpolation()
		{
			string name = "Kikuchyo";
			var samurais = context.Samurais;
			samurais.FromSqlInterpolated($"Select * from Samurais Where Name = {name}")
			.ToList();


		}

	}



}
