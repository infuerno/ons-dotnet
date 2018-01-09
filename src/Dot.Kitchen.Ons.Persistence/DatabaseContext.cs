using System;
using Microsoft.EntityFrameworkCore;
using Dot.Kitchen.Ons.Domain;
using Dot.Kitchen.Ons.Application;
using Dot.Kitchen.Ons.Application.Models;

namespace Dot.Kitchen.Ons.Persistence
{
    public class DatabaseContext : DbContext, IDatabaseContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<ScrapeItem> ScrapeItems { get; set; }
        public DbSet<Scrape> Scrapes { get; set; }
        public DbSet<Source> Sources { get; set; }

        public new DbSet<T> Set<T>() where T : class, IEntity
        {
            return base.Set<T>();
        }

        public void Save()
        {
            this.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Source>(entity =>
            {
                entity.HasKey(e => e.Id)
                      .HasName("PK_Sources");

                entity.HasAlternateKey(e => e.Name);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FriendlyName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Description)
                    .IsRequired(false)
                    .HasMaxLength(250);
            });
                                        
            modelBuilder.Entity<Scrape>(entity =>
            {
                entity.HasKey(e => e.Id)
                      .HasName("PK_Scrapes");

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.NumberOfRecordsScraped)
                    .IsRequired()
                    .HasDefaultValue(0);

                entity.Property(e => e.StartedAt)
                    .IsRequired();

                entity.Property(e => e.FinishedAt)
                    .IsRequired(false);
            });
        }
    }
}
