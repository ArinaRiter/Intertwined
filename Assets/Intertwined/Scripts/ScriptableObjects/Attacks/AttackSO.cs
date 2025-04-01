using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Attack", fileName = "New Attack", order = 53)]
public class AttackSO : ScriptableObject
{
    [SerializeField] private List<Attack> attacks;

    public ReadOnlyCollection<Attack> Attacks => new(attacks);
    
}

[Serializable]
public struct Attack
{
    public AttackType Type;
    public float Multiplier;
}