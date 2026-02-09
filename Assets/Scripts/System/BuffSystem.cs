using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSystem : Singleton<BuffSystem>
{
    [SerializeField] private string dataPath;
    private Dictionary<BuffType, BuffData> buffDataMap = new();

    private void Start()
    {
        LoadBuffDatas();
    }
    private void OnEnable()
    {
        ActionSystem.AttachPerfomer<AddBuffGA>(AddBuffPerformer);
        ActionSystem.AttachPerfomer<RemoveBuffGA>(RemoveBuffPerformer);
    }
    private void OnDisable()
    {
        ActionSystem.DetachPerfomer<AddBuffGA>();
        ActionSystem.DetachPerfomer<RemoveBuffGA>();
    }

    public IEnumerator AddBuffPerformer(AddBuffGA addBuffGA)
    {
        if (addBuffGA.Amount <= 0) yield break;

        BuffType buffType = addBuffGA.BuffType;
        int amount = addBuffGA.Amount;
        List<Entity> targets = addBuffGA.Targets;

        foreach (Entity target in targets)
        {
            if (!target.buffs.TryGetValue(buffType, out var buff))
            {
                buff = GetBuff(buffType);
                if (buff == null) yield break;
                target.buffs.Add(buffType, buff);
                buff.Add(target);
            }
            if (buff.Stackable) buff.Change(amount);
            target.BuffChange(buff);
        }
    }
    public IEnumerator RemoveBuffPerformer(RemoveBuffGA removeBuffGA)
    {
        if (removeBuffGA.Amount <= 0) yield break;

        BuffType buffType = removeBuffGA.BuffType;
        int amount = removeBuffGA.Amount;
        List<Entity> targets = removeBuffGA.Targets;

        foreach (Entity target in targets)
        {
            if (target.buffs.TryGetValue(buffType, out var buff))
            {
                if (buff.Stackable)
                {
                    if (!buff.Signed && amount > buff.Stack) amount = buff.Stack;
                    if (amount <= 0) yield break;

                    buff.Change(-amount);
                }
                if (!buff.Stackable || buff.Stack == 0)
                {
                    buff.Remove();
                    target.buffs.Remove(buffType);
                }
                target.BuffChange(buff);
            }
        }
    }

    public Buff GetBuff(BuffType buffType)
    {
        BuffData data = GetBuffData(buffType);
        if (data == null) return null;
        return new(data);
    }
    public BuffData GetBuffData(BuffType buffType)
    {
        if (buffDataMap.ContainsKey(buffType)) return buffDataMap[buffType];
        return null;
    }
    public void LoadBuffDatas()
    {
        BuffData[] datas = Resources.LoadAll<BuffData>(dataPath);
        int count = datas.Length;
        for (int i = 0; i < count; i++)
        {
            buffDataMap.Add(datas[i].BuffType, datas[i]);
        }
    }
}

public enum BuffType
{
    None,
    Block,
    Strength,
    Weak,
    Vulnerable,
    Dexterity,
    Frail,
}