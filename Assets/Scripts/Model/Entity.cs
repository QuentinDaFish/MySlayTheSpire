using System;
using System.Collections.Generic;
using UnityEngine;

public class Entity
{
    public event Action Dead;
    public event Action<float, float> HealthChanged;
    public event Action<Buff> BuffChanged;
    public event Action<ExposeType, int, int> PreviewChanged;

    public EntityData Data { get; protected set; }
    public string EntityName => Data.EntityName;
    public string Desc => Data.Desc;
    public Sprite Image => Data.Image;
    public int MaxHealth { get; protected set; }
    public Faction Faction { get; protected set; }
    private int currentHealth;
    public int CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            value = Mathf.Min(value, MaxHealth);
            value = Mathf.Max(value, 0);
            if (currentHealth != value)
            {
                currentHealth = value;
                HealthChanged?.Invoke(CurrentHealth, MaxHealth);
                if (currentHealth <= 0)
                {
                    isDead = true;
                    Dead?.Invoke();
                }
            }
        }
    }
    public bool isDead { get; protected set; }
    public readonly Dictionary<BuffType, Buff> buffs = new();
    public Intention currentIntention { get; protected set; }

    public float attackMultiplier;
    public float blockMultiplier;
    public float fragileMultipllier;

    public Entity(EntityData data, Faction faction)
    {
        Data = data;
        MaxHealth = data.MaxHealth;
        Faction = faction;

        CurrentHealth = data.MaxHealth;
    }

    public int Damaged(int damage)
    {
        int finalDamage = damage;
        int armor = GetBuffStack(BuffType.Block);
        if (armor > 0)
        {
            finalDamage = damage - armor;

            RemoveBuffGA removeBuffGA = new(BuffType.Block, damage, new() { this });
            ActionSystem.Instance.AddReaction(removeBuffGA);

        }
        if (finalDamage > 0) CurrentHealth -= finalDamage;
        return finalDamage;
    }
    public void LostHealth(int amount)
    {
        CurrentHealth -= amount;
    }
    public void BuffChange(Buff buff) => BuffChanged?.Invoke(buff);
    public int GetBuffStack(BuffType buffType)
    {
        if (buffs.TryGetValue(buffType, out var buff)) return buff.Stack;
        return 0;
    }
    public void UpdatePreview()
    {
        if (currentIntention == null) PreviewChanged?.Invoke(ExposeType.None, 0, 0);
        // if player should not see, also pass None.

        Intention intention = currentIntention;
        ExposeType type = intention.exposeType;
        int attackTimes = intention.attackTimes;
        int damage = intention.damage;

        if (attackTimes > 0)
        {  
            Entity target = PlayerSystem.Instance.Hero;
            damage += GetBuffStack(BuffType.Strength);
            damage = (int)(damage * (1f + attackMultiplier));
            if (target != null) damage = (int)(damage * (1f + target.fragileMultipllier));
        }

        PreviewChanged?.Invoke(type, attackTimes, damage);
    }
}

public enum Faction
{
    Hero,
    Enemy,
}