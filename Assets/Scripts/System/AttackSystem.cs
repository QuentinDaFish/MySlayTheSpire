using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSystem : Singleton<AttackSystem>
{
    private void OnEnable()
    {
        ActionSystem.AttachPerfomer<AttackGA>(AttackPerformer);
        ActionSystem.AttachPerfomer<DealDamageGA>(DealDamagePerformer);
        ActionSystem.AttachPerfomer<LoseHealthGA>(LoseHealthPerformer);
        ActionSystem.AttachPerfomer<AddBlockGA>(AddBlockPerformer);
    }
    private void OnDisable()
    {
        ActionSystem.DetachPerfomer<AttackGA>();
        ActionSystem.DetachPerfomer<DealDamageGA>();
        ActionSystem.DetachPerfomer<LoseHealthGA>();
        ActionSystem.DetachPerfomer<AddBlockGA>();
    }

    public IEnumerator AttackPerformer(AttackGA attackGA)
    {
        int damage = attackGA.Damage;
        List<Entity> targets = attackGA.Targets;
        Entity caster = attackGA.Caster;

        foreach (Entity target in targets)
        {
            if (target.isDead) continue;

            int finalDamage = damage;

            finalDamage += caster.GetBuffStack(BuffType.Strength);
            finalDamage = (int)(finalDamage * (1 + caster.attackMultiplier));
            finalDamage = (int)(finalDamage * (1 + target.fragileMultipllier));

            DealDamageGA dealDamageGA = new(finalDamage, new() { target });
            ActionSystem.Instance.AddReaction(dealDamageGA);
        }
        yield return null;
    }
    public IEnumerator DealDamagePerformer(DealDamageGA dealDamageGA)
    {
        int damage = dealDamageGA.Damage;
        List<Entity> targets = dealDamageGA.Targets;

        foreach(Entity target in targets)
        {
            if (target.isDead) continue;

            int finalDamage = damage;
            int armor = target.GetBuffStack(BuffType.Block);
            if (armor > 0)
            {
                finalDamage = damage - armor;
                RemoveBuffGA removeBuffGA = new(BuffType.Block, damage, new() { target });
                ActionSystem.Instance.AddReaction(removeBuffGA);
            }
            if (finalDamage > 0)
            {
                LoseHealthGA loseHealthGA = new(finalDamage, new() { target });
                ActionSystem.Instance.AddReaction(loseHealthGA);
            }
        }

        yield return null;
    }
    public IEnumerator LoseHealthPerformer(LoseHealthGA loseHealthGA)
    {
        int amount = loseHealthGA.Amount;
        List<Entity> targets = loseHealthGA.Targets;

        foreach (Entity target in targets)
        {
            if (target.isDead) continue;
            if (amount > 0) target.CurrentHealth -= amount;
        }

        yield return null;
    }
    public IEnumerator AddBlockPerformer(AddBlockGA addBlockGA)
    {
        if (addBlockGA.Amount <= 0) yield break;

        int amount = addBlockGA.Amount;
        List<Entity> targets = addBlockGA.Targets;
        bool fromCard = addBlockGA.FromCard;

        foreach (Entity target in targets)
        {
            int realAmount = amount;

            if (fromCard)
            {
                realAmount += target.GetBuffStack(BuffType.Dexterity);
                realAmount = (int)(realAmount * (1 + target.blockMultiplier));
            }

            AddBuffGA addBuffGA = new(BuffType.Block, realAmount, new() { target });
            ActionSystem.Instance.AddReaction(addBuffGA);
        }
    }
}
