using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveController : MonoBehaviour
{
    private static bool _loadGame;

    private void Start()
    {
        if (_loadGame)
        {
            LoadGame();
            _loadGame = false;
        }
    }

    [ContextMenu("Save Game")]
    public void SaveGame()
    {
        var entities = FindObjectsByType<EntityStats>(FindObjectsSortMode.None);
        SaveModel.SetData(entities);
        SaveModel.SaveData();
    }

    [ContextMenu("Load Game")]
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

    public void PromptLoadGame()
    {
        _loadGame = true;
        Time.timeScale = 1;
        SceneManager.LoadScene("Main");
    }
}