using SerializeReferenceEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AddBuffEffect : Effect
{
    [field: SerializeField] public BuffType buffType { get; private set; }
    [field: SerializeReference, SR] public IntValue stack { get; private set; }
    public override GameAction GetGameAction(EffectContext ctx, List<Entity> targets, Entity caster)
    {
        return new AddBuffGA(buffType, stack.Eval(ctx), targets);
    }
}
