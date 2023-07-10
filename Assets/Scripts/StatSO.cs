using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Entity Stats", fileName = "New Stats")]
public class StatSO : ScriptableObject
{
    [SerializeField] private List<StatInfo> stats;
    public List<StatInfo> Stats => stats;
}

[Serializable]
public struct StatInfo
{
    public StatType StatType;
    public float BaseValue;
}