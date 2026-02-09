using System.Collections.Generic;
using UnityEngine;

public class PerformEffectGA : GameAction
{
    public List<WrappedEffects> Wrapper;
    public Entity Caster;
    public Entity Target;
    public EffectContext Ctx;

    public PerformEffectGA(List<WrappedEffects> wrapper, Entity caster, Entity target = null, EffectContext ctx = null)
    {
        Wrapper = wrapper;
        Caster = caster;
        Target = target;
        Ctx = ctx;
    }
}