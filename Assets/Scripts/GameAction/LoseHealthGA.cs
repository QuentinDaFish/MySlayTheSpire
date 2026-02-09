using System.Collections.Generic;
using UnityEngine;

public class LoseHealthGA : GameAction
{
    public int Amount;
    public List<Entity> Targets;
    public bool FromCard;
    public LoseHealthGA(int amount, List<Entity> targets, bool fromCard = false)
    {
        Amount = amount;
        Targets = targets;
        FromCard = fromCard;
    }
}
