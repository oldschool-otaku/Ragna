using Ragna.Characters;
using Ragna.Gameplay;

namespace Ragna.Mechanics;

public class Skill
{
    public readonly bool Aoe;
    private readonly bool BuffSelf;
    public readonly double Damage;
    private readonly bool IsMoveSkill;
    public readonly bool MarkDamage;
    private readonly string Name;
    public readonly List<Status> StatusList = new();
    public readonly List<int> Targets = new() {0, 1, 2, 3};
    public readonly List<int> UsableFrom = new() {0, 1, 2, 3};
    public readonly bool UseOnAllies;
    private readonly bool UseOnSelf;
    private int Move;

    public Skill(string name, double damage = 0, List<int>? targets = null, List<Status>? statusList = null,
        List<int>? usablefrom = null, bool useonaliies = false, bool useonself = false, bool aoe = false,
        bool markdamage = false,
        bool buffself = false, bool isMoveSkill = false, int move = 0)
    {
        Damage = damage;
        Name = name;
        if (targets != null)
            Targets = targets;
        if (statusList != null)
            StatusList = statusList;
        if (usablefrom != null)
            UsableFrom = usablefrom;
        UseOnAllies = useonaliies;
        UseOnSelf = useonself;
        MarkDamage = markdamage;
        BuffSelf = buffself;
        Aoe = aoe;
        Move = move;
        IsMoveSkill = isMoveSkill;
    }

    public void Use(Character subject, List<Character> targets)
    {
        foreach (Character t in targets)
        {
            Character? target = t;
            int damageDealt;
            if (UseOnAllies)
            {
                damageDealt = Damage == 0 ? 0 : Convert.ToInt32(subject.Hp * 0.25 * Damage);
                target.Heal(damageDealt);
                Console.WriteLine(damageDealt != 0
                    ? $"{subject.Name} using {Name} healed {damageDealt} to {target.Name}\n{target.Name} Hp: {target.Hp}"
                    : $"{subject.Name} using {Name} to {target.Name}");
                Thread.Sleep(3000);
                foreach (Status i in StatusList)
                {
                    if (BuffSelf)
                    {
                        Status.ApplyStatus(subject, i);
                        Console.WriteLine($"{subject.Name} {i.OnApply}");
                        continue;
                    }

                    Status.ApplyStatus(target, i);
                    Console.WriteLine($"{target.Name} {i.OnApply}");
                }
            }

            if (UseOnSelf)
                if (IsMoveSkill)
                {
                    Console.WriteLine($"What position to move: 1 - {Program.Game!.Allies.Count}");
                    Move = Misc.VerfiedInput(Program.Game.Allies.Count);
                    ref List<Character> allies = ref Program.Game.Allies;
                    foreach (int i in Enumerable.Range(1, Move < 0 ? Move * -1 : Move))
                    {
                        if ((allies.IndexOf(target) == 3) & (Move > 0) ||
                            (allies.IndexOf(target) == 0) & (Move < 0))
                            break;
                        (allies[allies.IndexOf(target)], allies[allies.IndexOf(target) + (Move < 0 ? -1 : 1)]) = (
                            allies[allies.IndexOf(target) + (Move < 0 ? -1 : 1)], allies[allies.IndexOf(target)]);
                    }
                }

            if (!(!UseOnAllies & !UseOnSelf)) continue;
            {
                Console.WriteLine($"{subject.Name} used {Name} on {target.Name}");
                Thread.Sleep(3000);

                if (Misc.Roll(target.Dodge - subject.Acc)) 
                    Console.WriteLine($"{target.Name} dodges");

                else
                {
                    if (target.StatusList.Any(x => x.Type == "guard"))
                    {
                        Console.WriteLine("guard");
                        target = (Program.Game != null && Program.Game.Allies.Contains(target)
                                ? Program.Game.Allies
                                : Program.Game!.Enemies)
                            .Find(x =>
                                x.Skills.Any(a => a.StatusList.Any(b => b.Type == "guard")));
                    }

                    damageDealt = Convert.ToInt32(subject.Dmg * Damage * (1.0 - target!.Armor));

                    if (MarkDamage & target.StatusList.Any(x => x.Type == "mark"))
                        damageDealt *= 2;

                    if (Misc.Roll(subject.Crit))
                    {
                        damageDealt *= 2;
                        Console.WriteLine("Critical Strike!");
                    }

                    target.TakeDamage(damageDealt);
                    Console.WriteLine(damageDealt != 0
                        ? $"{subject.Name} dealt {damageDealt} to {target.Name}\n{target.Name} Hp: {target.Hp}"
                        : "");
                    Thread.Sleep(3000);
                    if (target.StatusList.Any(x => x.Type == "riposte"))
                        new Skill("riposte attack", 1, new List<int> {0, 1, 2, 3}, new List<Status>()).Use(target,
                            new List<Character> {subject});

                    foreach (Status i in StatusList)
                    {
                        if (BuffSelf)
                        {
                            Status.ApplyStatus(subject, i);
                            Console.WriteLine($"{subject.Name} {i.OnApply}");
                            continue;
                        }

                        Status.ApplyStatus(target, i);
                        Console.WriteLine($"{target.Name} {i.OnApply}");
                    }

                    if (Move == 0) continue;
                    {
                        Console.WriteLine($"{target.Name} is " + (Move > 0 ? "Pushed back " : "Pulled in ") + "by " +
                                          (Move > 0 ? $"{Move}" : $"{Move * -1}"));
                        ref List<Character> allies = ref Program.Game!.Allies;
                        ref List<Character> enemies = ref Program.Game.Enemies;
                        List<Character> targetTeam = Program.Game.Allies.Contains(target) ? allies : enemies;
                        foreach (int i in Enumerable.Range(1, Move < 0 ? Move * -1 : Move))
                        {
                            if ((targetTeam.IndexOf(target) == 3) & (Move > 0) ||
                                (targetTeam.IndexOf(target) == 0) & (Move < 0))
                                break;
                            (targetTeam[targetTeam.IndexOf(target)],
                                targetTeam[targetTeam.IndexOf(target) + (Move < 0 ? -1 : 1)]) = (
                                targetTeam[targetTeam.IndexOf(target) + (Move < 0 ? -1 : 1)],
                                targetTeam[targetTeam.IndexOf(target)]);
                        }
                    }
                }
            }
        }
    }

