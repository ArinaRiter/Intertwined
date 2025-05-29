using UnityEngine;

public class PlaySoundEventReaction : BaseEventReaction
{
    [SerializeField] private SoundType soundType;
    
    public override void React()
    {
        var position = FindFirstObjectByType<Camera>().transform.position;
        AudioManagerSO.Play(soundType, position);
    }
}
