using System.Collections.Generic;
using UnityEngine;

public class ChangeEntityStatsGA : GameAction
{
    public float AttackMultiplier;
    public float BlockMultiplier;
    public float FragileMultiplier;
    public List<Entity> Targets;
    public ChangeEntityStatsGA(float attackMultiplier, float blockMultiplier, float fragileMultiplier, List<Entity> targets)
    {
        AttackMultiplier = attackMultiplier;
        BlockMultiplier = blockMultiplier;
        FragileMultiplier = fragileMultiplier;
        Targets = targets;
    }
}
