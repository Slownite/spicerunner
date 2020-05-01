using UnityEngine;

namespace Playniax.Ignition.Framework
{
    public class MathHelpers
    {
        public static float Mod(float devidend, float devider)
        {
            return devidend - devider * Mathf.Floor(devidend / devider);
        }
    }
}