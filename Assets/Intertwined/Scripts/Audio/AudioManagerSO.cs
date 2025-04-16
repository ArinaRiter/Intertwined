using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Scriptable Object/Audio Manager", fileName = "Audio Manager")]
public class AudioManagerSO : ScriptableObject
{
    [SerializeField] private SoundGroup[] soundGroups;

    private static AudioSource _audioSource;
    private static SoundGroup[] _soundGroups;
    private static Dictionary<SoundType, SoundGroup> _soundDict;
    
#if UNITY_EDITOR
    private void Reset()
    {
        soundGroups = Enum.GetNames(typeof(SoundType)).Select(groupName => new SoundGroup(groupName, Resources.LoadAll<AudioClip>($"Audio/{groupName}"))).ToArray();
        OnValidate();
    }

    private void OnValidate()
    {
        _audioSource = Resources.Load<AudioSource>("Audio/AudioSource");
        _soundDict = ((SoundType[])Enum.GetValues(typeof(SoundType)))
            .Zip(soundGroups, (key, value) => new { Key = key, Value = value })
            .ToDictionary(item => item.Key, item => item.Value);
    }
    
    [ContextMenu("Sort Resources")]
    private void SortResources()
    {
        var soundTypes = Enum.GetNames(typeof(SoundType));
        
        foreach (var soundType in soundTypes)
        {
            if (!AssetDatabase.IsValidFolder($"Assets/Resources/Audio/{soundType}")) AssetDatabase.CreateFolder("Assets/Resources/Audio", soundType);
            var clips = AssetDatabase.FindAssets($"{soundType} t:AudioClip", new[] { "Assets/Resources/Audio" });
            
            foreach (var clip in clips)
            {
                var path = AssetDatabase.GUIDToAssetPath(clip);
                var assetName = AssetDatabase.LoadMainAssetAtPath(path).name;
                if (assetName.Split()[0] != soundType) continue;
                var index = AssetDatabase.FindAssets("", new[] { $"Assets/Resources/Audio/{soundType}" }).Length;
                AssetDatabase.MoveAsset(path, $"Assets/Resources/Audio/{soundType}/{soundType} {index}.wav");
            }
        }
    }
#endif
    
    public static void Play(SoundType type, Vector3 position, float volume = 1)
    {
        _audioSource ??= Resources.Load<AudioSource>("Audio/AudioSource");
        _soundGroups ??= Enum.GetNames(typeof(SoundType)).Select(groupName => new SoundGroup(groupName, Resources.LoadAll<AudioClip>($"Audio/{groupName}"))).ToArray();
        _soundDict ??= ((SoundType[])Enum.GetValues(typeof(SoundType)))
            .Zip(_soundGroups, (key, value) => new { Key = key, Value = value })
            .ToDictionary(item => item.Key, item => item.Value);
        
        var soundGroup = _soundDict[type];
        var source = Instantiate(_audioSource, position, Quaternion.identity);

        source.clip = soundGroup.Clips[Random.Range(0, soundGroup.Clips.Length)];
        source.volume = Mathf.Clamp(Random.Range(volume - soundGroup.VolumeRange, volume + soundGroup.VolumeRange), 0, 1);
        source.pitch = Mathf.Clamp(Random.Range(1 - soundGroup.PitchRange, 1 + soundGroup.PitchRange), 0, 3);
        source.Play();
        Destroy(source.gameObject, source.clip.length);
    }
}

[Serializable]
internal struct SoundGroup
{
    [HideInInspector] public string name;

    [SerializeField] private AudioClip[] clips;
    [SerializeField, Range(0, 1)] private float volumeRange;
    [SerializeField, Range(0, 1)] private float pitchRange;

    public AudioClip[] Clips => clips;
    public float VolumeRange => volumeRange;
    public float PitchRange => pitchRange;

    public SoundGroup(string groupName, AudioClip[] groupClips)
    {
        name = groupName;
        clips = groupClips;
        volumeRange = 0.1f;
        pitchRange = 0.1f;
    }
}

public enum SoundType
{
    Button,
    Walk,
    Run,
    Jump,
    Land,
    Sheath,
    Unsheath,
    Attack,
    AttackImpact,
    Fireball,
    FireballImpact,
    Hurt,
    Die,
}