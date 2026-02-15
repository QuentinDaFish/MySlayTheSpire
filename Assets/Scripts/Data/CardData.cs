using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Data/Card")]
public class CardData : ScriptableObject
{
    [field: SerializeField] public string CardID { get; private set; }
    [field: SerializeField] public string CardName { get; private set; }
    [field: SerializeField] public string Desc { get; private set; }
    [field: SerializeField] public Sprite Image { get; private set; }
    [field: SerializeField] public CardType CardType { get; private set; }
    [field: SerializeField] public Rarity Rarity { get; private set; }
    [field: SerializeField] public int Mana { get; private set; }
    [field: SerializeField] public List<Stat> Stats { get; private set; }
    [field: SerializeField] public List<WrappedEffects> Wrappers { get; private set; }
    [field: SerializeField] public bool ExhaustAfterPlay { get; private set; }
    [field: SerializeField] public int MaxLevel { get; private set; }
    [field: SerializeField] public List<CardUpgrade> Upgrades { get; private set; }
}

public enum CardType
{
    Attack,
    Skill,
    Ability,
    Curse,
}

public enum Rarity
{
    Normal,
    Rare,
    Epic,
}