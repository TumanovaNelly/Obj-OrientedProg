using Obj_OrientedProg.Lab1.Core.Models;

namespace Obj_OrientedProg.Lab1.Contracts.DTOs;

public record WalletInfo(int TotalAmount, IReadOnlyDictionary<NominalValue, int> CoinCounts);