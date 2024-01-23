
using UnityEngine;

namespace Leon.Extensions
{
    public static class LayerMaskExtensions
    {
        public static bool IsInLayerMask(this LayerMask mask, int layer) =>
            (mask.value & (1 << layer)) > 0;

        public static bool IsInLayerMask(this LayerMask mask, GameObject gobj) =>
            (mask.value & (1 << gobj.layer)) > 0;
    }
}