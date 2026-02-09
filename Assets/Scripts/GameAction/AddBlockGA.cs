using System.Collections.Generic;
using UnityEngine;

public class AddBlockGA : GameAction
{
    public int Amount;
    public List<Entity> Targets;
    public bool FromCard;
    public AddBlockGA(int amount, List<Entity> targets, bool fromCard = false)
    {
        Amount = amount;
        Targets = targets;
        FromCard = fromCard;
    }
}
