using UnityEngine;

public class Enemy : Entity
{
    public Intention currentIntention { get; private set; }
    public Enemy(EnemyData data) : base(data, Faction.Enemy)
    {
    }

    public void Action()
    {

    }
}
