using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Playniax.Ignition.SpriteSystem
{
    [AddComponentMenu("Playniax/Ignition/SpriteSystem/SpriteBoxCollider")]
    public class SpriteBoxCollider : SpriteColliderBase
    {
        public SpriteRenderer spriteRenderer;

        [Tooltip("Whether to create a collider automatically when collider is missing.")]
        public bool generateBoxCollider = true;

        public virtual void Awake()
        {
            if (spriteCollider == null) spriteCollider = GetComponent<Collider2D>();

            if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();

            if (generateBoxCollider && spriteCollider == null)
            {
                spriteCollider = gameObject.AddComponent<BoxCollider2D>();

                if (spriteRenderer) (spriteCollider as BoxCollider2D).size = spriteRenderer.bounds.size;

                print("BoxCollider for " + gameObject.name + "is missing");
            }
        }
    }
}