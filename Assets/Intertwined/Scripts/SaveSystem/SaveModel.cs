using System;
using System.IO;
using System.Linq;
using UnityEngine;

public static class SaveModel
{
    private static EntityData[] _entitiesData;

    public static void SaveData()
    {
        var serializableSaveData = new SerializableSaveData(_entitiesData);
        var json = JsonUtility.ToJson(serializableSaveData, true);
        Debug.Log(json);
        File.WriteAllText(Application.persistentDataPath + "/save.json", json);
    }

    public static void LoadData()
    {
        var json = File.ReadAllText(Application.persistentDataPath + "/save.json");
        _entitiesData = JsonUtility.FromJson<SerializableSaveData>(json).entitiesData;
    }
    
    public static void SetData(EntityStats[] entities)
    {
        _entitiesData = entities.Select(entity => (EntityData)entity.SaveData()).ToArray();
    }

    public static SaveData GetData(string guid)
    {
        return _entitiesData.FirstOrDefault(x => x.guid == guid);
    }
}

[Serializable]
public struct SerializableSaveData
{
    public EntityData[] entitiesData;

    public SerializableSaveData(EntityData[] entitiesData)
    {
        this.entitiesData = entitiesData;
    }
}