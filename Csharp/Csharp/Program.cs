var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<Order> repo = 
[
    new(1, 30, 2, 2077, "имплант ноги", "замыкание", "описания нету", "Кирмич", "Обнулен", "Виктор"),
];



app.MapGet("/", () => repo);
app.MapPost("/", (Order o) => repo.Add(o));
app.MapPost("/{number}", (int number,OrderUpdateDTO dto) =>
{
    Order Buffer = repo.Find(o => o.Number == number);
    if (Buffer == null)
        return Results.NotFound("не нашлось");
    Buffer.Status = dto.Status;
    Buffer.Master = dto.Master;
    Buffer.Description = dto.Descrption;
    return Results.Json(Buffer);
});

app.MapDelete("/{number}", (int number) =>
{
    var orderToRemove = repo.Find(o => o.Number == number);
    if (orderToRemove == null)
        return Results.NotFound("Заказ не найден");

    repo.Remove(orderToRemove);
    return Results.NoContent(); // 204 No Content
});




app.Run();

class OrderUpdateDTO
{
    string status;
    string descrption;
    string master;

    public string Status { get => status; set => status = value; }
    public string Descrption { get => descrption; set => descrption = value; }
    public string Master { get => master; set => master = value; }
}

class Order
{
    int number;
    int day;
    int month;
    int year;
    string device;
    string problemType;
    string description;
    string client;
    string status;
    string master;

    public Order(int number, int day, int month, int year, string device, string problemType, string description, string client, string status, string master)
    {
        Number = number;
        Day = day;
        Month = month;
        Year = year;
        Device = device;
        ProblemType = problemType;
        Description = description;
        Client = client;
        Status = status;
        Master = master;
    }

    public int Number { get => number; set => number = value; }
    public int Day { get => day; set => day = value; }
    public int Month { get => month; set => month = value; }
    public int Year { get => year; set => year = value; }
    public string Device { get => device; set => device = value; }
    public string ProblemType { get => problemType; set => problemType = value; }
    public string Description { get => description; set => description = value; }
    public string Client { get => client; set => client = value; }
    public string Status { get => status; set => status = value; }
    public string Master { get => master; set => master = value; }
}