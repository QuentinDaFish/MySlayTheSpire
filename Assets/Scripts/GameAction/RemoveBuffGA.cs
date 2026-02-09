using System.Collections.Generic;
using UnityEngine;

public class RemoveBuffGA : GameAction
{
    public BuffType BuffType;
    public int Amount;
    public List<Entity> Targets;
    public RemoveBuffGA(BuffType buffType, int amount, List<Entity> targets)
    {
        BuffType = buffType;
        Amount = amount;
        Targets = targets;
    }
}
