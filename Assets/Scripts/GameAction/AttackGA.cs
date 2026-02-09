using System.Collections.Generic;
using UnityEngine;

public class AttackGA : GameAction
{
    public int Damage;
    public List<Entity> Targets;
    public Entity Caster;
    public bool FromAttack;
    public int finalDamage;
    public AttackGA(int damage, List<Entity> targets, Entity caster, bool fromAttack = true)
    {
        Damage = damage;
        Targets = targets;
        Caster = caster;
        FromAttack = fromAttack;
    }
}