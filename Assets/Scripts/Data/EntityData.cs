using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Entity", menuName = "Data/Entity")]
public class EntityData : ScriptableObject
{
    [field: SerializeField] public string EntityName;
    [field: SerializeField] public string Desc;
    [field: SerializeField] public Sprite Image;
    [field: SerializeField] public int MaxHealth;
    [field: SerializeField] public List<WrappedEffects> BeginAction;
    [field: SerializeField] public List<Intention> intentions;
}