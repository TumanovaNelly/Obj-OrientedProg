namespace Obj_OrientedProg.Lab0.Core.Models;

public class ProductBox
{
    private readonly Queue<Product> _products = [];
    
    private int _pricePerProduct;
    public int PricePerProduct
    {
        get => _pricePerProduct;
        set
        {
            if (value < 0) 
                throw new ArgumentException("Price cannot be negative");
            _pricePerProduct = value;
        }
    }
    
    public int GetProductCount() => _products.Count;

    public void AddProduct(Product product) => _products.Enqueue(product);
    
    public Product GetProduct() => _products.Dequeue();
}