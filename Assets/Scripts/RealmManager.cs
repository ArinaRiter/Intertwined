using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;
using Realms;
using Debug = UnityEngine.Debug;
using MongoDB.Bson.IO;

public class RealmManager : MonoBehaviour
{
    [SerializeField] private RealmType realmType;
    
    private Realm _realm;
    private readonly string _path = Application.dataPath + "/Realm/";

    public void ReadFromRealm()
    {
        _realm = Realm.GetInstance(_path + realmType + ".realm");
        var item = _realm.All<StatModItem>().ToArray()[0];
        Debug.Log($"{item.Name}");
        _realm.Dispose();
    }
    
    public void AddToRealm()
    {
        _realm = Realm.GetInstance(_path + realmType + ".realm");
        var item = new StatModItem();
        _realm.Write(() => { _realm.Add(item); });
        _realm.Dispose();
        Debug.Log("entityName" + " added!");
    }

    public void ImportCollection()
    {
        ExportJson();
        var item = (StatModItem)ParseJson();

        var config = new RealmConfiguration(_path + realmType + ".realm");
        Realm.DeleteRealm(config);
        _realm = Realm.GetInstance(config);
        _realm.Write(() => { _realm.Add(item); });
        Debug.Log($"{realmType} imported {item.Name}!");
        _realm.Dispose();
    }

    private void ExportJson()
    {
        var cmdInfo = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = "/c mongoexport --db Intertwined --collection " + realmType + " --out " + _path + realmType + ".json",
            WindowStyle = ProcessWindowStyle.Hidden
        };
        var cmd = Process.Start(cmdInfo);
        cmd?.WaitForExit();
    }
    
    private RealmObject ParseJson()
    {
        using var r = new StreamReader(_path + realmType + ".json");
        var json = r.ReadToEnd();
        Debug.Log(json);
        var statusEffectItem = new StatModItem();
        using var jsonReader = new JsonReader(json);
        jsonReader.ReadStartDocument();
        jsonReader.ReadObjectId();
        statusEffectItem.Name = jsonReader.ReadString();
        statusEffectItem.IsBuff = jsonReader.ReadBoolean();
        statusEffectItem.Duration = jsonReader.ReadInt32();
        jsonReader.ReadStartArray();
        jsonReader.ReadBsonType();
        while (jsonReader.State == BsonReaderState.Value)
        {
            jsonReader.ReadStartDocument();
            var stat = jsonReader.ReadString();
            var mod = Convert.ToString(jsonReader.ReadInt32());
            var value = Convert.ToString(jsonReader.ReadInt32(), CultureInfo.InvariantCulture);
            statusEffectItem.StatMods.Add($"{stat} {mod} {value}");
            jsonReader.ReadEndDocument();
            jsonReader.ReadBsonType();
        }
        jsonReader.Close();
        return statusEffectItem;
    }
}

public class StatModItem : RealmObject
{
    [PrimaryKey] public string Name { get; set; }
    public bool IsBuff { get; set; }
    public float Duration { get; set; }
    public IList<string> StatMods { get; }

    public StatModItem(){}
    
    public StatModItem(string name, bool isBuff, float duration, IList<string> statMods)
    {
        Name = name;
        IsBuff = isBuff;
        Duration = duration;
        StatMods = statMods;
    }
}

internal enum RealmType
{
    StatusEffects
}