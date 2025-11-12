using Obj_OrientedProg.Lab0.Contracts.DTOs;

namespace Obj_OrientedProg.Lab0.Core.Models;

public class Human
{
    private readonly Wallet _wallet = new();

    private readonly Queue<Product> _productsPackage = [];

    public void GetSalary(List<Coin> money) => _wallet.PutMoney(money);

    public Coin SpendSalary(NominalValue nominalValue) => _wallet.GetCoin(nominalValue);
    
    public void GetProduct(Product product) => _productsPackage.Enqueue(product);

    public HumanInfo GetInfo() => new(GetWalletInfo(), GetProductsInfo());

    private WalletInfo GetWalletInfo() => _wallet.GetWalletInfo();

    private List<string> GetProductsInfo() => _productsPackage.Select(product => product.Name).ToList();
}