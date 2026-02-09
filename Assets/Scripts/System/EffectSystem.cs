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

    public IEnumerator PerformEffectPerformer(PerformEffectGA ga)
    {
        var caster = ga.Caster;
        var ctx = ga.Ctx;

        foreach (var wrapped in ga.Wrapper)
        {
            List<Entity> targets = new();

            if (wrapped.targetMode == TargetMode.Caster) targets.Add(caster);
            else if (wrapped.targetMode == TargetMode.Target) targets.Add(ga.Target);
            else targets.AddRange(EntityManager.Instance.GetEntity(wrapped.targetMode));

            foreach (var effect in wrapped.effects)
            {
                var next = effect.GetGameAction(ctx, targets, caster);
                if (next != null) ActionSystem.Instance.AddReaction(next);
            }
        }
        yield return null;
    }
}
