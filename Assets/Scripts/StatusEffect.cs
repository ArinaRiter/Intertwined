public class StatusEffect
{
    public float Duration;
    public readonly string Name;
    public readonly bool IsBuff;
    public readonly StatMod Mod;

    public StatusEffect(string name, bool isBuff, float duration, StatMod mod)
    {
        Name = name;
        Mod = mod;
        Duration = duration;
        IsBuff = isBuff;
    }
}