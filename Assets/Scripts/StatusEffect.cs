using UnityEngine;

public class StatusEffect : MonoBehaviour
{
    private float _duration;

    public readonly string Name;
    public readonly StatType Type;
    public readonly StatMod Mod;
    public float Duration => _duration;

    public StatusEffect(string name, StatType type, StatMod mod, float duration)
    {
        Name = name;
        Type = type;
        Mod = mod;
        _duration = duration;
    }

    private void Update()
    {
        if (_duration > 0)
        {
            _duration -= Time.deltaTime;
        }
        else
        {
            _duration = 0;
        }
    }
}