namespace SportsTracker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

public class AppDbContext : DbContext
{
    public DbSet<Borrower> Borrower { get; set; }
    public DbSet<Equipment> Equipment { get; set; }
    public DbSet<Reservation> Reservation { get; set; }
    public DbSet<EquipmentDamage> EquipmentDamage { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=sports.db");
    }
}