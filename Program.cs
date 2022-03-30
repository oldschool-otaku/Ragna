using Cosoleapp3;

namespace Ragna
{
    // #TODO
    // GENERAL
    // ДОПИСАТЬ ОГРАНИЧЕНИЯ ПРИМЕНЕНИЯ СКИЛОВ В ИИ => СТАТУС И СКИЛЛ ПЕРЕМЕЩЕНИЯ
    // ДОБАВИТЬ БРОНЮ В ФОРМУЛЫ РАССЧЕТА УРОНА


    // ИИ
    // ВЫБОР УРОВНЯ СЛОЖНОСТИ
    // ДЕЙТВИЕ САППОРТ - ЗАЩИТНЫЙ/АТАКУЮЩИЙ БАФ/ДЕБАФ ПО ТИПУ И КЛАССУ С РАЗВТВЛЕНИЕМ ЧЕРЕЗ РАНДОМ
    // СЕЙВ С ВОЗМОЖНОСТЯМИ ЗАСВИТЬ СОЮЗНИКА
    // ДОБАВИТЬ В ВЫБОР ЦЕЛИ УЧЕТ БРОНИ И УКЛОНЕНИЯ
    // ПЕРЕПИСАТЬ ТАРГЕТИНГ ИСПОЛЬЗУЯ THENBY
    // ИСПОЛЬЗОВАНИЕ СКИЛЛА ПЕРЕМЕЩЕНИЯ ПО ОЦЕНКИ ДОСТУПНЫХ СКИЛЛОВ

    // КОНТЕНТ
    // ВЫВОДИТЬ ХП ШАНС ПОПАДАНИЯ И УРОН ПРОСЛЕ ВЫБОРА СКИЛЛА
    // МАКСИМАЛЬНО ИГРАБЕЛЬНЫЙ ВЫВОД НА КОНСОЛЬ
    // МЕНЮ СТАРТА ИГРЫ
    // МЕНЮ ВЫБОРА ПЕРСОНАЖЕЙ
    // 3 ПОЛНОЦЕННЫХ БИТВЫ С ПЕРСОНАЖАМИ

    // КОД
    // УЛУЧШИТЬ ЛОГИКУ ПРИМЕНЕНИЯ НА СЕБЯ/СОЮЗНИКОВ/ВРАГОВ

    public class Program
    {
        internal static Game? Game;

        private static void Main(string[] args)
        {
            Status.GenerateStatuses();
            Skill attack = new Skill("Attack", 1, new List<int> {0}, new List<Status>(), move: 1);
            Skill lasthit = new Skill("Lasthit", 2, new List<int> {0, 1}, new List<Status>());
            Skill rangedAttack = new Skill("Ranged Attack", 0.75, new List<int> {0, 1, 2}, new List<Status>(), markdamage: true);
            Skill sniperMark = new Skill("Sniper Mark", 0, new List<int> {0, 1, 2, 3}, new List<Status> {Status.GetStatus("Mark")});
            Skill cleave = new Skill("Cleave", 0.33, new List<int> {0, 1, 2}, new List<Status>(), aoe: true);
            Skill bleed = new Skill("Bleed", 0.5, new List<int> {0, 1, 2, 3},
                new List<Status> {Status.GetStatus("bleed")});
            Skill move = new Skill("Move", useonself: true, isMoveSkill: true);
            Skill fortify = new Skill("fortify", 0, new List<int> {0, 1, 2, 3},
                new List<Status> {Status.GetStatus("ArmorBuff")}, useonaliies: true, aoe: true);
            Skill heal = new Skill("Heal", 1, new List<int> {0, 1, 2, 3}, new List<Status>(), useonaliies: true);
            Skill unholyGuard = new Skill("Unholy Guard", statusList: new List<Status> {Status.GetStatus("Guard")}, useonaliies: true);
            Character hero = new Character("Hero", 100, 100, 0, 5, 0.40, 0, 30,
                new List<Skill> {attack, cleave, fortify, sniperMark, rangedAttack, unholyGuard, move});
            Character obama = new Character("Obama", 100, 50, 0, 5, 0.10, 10, 6, 
                new List<Skill> {attack, bleed, heal, sniperMark, rangedAttack});
            Character joeBaiden = new Character("JoeBaiden", 80, 50, 0, 5, 0.10, 5, 29,
                new List<Skill> {attack, bleed, heal, sniperMark, rangedAttack});
            
            Skill spikedMace = new Skill("Spiked Mace", targets: new List<int> {0, 1}, statusList: new List<Status> {Status.GetStatus("bleed")});
            Skill shieldBash = new Skill("Shield Bash", 0.5, new List<int> {0, 1}, new List<Status> {Status.GetStatus("stun")});
            Character skeletonVeteran = new Character("Skeleton Veteran", 150, 60, 0, 5, 0.4, 10, 12,
                new List<Skill> {spikedMace, shieldBash}, role: "Tank");
            
            Skill spearCharge = new Skill("Spear Charge", 0.67, new List<int> {0, 1, 2}, new List<Status>(), aoe: true);
            Skill spearStrike = new Skill("Spear Strike", 1, new List<int> {0, 1, 2}, new List<Status>());
            Skill spearRiposte = new Skill("Riposte", 0.5, new List<int> {0, 1, 2, 4}, new List<Status> {Status.GetStatus("Riposte")}, buffself: true);
            Character skeletonSpearman = new Character("Skeleton Spearman", 115, 50, 0, 15, 0.25, 10, 15,
                new List<Skill> {spearStrike, spearCharge, spearRiposte}, role: "DD");
            
            Skill bannerstrike = new Skill("Unexpected attack", 1, new List<int> {0, 1 ,2 ,3}, new List<Status> {Status.GetStatus("stun")}, usablefrom: new List<int> {3});
            Skill bannerlordrally = new Skill("Rally To The Flame", 0, new List<int> {0, 1, 2, 3}, new List<Status> {Status.GetStatus("Rallybuff")}, useonaliies: true, aoe: true, usablefrom: new List<int> {3});
            Skill unholyheal = new Skill("Unholy Restoration", 1, new List<int> {0, 1, 2, 3}, new List<Status>(), useonaliies: true, aoe: true, usablefrom: new List<int> {3});
            Skill bannermark = new Skill("Mark for death", 0, new List<int> {0, 1, 2, 3}, new List<Status> {Status.GetStatus("Mark")}, usablefrom: new List<int> {3});
            Character skeletonBannerLord = new Character("Skeleton Banner Lord", 85, 50, 0, 25, 0.75, 5, 50,
                new List<Skill> {bannerstrike, bannerlordrally, unholyheal, bannermark});
            
            Skill crosbowbolt = new Skill("Crosbow Bolt", 1, new List<int> {0, 1 ,2 ,3}, new List<Status>(), markdamage: true);
            Skill suppressingfire = new Skill("Suppressing Fire", 0.67, new List<int> {1 ,2 ,3}, new List<Status>(), aoe: true);
            Character skeletonArcher = new Character("Skeleton Crossbowman", 100, 65, 0, 10, 0.20, 15, 10,
                new List<Skill> {crosbowbolt, suppressingfire}, role: "DD");

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
}