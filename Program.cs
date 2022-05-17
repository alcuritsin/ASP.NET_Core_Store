List<Product> catalog = new List<Product>();
CreateCatalog();

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "ASP.NET_Core-Store");
app.MapGet("/catalog", GetCatalog);
app.MapPost("/catalog/add", AddProduct);

app.Run();

void CreateCatalog()
{
    //  Начальное заполнение каталога.
    catalog.Clear();
    
    catalog.Add(new Product(name:"Продукт_01", price: 11.1));
    catalog.Add(new Product(name:"Продукт_02", price: 22.2));
    catalog.Add(new Product(name:"Продукт_03", price: 33.3));
}

string GetCatalog()
{
    //  Возвращает каталог в текствовом формате
    int counter = 1; 
    string result = "Каталог продуктов:\n\n";
    
    foreach (var product in catalog)
    {
        result += $"{counter++}. {product.ToString()}\n";
    }

    result += $"\n===================================\n" +
              $"Количество записей: {catalog.Count}";
    
    return result;
}

string AddProduct(Product product)
{
    //  Добавление продукта в каталог
    catalog.Add(product);

    return GetCatalog();
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