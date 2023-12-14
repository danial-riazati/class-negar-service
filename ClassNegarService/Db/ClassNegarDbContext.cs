using System;
using Microsoft.EntityFrameworkCore;

namespace ClassNegarService.Db
{
    public class ClassNegarDbContext : DbContext
    {
        public ClassNegarDbContext(DbContextOptions<ClassNegarDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<User> Users { get; set; }

    }
}

