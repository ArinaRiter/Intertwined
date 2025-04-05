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
using Random = UnityEngine.Random;

public class RealmManager : MonoBehaviour
{
    private static Realm _realm;
    private static readonly string _path = Application.dataPath + "/Intertwined/Realm/";

    public static void ImportCollection()
    {
        foreach (var collection in new[]{"StatusEffects", "Entities"})
        {
            ExportJson(collection);
            var item = ParseJson(collection);
            var config = new RealmConfiguration(_path + collection + ".realm");
            Realm.DeleteRealm(config);
            _realm = Realm.GetInstance(config);
            _realm.Write(() => { _realm.Add(item); });
            Debug.Log($"{collection} collection imported!");
            _realm.Dispose();
        }
    }

    private static void ExportJson(string collection)
    {
        var cmdInfo = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = $"/c mongoexport --db Intertwined --collection {collection} --out {_path + collection}.json",
            WindowStyle = ProcessWindowStyle.Hidden
        };
        var cmd = Process.Start(cmdInfo);
        cmd?.WaitForExit();
    }

    private static List<RealmObject> ParseJson(string collection)
    {
        using var r = new StreamReader(_path + collection + ".json");
        var json = r.ReadToEnd();
        Debug.Log(json);
        if (collection == "StatusEffects") return ParseStatusEffects(json);
        else return ParseEntities(json);
    }

    private static List<RealmObject> ParseStatusEffects(string json)
    {
        var realmObjects = new List<RealmObject>();
        using var jsonReader = new JsonReader(json);
        while (jsonReader.ReadBsonType() == BsonType.Document)
        {
            jsonReader.ReadStartDocument();
            jsonReader.ReadObjectId();

            var statModItem = new StatusEffectItem
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

    private static List<RealmObject> ParseEntities(string json)
    {
        var realmObjects = new List<RealmObject>();
        using var jsonReader = new JsonReader(json);
        while (jsonReader.ReadBsonType() == BsonType.Document)
        {
            jsonReader.ReadStartDocument();
            jsonReader.ReadObjectId();

            var entityItem = new EntityItem
            {
                Name = jsonReader.ReadString(),
                DangerLevel = jsonReader.ReadInt32(),
                IsBoss = jsonReader.ReadBoolean(),
                IsElite = jsonReader.ReadBoolean()
            };
            jsonReader.ReadStartArray();
            while (jsonReader.ReadBsonType() == BsonType.Document)
            {
                jsonReader.ReadStartDocument();
                var stat = jsonReader.ReadString();
                var value = jsonReader.ReadBsonType() == BsonType.Int32
                    ? Convert.ToString(jsonReader.ReadInt32(), CultureInfo.InvariantCulture)
                    : Convert.ToString(jsonReader.ReadDouble(), CultureInfo.InvariantCulture);
                entityItem.Stats.Add($"{stat} {value}");
                jsonReader.ReadEndDocument();
            }

            jsonReader.ReadEndArray();

            realmObjects.Add(entityItem);
            jsonReader.ReadEndDocument();
        }

        jsonReader.Close();
        return realmObjects;
    }

    public static IEnumerable<StatusEffect> QueryRealm(Expression<Func<StatusEffectItem, bool>> expression)
    {
        _realm = Realm.GetInstance(_path + "StatusEffects.realm");
        var query = _realm.All<StatusEffectItem>().Where(expression);
        var results = ParseStatusEffectItem(query);
        _realm.Dispose();
        return results;
    }

    private static IEnumerable<StatusEffect> ParseStatusEffectItem(IEnumerable<StatusEffectItem> query)
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
    
    public static Dictionary<StatType, Stat> QueryRealmEntity(Expression<Func<EntityItem, bool>> expression)
    {
        _realm = Realm.GetInstance(_path + "Entities.realm");
        var query = _realm.All<EntityItem>().Where(expression);
        var results = ParseEntityItem(query);
        _realm.Dispose();
        return results;
    }
    
    private static Dictionary<StatType, Stat> ParseEntityItem(IEnumerable<EntityItem> query)
    {
        var characterStats = new List<Dictionary<StatType, Stat>>();
        foreach (var item in query)
        {
            var stats = new Dictionary<StatType, Stat>();
            foreach (var stat in item.Stats)
            {
                var parsed = stat.Split(' ');
                stats.Add(Enum.Parse<StatType>(parsed[0]), new Stat((float)Convert.ToDouble(parsed[1], CultureInfo.InvariantCulture)));
            }
            characterStats.Add(stats);
        }
        return characterStats[Random.Range(0, characterStats.Count - 1)];
    }
}

public class StatusEffectItem : RealmObject
{
    [PrimaryKey] public string Name { get; set; }
    public int Rarity { get; set; }
    public bool IsBuff { get; set; }
    public bool IsEntity { get; set; }
    public bool IsItem { get; set; }
    public IList<string> StatMods { get; }

    public StatusEffectItem()
    {
    }

    public StatusEffectItem(string name, int rarity, bool isBuff, bool isEntity, bool isItem, IList<string> statMods)
    {
        Name = name;
        Rarity = rarity;
        IsBuff = isBuff;
        IsEntity = isEntity;
        IsItem = isItem;
        StatMods = statMods;
    }
}

public class EntityItem : RealmObject
{
    [PrimaryKey] public string Name { get; set; }
    public int DangerLevel { get; set; }
    public bool IsBoss { get; set; }
    public bool IsElite { get; set; }
    public IList<string> Stats { get; }

    public EntityItem()
    {
    }

    public EntityItem(string name, int dangerLevel, bool isBoss, bool isElite, IList<string> stats)
    {
        Name = name;
        DangerLevel = dangerLevel;
        IsBoss = isBoss;
        IsElite = isElite;
        Stats = stats;
    }
}