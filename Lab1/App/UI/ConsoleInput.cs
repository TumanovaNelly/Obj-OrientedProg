using Obj_OrientedProg.Lab1.Core.Models;

namespace Obj_OrientedProg.Lab1.App.UI;

public static class ConsoleInput
{

    public static bool TryReadWord(out string word)
    {
        string? input = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(input))
        {
            word = input;
            return true;
        }
        
        word = string.Empty;
        return false;
    }

    public static bool TryReadWords(out List<string> words)
    {
        if (!TryReadWord(out string wordsData))
        {
            words = [];
            return false;
        }
            
        words = wordsData
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToList();
        
        return true;
    }

    public static bool TryReadNumber(out int number)
    {
        number = 0;
        return TryReadWord(out string wordsData) && int.TryParse(wordsData, out number);
    }

    public static bool TryReadNominals(out List<int> validNominals, out List<string> invalidNominals)
    {
        validNominals = [];
        invalidNominals = [];
        
        if (!TryReadWords(out var nominalsData))
            return false;

        foreach (var nominalData in nominalsData)
        {
            if (!int.TryParse(nominalData, out int nominal) || !Enum.IsDefined(typeof(NominalValue), nominal))
                invalidNominals.Add(nominalData);
            else validNominals.Add(nominal);
        }
        
        return true;
    }
}