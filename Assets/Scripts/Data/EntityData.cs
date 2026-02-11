using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Entity", menuName = "Data/Entity")]
public class EntityData : ScriptableObject
{
    [field: SerializeField] public string EntityID { get; private set; }
    [field: SerializeField] public string EntityName { get; private set; }
    [field: SerializeField] public string Desc { get; private set; }
    [field: SerializeField] public Sprite Image { get; private set; }
    [field: SerializeField] public int MaxHealth { get; private set; }
}