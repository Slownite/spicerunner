using UnityEngine;
using Playniax.Ignition.Framework;

namespace Playniax.Ignition.Prototyping
{
    public class OrthographicResolution : MonoBehaviour
    {
        [System.Serializable]
        public class AdditionalSettings
        {
            public Camera camera;
        }

        public Vector2Int resolution;
        public AdditionalSettings additionalSettings;

        void Awake()
        {
            if (additionalSettings.camera == null) additionalSettings.camera = GetComponent<Camera>();
            if (additionalSettings.camera == null) additionalSettings.camera = Camera.main;
            if (additionalSettings.camera == null) return;

            CameraHelpers.SetOrthographic(resolution.x, resolution.y, additionalSettings.camera);
        }
    }
}