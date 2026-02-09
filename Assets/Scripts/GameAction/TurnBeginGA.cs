using UnityEngine;

public class TurnBeginGA : GameAction
{
    public Faction Faction;
    public TurnBeginGA(Faction faction)
    {
        Faction = faction;
    }
}
