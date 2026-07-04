using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace carWorldDbFirstProject.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    public virtual DbSet<Cars> Cars { get; set; }

    public virtual DbSet<Customers> Customers { get; set; }

    public virtual DbSet<Rentals> Rentals { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=Default");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cars>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Araclar__3214EC07E3723C52");

            entity.Property(e => e.Fiyat).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ImageUrl).HasMaxLength(500);
            entity.Property(e => e.Marka).HasMaxLength(50);
            entity.Property(e => e.Model).HasMaxLength(50);
        });

        modelBuilder.Entity<Customers>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Musteril__3214EC07BCBBA975");

            entity.Property(e => e.AdSoyad).HasMaxLength(100);
            entity.Property(e => e.Telefon).HasMaxLength(20);
        });

        modelBuilder.Entity<Rentals>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Kiralama__3214EC0758D476E8");

            entity.HasOne(d => d.Cars).WithMany(p => p.Rentals)
                .HasForeignKey(d => d.AracId)
                .HasConstraintName("FK__Kiralamal__AracI__60A75C0F");

            entity.HasOne(d => d.Customers).WithMany(p => p.Rentals)
                .HasForeignKey(d => d.MusteriId)
                .HasConstraintName("FK__Kiralamal__Muste__619B8048");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
