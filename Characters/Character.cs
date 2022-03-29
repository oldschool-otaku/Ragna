using System.Security.Cryptography;
using Ragna.Mechanics;

namespace Ragna.Characters;

public class Character
{
    //TODO: сделать френдли мобов
    private int DamageDealt;
    private int maxHP;
    private readonly List<Skill?> Skills;
    public List<Status> StatusList = new();
    public bool Stunned = false;

    /*
     public Character(string name, int hp, int dmg, int initiative, List<Skill> skills)
    {
        Hp = hp;
        Skills = skills;
        Dmg = dmg;
        Name = name;
        Initiative = initiative;
        OrigHp = hp;
    }
     */
    
    public Character(string name, int hp, int dmg, List<Skill?> skills)
    {
        Health = hp;
        maxHP = hp;
        Damage = dmg;
        Skills = skills;
        Name = name;
    }

    public int Health { get; set; }
    public int Damage { get; }
    public string Name { get; }
    private bool Dead { get; set; }
    
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
    
    public void Tick()
    {
        List<Status> temp = new List<Status>(StatusList);
        foreach (Status i in StatusList)
        {
            i.Fn(this);
            i.Duration -= 1;
            Console.WriteLine($"Turns remaining: {i.Duration}");
            Thread.Sleep(3000);
            if (i.Duration <= 0) { temp.Remove(i); }
        }
        StatusList = temp;
    }

    public void TakeDamage(int dmg)
    {
        Health = Health - dmg <= 0 ? 0 : Health - dmg;
        Dead = Health == 0;
        if (!Dead) return;
        Console.WriteLine($"{Name} is dead");
        Thread.Sleep(3000);
    }

    
}