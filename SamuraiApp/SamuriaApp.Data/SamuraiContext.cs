using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SamuraiApp.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SamuriaApp.Data
{
	public class SamuraiContext : DbContext
	{
		public DbSet<Samurai> Samurais { get; set; }

		public DbSet<Quote> Quotes { get; set; }

		public DbSet<Clan> Clans { get; set; }

		public DbSet<Battle> Battles { get; set; }

		public DbSet<SamuraiBattleStat> SamuraiBattleStats { get; set; }

		public static readonly ILoggerFactory ConsoleLoggerFactory
			= LoggerFactory.Create(builder =>
			{
				builder
				.AddFilter((category, level) =>
				category == DbLoggerCategory.Database.Command.Name
				&& level == LogLevel.Information)
				.AddConsole();
			});




		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder
				.UseLoggerFactory(ConsoleLoggerFactory)
				.UseSqlServer(
				"Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = SamuraiAppData");
			
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<SamuraiBattle>().HasKey(s => new { s.SamuraiId, s.BattleId }); //sets the primary key for this entity.
			modelBuilder.Entity<Horse>().ToTable("Horses"); //I am teliing the modelbuilder that the horse entity is mapped to a table called horses, No table of horse would be created has it doesn't have a db Set.
			modelBuilder.Entity<SamuraiBattleStat>().HasNoKey();
		}
	}
}
