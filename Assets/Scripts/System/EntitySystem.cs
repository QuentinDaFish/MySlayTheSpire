using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySystem : Singleton<EntitySystem>
{
    public readonly List<Entity> enemies = new();

    private void OnEnable()
    {
        ActionSystem.AttachPerfomer<EnemyActionGA>(EnemyActionPerformer);
        ActionSystem.AttachPerfomer<ChangeEntityStatsGA>(ChangeEntityStatsPerformer);
    }
    private void OnDisable()
    {
        ActionSystem.DetachPerfomer<EnemyActionGA>();
        ActionSystem.DetachPerfomer<ChangeEntityStatsGA>();
    }

    public IEnumerator EnemyActionPerformer(EnemyActionGA enemyActionGA)
    {
        Enemy enemy = enemyActionGA.Enemy;
        Intention intention = enemy.currentIntention;



        yield return null;
    }
    public IEnumerator ChangeEntityStatsPerformer(ChangeEntityStatsGA changeEntityStatsGA)
    {
        float attackMultiplier = changeEntityStatsGA.AttackMultiplier;
        float blockMultiplier = changeEntityStatsGA.BlockMultiplier;
        float fragileMultiplier = changeEntityStatsGA.FragileMultiplier;
        List<Entity> targets = changeEntityStatsGA.Targets;

        foreach (Entity target in targets)
        {
            target.attackMultiplier += attackMultiplier;
            target.blockMultiplier += blockMultiplier;
            target.fragileMultipllier += fragileMultiplier;
        }

        yield return null;
    }

    public void SetupHero(HeroData heroData)
    {
        Hero hero = new(heroData);
        PlayerSystem.Instance.SetHero(hero);
        EntityManager.Instance.CreateEntity(hero, GameManager.Instance.heroPoint.position);
    }
    public void SetupEnemies(List<EnemyData> enemyDatas)
    {
        enemies.Clear();
        int count = enemyDatas.Count;
        for (int i = 0; i < count; i++)
        {
            Enemy enemy = new(enemyDatas[i]);
            enemies.Add(enemy);
            EntityManager.Instance.CreateEntity(enemy, GameManager.Instance.enemyPoints[i].position);
        }
    }
    public List<Entity> GetEntities(Faction faction)
    {
        List<Entity> entities = new();
        if (faction == Faction.Hero) entities.Add(PlayerSystem.Instance.Hero);
        else if (faction == Faction.Enemy) entities.AddRange(enemies);
        return entities;
    }
}
