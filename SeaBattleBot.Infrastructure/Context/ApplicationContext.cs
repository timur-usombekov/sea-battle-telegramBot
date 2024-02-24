using Microsoft.EntityFrameworkCore;
using SeaBattleBot.Core.Domain.Entities;

namespace SeaBattleBot.Infrastructure.Context
{
	public class ApplicationContext: DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<GameState> GamesState { get; set; }
		public DbSet<EnemyState> EnemiesState { get; set; }
		public string DbPath { get; }

		public ApplicationContext()
		{
			var folder = Environment.SpecialFolder.LocalApplicationData;
			var path = Environment.GetFolderPath(folder);
			DbPath = Path.Join(path, "SeaBattle.db");
			Database.EnsureCreated();
		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);
			optionsBuilder.UseSqlite(@$"Data Source={DbPath}");
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}
	}
}
