using System;
using UnityEngine;

[Serializable]
public class CardUpgrade
{
    [field: SerializeField] public string id { get; private set; }
    [field: SerializeField] public int adjust { get; private set; }
}
