namespace ASP.NET_Core_Store;

public class Product
{
    public Product(string name, double price)
    {
        this.Name = name;
        this.Price = price;
    }

    public override string ToString()
    {
        return $"Товар: {this.Name}; цена: {Math.Round(this.Price, 2)}";
    }

    public string ToString(double multiplier)
    {
        return $"Товар: {this.Name}; цена: {Math.Round(this.Price * multiplier, 2)}";
    }
    public string Name { get; set; }
    public double Price { get; set; }

}