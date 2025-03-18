var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

// напоминани мне потому что память у меня рыбкина


var orders = new List<Nahodki>
{
    new Nahodki(1001, 14, 02, 2025, "Бомба_SC1000", "Евразия", "Виктор_Перевертышь_Валентинов"),
};

// Получение всех заказов
app.MapGet("/", () => Results.Ok(orders));

// Добавление нового заказа
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

// Обновление существующего заказа
app.MapPut("/{number}", (int number, OrderUpdateDTO dto) =>
{
    var existingOrder = orders.FirstOrDefault(o => o.Number == number);
    if (existingOrder == null)
        return Results.NotFound("Вещь не найдена");

    try
    {
        existingOrder.Update(dto);
        return Results.Ok(existingOrder);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

// Удаление заказа
app.MapDelete("/{number}", (int number) =>
{
    var orderToRemove = orders.FirstOrDefault(o => o.Number == number);
    if (orderToRemove == null)
        return Results.NotFound("Вещь не найдена");

    orders.Remove(orderToRemove);
    return Results.NoContent();
});

app.Run();

// DTO для обновления вещи
public class OrderUpdateDTO
{
    public string item { get; set; }
    public string where_naideno { get; set; }
    public string Who_found_it { get; set; }
}

// Класс найденого предмета
public class Nahodki
{
    public int Number { get; set; }
    public int Day { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public string item { get; set; }
    public string where_naideno { get; set; }
    public string Who_found_it { get; set; }

    public Nahodki(int number, int day, int month, int year, string Item, string Where_naideno, string who_found_it)
    {
        Validate(number, day, month, year, Item, Where_naideno, who_found_it);

        Number = number;
        Day = day;
        Month = month;
        Year = year;
        item = Item;
        where_naideno = Where_naideno;
        Who_found_it = who_found_it;
    }

    private void Validate(int number, int day, int month, int year, string item, string where_naideno, string who_found_it)
    {
        if (number <= 0)
            throw new ArgumentException("Номер должен быть положительным.");

        if (day < 1 || day > 31)
            throw new ArgumentException("День должен быть от 1 до 31.");

        if (month < 1 || month > 12)
            throw new ArgumentException("Месяц должен быть от 1 до 12.");

        if (year < DateTime.Now.Year)
            throw new ArgumentException("Год не может быть меньше текущего.");

        if (string.IsNullOrWhiteSpace(item))
            throw new ArgumentException("Название вещи не может быть пустым.");

        if (string.IsNullOrWhiteSpace(where_naideno))
            throw new ArgumentException("Место нахождения не может быть пустым.");

        if (string.IsNullOrWhiteSpace(who_found_it))
            throw new ArgumentException("Имя находившего не может быть пустым.");
    }

    public void Update(OrderUpdateDTO dto)
    {
        if (!string.IsNullOrEmpty(dto.item))
            item = dto.item;

        if (!string.IsNullOrEmpty(dto.where_naideno))
            where_naideno = dto.where_naideno;

        if (!string.IsNullOrEmpty(dto.Who_found_it))
            Who_found_it = dto.Who_found_it;
    }
}

