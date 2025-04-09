using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/AI Stats", fileName = "New AI Stats", order = 56)]
public class AIStatsSO : ScriptableObject
{
    [SerializeField] private float detectionRange;
    [SerializeField] private float detectionAngle;
    [SerializeField] private float detectionTime;

    public float DetectionRange => detectionRange;
    public float DetectionAngle => detectionAngle;
    public float DetectionTime => detectionTime;
}