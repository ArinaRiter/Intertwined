using UnityEngine;


public static class Bootstrapper 
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Bootstrap()
    {
        Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load("Systems")));
        
        LoadGameScene();
    }

    private static void LoadGameScene()
    {
        //UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }
}
