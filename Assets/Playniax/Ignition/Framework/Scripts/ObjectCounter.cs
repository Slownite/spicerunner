using UnityEngine;

namespace Playniax.Ignition.Framework
{
    [AddComponentMenu("Playniax/Ignition/Framework/ObjectCounter")]
    public class ObjectCounter : MonoBehaviour
    {
        public static int counter;

        void OnEnable()
        {
            counter += 1;
        }

        void OnDisable()
        {
            counter -= 1;
        }
    }
}