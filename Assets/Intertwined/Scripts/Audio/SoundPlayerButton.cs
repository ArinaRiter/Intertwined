using UnityEngine;

public class SoundPlayerButton : MonoBehaviour
{
    private Camera _camera;

    private void Awake()
    {
        _camera = FindFirstObjectByType<Camera>();
    }

    public void PlaySound()
    {
        AudioManagerSO.Play(SoundType.Button, _camera.ScreenToWorldPoint(transform.position));
    }
}
