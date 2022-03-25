using Ragna.Characters;

namespace Ragna.Mechanics;

public class AI
{
    //TODO: сделать аи чтобы заебись

    public static void RandomChoice(Character Boss, Player player)
    {
        if (Boss.IsDead() is not false) return;
        if (Gameplay.ThrowTheDice() < 3)
            Boss.GetSelfHealed();
        else
            Boss.AttackPlayer(Boss, player);
    }

    public static void HealChoice(Character Boss, Player player)
    {
        if (Boss.IsDead() is not false) return;
        if (Gameplay.ThrowTheDice() < 9)
            Boss.GetSelfHealed();
        else
            Boss.AttackPlayer(Boss, player);
    }
}