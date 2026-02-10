using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class GameActionTrigger
{
    [field: SerializeField] public ReactionTiming timing { get; private set; } = ReactionTiming.Pre;
    public abstract Type ActionType { get; }
    [field: SerializeField] public List<WrappedEffects> wrappers { get; private set; }
    public abstract void TryBind(GameAction gameAction, EffectContext ctx);
}

[Serializable]
public class OnAttackTrigger : GameActionTrigger
{
    public override Type ActionType => typeof(AttackGA);

    [SerializeField] private string damage = "damage";
    [SerializeField] private string caster = "caster";
    [SerializeField] private string targets = "targets";

    public override void TryBind(GameAction gameAction, EffectContext ctx)
    {
        AttackGA attackGA = (AttackGA)gameAction;
        ctx.Vars[damage] = attackGA.Damage;
        ctx.Vars[caster] = attackGA.Caster;
        ctx.Vars[targets] = attackGA.Targets;
    }
}