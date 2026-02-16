using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;

public class IfBeAttackEffect : Effect
{
    [field: SerializeReference, SR] public Effect effect { get; private set; }
    public override GameAction GetGameAction(EffectContext ctx, List<Entity> targets, Entity caster)
    {
        List<Entity> _targets = ctx.Get<List<Entity>>("targets");
        Entity holder = ctx.Get<Entity>("holder");
        if (_targets.Contains(holder)) return effect.GetGameAction(ctx, targets, caster);
        return null;
    }
}
