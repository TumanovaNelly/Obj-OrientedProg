using Obj_OrientedProg.Lab1.Contracts.DTOs;

namespace Obj_OrientedProg.Lab1.Core.Models;

public class VendingMachine
{
    private readonly Wallet _revenueMoney = new();

    private readonly ProductStorage _productStorage = new();
    public int DepositedAmount { get; private set; }

    public void AcceptCoin(Coin coin)
    {
        DepositedAmount += (int)coin.NominalValue;
        PutCoinInCashRegister(coin);
    }

    public void PutCoinInCashRegister(Coin coin)
    {
        _revenueMoney.PutCoin(coin);
    }
    
    public List<Coin> ReturnDepositedAmount()
    {
        int toReturnAmount = DepositedAmount;
        DepositedAmount = 0;
        return _revenueMoney.GetCoinsForAmountOf(toReturnAmount);
    }

    public List<Coin> StealRevenue()
    {
        return _revenueMoney.GetAllCoins();
    }
        
    public Product BuyProduct(string productName)
    {
        var price = _productStorage.GetProductPrice(productName);
        
        if (price > DepositedAmount)
            throw new ApplicationException($"You do not have enough money to buy product \"{productName}\"");
        
        DepositedAmount -= price;
        return _productStorage.GetProduct(productName);
    }

    public void AddProductToStorage(Product product) => _productStorage.PutProduct(product);
    
    public void ChangeProductsPrice(string productName, int newPrice) => 
        _productStorage.ChangeProductPrice(productName, newPrice);

    public List<ProductInfo> GetProductStorageInfo() => _productStorage.GetAllProductsInfo();
    
    public ProductInfo GetProductInfo(string productName) => _productStorage.GetProductInfo(productName);
    public WalletInfo GetRevenueMoneyInfo() => _revenueMoney.GetWalletInfo();
    
}