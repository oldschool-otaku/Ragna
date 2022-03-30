﻿using System.Diagnostics;
using Ragna.Mechanics;

namespace Ragna.Characters;

public class Character
{
    public readonly List<int> BestPositon;
    public readonly int MaxAcc;
    public readonly double MaxArmor;
    public readonly int MaxCrit;
    public readonly int MaxDmg;
    public readonly int MaxDodge;
    public readonly int MaxHp;
    public readonly int MaxInitiative;
    public readonly string Name;
    public readonly List<Skill> Skills;
    public int Acc;
    public double Armor;
    public int Crit;
    public bool Dead;
    public int Dmg;
    public int Dodge;
    public int Hp;
    public int Initiative;
    public bool IsAi;
    public List<Status> StatusList = new();
    public bool Stunned = false;

    public Character(string name, int hp, int dmg, int acc, int dodge, double armor, int crit, int initiative,
        List<Skill> skills, List<Func<bool>>? pattern = null)
    {
        Hp = hp;
        MaxHp = hp;
        Dmg = dmg;
        MaxDmg = dmg;
        Acc = acc;
        MaxAcc = acc;
        Dodge = dodge;
        MaxDodge = dodge;
        Initiative = initiative;
        MaxInitiative = initiative;
        Crit = crit;
        MaxCrit = crit;
        Armor = armor;
        MaxArmor = armor;
        Skills = skills;
        Name = name;
        BestPositon = Enumerable.Range(0, 3).Where(x => Skills.All(a => a.UsableFrom.Contains(x))).ToList();
    }

    public Skill GetSkill()
    {
        List<Skill> usableSkills = Skills.Where(x =>
        {
            Debug.Assert(Program.Game != null, "Program.Game != null");
            return x.UsableFrom.Contains(Program.Game.Allies.IndexOf(this));
        }).ToList();

        if (IsAi) return usableSkills[new Random().Next(Skills.Count)];

        Console.WriteLine($"Select a skill:\n{Skill.GetNames(usableSkills)}");
        return usableSkills[Misc.VerfiedInput(usableSkills.Count)];
    }

    public void ProcessStatuses()
    {
        List<Status> temp = new(StatusList);
        foreach (Status i in StatusList)
        {
            if (!i.IsInstant)
            {
                i.Fn(this);
                Console.WriteLine($"Turns remaining: {i.Duration - 1}");
            }

            i.Duration -= 1;
            Thread.Sleep(3000);
            if (i.Duration <= 0) temp.Remove(i);
        }

        StatusList = temp;
        Dmg = MaxDmg;
        Acc = MaxAcc;
        Dodge = MaxDodge;
        Initiative = MaxInitiative;
        Crit = MaxCrit;
        Armor = MaxArmor;
        foreach (Status i in StatusList.Where(i => i.IsInstant)) i.Fn(this);
    }

    public void TakeDamage(int dmg)
    {
        Hp = Hp - dmg <= 0 ? 0 : Hp - dmg;
        Dead = Hp == 0;
        if (!Dead) return;
        Console.WriteLine($"{Name} is dead");
        Thread.Sleep(3000);
    }

    public void Heal(int dmg)
    {
        Hp = Hp + dmg > MaxHp ? MaxHp : Hp + dmg;
        Dead = Hp == 0;
        if (!Dead) return;
        Console.WriteLine($"{Name} is dead");
        Thread.Sleep(3000);
    }

    public string GetStatuses() => StatusList.Aggregate("", (current, i) => 
        current + i.Name + " ").Trim();
    
}