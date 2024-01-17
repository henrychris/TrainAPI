using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using TrainAPI.Domain.Entities;

namespace TrainAPI.Infrastructure.Data;

public class DataContext : IdentityDbContext<ApplicationUser>
{
    public DataContext()
    {

    }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("TrainDb");

		modelBuilder.Entity<Coach>()
        .OwnsOne(c => c.TravellerCategories, d => d.ToJson());
    }

    public virtual DbSet<Station> Stations { get; set; } = null!;
    public virtual DbSet<Train> Trains { get; set; } = null!;
    public virtual DbSet<Trip> Trips { get; set; } = null!;
    public virtual DbSet<Coach> Coaches { get; set; } = null!;
}