using Ragna.Characters;
using Ragna.Menu;

namespace Ragna.Mechanics;

public class Fight
{
    private static string _pick = "";
    private static string _bosspick = "";
    private static Character Boss = null!;
    private static int _turnNum;

    /// <summary>
    ///     Start the fight
    /// </summary>
    /// <param name="p">Player</param>
    public static void Start(Player p)
    {
        p.SetProperties();
        while (true)
        {
            Console.WriteLine("Please select difficulty");
            Console.WriteLine("1. Easy      2. Normal       3. Hard     4. Random   0. Leave");

            _bosspick = Console.ReadLine()!;

            if (_bosspick is "1" or "2" or "3" or "4" or "0") break;
        }

        if (_bosspick is "0") MainMenu.Start();

        Boss = Bosses.BossPick(Convert.ToInt32(_bosspick));
        Thread.Sleep(1000);
        Console.Clear();
        MainFight(p);
    }

    /// <summary>
    ///     Basically a turn mechanic
    /// </summary>
    /// <param name="player">Player</param>
    private static void MainFight(Player player)
    {
        Console.WriteLine("Your enemy is {0}", Boss.Name);
        Console.WriteLine("He has {0} HP", Boss.Health);
        Console.WriteLine("His damage is {0}", Boss.Damage);
        Console.WriteLine("");

        Thread.Sleep(1000);

        while (true)
        {
            _turnNum++;

            Console.WriteLine("Turn: {0}", _turnNum);
            Console.WriteLine("{0}'s HP: {1}", Boss.Name, Boss.Health);
            Console.WriteLine("{0}'s HP: {1}, Mana: {2}", player.Name, player.Health, player.Mana);

            Thread.Sleep(500);

            FightMenu(player);

            if (Boss.Health <= Boss.Health * 0.2) AI.HealChoice(Boss, player);
            else AI.RandomChoice(Boss, player);

            Console.WriteLine("+15 Mana");
            Thread.Sleep(1500);
            Console.WriteLine("");

            player.RegenMana();
            FinishFight(player);
        }
    }

    /// <summary>
    ///     Player's fight menu
    /// </summary>
    /// <param name="player">Player</param>
    private static void FightMenu(Player player)
    {
        switch (player.Class)
        {
            // TANK
            case "Tank":
            {
                while (true)
                {
                    Console.WriteLine("1. Attack        2. Take Damage     3. Heal yourself(50 mana)");
                    _pick = Console.ReadLine()!;
                    if (_pick is "1" or "2" or "3") break;
                }

                Console.WriteLine("");
                switch (_pick)
                {
                    case "1":
                        player.DealDamage(player, Boss);
                        break;

                    case "2":
                        Boss.ForcePick(Boss, player);
                        break;

                    case "3":
                        player.GetSelfHealed();
                        break;
                }

                break;
            }

            // HEAL
            case "Heal":
            {
                while (true)
                {
                    Console.WriteLine("1. Attack        2. Heal someone(25 mana)      3. Heal yourself(50 mana)");
                    _pick = Console.ReadLine()!;
                    if (_pick is "1" or "2" or "3") break;
                }

                Console.WriteLine("");
                switch (_pick)
                {
                    case "1":
                        player.DealDamage(player, Boss);
                        break;

                    case "2":
                        Player.Heal(player, Boss);
                        break;

                    case "3":
                        player.GetSelfHealed();
                        break;
                }

                break;
            }

            // DD
            case "DD":
            {
                while (true)
                {
                    Console.WriteLine("1. Attack        2. Power Attack(50 mana)    3. Heal yourself(50 mana)");
                    _pick = Console.ReadLine()!;
                    if (_pick is "1" or "2" or "3") break;
                }

                switch (_pick)
                {
                    case "1":
                        player.DealDamage(player, Boss);
                        break;

                    case "2":
                        player.PowerAttack(player, Boss);
                        break;

                    case "3":
                        player.GetSelfHealed();
                        break;
                }

                break;
            }
        }
    }

    /// <summary>
    ///     Check if boss or player is dead
    /// </summary>
    /// <param name="player">Player</param>
    private static void FinishFight(Player player)
    {
        if (Boss.IsDead())
        {
            player.RestoreStats();
            Boss.RestoreCharacter();
            _turnNum = 0;
            MainMenu.Start();
        }

        if (!player.IsDead()) return;
        player.RestoreStats();
        Boss.RestoreCharacter();
        _turnNum = 0;
        MainMenu.Start();
    }
}