using System.Security.Cryptography;
using Ragna.timur_version;

namespace Ragna.Mechanics;

public class Gameplay
{
    /// <summary>
    /// Throw 2 dices.
    /// <returns> a number between 1 and 12 </returns>
    /// </summary>
    internal static int ThrowTheDice() => RandomNumberGenerator.GetInt32(1, 12);
    
    public static int VerfiedInput(int limit)
    {
        Console.Write(">> ");
        int input = Convert.ToInt32(Console.ReadLine());
        while (input > limit || input < 1)
        {
            Console.WriteLine(input);
            Console.Write(">> ");
            input = Convert.ToInt32(Console.ReadLine());
        }
        return input - 1;
    }
    
    public static string GetCharsNames(List<Character> ls)
    {
        return Enumerable.Range(0, ls.Count).Aggregate("", (current, i) => current + $"{i + 1}: {ls[i].Name}   ");
    }
}