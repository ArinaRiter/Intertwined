using System;
using UnityEngine;

public abstract class BaseEventTrigger : MonoBehaviour
{
    private protected bool _triggered;
    
    public abstract event Action OnEventTriggered;
}
