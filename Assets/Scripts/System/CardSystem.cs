using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSystem : Singleton<CardSystem>
{
    public static event Action<int> drawPileChanged;
    public static event Action<int> discardPileChanged;

    [SerializeField] private string dataPath;
    private Dictionary<string, CardData> cardDataMap = new();

    public readonly List<Card> drawPile = new();
    public readonly List<Card> discardPile = new();
    public readonly List<Card> hands = new();

    private void Start()
    {  
        LoadCardDatas();
    }
    private void OnEnable()
    {
        ActionSystem.AttachPerfomer<DrawHandsAtStartGA>(DrawHandsAtStartPerformer);
        ActionSystem.AttachPerfomer<DiscardAllHandsGA>(DiscardAllHandsPerformer);
        ActionSystem.AttachPerfomer<ShuffleGA>(ShufflePerformer);
        ActionSystem.AttachPerfomer<PlayCardGA>(PlayCardPerformer);
    }
    private void OnDisable()
    {
        drawPileChanged = null;
        discardPileChanged = null;

        ActionSystem.DetachPerfomer<DrawHandsAtStartGA>();
        ActionSystem.DetachPerfomer<DiscardAllHandsGA>();
        ActionSystem.DetachPerfomer<ShuffleGA>();
        ActionSystem.DetachPerfomer<PlayCardGA>();
    }

    public IEnumerator DrawHandsAtStartPerformer(DrawHandsAtStartGA drawHandsAtStartGA)
    {
        int amount = drawHandsAtStartGA.Amount;
        int realAmount = Mathf.Min(amount, drawPile.Count);
        for (int i = 0; i < realAmount; i++) DrawHand();
        int lastAmount = amount - realAmount;
        if (lastAmount > 0 && discardPile.Count > 0)
        {
            ShuffleGA shuffleGA = new();
            ActionSystem.Instance.AddReaction(shuffleGA);
            DrawHandsAtStartGA secondDrawGA = new(lastAmount);
            ActionSystem.Instance.AddReaction(secondDrawGA);
        }
        yield return null;
    }
    public IEnumerator DiscardAllHandsPerformer(DiscardAllHandsGA discardAllHandsGA)
    {
        int count = hands.Count;
        for (int i = 0; i < count; i++) DiscardHand(hands[0]);
        yield return null;
    }
    public IEnumerator ShufflePerformer(ShuffleGA shuffleGA)
    {
        drawPile.AddRange(discardPile);
        discardPile.Clear();
        for (int i = drawPile.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (drawPile[i], drawPile[j]) = (drawPile[j], drawPile[i]);
        }

        drawPileChanged?.Invoke(drawPile.Count);
        discardPileChanged?.Invoke(discardPile.Count);

        yield return new WaitForSeconds(0.5f);
    }
    public IEnumerator PlayCardPerformer(PlayCardGA playCardGA)
    {
        Card card = playCardGA.Card;
        Entity target = playCardGA.Target;
        Entity caster = EntityManager.Instance.hero.Entity;

        EffectContext ctx = new EffectContext(playCardGA);
        foreach (var kv in card.StatMap) ctx.Vars[kv.Key] = kv.Value.GetValue();

        PerformEffectGA performEffectGA = new(card.Wrappers, caster, target, ctx);
        ActionSystem.Instance.AddReaction(performEffectGA);

        yield return null;
    }

    public void Setup(List<Card> deck)
    {
        drawPile.Clear();
        discardPile.Clear();
        hands.Clear();

        drawPile.AddRange(deck);

        drawPileChanged?.Invoke(drawPile.Count);
        discardPileChanged?.Invoke(discardPile.Count);
    }
    public void DrawHand()
    {
        if (drawPile.Count == 0)
        {
            Debug.LogWarning("Draw pile is empty.");
            return;
        }

        Card card = drawPile[0];
        drawPile.RemoveAt(0);

        hands.Add(card);
        CardManager.Instance.AddHand(card);

        drawPileChanged?.Invoke(drawPile.Count);
    }
    public void Play(Card card, Entity target = null)
    {
        GameManager.Instance.CurrentMana -= card.Mana;

        SelectSystem.Instance.SelectHand(null);

        PlayCardGA playCardGA = new(card, target);
        ActionSystem.Instance.Perform(playCardGA);

        DiscardHand(card);
    }
    public void DiscardHand(Card card)
    {
        if (card == null) return;

        hands.Remove(card);

        discardPile.Add(card);
        CardManager.Instance.RemoveHand(card);

        discardPileChanged?.Invoke(discardPile.Count);
    }
    public void DiscardAllHands(bool all = false)
    {
        int count = hands.Count;
        for (int i = 0; i < count; i++) DiscardHand(hands[0]);
    }

    public void LoadCardDatas()
    {
        CardData[] datas = Resources.LoadAll<CardData>(dataPath);
        int count = datas.Length;
        for (int i = 0; i < count; i++)
        {
            cardDataMap.Add(datas[i].CardID, datas[i]);
        }
    }
}
