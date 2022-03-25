using Ragna.Characters;
using Ragna.Menu;

namespace Ragna;

internal static class Program
{
    public static readonly Player Player1 = new(Starting._class,
        Convert.ToInt32(Starting._strength),
        Convert.ToInt32(Starting._intelligence),
        Convert.ToInt32(Starting._defense));

    private static void Main()
    {
        // всё это я написал под Russian Bzdoomer Mix vol. 6
        // следующая часть кода была написана под буерак и молчат дома
        Starting.Welcome();
        Player1.SetProperties();
        Tutorial.Start(Player1);

        Console.Clear();
        MainMenu.Start();
    }
}