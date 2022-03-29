using System.Security.Cryptography;

namespace Ragna.Characters;

public class Character
{
    //TODO: сделать френдли мобов
    private int DamageDealt;
    private int maxHP;
    private bool Bleeding = false;

    public Character(string name, int hp, int dmg, bool friend)
    {
        Health = hp;
        maxHP = hp;
        Damage = dmg;
        Name = name;
        //Friendly = friend;
    }

    public int Health { get; set; }
    public int Damage { get; }
    public string Name { get; }
    //private bool Friendly { get; }


    /// <summary>
    ///     Check if Character is dead or not
    /// </summary>
    public bool IsDead()
    {
        if (Health > 0) return false;
        Console.WriteLine("{0} is dead. You win!", Name);
        return true;
    }

    /// <summary>
    ///     Attacking player.
    /// </summary>
    /// <param name="b">Character</param>
    /// <param name="obj">Player</param>
    internal void AttackPlayer(Character b, Player obj)
    {
        DamageDealt = RandomNumberGenerator.GetInt32(b.Damage / 2, b.Damage);
        if (obj.Health - DamageDealt / (obj.Defense / 2) <= 0)
        { obj.Health = 0; return; }

        obj.Health -= DamageDealt / (obj.Defense / 2);
        Console.WriteLine("Enemy has damaged you by {0} HP", DamageDealt);
    }

    /// <summary>
    ///     Character's selfhealing
    /// </summary>
    public void GetSelfHealed()
    {
        int amount = RandomNumberGenerator.GetInt32(25, 50);
        Health += amount;
        Console.WriteLine("Your enemy healed himself, restored {0}", amount);
    }

    /// <summary>
    ///     When being called character must attack this player
    /// </summary>
    /// <param name="b">Character</param>
    /// <param name="player">Player</param>
    public void ForcePick(Character b, Player player) => AttackPlayer(b, player); 

    /// <summary>
    ///     Restoring Characters HP after fight
    /// </summary>
    public void RestoreCharacter() => Health = maxHP;

    /// <summary>
    /// Attack that starts bleeding
    /// </summary>
    /// <param name="Boss">who will attack with bleed</param>
    /// <param name="p">Bleeding player</param>
    /// <param name="bleedTurns">amount of turns with bleeding</param>
    public void BleedPlayer(Character Boss, Player p, int bleedTurns)
    {
        Boss.AttackPlayer(Boss, p);
        p.Bleed(bleedTurns);
    }
}