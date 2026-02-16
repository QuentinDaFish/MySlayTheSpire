using UnityEngine;

public class AddPerkCountGA : GameAction
{
    public PerkData PerkData { get; private set; }
    public int Count { get; private set; }
    public AddPerkCountGA(PerkData perkData, int count = 1)
    {
        PerkData = perkData;
        Count = count;
    }
}
