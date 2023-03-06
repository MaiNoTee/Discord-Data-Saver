using DiscordDataSaver.Entities;
using Microsoft.EntityFrameworkCore;

namespace DiscordDataSaver.Services.Database;

public sealed class DatabaseContext : DbContext
{
	public DbSet<User> Users => Set<User>();
	public DatabaseContext() => Database.EnsureCreated();

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSqlite("Data Source=first.db");
	}
}
