using Obj_OrientedProg.Lab0.Core.Models;

namespace Obj_OrientedProg.Lab0.Contracts.DTOs;

public record WalletInfo(int TotalAmount, IReadOnlyDictionary<NominalValue, int> CoinCounts);