using AvgRate.Reconciliation.Demo.Domain.Entities;
using AvgRate.Reconciliation.Demo.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PercissionIssue;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False;Command Timeout=30"));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();
}


app.MapGet("/", () => "Welcome to Wallet Tracker. GET /run-scenario to execute the sample logic.");

app.MapGet("/sum", () =>
{
    // 198.302918391028301283012830129830128302138

    // num    : 198302918391028301283012830129830128302138
    // scale  : 25

    // ******
    // 777.00001
    // num    : 77700001
    // scale  : 5


    BigInteger a = BigInteger.Parse("198302918391028301283012830129830128302138");
    BigInteger b = BigInteger.Parse("198302918391028301283012830129830128302138");
    return (a + b).ToString();
});


app.MapGet("/seed", async (AppDbContext db, CancellationToken cancellationToken) =>
{
    List<Wallet> wallets = [
        Wallet.Create(
            2000,
            7.12345123891723879812m),
        Wallet.Create(
            8120.5555m,
            8.23110111m),
        Wallet.Create(
            5230.1235621312m,
            12.1312300000000000123m),
        Wallet.Create(
            10223.111231111m,
            4.1230000000000123123m),
    ];

    /*
     (2000 * 7.12345123891723879812 + 8120.5555 * 8.23110111 + 5230.1235621312 * 12.1312300000000000123 + 10223.111231111 * 4.1230000000000123123) / (2000 + 8120.5555 + 5230.1235621312 + 10223.111231111) = 
     */

    await db.Wallets.AddRangeAsync(wallets, cancellationToken);

    await db.SaveChangesAsync(cancellationToken);
});

app.MapGet("/avg-all", async (
    AppDbContext db,
    CancellationToken cancellationToken) =>
{

    List<Wallet> wallets = await db.Wallets.ToListAsync(cancellationToken);
    CustomDecimal sum = new(0, 0);
    CustomDecimal count = new(0, 0);
    foreach (var item in wallets)
    {
        var amount = new CustomDecimal(item.Amount, item.ScaleAmount);
        var rate = new CustomDecimal(item.Rate, item.ScaleRate);
        sum = sum + (amount * rate);
        count = count + amount;
    }
    return (sum / count).ToString();
});

app.Run();


