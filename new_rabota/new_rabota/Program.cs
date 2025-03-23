using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

var orders = new List<Nahodki>()
{
    new Nahodki()
};

app.MapGet("/get", () =>
{
    using ApplicationContext db = new ApplicationContext();
    return db.Nahodki.ToList();
});
app.MapPost("/post", (Nahodki o) =>
{
    using ApplicationContext db = new ApplicationContext();
    db.Nahodki.Add(o);
    db.SaveChanges();
});
app.MapPut("/put", (Guid id, OrderUpdateDTO dto) =>
{
    using ApplicationContext db = new ApplicationContext();
    Nahodki buffer = db.Nahodki.Find(id);
    buffer.Number = dto.Number;
    buffer.Day = dto.Day;
    buffer.Month = dto.Month;
    buffer.Item = dto.Item;
    buffer.WhereNaideno = dto.Where_naideno;
    buffer.WhoFoundIt = dto.Who_found_it;
    buffer.Year = dto.Year;
    db.SaveChanges();
});
app.MapDelete("/", ([FromQuery] Guid Id) =>
{
    using ApplicationContext db = new ApplicationContext();
    Nahodki buffer = db.Nahodki.Find(Id);
    db.Nahodki.Remove(buffer);
    db.SaveChanges();
});

app.Run();

public class Nahodki
{
    [Range(1, int.MaxValue)]
    public int Number { get; set; }

    [Range(1, 31)]
    public int Day { get; set; }

    [Range(1, 12)]
    public int Month { get; set; }

    [Range(1900, int.MaxValue)]
    public int Year { get; set; }

    [Required]
    public string Item { get; set; }

    [Required]
    public string WhereNaideno { get; set; }

    [Required]
    public string WhoFoundIt { get; set; }

    public int id { get; set; }
}
record class OrderUpdateDTO(int Number, int Day, int Month, int Year, string Item, string Where_naideno, string Who_found_it);



public class ApplicationContext : DbContext
{
    public DbSet<Nahodki> Nahodki { get; set; } = null!;
    public ApplicationContext()
    {
        Database.EnsureCreated();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=Nahodki.db");
    }
}