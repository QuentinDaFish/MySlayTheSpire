using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor;
using UnityEngine;
using UnityEngine.AdaptivePerformance;

public class Perk
{
    public PerkData Data { get; private set; }
    public string PerkID => Data.PerkID;
    public string PerkName => Data.PerkName;
    public string Desc => Data.Desc;
    public Sprite Icon => Data.Icon;
    public List<GameActionTrigger> Triggers => Data.Triggers;
    public bool Countable => Data.Countable;
    public int count { get; private set; }
    public Perk(PerkData data)
    {
        Data = data;
    }

    private readonly List<ReactionToken> handles = new();
    public void Add()
    {
        for (int i = 0; i < Data.Triggers.Count; i++)
        {
            GameActionTrigger trigger = Data.Triggers[i];
            void wrapper(GameAction gameAction) => Trigger(gameAction, trigger);
            ReactionToken token = ActionSystem.SubscribeReaction(trigger.ActionType, trigger.timing, wrapper);
            handles.Add(token);
        }

        if (Data.OnAdd != null)
        {
            PerformEffectGA performEffectGA = new(Data.OnAdd, null, PlayerSystem.Instance.Hero);
            ActionSystem.Instance.AddReaction(performEffectGA);
        }
    }
    public void Remove()
    {
        for (int i = 0; i < handles.Count; i++) handles[i].Dispose();
        handles.Clear();

        if (Data.OnRemove != null)
        {
            PerformEffectGA performEffectGA = new(Data.OnRemove, null, PlayerSystem.Instance.Hero);
            ActionSystem.Instance.AddReaction(performEffectGA);
        }
    }
    private void Trigger(GameAction gameAction, GameActionTrigger trigger)
    {
        EffectContext ctx = new EffectContext(gameAction);

        trigger.TryBind(gameAction, ctx);
        ctx.Set("holder", PlayerSystem.Instance.Hero);
        ctx.Set("count", count);

        PerformEffectGA performEffectGA = new(trigger.wrappers, PlayerSystem.Instance.Hero, ctx.Get<Entity>("caster"), ctx);
        ActionSystem.Instance.AddReaction(performEffectGA);
    }
}
