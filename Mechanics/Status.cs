using Ragna.Characters;

namespace Ragna.Mechanics;

public class Status
{
    public readonly string Name;
    public readonly Action<Character> Fn;
    public int Duration; 
    private static Dictionary<string, Status> StatusList = new Dictionary<string, Status>();

    public Status(string name, int duration, Action<Character> fn)
    {
        Name = name;
        Fn = fn;
        Duration = duration;
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
            int damage = Convert.ToInt32(obj.Health * 0.1);
            obj.TakeDamage(damage);
            Console.WriteLine($"{obj.Name} bleeding for {damage}");
            Thread.Sleep(3000);
        }
        
        AddStatus("stun", 1, Stun);
        AddStatus("bleed", 2, Bleed);
    }
    
    public void ApplyStatus(Character obj, Status status)
    {
        obj.StatusList.Add(status);
    }

    private static void AddStatus(string name, int duration, Action<Character> fn)
    {
        Status statusToAdd = new Status(name, duration, fn);
        StatusList.Add(statusToAdd.Name, statusToAdd);
    }
    
    public static Status GetStatus(string name)
    {
        return StatusList[name];
    }
}