using SerializeReferenceEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LoseHealthEffect : Effect
{
    [field: SerializeReference, SR] public IntValue amount { get; private set; }
    [field: SerializeField] public bool fromCard { get; private set; }
    public override GameAction GetGameAction(EffectContext ctx, List<Entity> targets, Entity caster)
    {
        return new LoseHealthGA(amount.Eval(ctx), targets, fromCard);
    }
}
