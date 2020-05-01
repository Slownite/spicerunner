using UnityEngine;

namespace Playniax.Ignition.Framework
{
    public static class CameraHelpers
    {
        public static bool IsVisible(Renderer renderer, Camera camera = null)
        {
            if (camera == null) camera = Camera.main;

            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
            return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
        }

        public static Bounds OrthographicBounds(Camera camera = null)
        {
            if (camera == null) camera = Camera.main;

            float aspect = (float)Screen.width / (float)Screen.height;
            float height = camera.orthographicSize * 2;
            return new Bounds(camera.transform.position, new Vector3(height * aspect, height, 0));
        }

        public static Vector3 GetMousePosition(Camera camera = null)
        {
            if (camera == null) camera = Camera.main;

            var ray = camera.ScreenPointToRay(Input.mousePosition);

            return ray.GetPoint(-camera.transform.position.z);
        }

        public static void SetOrthographicWidth(int virtualWidth, Camera camera = null)
        {
            if (camera == null) camera = Camera.main;

            float width = Screen.width;
            float height = Screen.height;

            var aspect = width / height;

            camera.orthographicSize = (float)virtualWidth / aspect / 200;
        }

        public static void SetOrthographicHeight(int virtualHeight, Camera camera = null)
        {
            if (camera == null) camera = Camera.main;

            float width = Screen.width;
            float height = Screen.height;

            camera.orthographicSize = (float)virtualHeight / 200;
        }

        public static void SetOrthographic(int virtualWidth, int virtualHeight, Camera camera = null)
        {
            if (camera == null) camera = Camera.main;

            float width = Screen.width;
            float height = Screen.height;

            var aspect = width / height;

            var x = (float)virtualWidth / aspect / 200;
            var y = camera.orthographicSize = (float)virtualHeight / 200;

            camera.orthographicSize = Mathf.Max(x, y);
        }
    }
}