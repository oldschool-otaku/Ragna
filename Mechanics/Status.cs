using Ragna.Characters;

namespace Ragna.Mechanics;

public class Status
{
    private static Dictionary<string, Status> StatusList = new();
    public int Duration;
    public readonly Action<Character> Fn;
    public readonly bool IsInstant;
    public readonly string Name;
    public readonly string OnApply;
    public readonly string Type;

    private Status(string name, int duration, Action<Character> fn, string type, string onapply, bool isInstant = false)
    {
        Name = name;
        Fn = fn;
        Duration = duration;
        Type = type;
        IsInstant = isInstant;
        OnApply = onapply;
    }

    public static void GenerateStatuses()
    {
        void Stun(Character obj)
        {
            obj.Stunned = true;
            Console.WriteLine($"{obj.Name} is stunned, skipping turn");
            Thread.Sleep(3000);
        }

        void Bleed(Character obj)
        {
            int damage = Convert.ToInt32(obj.MaxHp * 0.15);
            obj.TakeDamage(damage);
            Console.WriteLine($"{obj.Name} bleeding for {damage}");
            Thread.Sleep(3000);
        }

        void Rallybuff(Character obj)
        {
            obj.Initiative += 10;
            obj.Crit += 25;
        }

        void Mark(Character obj)
        {
            Console.WriteLine($"{obj.Name} is marked");
        }

        void Riposte(Character obj)
        {
            Console.WriteLine($"{obj.Name} is riposting");
        }

        void Guard(Character obj)
        {
            Console.WriteLine($"{obj.Name} is riposting");
        }

        void ArmorBuff(Character obj)
        {
            obj.Armor += 0.2;
        }

        AddStatus("stun", 1, Stun, "stun", "is stunned");
        AddStatus("bleed", 2, Bleed, "damage", "is bleeding");
        AddStatus("Mark", 2, Mark, "mark", "is marked");
        AddStatus("Riposte", 3, Riposte, "riposte", "is riposting");
        AddStatus("Guard", 3, Guard, "guard", "is guarded");
        AddStatus("Rallybuff", 3, Rallybuff, "agressivebuff", "is empowered", true);
        AddStatus("ArmorBuff", 3, ArmorBuff, "defensivebuff", "is fortyfied", true);
    }

    private static void AddStatus(string name, int duration, Action<Character> fn, string type, string onapply,
        bool isinstant = false)
    {
        Status statusToAdd = new(name, duration, fn, type, onapply, isinstant);
        StatusList.Add(statusToAdd.Name, statusToAdd);
    }

    public static Status GetStatus(string name)
    {
        return StatusList[name];
    }

    public static void ApplyStatus(Character obj, Status status)
    {
        obj.Dmg = obj.MaxDmg;
        obj.Acc = obj.MaxAcc;
        obj.Dodge = obj.MaxDodge;
        obj.Initiative = obj.MaxInitiative;
        obj.Crit = obj.MaxCrit;
        obj.Armor = obj.MaxArmor;
        if (status.IsInstant)
            status.Fn(obj);
        if (obj.StatusList.Contains(status))
            obj.StatusList.Remove(status);

        obj.StatusList.Add(status);
    }
}