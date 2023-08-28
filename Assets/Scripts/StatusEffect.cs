public class StatusEffect
{
    public float Duration;
    public readonly string Name;
    public readonly StatType Type;
    public readonly StatMod Mod;

    public StatusEffect(string name, StatType type, StatMod mod, float duration)
    {
        Name = name;
        Type = type;
        Mod = mod;
        Duration = duration;
    }
}