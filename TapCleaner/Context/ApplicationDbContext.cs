using Microsoft.EntityFrameworkCore;
using TapCleaner.Models;

namespace TapCleaner.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users {  get; set; }
        public DbSet<Container> Containers { get; set; }

    }

}
