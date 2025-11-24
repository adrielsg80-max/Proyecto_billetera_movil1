using Banca_movil.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;   
using System.Reflection.Emit;

namespace Banca_movil.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "Usuario" }
                );
            base.OnModelCreating(modelBuilder);
        }
    }
}