using UnityEngine;

public class PeaceModeManager : MonoBehaviour
{
    public static bool IsPeaceMode = false;
    private AIController[] _entities;

    public void SetPeaceMode(bool peaceful)
    {
        IsPeaceMode = peaceful;
    }
}
