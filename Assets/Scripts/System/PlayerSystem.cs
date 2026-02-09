using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSystem : Singleton<PlayerSystem>
{
    public static event Action<int> DeckChanged;

    public Entity Hero { get; private set; }
    public List<Card> Deck { get; private set; } = new();
    public int Money { get; private set; }

    //public List<Peak> Peaks { get; private set; } = new();

    private void OnDisable()
    {
        DeckChanged = null;
    }

    public void SetHero(Entity hero) => Hero = hero;
    public void AddCard(Card card)
    {
        Deck.Add(card);
        DeckChanged?.Invoke(Deck.Count);
    }
    public void RemoveCard(Card card)
    {
        Deck.Remove(card);
        DeckChanged?.Invoke(Deck.Count);
    }
}
