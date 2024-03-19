using System.Collections.Generic;

public class StatusEffect
{
    public float Duration;
    public readonly string Name;
    public readonly bool IsBuff;
    public readonly List<StatMod> StatMods;

    public StatusEffect(string name, bool isBuff, float duration, List<StatMod> statMods)
    {
        Name = name;
        StatMods = statMods;
        Duration = duration;
        IsBuff = isBuff;
    }
}