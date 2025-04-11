using System;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    public readonly List<Collider> Targets = new();
    
    public event Action OnTargetsChanged;
    
    private void OnTriggerEnter(Collider other)
    {
        Targets.Add(other);
        OnTargetsChanged?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        Targets.Remove(other);
        OnTargetsChanged?.Invoke();
    }
}
