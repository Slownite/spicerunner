using System.Collections.Generic;
using UnityEngine;

namespace Playniax.Ignition.Prototyping
{
    public class Instantiator : MonoBehaviour
    {
        public List<GameObject> instantiate = new List<GameObject>();
        public List<GameObject> dontDestroyOnLoad = new List<GameObject>();

        void Awake()
        {
            // https://docs.unity3d.com/ScriptReference/Resources.FindObjectsOfTypeAll.html
            // print("Components " + Resources.FindObjectsOfTypeAll(typeof(MonoBehaviour)).Length);
            for (int i = 0; i < dontDestroyOnLoad.Count; i++)
            {
                if (GameObject.Find(dontDestroyOnLoad[i].name) == null && GameObject.Find(dontDestroyOnLoad[i].name + "(Clone)") == null)
                {
                    DontDestroyOnLoad(Instantiate(dontDestroyOnLoad[i]));
                }
            }

            for (int i = 0; i < instantiate.Count; i++)
            {
                Instantiate(instantiate[i]);
            }
        }
    }
}