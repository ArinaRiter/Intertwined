using System;
using UnityEngine;

public class Detector : MonoBehaviour
{
    public event Action<Collider, bool> OnTargetsChanged;
    
    private void OnTriggerEnter(Collider other)
    {
        OnTargetsChanged?.Invoke(other, true);
    }

    private void OnTriggerExit(Collider other)
    {
        OnTargetsChanged?.Invoke(other, false);
    }
}
