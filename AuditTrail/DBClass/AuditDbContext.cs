using AuditTrail.Models;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;

namespace AuditTrail.DBClass
{
    public class AuditDbContext : DbContext
    {
        public AuditDbContext(DbContextOptions<AuditDbContext> options) : base(options)
        {
        }

        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<AuditLogChange> AuditLogChanges { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasMany(e => e.Changes)
                      .WithOne(c => c.AuditLog)
                      .HasForeignKey(c => c.AuditLogId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<AuditLogChange>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
        }
    }

}
