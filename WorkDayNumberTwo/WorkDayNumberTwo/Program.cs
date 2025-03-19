using System.ComponentModel.DataAnnotations;
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

app.MapGet("/", () => Results.Ok(orders));
app.MapPost("/", (Nahodki order) =>
{
    try
    {
        orders.Add(order);
        return Results.Created($"/{order.Number}", order);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }
});
app.MapPut("/{number}", (int number, OrderUpdateDTO dto) =>
{
    var existingOrder = orders.Find(o => o.Number == number);
    if (existingOrder == null)
        return Results.NotFound("Вещь не найдена");

    try
    {
        return Results.Ok(existingOrder);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }
});
app.MapDelete("/{number}", (int number) =>
{
    var orderToRemove = orders.FirstOrDefault(o => o.Number == number);
    if (orderToRemove == null)
        return Results.NotFound("Вещь не найдена");

    orders.Remove(orderToRemove);
    return Results.NoContent();
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

}
record class OrderUpdateDTO(int number, int day, int month, int year, string Item, string Where_naideno, string who_found_it);