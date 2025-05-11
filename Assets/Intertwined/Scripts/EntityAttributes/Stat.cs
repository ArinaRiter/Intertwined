using System;
using System.Collections.Generic;
using UnityEngine;

public class Stat
{
    public delegate void StatHandler(float value);
    public event StatHandler ChangedValue;
    public float Value { get; private set; }

    private readonly float _baseValue;
    private readonly List<StatMod> _statMods = new();

    public Stat(float baseValue)
    {
        _baseValue = baseValue;
        Value = _baseValue;
    }

    public void AddModifier(StatMod statMod)
    {
        _statMods.Add(statMod);
        CalculateValue();
    }

    public void RemoveModifier(StatMod statMod)
    {
        _statMods.Remove(statMod);
        CalculateValue();
    }

    public void ClearModifiers()
    {
        _statMods.Clear();
        CalculateValue();
    }

    private void CalculateValue()
    {
        float baseMod = 0;
        float flatMod = 0;
        float percentAddMod = 1;
        float percentMultMod = 1;
        foreach (var statMod in _statMods)
        {
            switch (statMod.Type)
            {
                case ModType.Base:
                    baseMod += statMod.Value;
                    break;
                case ModType.Flat:
                    flatMod += statMod.Value;
                    break;
                case ModType.PercentAdd:
                    percentAddMod += statMod.Value;
                    break;
                case ModType.PercentMult:
                    percentMultMod *= statMod.Value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        Value = ((_baseValue + baseMod) * percentAddMod + flatMod) * percentMultMod; 
        ChangedValue?.Invoke(Value);
    }
}

[Serializable]
public struct StatMod
{
    [SerializeField] private StatType stat;
    [SerializeField] private ModType type;
    [SerializeField] private float value;
    
    public StatType Stat => stat;
    public ModType Type => type;
    public float Value => value;
    
    public StatMod(StatType stat, ModType type, float value)
    {
        this.stat = stat;
        this.type = type;
        this.value = value;
    }
}