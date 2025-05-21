using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ScoreOnDeath : MonoBehaviour
{
    private ScoreController _scoreController;
    private EntityStats _entityStats;

    private void Start()
    {
        _scoreController = FindFirstObjectByType<ScoreController>();
    }

    private readonly Dictionary<StatType, float> _scoreWeights = new Dictionary<StatType, float>()
    {
        { StatType.MaxHealth, 1},
        { StatType.Armor, 3},
        { StatType.Power, 10},
    };

    private void Awake()
    {
        _entityStats = GetComponent<EntityStats>();
    }

    private void OnEnable()
    {
        _entityStats.OnDeath += AddEnemyScore;
    }

    private void OnDisable()
    {
        _entityStats.OnDeath -= AddEnemyScore;
    }

    private void AddEnemyScore()
    {
        float score = 0;
        Stat stat;
        foreach (var scoreWeight in _scoreWeights)
        {
            if (_entityStats.Stats.TryGetValue(scoreWeight.Key, out stat))
            {
                score += stat.Value * scoreWeight.Value ;
            }
        }
        
        _scoreController.AddScore(Mathf.RoundToInt(score));
    }

}