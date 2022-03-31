using Ragna.Characters;
using Ragna.Gameplay;
using Ragna.Mechanics;

namespace Ragna;

public class Program
{
    internal static Game? Game;
    private static void Main()
    {
        Status.GenerateStatuses();
        
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
        
        Character hero = new("Hero", 100, 100, 0, 5, 0.40, 0, 30,
            new List<Skill> {attack, cleave, fortify, sniperMark, rangedAttack, unholyGuard, move});
        Character obama = new("Obama", 100, 50, 0, 5, 0.10, 10, 6,
            new List<Skill> {attack, bleed, heal, sniperMark, rangedAttack});
        Character joeBaiden = new("JoeBaiden", 80, 50, 0, 5, 0.10, 5, 29,
            new List<Skill> {attack, bleed, heal, sniperMark, rangedAttack});
        Character skeletonVeteran = new("Skeleton Veteran", 150, 60, 0, 5, 0.4, 10, 12,
            new List<Skill> {spikedMace, shieldBash});
        Character skeletonSpearman = new("Skeleton Spearman", 115, 50, 0, 15, 0.25, 10, 15,
            new List<Skill> {spearStrike, spearCharge, spearRiposte});
        Character skeletonBannerLord = new("Skeleton Banner Lord", 85, 50, 0, 25, 0.75, 5, 50,
            new List<Skill> {bannerstrike, bannerlordrally, unholyheal, bannermark});
        Character skeletonArcher = new("Skeleton Crossbowman", 100, 65, 0, 10, 0.20, 15, 10,
            new List<Skill> {crosbowbolt, suppressingfire});

        // var charlist = new List<Character> {hero, obama, joeBaiden};
        // var allies = new List<Character>();
        // while (allies.Count != 4)
        // {
        //     Console.WriteLine($"Select Characters \n{Misc.GetCharsNamesWithInfo(charlist)}\n");
        //     Console.WriteLine($"{Misc.GetCharsNames(allies)}");
        //     var character = charlist[Misc.VerfiedInput(charlist.Count)];
        //     charlist.Remove(character);
        //     allies.Add(character);
        //     Thread.Sleep(1000);
        //     Console.Clear();
        //     Thread.Sleep(1000);
        // }
        //
        // Console.WriteLine("Select a difficulty 0-100\nmore means harder");
        // Difficulty = Misc.VerfiedInput(100);

        Game = new Game(new List<Character> {hero, obama, joeBaiden},
            new List<Character> {skeletonVeteran, skeletonSpearman, skeletonArcher, skeletonBannerLord});
        Game.Start();
    }
}