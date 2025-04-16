using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] private float volume = 1;

    public void PlaySound(SoundType soundType)
    {
        AudioManagerSO.Play(soundType, transform.position, volume);
    }
}