using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public class Card
{
    public CardData Data { get; private set; }
    public string CardName { get; private set; }
    public string Desc => Data.Desc;
    public Sprite Image => Data.Image;
    public CardType CardType => Data.CardType;
    public Rarity Rarity => Data.Rarity;
    public int Mana { get; private set; }
    public bool NeedTarget => Data.Wrappers.Count != 0 ? Data.Wrappers.Any(wrapper => wrapper.targetMode == TargetMode.Target) : false;
    public Dictionary<string, Stat> Stats;
    public List<WrappedEffects> Wrappers => Data.Wrappers;
    public bool ExhaustAfterPlay => Data.ExhaustAfterPlay;
    public int MaxLevel => Data.MaxLevel;
    public int Level = 0;
    public Card(CardData data)
    {
        Data = data;
        CardName = data.CardName;
        Mana = data.Mana;

        GenerateStatMap();
    }
    private void GenerateStatMap()
    {
        Stats = new Dictionary<string, Stat>(Data.Stats?.Count ?? 0);
        if (Data.Stats == null) return;

        for (int i = 0; i < Data.Stats.Count; i++)
        {
            Stat stat = Data.Stats[i].Clone();
            Stats[stat.id] = stat;
        }
    }
    public void Upgrade()
    {
        if (Level >= MaxLevel) return;

        foreach (CardUpgrade upgrade in Data.Upgrades)
        {
            string id = upgrade.id;
            int adjust = upgrade.adjust;
            if (id == "Mana") Mana = adjust;
            else if (id == "Exhaust") { }
            else if (Stats.ContainsKey(id)) Stats[id].ChangeValue(adjust + Level);
        }

        Level++;

        CardName = CardName + (MaxLevel >= 1 ? "+" + Level.ToString() : "+");
    }
    public string GetDesc(Entity target = null, bool preview = true)
    {
        var sb = new StringBuilder(Desc);

        foreach (var kv in Stats)
        {
            string id = kv.Key;
            Stat stat = kv.Value;
            if (stat == null || string.IsNullOrWhiteSpace(id)) continue;

            int baseValue = stat.GetValue();
            int finalValue = baseValue;

            if (preview)
            {
                if (stat.type == StatType.Attack)
                {
                    Entity caster = PlayerSystem.Instance.Hero;
                    finalValue += caster.GetBuffStack(BuffType.Strength);
                    finalValue = (int)(finalValue * (1f + caster.attackMultiplier));
                    if (target != null) finalValue = (int)(finalValue * (1f + target.fragileMultipllier));
                }
                else if (stat.type == StatType.Block)
                {
                    Entity caster = PlayerSystem.Instance.Hero;
                    finalValue += caster.GetBuffStack(BuffType.Dexterity);
                    finalValue = (int)(finalValue * (1f + caster.blockMultiplier));
                }
            }

            string color = finalValue > baseValue ? "#34C759" : finalValue < baseValue ? "#FF3B30" : "#FFFFFF";

            sb.Replace("{" + id + "}", $"<color={color}>{finalValue}</color>");
        }

        if (ExhaustAfterPlay) sb.Append("\nÏûºÄ");

        return sb.ToString();
    }
}
