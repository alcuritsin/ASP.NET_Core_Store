using System.Net.Http.Json;

using DataModel;

public class StoreLib
{
    private readonly string _host;
    private readonly HttpClient _httpClient;

    public StoreLib(string host, HttpClient? httpClient = null)
    {
        _host = host;
        _httpClient = httpClient ?? new HttpClient();
    }

    public async Task<IReadOnlyList<Product>?> GetProduct()
    {
        var uri = $"{_host}/catalog";
        var response = await _httpClient.GetFromJsonAsync<IReadOnlyList<Product>>(uri);
        return response;
    }

    public async void AddProduct(Product product)
    {
        var uri = $"{_host}/catalog/add";
        var response = await _httpClient.PostAsJsonAsync(uri, product);
    }
    
}