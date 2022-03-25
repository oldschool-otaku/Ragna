using Ragna.Mechanics;

namespace Ragna.Menu;

public class MainMenu
{
    private static string _picked = "";

    public static void Start()
    {
        while (_picked is not "1" or "2")
        {
            Console.WriteLine("{0}, what do you want to do?", Program.Player1.Name);
            Console.WriteLine("1 = Go on raid");
            Console.WriteLine("2 = Tutorial");
            Console.WriteLine("9 = Leave");
            _picked = Console.ReadLine()!;
        }

        Pick();
    }

    private static void Pick()
    {
        switch (_picked)
        {
            case "1":
                Fight.Start(Program.Player1);
                break;

            case "2":
                Tutorial.Start(Program.Player1);
                break;

            case "9":
                break;

            default:
                _picked = Console.ReadLine()!;
                break;
        }
    }
}