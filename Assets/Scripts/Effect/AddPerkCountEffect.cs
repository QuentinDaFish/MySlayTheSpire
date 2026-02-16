using System.Collections.Generic;
using UnityEngine;

public class AddPerkCountEffect : Effect
{
    [field: SerializeField] public PerkData perkData { get; private set; }
    [field: SerializeField] public int count { get; private set; }
    public override GameAction GetGameAction(EffectContext ctx, List<Entity> targets, Entity caster)
    {
        AddPerkCountGA addPerkCountGA = new(perkData, count);
        return addPerkCountGA;
    }
}
