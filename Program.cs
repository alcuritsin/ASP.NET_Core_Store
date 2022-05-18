using System.Runtime.Serialization.Json;
using Microsoft.Extensions.Primitives;

List<Product> catalog = new List<Product>();    //  Локальная переменная для хранения каталога

IHeaderDictionary headers = new HeaderDictionary(); //  Локальная переменная для хранения HTTP-заголовков из предыдущего запроса
string path = String.Empty; //  Локальная переменная для хранения пути предыдущего запроса

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

string CatalogToString(List<Product> _catalog)
{
    //  Возвращает каталог в текстовом формате
    int counter = 1; 
    string result = "Каталог продуктов:\n" +
                    "===================================\n";
    
    foreach (var product in _catalog)
    {
        result += $"{counter++}. {product.ToString()}\n";
    }

    result += "===================================\n" +
              $"Количество записей: {_catalog.Count}\n";
    
    return result;
}

List<Product> ListClone(List<Product> source)
{
    //  Создаёт клон каталога
    List<Product> clone = new List<Product>();
    foreach (Product product in source)
    {
        clone.Add(new Product(product.Name, product.Price));
    }

    return clone;
}

string GetCatalog(HttpContext context)
{
    //  Возвращает каталог
    SaveHeaders(context);
    
    string result = String.Empty;   //  Результирующая строка для вывода
    List<Product> resCatalog = ListClone(catalog);  //  Копия каталога для вывода
    
    //  Пересчёт стоимости в среду.
    var hDate = context.Request.Headers.Date.ToString();
    
    //  DebugData
    result += $"hDate: '{hDate}'\n";

    if (hDate != "")
    {
        DateTime date = DateTime.ParseExact(context.Request.Headers.Date.ToString(), "d.m.yyyy",
            System.Globalization.CultureInfo.InvariantCulture);
        var dayOfWeek = (int) date.DayOfWeek;

        if (dayOfWeek == 3)
        {
            MulPrice(resCatalog, 1.5);
        }
    }

    //  Определение коэффициента в зависимости от устройства
    string userAgent = context.Request.Headers.UserAgent.ToString();
    
    //  DebugData
    result += $"userAgent: '{userAgent}'\n\n";

    if (userAgent != "")
    {
        double multiplier = 0;
        if (userAgent.Contains("Android"))
        {
            //  Скидка 10%
            multiplier = 0.9;
        }
        else if (userAgent.Contains("iPhone"))
        {
            //  Наценка 50%
            multiplier = 1.5;
        }

        if (multiplier != 0)
        {
            MulPrice(resCatalog, multiplier);
        }
    }
    
    //  Вывести копию каталога зависящую от условий
    result += CatalogToString(resCatalog);
    
    //  Показать оригинальные значения каталога
    result += "\n\nОригинал\t" + CatalogToString(catalog);
    
    return result;
}

void MulPrice(List<Product> source, double multiplier)
{
    //  Изменяет стоимость в каталоге умножением на коэффициент (multiplier).
    foreach (var product in source)
    {
        product.Price = Math.Round(product.Price * multiplier, 2);
    }
}

string AddProduct(Product product, HttpContext context)
{
    //  Добавление продукта в каталог
    SaveHeaders(context);

    string result = String.Empty;
    
    catalog.Add(product);

    result += CatalogToString(catalog);

    result += $"Добавлен: '{product.ToString()}'\n";
    
    return result;
}

void SaveHeaders(HttpContext context)
{
    //  Сохранить текущие HTML-заголовки и текущий путь, для возможности использовать их в 'end point' /headers
    headers = context.Request.Headers;
    path = context.Request.Path.ToString();
}

string HeadersToString()
{
    //  Возвращает все HTTP заголовки сохранённого запроса в виде строки
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