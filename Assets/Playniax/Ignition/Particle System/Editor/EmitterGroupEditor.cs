using UnityEditor;
using UnityEngine;

namespace Playniax.Ignition.ParticleSystem
{
    [CustomEditor(typeof(EmitterGroup))]
    public class EmitterGroupEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EmitterGroup myScript = (EmitterGroup)target;
            if (GUILayout.Button("Test"))
            {
                myScript.Play(Vector3.zero, 1, 0); ;
            }
        }
    }
}