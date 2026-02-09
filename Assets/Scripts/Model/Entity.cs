using System;
using System.Collections.Generic;
using UnityEngine;

public class Entity
{
    public event Action Dead;
    public event Action<float, float> HealthChanged;
    public event Action<Buff> BuffChanged;

    public EntityData Data { get; private set; }
    public string EntityName => Data.EntityName;
    public string Desc => Data.Desc;
    public Sprite Image => Data.Image;
    public int MaxHealth { get; private set; }
    public Faction Faction { get; private set; }
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
    public bool isDead { get; private set; }
    public readonly Dictionary<BuffType, Buff> buffs = new();

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
}

public enum Faction
{
    Hero,
    Enemy,
}