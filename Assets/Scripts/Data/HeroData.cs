using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hero", menuName = "Data/Entity/Hero")]
public class HeroData : EntityData
{
    [field: SerializeField] public HeroType HeroType { get; private set; } 
    [Header("Card Deck")]
    [field: SerializeField] public List<CardData> Deck { get; private set; }
}

public enum HeroType
{
    Warrior,
    Hunter,
}