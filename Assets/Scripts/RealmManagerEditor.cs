using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RealmManager))]
public class RealmManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var realmManager = (RealmManager)target;
        if(GUILayout.Button("Add to Realm"))
        {
            RealmManager.AddToRealm();
        }
        
        if(GUILayout.Button("Read from Realm"))
        {
            RealmManager.ReadFromRealm();
        }
        
        if(GUILayout.Button("Import Collection"))
        {
            RealmManager.ImportCollection();
        }
    }
}
