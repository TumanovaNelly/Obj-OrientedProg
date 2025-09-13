namespace Obj_OrientedProg.Lab1.Core.Models;

public class VendingMachine
{
    private readonly Wallet _revenueMoney = new Wallet();
    private readonly ProductStorage _productStorage = new ProductStorage();

    private int _depositedAmount;

    public void AcceptCoin(Coin coin)
    {
        _depositedAmount += (int)coin.NominalValue;
        _revenueMoney.PutCoin(coin);
    }
    
    public HashSet<Coin> ReturnDepositedAmount()
    {
        _depositedAmount = 0;
        return _revenueMoney.GetCoinsForAmountOf(_depositedAmount);
    }

    public HashSet<Coin> StealRevenue()
    {
        return _revenueMoney.GetAllCoins();
    }
        
    public Product BuyProduct(string productName)
    {
        ProductBox productBox = _productStorage.GetProductBox(productName);
        if (productBox.PricePerProduct > _depositedAmount)
            throw new ApplicationException("You do not have enough money to buy this product");
        
        _depositedAmount -= productBox.PricePerProduct;
        return productBox.GetProduct();
    }

    public void AddProduct(Product product)
    {
        _productStorage.PutProduct(product);
    }
}