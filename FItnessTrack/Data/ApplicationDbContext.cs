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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //Define Relationships and Keys

            builder.Entity<PersonalTraining>()
                    .HasOne(p => p.Service)
                    .WithMany(c => c.Training)
                    .HasForeignKey(p => p.TrainingId)
                    .HasConstraintName("FK_Training_ServiceId");
        }
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
