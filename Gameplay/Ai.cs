using Ragna.Characters;
using Ragna.Mechanics;

namespace Ragna.Gameplay;

public class Ai
{
    private static Skill skill = null!;
    private static List<Character> target = null!;
    private Dictionary<string, List<Func<bool>>> _patternList = new();
    private List<Character> Allies;
    private List<Character> Enemies;
    private Character Subject;

    public Ai(List<Character> allies, List<Character> enemies, Character subject)
    {
        Allies = allies;
        Enemies = enemies;
        Subject = subject;
    }

    private bool DealDamage()
    {
        Console.WriteLine("DealDamage");
        List<Skill> skillList = Subject.Skills.Where(x => !x.UseOnAllies & (x.Damage != 0)).ToList();
        skill = skillList[new Random().Next(0, skillList.Count)];
        target = skill.Aoe
            ? Allies.Where(x => skill.Targets.Contains(Allies.IndexOf(x))).ToList()
            : new List<Character>
                {Allies.Where(x => skill.Targets.Contains(Allies.IndexOf(x))).OrderBy(x => x.Hp).ToList()[0]};
        skill.Use(Subject, target);
        return true;
    }

    private bool LastHit()
    {
        Console.WriteLine("Lasthit");
        List<Character> targetList = Allies.Where(x => x.Hp < x.MaxHp * 0.5).OrderBy(a => a.Hp).ToList();
        List<Skill> skillList = Subject.Skills.Where(a => !a.UseOnAllies & (a.Damage != 0))
            .OrderByDescending(a => a.Damage).ToList();
        if (!targetList.Any()) return false;
        {
            foreach (Character i in targetList)
            {
                List<Skill> skillListT = skillList.Where(x => x.Targets.Contains(Allies.IndexOf(i))).ToList();
                if (!skillListT.Any()) continue;
                {
                    skill = skillList[0];
                    skillListT[0].Use(Subject,
                        skill.Aoe
                            ? Allies.Where(x => skillList[0].Targets.Contains(Allies.IndexOf(x))).ToList()
                            : new List<Character> {i});
                    return true;
                }
            }

            return false;
        }
    }

    private bool Control()
    {
        List<Skill> skillList = Subject.Skills.Where(x =>
            x.StatusList.Any(a => a.Type == "stun") & x.UsableFrom.Contains(Enemies.IndexOf(Subject))).ToList();
        if (!skillList.Any()) return false;
        {
            foreach (Character i in Allies
                         .Where(x => x.StatusList.All(a => a.Type != "stun"))
                         .OrderByDescending(x => x.Skills.Any(a => a.StatusList.Any(b => b.Type == "stun")))
                         .ThenByDescending(x => x.Skills.Any(a => a.StatusList.Any(b => b.Type == "guard")))
                         .ThenByDescending(x => x.Skills.Any(a => a.UseOnAllies & (a.Damage != 0))).ToList())
            {
                List<Skill> skillListT = skillList.Where(x => x.Targets.Contains(Allies.IndexOf(i))).ToList();
                if (!skillListT.Any()) continue;
                {
                    skill = skillListT[0];
                    skill.Use(Subject,
                        skill.Aoe
                            ? Allies.Where(x => skill.Targets.Contains(Allies.IndexOf(x))).ToList()
                            : new List<Character> {i});
                    return true;
                }
            }
        }
        return false;
    }

    private bool Heal()
    {
        List<Skill> skillList = Subject.Skills
            .Where(x => x.UseOnAllies & (x.Damage != 0) & x.UsableFrom.Contains(Enemies.IndexOf(Subject)))
            .OrderBy(a => a.Damage).ToList();
        if (!skillList.Any() & !Enemies.Any(x => x.Hp < x.MaxHp * 0.5)) return false;
        {
            target = new List<Character>
                {Enemies.Where(x => skill.Targets.Contains(Enemies.IndexOf(x))).OrderBy(x => x.Hp).ToList()[0]};
            skill = skillList.Where(x => skill.Targets.Contains(Allies.IndexOf(target[0]))).OrderBy(a => a.Damage)
                .ToList()[0];
            skill.Use(Subject,
                skill.Aoe ? Enemies.Where(x => skill.Targets.Contains(Enemies.IndexOf(x))).ToList() : target);
            return true;
        }
    }

    private bool Mark()
    {
        Console.WriteLine("Mark");
        List<Skill> skillList = Subject.Skills.Where(x =>
            x.StatusList.Any(a => a.Type == "mark") & x.UsableFrom.Contains(Enemies.IndexOf(Subject))).ToList();
        if (!(skillList.Any() & Enemies.Any(x => x.Skills.Any(a => a.MarkDamage)))) return false;
        {
            skill = skillList[new Random().Next(0, skillList.Count)];
            if (!Allies.All(x => x.StatusList.All(a => a.Type != "mark")))
                return false;
            {
                List<Character> targetList = Allies.Where(x =>
                    x.StatusList.All(a => a.Type != "mark") & skill.Targets.Contains(Allies.IndexOf(x))).ToList();
                target = new List<Character> {targetList.OrderBy(x => x.Hp).ToList()[0]};
                target = skill.Aoe ? Allies.Where(x => skill.Targets.Contains(Allies.IndexOf(x))).ToList() : target;
                skill.Use(Subject, target);
                return true;
            }
        }
    }

    private bool TargetMark()
    {
        List<Skill> skillList = Subject.Skills
            .Where(x => x.MarkDamage & x.UsableFrom.Contains(Enemies.IndexOf(Subject))).ToList();
        if (!skillList.Any()) return false;
        {
            skill = skillList[new Random().Next(0, skillList.Count)];
            if (!Allies.Any(x => x.StatusList.Any(a => a.Type == "mark") & skill.Targets.Contains(Allies.IndexOf(x))))
                return false;
            {
                List<Character> targetList = Allies.Where(x =>
                    x.StatusList.Any(a => a.Type == "mark") & skill.Targets.Contains(Allies.IndexOf(x))).ToList();
                target = new List<Character> {targetList.OrderBy(x => x.Hp).ToList()[0]};
                target = skill.Aoe ? Allies.Where(x => skill.Targets.Contains(Allies.IndexOf(x))).ToList() : target;
                skill.Use(Subject, target);
                return true;
            }
        }
    }

    private bool Riposte()
    {
        List<Skill> skillList = Subject.Skills.Where(x => x.StatusList.Any(a => a.Type == "riposte")).ToList();
        if (!skillList.Any()) return false;
        {
            skill = skillList[new Random().Next(0, skillList.Count)];
            if (Subject.StatusList.Any(a => a.Type == "riposte")) return false;
            target = skill.Aoe
                ? Allies.Where(x => skill.Targets.Contains(Allies.IndexOf(x))).ToList()
                : new List<Character>
                    {Allies.Where(x => skill.Targets.Contains(Allies.IndexOf(x))).OrderBy(x => x.Hp).ToList()[0]};
            skill.Use(Subject, target);
            return true;
        }
    }

    public void Act()
    {
        _patternList.Add("Skeleton Veteran", new List<Func<bool>> {LastHit, Control, DealDamage});
        _patternList.Add("Skeleton Spearman", new List<Func<bool>> {LastHit, Riposte, DealDamage});
        _patternList.Add("Skeleton Banner Lord", new List<Func<bool>> {Mark, Heal, LastHit, DealDamage});
        _patternList.Add("Skeleton Crossbowman", new List<Func<bool>> {TargetMark, LastHit, DealDamage});
        foreach (Func<bool> _ in _patternList[Subject.Name].Where(i => i()))
            break;
    }
}