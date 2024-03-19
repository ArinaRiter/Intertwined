using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Bson;
using UnityEngine;
using Realms;
using Debug = UnityEngine.Debug;
using MongoDB.Bson.IO;

public class RealmManager : MonoBehaviour
{
    private static Realm _realm;
    private static readonly string _path = Application.dataPath + "/Realm/";

    public static void ReadFromRealm()
    {
        _realm = Realm.GetInstance(_path + "StatMods.realm");
        var item = _realm.All<StatModItem>().ToArray()[0];
        Debug.Log($"{item.Name}");
        _realm.Dispose();
    }

    public static void AddToRealm()
    {
        _realm = Realm.GetInstance(_path + "StatMods.realm");
        var item = new StatModItem();
        _realm.Write(() => { _realm.Add(item); });
        _realm.Dispose();
        Debug.Log("entityName" + " added!");
    }

    public static void ImportCollection()
    {
        ExportJson();
        var item = ParseJson();

        var config = new RealmConfiguration(_path + "StatMods.realm");
        Realm.DeleteRealm(config);
        _realm = Realm.GetInstance(config);
        _realm.Write(() => { _realm.Add(item); });
        Debug.Log($"StatMods imported {item}!");
        _realm.Dispose();
    }

    private static void ExportJson()
    {
        var cmdInfo = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = "/c mongoexport --db Intertwined --collection StatMods --out " + _path + "StatMods.json",
            WindowStyle = ProcessWindowStyle.Hidden
        };
        var cmd = Process.Start(cmdInfo);
        cmd?.WaitForExit();
    }

    private static List<RealmObject> ParseJson()
    {
        using var r = new StreamReader(_path + "StatMods.json");
        var json = r.ReadToEnd();
        Debug.Log(json);
        var realmObjects = new List<RealmObject>();
        using var jsonReader = new JsonReader(json);
        while (jsonReader.ReadBsonType() == BsonType.Document)
        {
            jsonReader.ReadStartDocument();
            jsonReader.ReadObjectId();

            var statModItem = new StatModItem
            {
                Name = jsonReader.ReadString(),
                Rarity = jsonReader.ReadInt32(),
                IsBuff = jsonReader.ReadBoolean(),
                IsEntity = jsonReader.ReadBoolean(),
                IsItem = jsonReader.ReadBoolean()
            };

            jsonReader.ReadStartArray();
            while (jsonReader.ReadBsonType() == BsonType.Document)
            {
                jsonReader.ReadStartDocument();
                var stat = jsonReader.ReadString();
                var mod = Convert.ToString(jsonReader.ReadInt32());
                var value = jsonReader.ReadBsonType() == BsonType.Int32
                    ? Convert.ToString(jsonReader.ReadInt32(), CultureInfo.InvariantCulture)
                    : Convert.ToString(jsonReader.ReadDouble(), CultureInfo.InvariantCulture);
                statModItem.StatMods.Add($"{stat} {mod} {value}");
                jsonReader.ReadEndDocument();
            }

            jsonReader.ReadEndArray();

            realmObjects.Add(statModItem);
            jsonReader.ReadEndDocument();
        }

        jsonReader.Close();
        return realmObjects;
    }

    public static IEnumerable<StatusEffect> QueryRealm(Expression<Func<StatModItem, bool>> expression)
    {
        _realm = Realm.GetInstance(_path + "StatMods.realm");
        var query = _realm.All<StatModItem>().Where(expression);
        var results = ParseRealm(query);
        _realm.Dispose();
        return results;
    }

    private static IEnumerable<StatusEffect> ParseRealm(IEnumerable<StatModItem> query)
    {
        var statusEffects = new List<StatusEffect>();
        foreach (var item in query)
        {
            var statMods = new List<StatMod>();
            foreach (var statMod in item.StatMods)
            {
                var parsed = statMod.Split(' ');
                statMods.Add(new StatMod(Enum.Parse<StatType>(parsed[0]), (ModType)Convert.ToInt32(parsed[1]),
                    (float)Convert.ToDouble(parsed[2], CultureInfo.InvariantCulture)));
            }
            var statusEffect = new StatusEffect(item.Name, item.IsBuff, 0, statMods);
            statusEffects.Add(statusEffect);
        }
        return statusEffects;
    }
}

public class StatModItem : RealmObject
{
    [PrimaryKey] public string Name { get; set; }
    public int Rarity { get; set; }
    public bool IsBuff { get; set; }
    public bool IsEntity { get; set; }
    public bool IsItem { get; set; }
    public IList<string> StatMods { get; }

    public StatModItem()
    {
    }

    public StatModItem(string name, int rarity, bool isBuff, bool isEntity, bool isItem, IList<string> statMods)
    {
        Name = name;
        Rarity = rarity;
        IsBuff = isBuff;
        IsEntity = isEntity;
        IsItem = isItem;
        StatMods = statMods;
    }
}