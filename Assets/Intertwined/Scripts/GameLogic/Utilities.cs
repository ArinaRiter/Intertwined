using UnityEngine;

public class Utilities
{
    public static bool IsLayerInMask(int layer, LayerMask layerMask)
    {
        return (1 << layer & layerMask.value) != 0;
    }
}