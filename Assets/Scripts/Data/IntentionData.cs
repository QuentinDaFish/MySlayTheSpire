using UnityEngine;

[CreateAssetMenu(fileName = "New Intention", menuName = "Data/Intention")]
public class IntentionData : ScriptableObject
{
    [field: SerializeField] public string IntentionName { get; private set; }
    [field: SerializeField] public string Desc { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public ExposeType ExposeType { get; private set; }
}
