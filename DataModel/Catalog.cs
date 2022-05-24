using System.Collections.Concurrent;

namespace DataModel;

public class Catalog
{
    private ConcurrentBag<Product> Products;

    public Catalog()
    {
        Products = InitCatalog();
    }

    private static ConcurrentBag<Product> InitCatalog()
    {
        return new ConcurrentBag<Product>();
    }
    
    public void AddProduct(Product product)
    {
        Products.Add(product);
    }

    public List<Product> GetProducts(string? userAgent)
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

        List<Product> layoutCatalog = new List<Product>();

        foreach (Product product in Products)
        {
            layoutCatalog.Add(
                new Product(
                    name: product.Name,
                    price: Math.Round(product.Price * multiplier, 2)
                    ));
        }

        return layoutCatalog;
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