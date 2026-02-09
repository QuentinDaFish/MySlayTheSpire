using System;
using UnityEngine;

public class Stat
{
    public int BaseValue;
    public StatType Type;

    private int addition;

    public Stat(int baseValue, StatType type)
    {
        BaseValue = baseValue;
        Type = type;
    }

    public void SetAddition(int amount) => addition = amount;
    public void ChangeAddition(int amount) => SetAddition(addition + amount);

    public int GetValue() => BaseValue + addition;
}

[Serializable]
public class StatData
{
    [field: SerializeField] public string id;
    [field: SerializeField] public int value;
    [field: SerializeField] public StatType type = StatType.Generic;
}

public enum StatType
{
    Generic,
    Attack,
    Block,
}
