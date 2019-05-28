using System;
using System.Collections.Generic;
using Domain.Model;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Database
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
        {
            if (this.Albums.Count() == 0)
            {
                this.Albums.AddRange(ServiceSeedDB.GetAlbums());
                this.CashBack.AddRange(ServiceSeedDB.GetCashBack());
                this.SaveChanges();
            }
        }

        public DbSet<Sales> Sales { get; set; }
        public DbSet<Albums> Albums { get; set; }
        public DbSet<AlbumsSold> AlbumsSold { get; set; }
        public DbSet<CashBack> CashBack { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
            .Entity<Sales>()
            .HasMany(c => c.Albums)
            .WithOne()
            .HasForeignKey(x => x.AlbumsSoldId);

            modelBuilder.Entity<AlbumsSold>().
            HasOne(e => e.Sales).
            WithMany(c => c.Albums).
            HasForeignKey(m => m.SalesId);

            modelBuilder.Entity<AlbumsSold>().
            HasOne(e => e.Album).
            WithMany().
            HasForeignKey(m => m.AlbumsId);
        }
    }
}