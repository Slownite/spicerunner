using UnityEngine;

namespace Playniax.Ignition.Framework
{
    [AddComponentMenu("Playniax/Ignition/Framework/Timing")]
    public class Timing : MonoBehaviour
    {
        public int targetFrameRate = -1; // https://docs.unity3d.com/ScriptReference/Application-targetFrameRate.html
        public float timeScale = 1;
        public int vSyncCount;

#if UNITY_EDITOR
        public KeyCode pauseKey = KeyCode.None;
#endif

        public static bool Paused
        {
            get
            {
                if (_timeScale == -1) _timeScale = Time.timeScale;

                if (Time.timeScale == 0) return true;
                return false;
            }
            set
            {
                if (_timeScale == -1) _timeScale = Time.timeScale;

                if (value == true)
                {
                    if (Time.timeScale != 0)
                    {
                        _timeScale = Time.timeScale;

                        Time.timeScale = 0;
                    }
                }
                else
                {
                    Time.timeScale = _timeScale;
                }
            }
        }

        void LateUpdate()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(pauseKey)) Timing.Paused = !Timing.Paused;
#endif
        }

        void OnEnable()
        {
            OnValidate();
        }

        void OnValidate()
        {
            Application.targetFrameRate = targetFrameRate;
            QualitySettings.vSyncCount = vSyncCount;
            Time.timeScale = timeScale;
        }

        static float _timeScale = -1;
    }

}