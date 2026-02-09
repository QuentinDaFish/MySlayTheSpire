using System.Collections.Generic;
using UnityEngine;

public class EntityManager : Singleton<EntityManager>
{
    [SerializeField] private EntityView entityViewPrefab;

    public EntityView hero { get; private set; }
    public readonly List<EntityView> enemies = new();

    

    public EntityView CreateEntity(Entity entity, Vector2 pos)
    {
        EntityView view = Instantiate(entityViewPrefab, transform);
        view.Setup(entity);
        
        if (view.Entity.Faction == Faction.Hero) hero = view ;
        else if(view.Entity.Faction == Faction.Enemy) enemies.Add(view);
        
        view.transform.position = pos;

        return view;
    }

    public List<Entity> GetEntity(TargetMode targetMode)
    {
        List<Entity> targets = new();
        if (targetMode == TargetMode.Hero) targets.Add(hero.Entity);
        else if (targetMode == TargetMode.AllEnemy)
        {
            foreach (EntityView enemy in enemies) targets.Add(enemy.Entity);
        }
        else if (targetMode == TargetMode.RandomEnemy) targets.Add(enemies[Random.Range(0, enemies.Count)].Entity);
        return targets;
    }
}
