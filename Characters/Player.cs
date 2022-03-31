using System.Security.Cryptography;
using Ragna.Mechanics;
using Ragna.Menu;

namespace Ragna.Characters;

public class Player
{
    // система атаки = типо 2 кубика кидают если больше шанса то удар

    private int _mana = 0;
    private int DamageDealt;
    private int maxMana;
    private string? _pick;

    public Player(string @class, int strength, int intelligence, int defense)
    {
        Class = @class;
        Strength = strength;
        Intelligence = intelligence;
        Defense = defense;
        Mana = _mana;
    }

    public string Name => Starting._name;
    public int Health { get; set; }
    protected internal string Class { get; }
    protected internal int Damage { get; set; }
    public int Mana { get; private set; }
    private int Chance { get; set; }
    protected internal int Intelligence { get; set; }
    protected internal int Strength { get; set; }
    protected internal int Defense { get; set; }

    /// <summary>
    /// Setting up player's properties
    /// </summary>
    public void SetProperties()
    {
        switch (Class)
        {
            case "DD":
                Health = 100;
                Damage = RandomNumberGenerator.GetInt32(75, 100) * (Strength / 2);
                Chance = 2;
                Strength += 2;
                Mana = 100;
                maxMana = 100;
                return;

            case "Tank":
                Health = 500;
                Damage = RandomNumberGenerator.GetInt32(35, 50) * (Strength / 2);
                Chance = 4;
                Defense += 2;
                Mana = 100;
                maxMana = 100;
                return;

            case "Heal":
                Health = 100;
                Chance = 4;
                Damage = RandomNumberGenerator.GetInt32(25, 40) * (Strength / 2);
                Intelligence += 2;
                Mana = 250;
                maxMana = 250;
                return;
        }
    }

    /// <summary>
    /// Check if player is dead.
    /// </summary>
    public bool IsDead()
    {
        if (Health > 0) return false;
        Console.WriteLine("You're dead! Loshara blyat...");
        return true;
    }

    /// <summary>
    ///     Function to heal someone.
    /// </summary>
    /// <param name="p">Player</param>
    /// <param name="obj">Character</param>
    private static void Heal(Player p, Character obj)
    {
        p.Mana -= 25;
        int amount = RandomNumberGenerator.GetInt32(10, 25) * (p.Intelligence / 4);
        obj.Health += amount;
        Console.WriteLine("Healing was successful, restored {0}", amount);
    }


    /// <summary>
    /// Dealing damage to Character
    /// </summary>
    /// <param name="p">Player</param>
    /// <param name="obj">Character</param>
    private void DealDamage(Player p, Character obj)
    {
        DamageDealt = RandomNumberGenerator.GetInt32(p.Damage / 2, p.Damage);
        if (Gameplay.ThrowTheDice() < p.Chance) { Console.WriteLine("Attack failed!"); return; }
        
        if (obj.Health - DamageDealt <= 0) { obj.Health = 0; return; }

        obj.Health -= DamageDealt;
        Console.WriteLine("You have dealt {0} damage", DamageDealt);
    }

    /// <summary>
    ///     DD's skill to do a 5x damage
    /// </summary>
    /// <param name="p">Player</param>
    /// <param name="obj">Character</param>
    private void PowerAttack(Player p, Character obj)
    {
        p.Mana -= 50;
        if (obj.Health - p.Damage * 5 <= 0) { obj.Health = 0; return; }

        Console.WriteLine("You have dealt {0} damage", p.Damage * 5);
        obj.Health -= p.Damage * 5;
    }

    /// <summary>
    ///     Self healing.
    /// </summary>
    private void GetSelfHealed()
    {
        Mana -= 50;
        int amount = Class == "Heal"
            ? RandomNumberGenerator.GetInt32(25, 50) * (Intelligence / 4)
            : RandomNumberGenerator.GetInt32(10, 25) * (Intelligence / 4);
        Health += amount;

        Console.WriteLine("Healing was successful, restored {0}", amount);
    }

    /// <summary>
    /// Restoring stats after fight
    /// </summary>
    public void RestoreStats()
    {
        Health = Class == "Tank" ? 500 : 100;
        Mana = Class == "Heal" ? 250 : 100;
    }

    /// <summary>
    /// Regenerating mana
    /// </summary>
    protected internal void RegenMana()
    {
        Mana += 15;
        if (Mana > maxMana) Mana = maxMana;
    }

    /// <summary>
    ///     Bleeding
    /// </summary>
    /// <param name="bleedTurns">Amount of turns with bleeding effect</param>
    protected internal void Bleed(int bleedTurns)
    {
        Bleeding = true;
        int damage = Convert.ToInt32(Health * 0.1);
        Health -= damage;
        Console.WriteLine("You're bleeding! Next {0} turns your health will be drained!", bleedTurns);
        Thread.Sleep(1500);
    }

    public void TankMenu(Player player, Character Boss)
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
                    return;

                case "2":
                    Boss.ForcePick(Boss, player);
                    return;

                case "3":
                    player.GetSelfHealed();
                    return;
            }
    }
    
    public void HealMenu(Player player, Character Boss)
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
                return;

            case "2":
                Player.Heal(player, Boss);
                return;

            case "3":
                player.GetSelfHealed();
                return;
        }
    }

    public void DdMenu(Player player, Character Boss)
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
                return;

            case "2":
                player.PowerAttack(player, Boss);
                return;

            case "3":
                player.GetSelfHealed();
                return;
        }
    }
}