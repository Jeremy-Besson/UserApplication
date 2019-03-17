using BalticAmadeusTask.Models;
using Microsoft.EntityFrameworkCore;

namespace BalticAmadeusTask.Data
{
    public class BalticAmadeusTaskContext : DbContext
    {
        public BalticAmadeusTaskContext (DbContextOptions<BalticAmadeusTaskContext> options)
            : base(options)
        {
        }

        public DbSet<RegisteredUserD> UserModel { get; set; }
    }
}
