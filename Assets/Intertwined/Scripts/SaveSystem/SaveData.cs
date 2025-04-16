using System;
using UnityEngine;

[Serializable]
public class SaveData
{
    public string guid;
    public Vector3 position;
    public Quaternion rotation;

    public SaveData(string guid, Vector3 position, Quaternion rotation)
    {
        this.guid = guid;
        this.position = position;
        this.rotation = rotation;
    }
}