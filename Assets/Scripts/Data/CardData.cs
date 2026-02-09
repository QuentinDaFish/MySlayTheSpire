using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Data/Card")]
public class CardData : ScriptableObject
{
    [field: SerializeField] public int CardID;
    [field: SerializeField] public string CardName;
    [field: SerializeField] public string Desc;
    [field: SerializeField] public Sprite Image;
    [field: SerializeField] public CardType CardType;
    [field: SerializeField] public Rarity Rarity;
    [field: SerializeField] public int Mana;
    [field: SerializeField] public List<StatData> Stats;
    [field: SerializeField] public List<WrappedEffects> Wrappers;
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