using System;
using UnityEngine;

[Serializable]
public class Stat
{
    [field: SerializeField] public string id { get; private set; }
    [field: SerializeField] public int value { get; private set; }
    [field: SerializeField] public StatType type { get; private set; } = StatType.Generic;

    public Stat Clone()
    {
        return new Stat
        {
            id = this.id,
            value = this.value,
            type = this.type
        };
    }
    public int GetValue() => value;
    public int ChangeValue(int amount) => value += amount;
}

public enum StatType
{
    Generic,
    Attack,
    Block,
}
