namespace Ragna.Menu;

public static class Starting
{
    internal static string _name = "";
    internal static string _class = "";
    internal static string _intelligence = "";
    internal static string _strength = "";
    internal static string _defense = "";

    public static void Welcome()
    {
        Console.WriteLine("Welcome to Ragna!");
        Console.WriteLine("This is console game.");
        Console.WriteLine("You can go on raids, which is nice");
        Console.WriteLine("By the way, what's your name?");

        SetName();
        Console.Clear();

        Console.WriteLine("So, your name is {0}?", _name);
        Console.WriteLine("Let's create your character, shall we?");
        Console.WriteLine("DD, Heal or tank");

        SetClass();
        Console.Clear();

        Console.WriteLine("Let's make a build");
        SetAttributes();
        Console.WriteLine("Now we're talking");
        Console.WriteLine("Let's start playing this game");
        Thread.Sleep(500);
    }

    private static int ChechSum()
    {
        return Convert.ToInt32(_defense) + Convert.ToInt32(_intelligence) + Convert.ToInt32(_strength) != 15
            ? 0
            : 1;
    }

    private static void SetName()
    {
        _name = Console.ReadLine()!;
        while (_name.Length is 0 or > 16) _name = Console.ReadLine()!;
    }

    private static void SetClass()
    {
        _class = Console.ReadLine()!;
        while (_class is not ("DD" or "Heal" or "Tank"))
        {
            Console.WriteLine("Please type your class: ");
            _class = Console.ReadLine()!;
        }
    }

    private static void SetAttributes()
    {
        while (true)
        {
            Console.WriteLine("Please type your Strength: ");
            _strength = Console.ReadLine()!;
            while (Convert.ToInt32(_strength) is > 10 or 0)
            {
                Console.WriteLine("Please type your Strength: ");
                _strength = Console.ReadLine()!;
            }

            Console.WriteLine("Please type your Intelligence: ");
            _intelligence = Console.ReadLine()!;
            while (Convert.ToInt32(_intelligence) is > 10 or 0)
            {
                Console.WriteLine("Please type your Intelligence: ");
                _intelligence = Console.ReadLine()!;
            }

            Console.WriteLine("Please type your defense: ");
            _defense = Console.ReadLine()!;
            while (Convert.ToInt32(_defense) is > 10 or 0)
            {
                Console.WriteLine("Please type your Intelligence: ");
                _defense = Console.ReadLine()!;
            }

            if (ChechSum() == 1) break;
        }
    }
}