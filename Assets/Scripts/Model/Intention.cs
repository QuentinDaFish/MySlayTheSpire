using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Intention
{
    [field: SerializeField] public string intentionID { get; private set; }
    [field: SerializeField] public ExposeType exposeType { get; private set; }
    [field: SerializeField] public int attackTimes { get; private set; }
    [field: SerializeField] public int damage { get; private set; }
    [field: SerializeField] public List<WrappedEffects> toDo { get; private set; }
    [field: SerializeField] public string goToID { get; private set; }
}

public enum ExposeType
{
    None,
    Unkown,
    Attack,
    AttackDebuff,
    AttackBuff,
    Defend,
    DefendBuff,
    Buff,
    Debuff,
    StrongDebuff,
    Escape,
    Sleep,
    Stun,
}
