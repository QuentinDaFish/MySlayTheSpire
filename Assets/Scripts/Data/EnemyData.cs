using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Data/Entity/Enemy")]
public class EnemyData : EntityData
{
    [Header("Intention")]
    [field: SerializeField] public List<StatData> Stats { get; private set; }
    [field: SerializeField] public string FirstIntentionID { get; private set; }
    [field: SerializeField] public List<Intention> Intentions { get; private set; }
}
