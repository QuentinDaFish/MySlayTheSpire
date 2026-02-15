using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("¡Ÿ ±±‰¡ø")]
    public Transform heroPoint;
    public List<Transform> enemyPoints;
    public List<EnemyData> enemyDatas = new();
    public HeroData heroData;

    public static event Action<int, int> ManaChanged;

    public int maxManaInTurn { get; private set; }
    public int currentMana;
    public int CurrentMana
    {
        get { return currentMana; }
        set
        {
            if (currentMana == value) return;
            currentMana = value;
            ManaChanged?.Invoke(CurrentMana, maxManaInTurn);
        }
    }

    private void Start()
    {
        EntitySystem.Instance.SetupHero(heroData);
        EntitySystem.Instance.SetupEnemies(enemyDatas);

        maxManaInTurn = 3;

        LevelBeginGA levelBeginGA = new();
        ActionSystem.Instance.Perform(levelBeginGA, PlayerTurnBegin);
    }
    private void OnEnable()
    {
        ActionSystem.AttachPerfomer<LevelBeginGA>(LevelBeginPerformer);
        ActionSystem.AttachPerfomer<LevelOverGA>(LevelOverPerformer);
        ActionSystem.AttachPerfomer<TurnBeginGA>(TurnBeginPerformer);
        ActionSystem.AttachPerfomer<TurnOverGA>(TurnOverPerformer);
        ActionSystem.AttachPerfomer<EnemyTurnGA>(EnemyTurnPerformer);
        ActionSystem.AttachPerfomer<EnemyActionGA>(EnemyActionPerformer);
    }

    private void OnDisable()
    {
        ManaChanged = null;

        ActionSystem.DetachPerfomer<LevelBeginGA>();
        ActionSystem.DetachPerfomer<LevelOverGA>();
        ActionSystem.DetachPerfomer<TurnBeginGA>();
        ActionSystem.DetachPerfomer<TurnOverGA>();
        ActionSystem.DetachPerfomer<EnemyTurnGA>();
        ActionSystem.DetachPerfomer<EnemyActionGA>();
    }

    public IEnumerator LevelBeginPerformer(LevelBeginGA levelBeginGA)
    {
        CardSystem.Instance.Setup(PlayerSystem.Instance.Deck);

        List<Entity> enemies = EntityManager.Instance.GetEntity(TargetMode.AllEnemy);
        foreach (Enemy enemy in enemies)
        {
            Intention intention = enemy.GetIntention(enemy.EnemyData.FirstIntentionID);
            enemy.ChangeIntention(intention);
        }

        yield return null;
    }
    public IEnumerator LevelOverPerformer(LevelOverGA levelOverGA)
    {
        yield return null;
    }
    public IEnumerator TurnBeginPerformer(TurnBeginGA turnBeginGA)
    {
        Faction faction = turnBeginGA.Faction;
        if (faction == Faction.Hero)
        {
            CurrentMana = maxManaInTurn;
            DrawHandsAtStartGA drawHandsAtStartGA = new(5);
            ActionSystem.Instance.AddReaction(drawHandsAtStartGA);
        }

        yield return null;
    }
    public IEnumerator TurnOverPerformer(TurnOverGA turnOverGA)
    {
        Faction faction = turnOverGA.Faction;
        if (faction == Faction.Hero)
        {
            DiscardAllHandsGA discardAllHandsGA = new();
            ActionSystem.Instance.AddReaction(discardAllHandsGA);
        }
        else if(faction == Faction.Enemy)
        {
            List<Entity> enemies = EntityManager.Instance.GetEntity(TargetMode.AllEnemy);
            foreach (Enemy enemy in enemies)
            {
                Intention intention = enemy.GetNextIntention();
                enemy.ChangeIntention(intention);
            }
        }

        yield return null;
    }
    public IEnumerator EnemyTurnPerformer(EnemyTurnGA enemyTurnGA)
    {
        List<Entity> enemies = EntityManager.Instance.GetEntity(TargetMode.AllEnemy);
        foreach(Entity enemy in enemies)
        {
            EnemyActionGA enemyActionGA = new((Enemy)enemy);
            ActionSystem.Instance.AddReaction(enemyActionGA);
        }
        yield return null;
    }
    public IEnumerator EnemyActionPerformer(EnemyActionGA enemyActionGA)
    {
        Entity enemy = enemyActionGA.Enemy;

        yield return null;
    }

    public void PlayerTurnBegin()
    {
        TurnBeginGA turnBeginGA = new(Faction.Hero);
        ActionSystem.Instance.Perform(turnBeginGA);
    }
    public void PlayerTurnOver()
    {
        TurnOverGA turnOverGA = new(Faction.Hero);
        ActionSystem.Instance.Perform(turnOverGA, EnemyTurnBegin);
    }
    public void EnemyTurnBegin()
    {
        TurnBeginGA turnBeginGA = new(Faction.Enemy);
        ActionSystem.Instance.Perform(turnBeginGA, EnemyTurn);
    }
    public void EnemyTurn()
    {
        EnemyTurnGA enemyTurnGA = new();
        ActionSystem.Instance.Perform(enemyTurnGA, EnemyTurnOver);
    }
    public void EnemyTurnOver()
    {
        TurnOverGA turnOverGA = new(Faction.Enemy);
        ActionSystem.Instance.Perform(turnOverGA, PlayerTurnBegin);
    }

}

public enum GameState
{
    None,
    Ready,
    Playing,
}
