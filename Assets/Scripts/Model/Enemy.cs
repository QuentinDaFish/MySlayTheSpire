using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Enemy : Entity
{
    public EnemyData EnemyData { get; private set; }
    public Dictionary<string, Intention> intentionMap;
    public Enemy(EnemyData data) : base(data, Faction.Enemy)
    {
        EnemyData = data;
        intentionMap = new Dictionary<string, Intention>(data.Intentions?.Count ?? 0);
        if (data.Intentions == null) return;

        foreach (Intention intention in data.Intentions)
        {
            intentionMap.Add(intention.intentionID, intention);
        }
    }
    public Intention GetIntention(string intentionID)
    {
        if (intentionMap.TryGetValue(intentionID, out Intention intention)) return intention;
        else return null;
    }
    public Intention GetNextIntention()
    {
        if (isDead) return null;
        if (currentIntention != null && intentionMap.Count > 0)
        {
            string id = currentIntention.goToID;
            return GetIntention(id);
        }

        return null;
    }
    public void ChangeIntention(Intention intention)
    {
        currentIntention = intention;
        UpdatePreview();
    }
}
