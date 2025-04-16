using System;
using UnityEngine;

[Serializable]
public class EntityData : SaveData
{
    public float health;
    public float stamina;

    public EntityData(string guid, Vector3 position, Quaternion rotation, float health, float stamina) : base(guid, position, rotation)
    {
        this.health = health;
        this.stamina = stamina;
    }
}