using System;
using UnityEngine;

public class LevelEventManager : MonoBehaviour
{
    [SerializeField] private LevelEvent[] levelEvents;

    private void OnEnable()
    {
        foreach (var levelEvent in levelEvents)
        {
            levelEvent.trigger.OnEventTriggered += levelEvent.reaction.React;
        }
    }
    
    private void OnDisable()
    {
        foreach (var levelEvent in levelEvents)
        {
            levelEvent.trigger.OnEventTriggered -= levelEvent.reaction.React;
        }
    }
}

[Serializable]
public struct LevelEvent
{
    public BaseEventTrigger trigger;
    public BaseEventReaction reaction;
}