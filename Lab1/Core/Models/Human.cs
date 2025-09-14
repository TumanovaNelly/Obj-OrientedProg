using Obj_OrientedProg.Lab1.Contracts.DTOs;

namespace Obj_OrientedProg.Lab1.Core.Models;

public class Human
{
    private readonly Wallet _wallet = new Wallet();

    private readonly Queue<Product> _productsPackage = [];

    public void GetSalary(List<Coin> money) => _wallet.PutMoney(money);

    public Coin SpendSalary(NominalValue nominalValue) => _wallet.GetCoin(nominalValue);
    
    public WalletInfo GetWalletInfo() => _wallet.GetWalletInfo();
    
    public void GetProduct(Product product) => _productsPackage.Enqueue(product);

    public Product EatProduct()
    {
        var product = _productsPackage.Dequeue();
        product.ToHarmed();
        return product;
    }
    
}