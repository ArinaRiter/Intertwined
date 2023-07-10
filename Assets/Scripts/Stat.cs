using System;
using System.Collections.Generic;

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
    
    /*
    public float MaxValue { get; private set; }
    public float CurrentValue { get; private set; }
    private float _baseValue;
    private float _flatBaseModifier;
    private float _flatModifier;
    private float _percentPrimaryModifier;
    private float _percentSecondaryModifier;

    public Stat(float baseValue)
    {
        _baseValue = baseValue;
        MaxValue = baseValue;
        CurrentValue = baseValue;
    }

    public void ApplyModifier(float value, ModifierType type, bool maxValueChange)
    {
        if (maxValueChange)
        {
            switch (type)
            {
                case ModifierType.Base:
                    _flatBaseModifier += value;
                    break;
                case ModifierType.Flat:
                    _flatModifier += value;
                    break;
                case ModifierType.Primary:
                    _percentPrimaryModifier += value;
                    break;
                case ModifierType.Secondary:
                    _percentSecondaryModifier += value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            RecalculateValue();
        }
        else
        {
            switch (type)
            {
                case ModifierType.Flat:
                    CurrentValue += value;
                    break;
                case ModifierType.Primary:
                    CurrentValue *= 1 + value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            CurrentValue = Mathf.Clamp(CurrentValue, 0, MaxValue);
        }
    }

    private void RecalculateValue()
    {
        MaxValue = ((_baseValue + _flatBaseModifier) * (1 + _percentPrimaryModifier) + _flatModifier) * (1 + _percentSecondaryModifier);
        MaxValue = Mathf.Clamp(MaxValue, 0, Mathf.Infinity);
        CurrentValue = Mathf.Clamp(CurrentValue, 0, MaxValue);
    }*/
}

public struct StatMod
{
    public readonly ModType Type;
    public readonly float Value;
    
    public StatMod(ModType type, float value)
    {
        Type = type;
        Value = value;
    }
}