using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

using System.Threading.Tasks;


public static class Bootstrapper 
{
    private const string SystemsPrefabAddress = "Systems";
    private const string MainSceneAddress = "Menu";
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Bootstrap()
    {
        InitializeAsync();
    }
    
    private static async void InitializeAsync()
    {

        var systemsHandle = Addressables.LoadAssetAsync<GameObject>(SystemsPrefabAddress);
        await systemsHandle.Task;
        
        if (systemsHandle.Status == AsyncOperationStatus.Succeeded)
        {
            var systemsInstance = Object.Instantiate(systemsHandle.Result);
            Object.DontDestroyOnLoad(systemsInstance);
            Addressables.Release(systemsHandle); 
        }
        else
        {
            Debug.LogError("Failed to load Systems prefab");
            return;
        }
        
        var preloadHandle = Addressables.LoadSceneAsync(MainSceneAddress, 
            LoadSceneMode.Single, false); 
        await preloadHandle.Task;
        
        if (preloadHandle.Status == AsyncOperationStatus.Succeeded)
        {
            var sceneInstance = preloadHandle.Result;
            await sceneInstance.ActivateAsync();
        }
        else
        {
            Debug.LogError("Failed to load Main scene");
        }
    }
}
