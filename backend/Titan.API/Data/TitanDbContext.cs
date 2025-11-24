using Microsoft.EntityFrameworkCore;
using Titan.API.Models;

namespace Titan.API.Data
{
    public class TitanDbContext : DbContext
    {
        public TitanDbContext(DbContextOptions<TitanDbContext> options) : base(options) { }


        public DbSet<User> Users { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<ProjectEmployee> ProjectEmployees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProjectEmployee>()
            .HasKey(pe => new { pe.ProjectId, pe.EmployeeId });

        }
    }
}