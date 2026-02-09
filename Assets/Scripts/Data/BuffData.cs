using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Buff", menuName = "Data/Buff")]
public class BuffData : ScriptableObject
{
    [field: SerializeField] public BuffType BuffType { get; protected set; }
    [field: SerializeField] public string BuffName { get; private set; }
    [field: SerializeField] public string Desc { get; private set; }
    [field: SerializeField] public Sprite Image { get; private set; }
    [field: SerializeField] public bool Stackable { get; private set; } = true;
    [field: SerializeField] public bool Signed { get; private set; } = false;
    [field: SerializeField] public BuffClearType ClearType { get; private set; }
    [field: SerializeReference, SR] public List<GameActionTrigger> Triggers { get; private set; }
    [field: SerializeField] public List<WrappedEffects> OnAdd { get; private set; }
    [field: SerializeField] public List<WrappedEffects> OnRemove { get; private set; }
}

public enum BuffClearType
{
    Forever,
    TurnBegin,
    TurnOver,
    ClearBegin,
    ClearOver,
}