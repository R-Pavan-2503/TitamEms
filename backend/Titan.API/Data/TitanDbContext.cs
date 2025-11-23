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
    }
}