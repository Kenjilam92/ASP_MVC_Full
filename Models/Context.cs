using Microsoft.EntityFrameworkCore;

namespace wedding_planner.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions options) : base (options){}
        public DbSet<User> Users {get;set;}
        public DbSet<Wedding> Weddings {get;set;}
        
        public DbSet<Associate> Associates {get;set;}
    }
}