#region --Using--
using Core.Entities;
using Microsoft.EntityFrameworkCore;
#endregion

namespace DAL.Context
{
    public class DatingAppContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Photo> Photos { get; set; }

        public DatingAppContext(DbContextOptions<DatingAppContext> options) : base(options)
        {
        
        }
    }
}