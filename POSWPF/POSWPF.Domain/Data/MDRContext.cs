using Microsoft.EntityFrameworkCore;
using ECR.Domain.Models;

namespace ECR.Domain.Data;

public partial class MDRContext : DbContext {
    public MDRContext() {
    }

    public MDRContext(DbContextOptions<MDRContext> options)
        : base(options) {
    }

    public virtual DbSet<HouseNumber> HouseNumbers { get; set; }
    public virtual DbSet<Area> Areas { get; set; }
    public virtual DbSet<Barangay> Barangays { get; set; }
    public virtual DbSet<Street> Streets { get; set; }
    public virtual DbSet<Caller> Callers { get; set; }
    public virtual DbSet<Agency> Endorsements { get; set; }
    public virtual DbSet<Login> Logins { get; set; }
    public virtual DbSet<Audio> Audios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-EP1IGTA\\SQLEXPRESS;Initial Catalog=EbarangayV2;Integrated Security=True; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
