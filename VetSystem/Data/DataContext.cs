using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VetSystem.Entities;
using VetSystem.Models;


namespace VetSystem.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Owner> Owners { get; set; }
        public DbSet<Animal> Animals { get; set; }
        public DbSet<Record> Records { get; set; }
        public DbSet<User> Users { get; set; }
        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // İlişkileri belirleme
            modelBuilder.Entity<Owner>()
                .HasMany(o => o.Animals)
                .WithOne(a => a.Owner)
                .HasForeignKey(a => a.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Animal>()
                .HasMany(a => a.Records)
                .WithOne(r => r.Animal)
                .HasForeignKey(r => r.AnimalId)
                .OnDelete(DeleteBehavior.Cascade);
        }*/
    }
}