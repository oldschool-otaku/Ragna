using System.Security.Cryptography;

namespace Ragna.Characters;

public class Bosses
{
    private static readonly List<Character> _easy = new()
    {
        new Character("DOM MOLCHIT", 100, 25),
        new Character("Emy Ploho", 100, 15),
        new Character("dolboeb", 1, 1)
    };

    private static readonly List<Character> _mid = new()
    {
        new Character("Lord Bueraque", 9999, 10),
        new Character("Ragna", 1500, 25),
        new Character("Real Programmer uwu", 250, 25)
    };

    private static readonly List<Character> _hard = new()
    {
        new Character("Yorushika", 2500, 45),
        new Character("Sewer slut", 4500, 25),
        new Character("my dead girlfriend", 1500, 75)
    };

    /// <summary>
    /// Picks random boss with difficulty
    /// <returns>Random boss</returns>
    /// </summary>
    public static Character BossPick(int choose)
    {
        return choose switch
        {
            1 => _easy[RandomNumberGenerator.GetInt32(_easy.Count)],
            2 => _mid[RandomNumberGenerator.GetInt32(_mid.Count)],
            3 => _hard[RandomNumberGenerator.GetInt32(_hard.Count)],
            4 => BossPick(RandomNumberGenerator.GetInt32(1, 3)),
            _ => throw new ArgumentException("Invalid choice")
        };
    }
}