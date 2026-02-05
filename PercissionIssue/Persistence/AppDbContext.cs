using Microsoft.EntityFrameworkCore;
using AvgRate.Reconciliation.Demo.Domain.Entities;
using System.Numerics;

namespace AvgRate.Reconciliation.Demo.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Wallet> Wallets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Wallet>().Property(x => x.Amount)
            .HasConversion(
                v => v.ToString(),
                v => BigInteger.Parse(v));

        modelBuilder.Entity<Wallet>().Property(x => x.Rate)
            .HasConversion(
                v => v.ToString(),
                v => BigInteger.Parse(v));

    }
}

