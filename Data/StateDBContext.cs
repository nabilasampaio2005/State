using Microsoft.EntityFrameworkCore;
using State.Models;

namespace State.Data
{
	public class StateDBContext : DbContext
	{
		public StateDBContext(DbContextOptions<StateDBContext> options) : base(options) { }
		public DbSet<TasksModel> TasksModel { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<TasksModel>().Property(t => t.State);

			base.OnModelCreating(modelBuilder);
		}
	}
}
