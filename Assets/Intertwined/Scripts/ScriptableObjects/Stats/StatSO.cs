using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Entity Stats", fileName = "New Stats", order = 55)]
public class StatSO : ScriptableObject
{
    [SerializeField] private List<StatInfo> stats;
    public ReadOnlyCollection<StatInfo> Stats => new(stats);
}

[Serializable]
public struct StatInfo
{
    public StatType StatType;
    public float BaseValue;
}