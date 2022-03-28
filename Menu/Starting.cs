using System.Security.Cryptography;
using Ragna.Mechanics;

namespace Ragna.Menu;

public static class Starting
{
    private static int _pick;
    internal static string _name = "";
    internal static string _class = "";
    internal static string _intelligence = "";
    internal static string _strength = "";
    internal static string _defense = "";
    
    private static List<string> _randNames = new()
    {
        "Sasha", "Egor", "Pivozavr", "God of Bueraque", "Sex12", "Lord Rings", "Why so serious?",
        "dolboeb", "Master of uwu", "femboy hooters"
    };

    public static void Welcome()
    {
        Console.WriteLine("Welcome to Ragna!");
        Console.WriteLine("This is console game.");
        Console.WriteLine("You can go on raids, which is nice");
        
        Console.WriteLine("Do you want to play with random stats(1) or you want to create your own build?(2)");
        while(_pick is not 1 or 2)
            _pick = Convert.ToInt32(Console.ReadLine());

        if (_pick == 1)
            RandomStats();
        else
            CreatePlayer();

        Console.WriteLine("So, you're {0} and you have this stats", _name);
        Console.WriteLine("Intelligence = {0}, Defence = {1}, Strength = {2}", _intelligence, _defense, _strength);
        
        Console.WriteLine("Let's start playing this game");
        Thread.Sleep(500);
    }

    private static int ChechSum() => 
        Convert.ToInt32(_defense) 
        + Convert.ToInt32(_intelligence) 
        + Convert.ToInt32(_strength) 
        != 15 
        ? 0 : 1;

    private static void CreatePlayer()
    {
        Console.WriteLine("What's your name?");
        _name = Console.ReadLine()!;
        while (_name.Length is 0 or >= 32) 
            _name = Console.ReadLine()!;
        Console.Clear();

        Console.WriteLine("So, your name is {0}?", _name);
        Console.WriteLine("Let's create your character, shall we?");
        Console.WriteLine("DD, Heal or tank");

        _class = Console.ReadLine()!;
        while (_class is not ("DD" or "Heal" or "Tank"))
            _class = Console.ReadLine()!;
        Console.Clear();

        Console.WriteLine("Let's make a build");
        SetAttributes();
        Console.WriteLine("Now we're talking");
    }


    private static void SetAttributes()
    {
        while (true)
        {
            Console.WriteLine("Please type your Strength: ");
            _strength = Console.ReadLine()!;
            while (Convert.ToInt32(_strength) is > 10 or 0)
                _strength = Console.ReadLine()!;
            

            Console.WriteLine("Please type your Intelligence: ");
            _intelligence = Console.ReadLine()!;
            while (Convert.ToInt32(_intelligence) is > 10 or 0)
                _intelligence = Console.ReadLine()!;
            

            Console.WriteLine("Please type your defense: ");
            _defense = Console.ReadLine()!;
            while (Convert.ToInt32(_defense) is > 10 or 0)
                _defense = Console.ReadLine()!;

            if (ChechSum() == 1) break;
        }
    }

    private static string RandClass() => 
        Gameplay.ThrowTheDice() < 3 ? _class = "DD" : 
        Gameplay.ThrowTheDice() < 5 ? _class = "Tank" : _class = "Heal";
    

    private static void RandomStats()
    {
        _name = _randNames[RandomNumberGenerator.GetInt32(_randNames.Count)];
        _class = RandClass();
        while (true)
        {
            _strength = Convert.ToString(RandomNumberGenerator.GetInt32(5));
            _intelligence = Convert.ToString(RandomNumberGenerator.GetInt32(5));
            _defense = Convert.ToString(15 - Convert.ToInt32(_strength) - Convert.ToInt32(_intelligence));
            
            if(ChechSum() == 1) break;
        }
    }
}