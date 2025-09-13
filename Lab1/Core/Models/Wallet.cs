namespace Obj_OrientedProg.Lab1.Core.Models;

public class Wallet
{
    public int TotalAmount { get; private set; }
    
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
        
        TotalAmount += (int)nominalValue;
    }
    
    public void PutMoney(HashSet<Coin> coins)
    {
        foreach (var coin in coins)
            PutCoin(coin);
    }

    public Coin GetCoin(NominalValue nominalValue)
    {
        if (!_fund.TryGetValue(nominalValue, out var coins) || coins.Count == 0)
            throw new ArgumentException($"There's no {nominalValue} coin");
        
        TotalAmount -= (int)nominalValue;
        
        return _fund[nominalValue].Dequeue();
    }

    public HashSet<Coin> GetAllCoins()
    {
        TotalAmount = 0;
        return _fund.Values.SelectMany(coinQueue => coinQueue).ToHashSet();
    } 

    public HashSet<Coin> GetCoinsForAmountOf(int requestedAmount)
    {
        HashSet<Coin> coinsOut = [];

        foreach (var nominalValue in _descendingValues)
        {
            if (!_fund.TryGetValue(nominalValue, out var coins))
                continue;
            
            int count = Math.Min(coins.Count, requestedAmount / (int)nominalValue);

            for (int i = 0; i < count; i++)
            {
                GetCoin(nominalValue);
                requestedAmount -= (int)nominalValue;
            }
        }

        if (requestedAmount == 0) return coinsOut;
        
        PutMoney(coinsOut);
        throw new ArgumentException($"Cannot get coins for amount of {requestedAmount} coins");
    }
}