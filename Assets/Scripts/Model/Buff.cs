using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Buff
{
    public BuffData Data { get; private set; }
    public BuffLogic Logic { get; private set; }
    public BuffType BuffType => Data.BuffType;
    public Sprite Image => Data.Image;
    public bool Stackable => Data.Stackable;
    public bool Signed => Data.Signed;
    public BuffClearType ClearType => Data.ClearType;
    public int Stack { get; private set; }
    public Entity Holder { get; private set; }
    public Buff(BuffData data)
    {
        Data = data;
    }
    private readonly List<ReactionToken> handles = new();
    public void Add(Entity holder)
    {
        Holder = holder;
        if (Data.Triggers == null) return;

        for (int i = 0; i < Data.Triggers.Count; i++)
        {
            GameActionTrigger trigger = Data.Triggers[i];
            void wrapper(GameAction gameAction) => Trigger(gameAction, trigger);
            ReactionToken token = ActionSystem.SubscribeReaction(trigger.ActionType, trigger.timing, wrapper);
            handles.Add(token);
        }

        SubscribeClear();

        if (Data.OnAdd != null)
        {
            PerformEffectGA performEffectGA = new(Data.OnAdd, null, Holder);
            ActionSystem.Instance.AddReaction(performEffectGA);
        }
    }
    public void Change(int amount) => Stack += amount;
    public void Remove()
    {
        for (int i = 0; i < handles.Count; i++) handles[i].Dispose();
        handles.Clear();

        if (Data.OnRemove != null)
        {
            PerformEffectGA performEffectGA = new(Data.OnRemove, null, Holder);
            ActionSystem.Instance.AddReaction(performEffectGA);
        }
    }

    private void Trigger(GameAction gameAction, GameActionTrigger trigger)
    {
        EffectContext ctx = new EffectContext(gameAction);

        trigger.TryBind(gameAction, ctx);
        ctx.Vars["holder"] = Holder;
        ctx.Vars["stack"] = Stack;

        PerformEffectGA performEffectGA = new(trigger.wrappers, Holder, null, ctx);
        ActionSystem.Instance.AddReaction(performEffectGA);
    }
    private void SubscribeClear()
    {
        if (ClearType == BuffClearType.Forever) return;

        void DoClear(Faction faction)
        {
            if (faction != Holder.Faction) return;
            int amount = (ClearType == BuffClearType.ClearBegin || ClearType == BuffClearType.ClearOver) ? Stack : 1;
            ActionSystem.Instance.AddReaction(new RemoveBuffGA(BuffType, amount, new List<Entity> { Holder }));
        }

        if (ClearType == BuffClearType.TurnBegin || ClearType == BuffClearType.ClearBegin)
            handles.Add(ActionSystem.SubscribeReaction<TurnBeginGA>((turnBeginGA) => DoClear(turnBeginGA.Faction), ReactionTiming.Pre));
        else
            handles.Add(ActionSystem.SubscribeReaction<TurnOverGA>((turnOverGA) => DoClear(turnOverGA.Faction), ReactionTiming.Pre));
    }
}

