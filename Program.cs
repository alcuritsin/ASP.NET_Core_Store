List<Product> catalog = new List<Product>();
CreateCatalog();

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();

void CreateCatalog()
{
    //  Начальное заполнение каталога.
    catalog.Clear();
    
    catalog.Add(new Product(name:"Продукт_01", price: 11.1));
    catalog.Add(new Product(name:"Продукт_02", price: 22.2));
    catalog.Add(new Product(name:"Продукт_03", price: 33.3));
}

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