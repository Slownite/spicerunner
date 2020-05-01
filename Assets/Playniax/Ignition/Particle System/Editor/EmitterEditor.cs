using UnityEditor;
using UnityEngine;

namespace Playniax.Ignition.ParticleSystem
{
    [CustomEditor(typeof(Emitter))]
    public class EmitterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Emitter myScript = (Emitter)target;
            if (GUILayout.Button("Test"))
            {
                myScript.Play(Vector3.zero, 1, 0);
            }
        }
    }
}