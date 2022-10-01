using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MonoVehicles.Models
{
    public partial class AnotherAppDBContext : DbContext
    {
        public AnotherAppDBContext()
        {
        }

        public AnotherAppDBContext(DbContextOptions<AnotherAppDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<VehicleMake> VehicleMakes { get; set; } = null!;
        public virtual DbSet<VehicleModel> VehicleModels { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-9T83QA0\\SQLEXPRESS;Database=AnotherAppDB;User ID=sa;Password=123456;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VehicleMake>(entity =>
            {
                entity.ToTable("VehicleMake");

                entity.Property(e => e.Abrv).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<VehicleModel>(entity =>
            {
                entity.ToTable("VehicleModel");

                entity.Property(e => e.Abrv).HasMaxLength(100);

                entity.Property(e => e.MakeId).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
