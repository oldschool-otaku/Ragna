using Ragna.Characters;

namespace Ragna.Mechanics;

public class Modificators
{
    //TODO: доделать
    public Modificators(string name, string type_of_buff, int buff)
    {
        Name = name;
        type = type_of_buff;
        Buff = buff;
    }

    private string type { get; }
    private string Name { get; }
    private int Buff { get; }

    /// <summary>
    /// This thing is being used to buff you
    /// </summary>
    public void Use(Player p, Modificators obj)
    {
        switch (obj.type)
        {
            case "Damage":
                p.Damage += Buff;
                return; 

            case "Health":
                p.Health += Buff;
                return;

            case "Strength":
                p.Strength += Buff;
                return;

            case "Intelligence":
                p.Intelligence += Buff;
                return;

            case "Defense":
                p.Defense += Buff;
                return;
        }
        Console.WriteLine("You have used {0}, +{1} to {2}", Name, Buff, type);
    }
}