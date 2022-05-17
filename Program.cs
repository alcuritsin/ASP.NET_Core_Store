var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();

public class Product
{
    public Product(string name, double price)
    {
        this.Name = name;
        this.Price = price;
    }

    public override string ToString()
    {
        return $"Товар: {this.Name}; цена: {this.Price}";
    }

    public string Name { get; set; }
    public double Price { get; set; }
}