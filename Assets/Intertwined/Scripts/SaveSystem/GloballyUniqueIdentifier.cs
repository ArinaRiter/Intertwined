using System;
using UnityEngine;

public class GloballyUniqueIdentifier : MonoBehaviour
{
    [SerializeField] private string guid;
    
    public string GUID => guid;
    
    private void Reset()
    {
        guid = Guid.NewGuid().ToString();
    }
}
