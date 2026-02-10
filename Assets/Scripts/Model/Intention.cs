using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Intention
{
    [field: SerializeField] public string intentionID;
    [field: SerializeField] public ExposeType exposeType;
    [field: SerializeField] public bool multiAttack;
    [field: SerializeField] public Effect attack;
    [field: SerializeField] public List<WrappedEffects> toDo;
    [field: SerializeField] public string goToID;
}

public enum ExposeType
{
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
