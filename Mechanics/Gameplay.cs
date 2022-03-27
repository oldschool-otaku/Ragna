using System.Security.Cryptography;

namespace Ragna.Mechanics;

public class Gameplay
{
    /// <summary>
    /// Throw 2 dices.
    /// <returns> a number between 1 and 12 </returns>
    /// </summary>
    internal static int ThrowTheDice() => RandomNumberGenerator.GetInt32(1, 12);
}