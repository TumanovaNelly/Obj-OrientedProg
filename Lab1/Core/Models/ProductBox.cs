namespace Obj_OrientedProg.Lab1.Core.Models;

public class ProductBox
{
    private readonly Queue<Product> _products = [];
    
    private int _pricePerProductPerProduct;
    public int PricePerProduct
    {
        get => _pricePerProductPerProduct;
        set
        {
            if (value < 0) 
                throw new ArgumentException("Price cannot be negative");
            _pricePerProductPerProduct = value;
        }
    }
    
    public int GetProductCount() => _products.Count;

    public void AddProduct(Product product) => _products.Enqueue(product);
    
    public Product GetProduct() => _products.Dequeue();
}