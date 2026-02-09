using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChangeMultiplierEffect : Effect
{
    [field: SerializeField] public float attackMultiplier { get; private set; }
    [field: SerializeField] public float blockMultiplier { get; private set; }
    [field: SerializeField] public float fragileMultiplier { get; private set; } 
    public override GameAction GetGameAction(EffectContext ctx, List<Entity> targets, Entity caster)
    {
        return new ChangeEntityStatsGA(attackMultiplier, blockMultiplier, fragileMultiplier, targets);
    }
}
