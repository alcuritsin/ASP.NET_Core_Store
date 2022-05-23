using System.Runtime.Serialization.Json;
using ASP.NET_Core_Store;
using Microsoft.Extensions.Primitives;

Catalog catalog = new Catalog();    //  Локальная переменная для хранения каталога

IHeaderDictionary headers = new HeaderDictionary(); //  Локальная переменная для хранения HTTP-заголовков из предыдущего запроса
string path = String.Empty; //  Локальная переменная для хранения пути предыдущего запроса

CreateCatalog();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapGet("/", Hello);
app.MapGet("/catalog", GetCatalog);
app.MapPost("/catalog/add", AddProduct);
app.MapGet("/headers", HeadersToString);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();

string Hello(HttpContext context)
{
    SaveHeaders(context);

    return "ASP.NET_Core-Store";
}

void CreateCatalog()
{
    //  Начальное заполнение каталога.
    
    catalog.AddProduct(new Product(name:"Продукт_01", price: 11.1));
    catalog.AddProduct(new Product(name:"Продукт_02", price: 22.2));
    catalog.AddProduct(new Product(name:"Продукт_03", price: 33.3));
}

string GetCatalog(HttpContext context)
{
    //  Возвращает каталог
    SaveHeaders(context);
    
    string result = String.Empty;   //  Результирующая строка для вывода
    
    string userAgent = context.Request.Headers.UserAgent.ToString();

    //  DebugData
    result += $"userAgent: '{userAgent}'\n\n";
    
    //  Вывести каталог зависящий от условий
    result += catalog.GetProducts(userAgent);
    
    return result;
}

string AddProduct(Product product, HttpContext context)
{
    //  Добавление продукта в каталог
    SaveHeaders(context);
    
    return catalog.AddProduct(product);
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