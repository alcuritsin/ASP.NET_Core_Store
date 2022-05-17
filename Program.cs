List<Product> catalog = new List<Product>();    //  Локальная переменная для хранения каталога

IHeaderDictionary headers = new HeaderDictionary(); //  Локальная переменная для хранения HTTP-заголовков из предидущего запроса
string path = String.Empty; //  Локальная переменная для хранения пути предидущего запроса

CreateCatalog();

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", Hello);
app.MapGet("/catalog", GetCatalog);
app.MapPost("/catalog/add", AddProduct);
app.MapGet("/headers", HeadersToString);

app.Run();

string Hello(HttpContext context)
{
    SaveHeaders(context);

    return "ASP.NET_Core-Store";
}

void CreateCatalog()
{
    //  Начальное заполнение каталога.
    catalog.Clear();
    
    catalog.Add(new Product(name:"Продукт_01", price: 11.1));
    catalog.Add(new Product(name:"Продукт_02", price: 22.2));
    catalog.Add(new Product(name:"Продукт_03", price: 33.3));
}

string CatalogToString()
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

string GetCatalog(HttpContext context)
{
    //  Возвращает каталог
    SaveHeaders(context);
    
    return CatalogToString();
}

string AddProduct(Product product, HttpContext context)
{
    //  Добавление продукта в каталог
    SaveHeaders(context);

    catalog.Add(product);

    return CatalogToString();
}

void SaveHeaders(HttpContext context)
{
    //  Сохранить текущие HTML-заголовки и текущий путь, для возможности использовать их в эндпоинте /headers
    headers = context.Request.Headers;
    path = context.Request.Path.ToString();

}

string HeadersToString()
{
    //  Возвращает все HTTP загаловки сохранённого запроса в виде строки
    // if (request == null) return "Ошибка: Запрос пустой";
    if (path == String.Empty) return "Ошибка: Предыдущий запрос не сохранен.";
    
    string result = String.Empty;
    
    result += $"Last request path:= {path}\n\n";

    foreach (var header in headers)
    {
        result += $"{header.Key} := {header.Value}\n";
    }

    headers = new HeaderDictionary();
    path = String.Empty;
    
    return result;
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