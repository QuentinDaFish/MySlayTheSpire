using System.Collections.Generic;
using UnityEngine;

public class DealDamageGA : GameAction
{
    public int Damage;
    public List<Entity> Targets;
    public DealDamageGA(int damage, List<Entity> targets)
    {
        Damage = damage;
        Targets = targets;
    }
}
