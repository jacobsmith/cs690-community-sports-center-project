namespace SportsTracker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

public class AppDbContext : DbContext
{
    public AppDbContext()
    {
        // nop, accept no args so Program can have default database while tests have in-memory sqlite
    }

    public AppDbContext(DbContextOptions? options): base(options)
    {
    }

    public DbSet<Borrower> Borrower { get; set; }
    public DbSet<Equipment> Equipment { get; set; }
    public DbSet<Reservation> Reservation { get; set; }
    public DbSet<EquipmentDamage> EquipmentDamage { get; set; }

    public DbSet<EquipmentReservation> EquipmentReservation { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=sports.db");
        }
    }
}