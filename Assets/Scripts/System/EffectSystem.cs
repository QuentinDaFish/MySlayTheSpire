using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSystem : Singleton<EffectSystem>
{
    private void OnEnable()
    {
        ActionSystem.AttachPerfomer<PerformEffectGA>(PerformEffectPerformer);
    }
    private void OnDisable()
    {
        ActionSystem.DetachPerfomer<PerformEffectGA>();
    }

    public IEnumerator PerformEffectPerformer(PerformEffectGA performEffectGA)
    {
        Entity caster = performEffectGA.Caster;
        EffectContext ctx = performEffectGA.Ctx;

        foreach (WrappedEffects wrappedEffects in performEffectGA.Wrapper)
        {
            List<Entity> targets = new();

            if (wrappedEffects.targetMode == TargetMode.Caster) targets.Add(caster);
            else if (wrappedEffects.targetMode == TargetMode.Target) targets.Add(performEffectGA.Target);
            else targets.AddRange(EntityManager.Instance.GetEntity(wrappedEffects.targetMode));

            foreach (Effect effect in wrappedEffects.effects)
            {
                GameAction next = effect.GetGameAction(ctx, targets, caster);
                if (next != null) ActionSystem.Instance.AddReaction(next);
            }
        }
        yield return null;
    }
}
