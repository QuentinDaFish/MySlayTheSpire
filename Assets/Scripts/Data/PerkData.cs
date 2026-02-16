using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Perk", menuName = "Data/Perk")]
public class PerkData : ScriptableObject
{
    [field: SerializeField] public string PerkID { get; private set; }
    [field: SerializeField] public string PerkName { get; private set; }
    [field: SerializeField] public string Desc { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public List<GameActionTrigger> Triggers { get; private set; }
    [field: SerializeField] public List<WrappedEffects> OnAdd { get; private set; }
    [field: SerializeField] public List<WrappedEffects> OnRemove { get; private set; }
    [field: SerializeField] public bool Countable { get; private set; }
}
