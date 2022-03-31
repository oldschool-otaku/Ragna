using System.Security.Cryptography;

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
        string? input = Console.ReadLine();
        int intInput;
        while (!int.TryParse(input, out intInput) || Convert.ToInt32(input) > limit || Convert.ToInt32(input) < 1)
        {
            Console.WriteLine(input);
            Console.Write(">> ");
            input = Console.ReadLine();
        }
        return Convert.ToInt32(intInput - 1);
    }

}