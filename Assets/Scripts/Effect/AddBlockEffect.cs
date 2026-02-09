using SerializeReferenceEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AddBlockEffect : Effect
{
    [field: SerializeReference, SR] public IntValue amount { get; private set; }
    [field: SerializeField] public bool fromCard { get; private set; } = true;
    public override GameAction GetGameAction(EffectContext ctx, List<Entity> targets, Entity caster)
    {
        return new AddBlockGA(amount.Eval(ctx), targets, fromCard);
    }
}
