using System.Collections.Concurrent;

namespace ASP.NET_Core_Store;

public class Catalog
{
    private ConcurrentBag<Product> Products;

    public Catalog()
    {
        Products = InitCatalog();
    }

    ConcurrentBag<Product> InitCatalog()
    {
        return new ConcurrentBag<Product>();
    }
    
    public string AddProduct(Product product)
    {
        Products.Add(product);
        // Products.Equals(product);
        string result = String.Empty;

        result += ToString();
        
        result += $"Добавлен: '{product.ToString()}'\n";

        return result;
    }

    public string GetProducts(string? userAgent)
    {
        double multiplier = 1;
        DateTime date = new DateTime(2022, 5, 19);
        
        if (date.DayOfWeek == DayOfWeek.Wednesday)
        {
            //  Наценка 50% в среду
            multiplier *= 1.5;
        }

        switch (userAgent)
        {
            case "Android":
                //  Скидка 10%
                multiplier *= 0.9;
                break;
            case "iPhone":
                //  Наценка 50%
                multiplier *= 1.5;
                break;
        }
        
        return ToString(multiplier);
    }

    private string ToString(double multiplier = 1)
    {
        string result = "Каталог продуктов:\n" +
                        "===================================\n";
        int counter = 1;
        foreach (var product in Products)
        {
            result += $"{counter++}. {product.ToString(multiplier)}\n";
        }
        
        result += "===================================\n" +
                  $"Количество записей: {Products.Count}\n";

        return result;
    }
    
}