using SerializeReferenceEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DealDamageEffect : Effect
{
    [field: SerializeReference, SR] public IntValue damage { get; private set; }
    public override GameAction GetGameAction(EffectContext ctx, List<Entity> targets, Entity caster)
    {
        return new DealDamageGA(damage.Eval(ctx), targets);
    }
}
