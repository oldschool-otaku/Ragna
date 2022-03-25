using Ragna.Characters;

namespace Ragna.Mechanics;

public class AI
{
    //TODO: сделать аи чтобы заебись

    public static void RandomChoice(Character Boss, Player player)
    {
        if (Boss.IsDead() is not false) return;
        if (Gameplay.ThrowTheDice() < 3) { Boss.GetSelfHealed(); return; }
        Boss.AttackPlayer(Boss, player);
    }

    public static void HealChoice(Character Boss, Player player)
    {
        if (Boss.IsDead() is not false) return;
        if (Gameplay.ThrowTheDice() < 9) { Boss.GetSelfHealed(); return; }
        Boss.AttackPlayer(Boss, player);
    }

    public static void Heal(Character Boss, Player player, int prev_pick)
    {
        if (Boss.IsDead() is not false) return;
        if (Boss.Health > Boss.Health / 4 
            && prev_pick != 3 
            && player.Health > player.Health / 4) return;
        Boss.GetSelfHealed();
    }

    public static void FinishPlayer(Character Boss, Player player)
    {
        if (Boss.IsDead() is not false) return;
        if (player.Health <= player.Health / 4)
            Boss.AttackPlayer(Boss, player);
    }

    public static void gfhgddfghg(Character Boss, Player player)
    {
        throw new NotImplementedException();
    }
}