using System;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    public event Action<Collider, bool> OnTargetsChanged;
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"OnTriggerEnter: {other.name}");
        OnTargetsChanged?.Invoke(other, true);
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"OnTriggerExit: {other.name}");
        OnTargetsChanged?.Invoke(other, false);
    }
}
