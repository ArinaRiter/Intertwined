using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatusEffect
{
    [SerializeField] private float duration;
    [SerializeField] private string name;
    [SerializeField] private bool isBuff;
    [SerializeField] private bool isPermanent;
    [SerializeField] private List<StatMod> statMods;
    
    public float Duration { get => duration; set => duration = value; }
    public string Name => name;
    public bool IsBuff => isBuff;
    public bool IsPermanent => isPermanent;
    public List<StatMod> StatMods => statMods;

    public StatusEffect(string name, bool isBuff, float duration, List<StatMod> statMods)
    {
        this.name = name;
        this.statMods = statMods;
        this.duration = duration;
        isPermanent = Duration == 0;
        this.isBuff = isBuff;
    }
}