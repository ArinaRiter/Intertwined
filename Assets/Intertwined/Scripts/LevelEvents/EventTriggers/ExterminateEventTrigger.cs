using System;
using UnityEngine;

public class ExterminateEventTrigger : BaseEventTrigger
{
    [SerializeField] private int exterminateThreshold;

    private EntityStats[] _entities;
    private int _counter;
    
    public override event Action OnEventTriggered;

    private void OnEnable()
    {
        _entities = FindObjectsByType<EntityStats>(FindObjectsSortMode.None);
        foreach (var entity in _entities)
        {
            entity.OnDeath += UpdateCounter;
        }
    }

    private void OnDisable()
    {
        _entities = FindObjectsByType<EntityStats>(FindObjectsSortMode.None);
        foreach (var entity in _entities)
        {
            entity.OnDeath -= UpdateCounter;
        }
    }

    private void UpdateCounter()
    {
        _counter++;
        if (!_triggered && _counter >= exterminateThreshold)
        {
            OnEventTriggered?.Invoke();
            _triggered = true;
        }
    }

}
