using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CollisionUtils
{
    public static bool IsLayerInMask(int layer, LayerMask layerMask)
    {
        return ((1 << layer) & layerMask.value) != 0;
    }
}