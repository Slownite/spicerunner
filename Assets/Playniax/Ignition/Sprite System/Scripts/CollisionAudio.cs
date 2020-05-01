using System.Collections.Generic;
using UnityEngine;
using Playniax.Ignition.Framework;

namespace Playniax.Ignition.SpriteSystem
{
    [AddComponentMenu("Playniax/Ignition/SpriteSystem/CollisionAudio")]
    public class CollisionAudio : MonoBehaviour
    {
        public string material1 = "Metal";
        public string material2 = "Metal";

        public AudioProperties audioProperties;

        public static void Play(string material1, string material2)
        {
            if (_collisionSound == null) return;

            for (int i = 0; i < _collisionSound.Count; i++)
            {
                if (_collisionSound[i].material1 != "" && _collisionSound[i].material2 != "" && _collisionSound[i].material1 + _collisionSound[i].material2 == material1 + material2 || _collisionSound[i].material1 + _collisionSound[i].material2 == material2 + material1) _collisionSound[i].audioProperties.Play();
            }
        }

        void OnDisable()
        {
            _collisionSound.Remove(this);
        }
        void OnEnable()
        {
            _collisionSound.Add(this);
        }

        static List<CollisionAudio> _collisionSound = new List<CollisionAudio>();
    }
}