using Ragna.Menu;
using Ragna.Mechanics;

namespace Ragna;

internal static class Program
{
    private static void Main()
    {
        Status.GenerateStatuses();
        Skill normalPunch = Skill.AddSkill("Normal Punch", 1);
        Skill openVeins = Skill.AddSkill("Open Veins", 0.5, Status.GetStatus("bleed"));
        Skill strongPunch = Skill.AddSkill("Strong Punch", 1.25, Status.GetStatus("stun"));

        Console.Clear();
        MainMenu.Start();
    }
}