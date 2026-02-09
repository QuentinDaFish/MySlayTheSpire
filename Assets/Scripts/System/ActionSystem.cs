using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class ActionSystem : Singleton<ActionSystem>
{
    private List<GameAction> reactions = new();
    private static readonly Dictionary<Type, List<Action<GameAction>>> preSubs = new();
    private static readonly Dictionary<Type, List<Action<GameAction>>> postSubs = new();
    private static readonly Dictionary<Type, Func<GameAction, IEnumerator>> performers = new();
    private bool isPerforming;

    public void Perform(GameAction gameAction, Action onFinished = null)
    {
        if (isPerforming) return;
        isPerforming = true;
        StartCoroutine(Flow(gameAction, () =>
        {
            isPerforming = false;
            onFinished?.Invoke();
        }));
    }
    private IEnumerator Flow(GameAction gameAction, Action onFinished = null)
    {
        reactions = gameAction.preReactions;
        PerformSubscribs(gameAction, preSubs);
        yield return PerformReactions();

        reactions = gameAction.performReactions;
        yield return PerformPerformer(gameAction);
        yield return PerformReactions();

        reactions = gameAction.postReactions;
        PerformSubscribs(gameAction, postSubs);
        yield return PerformReactions();

        onFinished?.Invoke();
    }
    private IEnumerator PerformPerformer(GameAction gameAction)
    {
        Type type = gameAction.GetType();
        if (performers.ContainsKey(type))
        {
            yield return performers[type](gameAction);
        }
    }
    private IEnumerator PerformReactions()
    {
        foreach(GameAction gameAction in reactions)
        {
            yield return Flow(gameAction);
        }
    }
    private void PerformSubscribs(GameAction gameAction, Dictionary<Type, List<Action<GameAction>>> subs)
    {
        Type type = gameAction.GetType();
        if (subs.ContainsKey(type))
        {
            var snapshot = subs[type].ToArray();
            for (int i = 0; i < snapshot.Length; i++) snapshot[i](gameAction);
        }
    }
    public static void AttachPerfomer<T>(Func<T, IEnumerator> performer) where T : GameAction
    {
        Type type = typeof(T);
        IEnumerator wrapper(GameAction gameAction) => performer((T)gameAction);
        performers[type] = wrapper;
    }
    public static void DetachPerfomer<T>()
    {
        Type type = typeof(T);
        if(performers.ContainsKey(type)) performers.Remove(type);
    }
    public static ReactionToken SubscribeReaction<T>(Action<T> reaction, ReactionTiming timing) where T : GameAction
    {
        Type type = typeof(T);
        Dictionary<Type, List<Action<GameAction>>> subs = timing == ReactionTiming.Pre ? preSubs : postSubs;
        void wrapped(GameAction action) => reaction((T)action);
        if (!subs.TryGetValue(type, out var list)) subs[type] = list = new();
        list.Add(wrapped);
        return new ReactionToken(timing, type, wrapped);
    }
    public static ReactionToken SubscribeReaction(Type gameActionType, ReactionTiming timing, Action<GameAction> reaction)
    {
        Dictionary<Type, List<Action<GameAction>>> subs = timing == ReactionTiming.Pre ? preSubs : postSubs;
        if (!subs.TryGetValue(gameActionType, out var list)) subs[gameActionType] = list = new();
        list.Add(reaction);
        return new ReactionToken(timing, gameActionType, reaction);
    }
    public static void UnsubscribeReaction(ReactionToken token)
    {
        var subs = token.Timing == ReactionTiming.Pre ? preSubs : postSubs;
        if (!subs.TryGetValue(token.GameActionType, out var list)) return;
        list.Remove(token.Wrapped);
        if (list.Count == 0) subs.Remove(token.GameActionType);
    }
    public void AddReaction(GameAction gameAction) => reactions?.Add(gameAction);
}

public enum ReactionTiming
{
    Pre,
    Post,
}

public class ReactionToken
{
    public ReactionTiming Timing { get; private set; }
    public Type GameActionType { get; private set; }
    public Action<GameAction> Wrapped { get; private set; }

    public ReactionToken(ReactionTiming timing, Type gameActionType, Action<GameAction> wrapped)
    {
        Timing = timing;
        GameActionType = gameActionType;
        Wrapped = wrapped;
    }

    public void Dispose() => ActionSystem.UnsubscribeReaction(this);
}