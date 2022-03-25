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
    ///     Setting up player's properties
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
                break;

            case "Tank":
                Health = 500;
                Damage = RandomNumberGenerator.GetInt32(35, 50) * (Strength / 2);
                Chance = 4;
                Defense += 2;
                Mana = 100;
                maxMana = 100;
                break;

            case "Heal":
                Health = 100;
                Chance = 4;
                Damage = RandomNumberGenerator.GetInt32(25, 40) * (Strength / 2);
                Intelligence += 2;
                Mana = 250;
                maxMana = 250;
                break;
        }
    }

    /// <summary>
    ///     Check if player is dead.
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
    public static void Heal(Player p, Character obj)
    {
        p.Mana -= 25;
        int amount = RandomNumberGenerator.GetInt32(10, 25) * (p.Intelligence / 4);
        obj.Health += amount;
        Console.WriteLine("Healing was successful, restored {0}", amount);
    }


    /// <summary>
    ///     Dealing damage to Character
    /// </summary>
    /// <param name="p">Player</param>
    /// <param name="obj">Character</param>
    public void DealDamage(Player p, Character obj)
    {
        DamageDealt = RandomNumberGenerator.GetInt32(p.Damage / 2, p.Damage);
        if (Gameplay.ThrowTheDice() < p.Chance)
        {
            Console.WriteLine("Attack failed!");
            return;
        }

        if (obj.Health - DamageDealt <= 0)
        {
            obj.Health = 0;
            return;
        }

        obj.Health -= DamageDealt;
        Console.WriteLine("You have dealt {0} damage", DamageDealt);
    }

    /// <summary>
    ///     DD's skill to do a 5x damage
    /// </summary>
    /// <param name="p">Player</param>
    /// <param name="obj">Character</param>
    public void PowerAttack(Player p, Character obj)
    {
        p.Mana -= 50;
        if (obj.Health - p.Damage * 5 <= 0)
        {
            obj.Health = 0;
            return;
        }

        Console.WriteLine("You have dealt {0} damage", p.Damage * 5);
        obj.Health -= p.Damage * 5;
    }

    /// <summary>
    ///     Self healing.
    /// </summary>
    public void GetSelfHealed()
    {
        Mana -= 50;
        int amount = Class == "Heal"
            ? RandomNumberGenerator.GetInt32(25, 50) * (Intelligence / 4)
            : RandomNumberGenerator.GetInt32(10, 25) * (Intelligence / 4);
        Health += amount;

        Console.WriteLine("Healing was successful, restored {0}", amount);
    }

    public void RestoreStats()
    {
        Health = Class == "Tank" ? 500 : 100;
        Mana = Class == "Heal" ? 250 : 100;
    }

    protected internal void RegenMana()
    {
        Mana += 15;
        if (Mana > maxMana) Mana = maxMana;
    }
}