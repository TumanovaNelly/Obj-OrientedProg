using Obj_OrientedProg.Lab1.Contracts.DTOs;

namespace Obj_OrientedProg.Lab1.Core.Models;

public class ProductStorage
{
    private readonly Dictionary<string, ProductBox> _storage = new();

    public void PutProduct(Product product)
    {
        var productName = product.Name;
        
        if (!_storage.ContainsKey(productName))
            _storage[productName] = new ProductBox();
        _storage[productName].AddProduct(product);
    }

    public Product GetProduct(string productName)
    {
        var box = GetBox(productName);
        if (box.GetProductCount() == 0)
            throw new KeyNotFoundException($"There's no \"{productName}\" product");

        return box.GetProduct();
    }

    public int GetProductPrice(string productName) => GetBox(productName).PricePerProduct;
    public void ChangeProductPrice(string productName, int newPrice) => GetBox(productName).PricePerProduct = newPrice;
    
    public List<ProductInfo> GetAllProductsInfo() => 
        _storage.Select(kvp => new ProductInfo(kvp.Key, kvp.Value.PricePerProduct, kvp.Value.GetProductCount()))
            .ToList();

    public ProductInfo GetProductInfo(string productName)
    {
        var box = GetBox(productName);
        return new ProductInfo(productName, box.PricePerProduct, box.GetProductCount());
    }

    private ProductBox GetBox(string productName)
    {
        if (!_storage.TryGetValue(productName, out var box))
            throw new KeyNotFoundException($"Product \"{productName}\" not found");
        
        return box;
    }
}