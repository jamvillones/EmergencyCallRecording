﻿using Microsoft.EntityFrameworkCore;
using ECR.Domain.Models;

namespace ECR.Domain.Data;

public partial class MDRContext : DbContext {
    public MDRContext() {
    }

    public MDRContext(DbContextOptions<MDRContext> options)
        : base(options) {
    }

    public virtual DbSet<Caller> Callers { get; set; }
    public virtual DbSet<Record> Records { get; set; }
    public virtual DbSet<Agency> Agencies { get; set; }
    public virtual DbSet<ContactDetail> ContactDetails { get; set; }
    public virtual DbSet<Login> Logins { get; set; }
    public virtual DbSet<Audio> Audios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {

        modelBuilder.Entity<Login>()
            .OwnsOne(l => l.Name);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
