using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Ability", fileName = "New Ability", order = 54)]
public class AbilitySO : ScriptableObject
{
    public string Name { get; private set; }
    public float Cost { get; private set; }
    public float CooldownTime { get; private set; }
    public float ActiveTime { get; private set; }

    public virtual void Activate(GameObject parent){}
}
