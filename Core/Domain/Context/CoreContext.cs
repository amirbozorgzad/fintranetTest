using Core.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Core.Domain.Context;
#nullable disable
public class CoreContext : DbContext
{
    public CoreContext()
    {
    }

    public CoreContext(DbContextOptions<CoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Calendar> Calendar { get; set; }
    public virtual DbSet<City> City { get; set; }
    public virtual DbSet<Fee> Fee { get; set; }
    public virtual DbSet<Holiday> Holiday { get; set; }
    public virtual DbSet<Toll> Toll { get; set; }
    public virtual DbSet<Vehicle> Vehicle { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Calendar>(entity =>
        {
            entity.ToTable("Calendar");
            entity.HasKey(c => c.Id);
            entity.HasOne(h => h.City)
                .WithOne(c => c.Calendar);
        });
        modelBuilder.Entity<City>(entity =>
        {
            entity.ToTable("City");
            entity.HasKey(c => c.Id);
        });
        modelBuilder.Entity<Fee>(entity =>
        {
            entity.ToTable("Fee");
            entity.HasKey(f => f.Id);
        });
        modelBuilder.Entity<Holiday>(entity =>
        {
            entity.ToTable("Holiday");
            entity.HasKey(h => h.CalendarId);
            entity.HasOne(h => h.Calendar)
                .WithMany(c => c.HoliDays)
                .HasForeignKey(h => h.CalendarId);
        });

        modelBuilder.Entity<Toll>(entity =>
        {
            entity.ToTable("Toll");
            entity.HasKey(t => t.Id);
            entity.HasOne(t => t.City)
                .WithMany(c => c.Toll)
                .HasForeignKey(h => h.CityId);
            entity.HasOne(t => t.Vehicle)
                .WithMany(v => v.Toll)
                .HasForeignKey(t => t.VehicleId);
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.ToTable("Vehicle");
            entity.HasKey(v => v.Id);
        });
    }
}