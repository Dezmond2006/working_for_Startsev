var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

// ���������� ��� ������ ��� ������ � ���� �������


var orders = new List<Nahodki>
{
    new Nahodki(1001, 14, 02, 2025, "�����_SC1000", "�������", "������_�����������_����������"),
};

// ��������� ���� �������
app.MapGet("/", () => Results.Ok(orders));

// ���������� ������ ������
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

// ���������� ������������� ������
app.MapPut("/{number}", (int number, OrderUpdateDTO dto) =>
{
    var existingOrder = orders.FirstOrDefault(o => o.Number == number);
    if (existingOrder == null)
        return Results.NotFound("���� �� �������");

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

// �������� ������
app.MapDelete("/{number}", (int number) =>
{
    var orderToRemove = orders.FirstOrDefault(o => o.Number == number);
    if (orderToRemove == null)
        return Results.NotFound("���� �� �������");

    orders.Remove(orderToRemove);
    return Results.NoContent();
});

app.Run();

// DTO ��� ���������� ����
public class OrderUpdateDTO
{
    public string item { get; set; }
    public string where_naideno { get; set; }
    public string Who_found_it { get; set; }
}

// ����� ��������� ��������
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
            throw new ArgumentException("����� ������ ���� �������������.");

        if (day < 1 || day > 31)
            throw new ArgumentException("���� ������ ���� �� 1 �� 31.");

        if (month < 1 || month > 12)
            throw new ArgumentException("����� ������ ���� �� 1 �� 12.");

        if (year < DateTime.Now.Year)
            throw new ArgumentException("��� �� ����� ���� ������ ��������.");

        if (string.IsNullOrWhiteSpace(item))
            throw new ArgumentException("�������� ���� �� ����� ���� ������.");

        if (string.IsNullOrWhiteSpace(where_naideno))
            throw new ArgumentException("����� ���������� �� ����� ���� ������.");

        if (string.IsNullOrWhiteSpace(who_found_it))
            throw new ArgumentException("��� ����������� �� ����� ���� ������.");
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

