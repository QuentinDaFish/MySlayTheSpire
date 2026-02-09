using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Intention
{
    [field: SerializeField] public int intentionID;
    [field: SerializeField] public ExposeType exposeType;
    [field: SerializeField] public List<WrappedEffects> attack;
    [field: SerializeField] public List<WrappedEffects> toDo;
    [field: SerializeField] public int goToID;
}

public enum ExposeType
{

}
