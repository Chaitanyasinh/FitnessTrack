using FItnessTrack.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FItnessTrack.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Service>Services { get; set; }
        public DbSet<PersonalTraining>Trainings { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //Define Relationships and Keys

            builder.Entity<PersonalTraining>()
                    .HasOne(p => p.Service)
                    .WithMany(c => c.Training)
                    .HasForeignKey(p => p.TrainingId)
                    .HasConstraintName("FK_Training_ServiceId");
            builder.Entity<Cart>()
                   .HasOne(p => p.Service)
                   .WithMany(c => c.Carts)
                   .HasForeignKey(p => p.ServiceId)
                   .HasConstraintName("FK_Carts_TrainingId");
            builder.Entity<OrderDetail>()
                   .HasOne(p => p.Service)
                   .WithMany(c => c.OrderDetails)
                   .HasForeignKey(p => p.ServiceId)
                   .HasConstraintName("FK_OrderDetails_ServiceId");
            builder.Entity<OrderDetail>()
                   .HasOne(p => p.Order)
                   .WithMany(c => c.OrderDetails)
                   .HasForeignKey(p => p.OrderId)
                   .HasConstraintName("FK_OrderDetails_OrderId");
        }
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
