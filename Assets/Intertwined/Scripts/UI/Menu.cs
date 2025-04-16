using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using System.Threading.Tasks;

public class Menu : MonoBehaviour
{
    [SerializeField] private AssetReference gameSceneRef;
    [SerializeField] private Button _button;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider loadingProgressBar;
    
    private AsyncOperationHandle<SceneInstance> _sceneLoadHandle;
    private bool _isLoading;
    private bool _isDestroyed;
    private const float MIN_LOADING_TIME = 3f;

    private void Start()
    {
        _button?.onClick.AddListener(OnPlayButtonClicked);

        loadingScreen?.SetActive(false);
        loadingProgressBar?.gameObject.SetActive(false);
    }

    private async void OnPlayButtonClicked()
    {
        if (_isLoading || _isDestroyed) return;
        
        _isLoading = true;
        if (_button is not null)
            _button.interactable = false;

        loadingScreen?.SetActive(true);
        if (loadingProgressBar != null)
            loadingProgressBar.gameObject.SetActive(true);

        var startTime = Time.time;

        try
        {
            _sceneLoadHandle = Addressables.LoadSceneAsync(
                gameSceneRef.RuntimeKey,
                activateOnLoad: false
            );

            while (_sceneLoadHandle.Status == AsyncOperationStatus.None && !_isDestroyed)
            {
                if (loadingProgressBar is not null)
                    loadingProgressBar.value = _sceneLoadHandle.PercentComplete * 0.5f; 
                await Task.Yield();
            }

            if (_isDestroyed) return;

            if (_sceneLoadHandle.Status == AsyncOperationStatus.Succeeded)
            {
                var activateOperation = _sceneLoadHandle.Result.ActivateAsync();
                while (!activateOperation.isDone && !_isDestroyed)
                {
                    var elapsedTime = Time.time - startTime;
                    var progress = Mathf.Clamp01(elapsedTime / MIN_LOADING_TIME);
                    
                    if (loadingProgressBar is not null)
                        loadingProgressBar.value = 0.5f + (activateOperation.progress * 0.5f); 
                        
                    if (progress >= 1f && activateOperation.progress >= 0.9f)
                        break;
                        
                    await Task.Yield();
                }
            }
            else
            {
                Debug.LogError($"Ошибка загрузки сцены: {_sceneLoadHandle.OperationException}");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Произошла ошибка при загрузке сцены: {e.Message}");
        }
        finally
        {
            if (!_isDestroyed)
            {
                _isLoading = false;
                if (_button is not null)
                    _button.interactable = true;
                loadingScreen?.SetActive(false);
                if (loadingProgressBar != null)
                    loadingProgressBar.gameObject.SetActive(false);
            }
        }
    }

    private void OnDestroy()
    {
        _isDestroyed = true;
        _button?.onClick.RemoveListener(OnPlayButtonClicked);

        if (_sceneLoadHandle.IsValid())
            Addressables.Release(_sceneLoadHandle);
    }
}