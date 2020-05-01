using System.Collections.Generic;
using UnityEngine;

namespace Playniax.Ignition.ParticleSystem
{
    public class EmitterGroup : MonoBehaviour
    {
        public string id;

        public float scale = 1;

        public int size;

        public void Play(Vector3 position, float scale = 1, int sortingOrder = 0)
        {
            for (int i = 0; i < _emitters.Length; i++)
            {
                _emitters[i].Play(position, scale * this.scale, sortingOrder);
            }
        }

        public static EmitterGroup Get(string id)
        {
            for (int g = 0; g < _emitterGroups.Count; g++)
            {
                if (_emitterGroups[g].id == id) return _emitterGroups[g];
            }
            return null;
        }

        public static void Play(string id, Vector3 position, float scale = 1, int sortingOrder = 0)
        {
            for (int g = 0; g < _emitterGroups.Count; g++)
            {
                if (_emitterGroups[g].id == id)
                {
                    for (int i = 0; i < _emitterGroups[g]._emitters.Length; i++)
                    {
                        _emitterGroups[g]._emitters[i].Play(position, scale * _emitterGroups[g].scale, sortingOrder);
                    }
                }
            }
        }

        void Awake()
        {
            _emitterGroups.Add(this);

            _emitters = GetComponents<Emitter>();
        }

        void OnDestroy()
        {
            _emitterGroups.Remove(this);
        }

        static List<EmitterGroup> _emitterGroups = new List<EmitterGroup>();

        Emitter[] _emitters;
    }
}