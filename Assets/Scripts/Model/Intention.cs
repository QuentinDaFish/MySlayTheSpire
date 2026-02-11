using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Intention
{
    public string intentionID { get; private set; }
    public ExposeType exposeType { get; private set; }
    public string attackDesc { get; private set; }
    public List<WrappedEffects> toDo { get; private set; }
    public string goToID { get; private set; }
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
