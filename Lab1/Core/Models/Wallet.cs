using Obj_OrientedProg.Lab1.Contracts.DTOs;

namespace Obj_OrientedProg.Lab1.Core.Models;

public class Wallet
{
    private int _totalAmount;
    
    private readonly Dictionary<NominalValue, Queue<Coin>> _fund = new();
    
    private readonly List<NominalValue> _descendingValues = 
        ((NominalValue[])Enum.GetValues(typeof(NominalValue)))
        .OrderByDescending(n => (int)n).ToList();
    
    public void PutCoin(Coin coin)
    {
        var nominalValue = coin.NominalValue;
        
        if (!_fund.ContainsKey(nominalValue))
            _fund[nominalValue] = [];
        _fund[nominalValue].Enqueue(coin);
        
        _totalAmount += (int)nominalValue;
    }
    
    public void PutMoney(List<Coin> coins)
    {
        foreach (var coin in coins)
            PutCoin(coin);
    }

    public Coin GetCoin(NominalValue nominalValue)
    {
        if (!_fund.TryGetValue(nominalValue, out var coins) || coins.Count == 0)
            throw new KeyNotFoundException($"There's no {(int)nominalValue}$ coin");
        
        _totalAmount -= (int)nominalValue;
        
        return _fund[nominalValue].Dequeue();
    }

    public List<Coin> GetAllCoins()
    {
        _totalAmount = 0;
        return _fund.Values.SelectMany(coinQueue => coinQueue).ToList();
    } 

    public List<Coin> GetCoinsForAmountOf(int requestedAmount)
    {
        int amount = requestedAmount;
        List<Coin> coinsOut = [];

        foreach (var nominalValue in _descendingValues)
        {
            if (!_fund.TryGetValue(nominalValue, out var coins))
                continue;
            
            int count = Math.Min(coins.Count, amount / (int)nominalValue);

            for (int i = 0; i < count; i++)
            {
                coinsOut.Add(GetCoin(nominalValue));
                amount -= (int)nominalValue;
            }
        }

        if (amount == 0) 
            return coinsOut;
        
        PutMoney(coinsOut);
        throw new ApplicationException($"Cannot get coins for amount of {requestedAmount} coins");
    }

    public WalletInfo GetWalletInfo() => new(_totalAmount, _fund.ToDictionary(
            kvp => kvp.Key, 
            kvp => kvp.Value.Count
        ));
}