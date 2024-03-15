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
            realmManager.AddToRealm();
        }
        
        if(GUILayout.Button("Read from Realm"))
        {
            realmManager.ReadFromRealm();
        }
        
        if(GUILayout.Button("Import Collection"))
        {
            realmManager.ImportCollection();
        }
    }
}
