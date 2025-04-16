using UnityEngine;

public class SaveController : MonoBehaviour
{
    private void Start()
    {
        LoadGame();
    }
    
    [ContextMenu("Save Game")]
    public void SaveGame()
    {
        var entities = FindObjectsByType<EntityStats>(FindObjectsSortMode.None);
        SaveModel.SetData(entities);
        SaveModel.SaveData();
    }

    public void LoadGame()
    {
        SaveModel.LoadData();
        
        var entities = FindObjectsByType<EntityStats>(FindObjectsSortMode.None);
        foreach (var entity in entities)
        {
            var guid = entity.GetComponent<GloballyUniqueIdentifier>().GUID;
            var data = SaveModel.GetData(guid);
            entity.LoadData(data);
        }
    }
}