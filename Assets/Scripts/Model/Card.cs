using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Card
{
    public CardData Data { get; private set; }
    public string CardName => Data.CardName;
    public string Desc => Data.Desc;
    public Sprite Image => Data.Image;
    public CardType CardType => Data.CardType;
    public Rarity Rarity => Data.Rarity;
    public int Mana { get; private set; }
    public bool NeedTarget => Data.Wrappers.Count != 0 ? Data.Wrappers.Any(wrapper => wrapper.targetMode == TargetMode.Target) : false;
    public List<WrappedEffects> Wrappers => Data.Wrappers;
    public Dictionary<string, Stat> StatMap;
    public Card(CardData data)
    {
        Data = data;
        Mana = data.Mana;

        GenerateStatMap();
    }
    private void GenerateStatMap()
    {
        StatMap = new Dictionary<string, Stat>(Data.Stats?.Count ?? 0);

        if (Data.Stats == null) return;

        for (int i = 0; i < Data.Stats.Count; i++)
        {
            StatData statData = Data.Stats[i];
            Stat stat = new(statData.value, statData.type);
            StatMap[statData.id] = stat;
        }
    }
    public string GetDesc(Entity target = null)
    {
        var sb = new StringBuilder(Desc);

        foreach (var kv in StatMap)
        {
            string id = kv.Key;
            Stat stat = kv.Value;
            if (stat == null || string.IsNullOrWhiteSpace(id)) continue;

            int baseValue = stat.BaseValue;
            int finalValue = stat.GetValue();

            if (stat.Type == StatType.Attack)
            {
                Entity caster = PlayerSystem.Instance.Hero;
                finalValue += caster.GetBuffStack(BuffType.Strength);
                finalValue = (int)(finalValue * (1f + caster.attackMultiplier));
                if (target != null) finalValue = (int)(finalValue * (1f + target.fragileMultipllier));
            }
            else if (stat.Type == StatType.Block)
            {
                Entity caster = PlayerSystem.Instance.Hero;
                finalValue += caster.GetBuffStack(BuffType.Dexterity);
                finalValue = (int)(finalValue * (1f + caster.blockMultiplier));
            }

            string color = finalValue > baseValue ? "#34C759" : finalValue < baseValue ? "#FF3B30" : "#FFFFFF";

            sb.Replace("{" + id + "}", $"<color={color}>{finalValue}</color>");
        }

        return sb.ToString();
    }
}
