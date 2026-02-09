using SerializeReferenceEditor;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
public abstract class Effect
{
    public abstract GameAction GetGameAction(EffectContext ctx, List<Entity> targets, Entity caster);
}

[Serializable]
public class WrappedEffects
{
    [field: SerializeField] public TargetMode targetMode;
    [field: SerializeReference, SR] public List<Effect> effects;
}

public class EffectContext
{
    public GameAction TriggerGA { get; }
    public Dictionary<string, object> Vars { get; }
      
    public EffectContext(GameAction triggerGA, Dictionary<string, object> vars = null)
    {
        TriggerGA = triggerGA;
        if (vars == null) Vars = new();
        else Vars = vars;
    }

    public void Set<T>(string key, T value) => Vars[key] = value;
    public T Get<T>(string key) => Vars.TryGetValue(key, out var v) ? (T)v : default;
    public bool Has(string key) => Vars.ContainsKey(key);
}

public enum TargetMode
{
    None,
    Caster,
    Target,
    AllEnemy,
    RandomEnemy,
    Hero,
}
