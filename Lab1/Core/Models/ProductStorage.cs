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
    
    public void PutProducts(HashSet<Product> products)
    {
        foreach (var product in products)
            PutProduct(product);
    }

    public void ChangeProductPrice(string productName, int newPrice)
    {
        if (!_storage.TryGetValue(productName, out var box))
            throw new KeyNotFoundException("Product not found");
        
        box.PricePerProduct = newPrice;
    }

    public ProductBox GetProductBox(string productName)
    {
        if (!_storage.TryGetValue(productName, out var box) || box.GetProductCount() == 0)
            throw new ArgumentException($"There's no {productName} product box");

        return box;
    }
}