    public List<Character> GetTargets()
    {
        List<Character> allies = Program.Game!.Allies;
        List<Character> enemies = Program.Game.Enemies;
        Thread.Sleep(3000);
        List<Character> targetTeam = UseOnAllies ? allies : enemies;
        targetTeam = targetTeam.Where(x => Targets.Contains(targetTeam.IndexOf(x))).ToList();

        if (Aoe)
            return targetTeam;

        if (IsMoveSkill)
            return new List<Character> {Game.Subject!};

        Console.WriteLine($"Select a target:\n{Misc.GetCharsNames(targetTeam)}");
        return new List<Character> {targetTeam[Misc.VerfiedInput(targetTeam.Count)]};
    }

    public static string GetNames(List<Skill> ls)
    {
        return Enumerable.Range(0, ls.Count).Aggregate("", (current, i) => current + $"{i + 1}: {ls[i].Name} ");
    }

    private string GetStatuses()
    {
        return StatusList.Aggregate("", (current, i) => current + i.Name + ", ");
    }

    public static string GetInfo(List<Skill> ls)
    {
        return Enumerable.Range(0, ls.Count).Aggregate("", (current, i) => current + $"\n{i + 1}: {ls[i].Name}" +
                                                                           (ls[i].Damage != 0
                                                                               ? $"\n{(ls[i].UseOnAllies ? "Heal" : "Damage")}: {ls[i].Damage * 100}%"
                                                                               : "") +
                                                                           $"\nTargets{(ls[i].UseOnAllies ? " Allies" : "")}: {(ls[i].Aoe ? "~" : "")}{ls[i].Targets.Aggregate("", (x, j) => x + $"{j + 1} ").Trim()}" +
                                                                           $"{(ls[i].StatusList.Any() ? $"\nStatus: {ls[i].GetStatuses()}" : "")}\n");
    }

    public static Skill LoadSkills(int skillNumber)
    {
        Skill attack = new("Attack", 1, new List<int> {0}, new List<Status>(), 
            move: 1);
        Skill lasthit = new("Lasthit", 2, new List<int> {0, 1}, new List<Status>());
        Skill rangedAttack = new("Ranged Attack", 0.75, new List<int> {0, 1, 2}, 
            new List<Status>(), markdamage: true);
        Skill sniperMark = new("Sniper Mark", 0, new List<int> {0, 1, 2, 3},
            new List<Status> {Status.GetStatus("Mark")});
        Skill cleave = new("Cleave", 0.33, new List<int> {0, 1, 2}, 
            new List<Status>(), aoe: true);
        Skill bleed = new("Bleed", 0.5, new List<int> {0, 1, 2, 3},
            new List<Status> {Status.GetStatus("bleed")});
        Skill move = new("Move", useonself: true, isMoveSkill: true);
        Skill fortify = new("fortify", 0, new List<int> {0, 1, 2, 3},
            new List<Status> {Status.GetStatus("ArmorBuff")}, useonaliies: true, aoe: true);
        Skill heal = new("Heal", 1, new List<int> {0, 1, 2, 3}, 
            new List<Status>(), useonaliies: true);
        Skill unholyGuard = new("Unholy Guard", statusList: new List<Status> {Status.GetStatus("Guard")},
            useonaliies: true);
        Skill spikedMace = new("Spiked Mace", targets: new List<int> {0, 1},
            statusList: new List<Status> {Status.GetStatus("bleed")});
        Skill shieldBash = new("Shield Bash", 0.5, new List<int> {0, 1}, 
            new List<Status> {Status.GetStatus("stun")});
        Skill spearCharge = new("Spear Charge", 0.67, new List<int> {0, 1, 2}, 
            new List<Status>(), aoe: true);
        Skill spearStrike = new("Spear Strike", 1, new List<int> {0, 1, 2}, 
            new List<Status>());
        Skill spearRiposte = new("Riposte", 0.5, new List<int> {0, 1, 2, 4},
            new List<Status> {Status.GetStatus("Riposte")}, buffself: true);
        Skill bannerstrike = new("Unexpected attack", 1, new List<int> {0, 1, 2, 3},
            new List<Status> {Status.GetStatus("stun")}, new List<int> {3});
        Skill bannerlordrally = new("Rally To The Flame", 0, new List<int> {0, 1, 2, 3},
            new List<Status> {Status.GetStatus("Rallybuff")}, useonaliies: true, aoe: true,
            usablefrom: new List<int> {3});
        Skill unholyheal = new("Unholy Restoration", 1, new List<int> {0, 1, 2, 3}, 
            new List<Status>(),
            useonaliies: true, aoe: true, usablefrom: new List<int> {3});
        Skill bannermark = new("Mark for death", 0, new List<int> {0, 1, 2, 3},
            new List<Status> {Status.GetStatus("Mark")}, new List<int> {3});
        Skill crosbowbolt = new("Crosbow Bolt", 1, new List<int> {0, 1, 2, 3}, 
            new List<Status>(), markdamage: true);
        Skill suppressingfire = new("Suppressing Fire", 0.67, new List<int> {1, 2, 3}, 
            new List<Status>(), aoe: true);
    }
}