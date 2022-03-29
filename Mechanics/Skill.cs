using Ragna.Characters;

namespace Ragna.Mechanics;

public class Skill
{
    public string Name;
    public Double Damage;
    public List<Status> StatusList = new List<Status>();
    private static Dictionary<string, Skill?> SkillList = new Dictionary<string, Skill?>();

    public Skill(string name, double damage, List<Status> statusList)
    {
        Damage = damage;
        Name = name;
        StatusList = statusList;
    }
    
    public Skill(string name, double damage, Status status)
    {
        Damage = damage;
        Name = name;
        StatusList.Add(status);
    }
    
    public Skill(string name, double damage)
    {
        Damage = damage;
        Name = name;
    }
    
    public static Skill AddSkill(string name, double damage)
    {
        Skill skillToAdd = new Skill(name, damage);
        SkillList.Add(name, skillToAdd);
        return skillToAdd;
    }
    
    public static Skill AddSkill(string name, double damage, Status statusList)
    {
        Skill skillToAdd = new Skill(name, damage, statusList);
        SkillList.Add(name, skillToAdd);
        return skillToAdd;
    }
    
    public static Skill AddSkill(string name, double damage, List<Status> statusList)
    {
        Skill skillToAdd = new Skill(name, damage, statusList);
        SkillList.Add(name, skillToAdd);
        return skillToAdd;
    }

    public void Use(Character subject, Character target)
    {
        int damageDealt = Convert.ToInt32(Math.Round(subject.Damage * Damage));
        target.TakeDamage(damageDealt);
        
        Console.WriteLine
        ($"{subject.Name} using {Name} dealing {damageDealt} to {target.Name}\n{target.Name} Hp: {target.Health}");
        Thread.Sleep(3000);
        
        foreach (Status i in StatusList)
        {
            Console.Write($"{target.Name} is {i.Name}");
            Thread.Sleep(3000);
            i.ApplyStatus(target, i);
        }
    }

    public static Skill? Get_Skill(string name)
    {
        SkillList.TryGetValue(name, out Skill? value);
        return value;
    }
    
    public static string GetNames(List<Skill?> ls) => 
        Enumerable.Range(0, ls.Count).Aggregate("", (current, i) => 
            current + $"{i + 1}: {ls[i]?.Name}   ");
}