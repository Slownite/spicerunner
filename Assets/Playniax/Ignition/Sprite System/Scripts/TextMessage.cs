using UnityEngine;
using Playniax.Ignition.Framework;

namespace Playniax.Ignition.SpriteSystem
{
    [AddComponentMenu("Playniax/Ignition/SpriteSystem/TextMessage")]
    public class TextMessage : MonoBehaviour
    {
        public float ttl = 3;
        public Color startColor = new Color(0, 1, 0, 1);
        public Color targetColor = new Color(0, 1, 0, 0);
        public Vector3 velocity;

        public TextMesh textMesh;

        void Awake()
        {
            if (textMesh == null) textMesh = GetComponent<TextMesh>();
        }

        void OnEnable()
        {
            _startColor = startColor;

            if (textMesh) textMesh.color = startColor;

            _ttl = ttl;
        }

        void Update()
        {
            if (textMesh == null) return;

            if (ttl > 0 && _ttl > 0)
            {
                transform.position += velocity * Time.deltaTime;
                textMesh.color = targetColor - (targetColor - _startColor) * (_ttl * 1.0f / ttl);
                _ttl -= 1 * Time.deltaTime;
            }

            else
            {
                if (name.Contains(GameObjectPooler.marker))
                {
                    gameObject.SetActive(false);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }

        Color _startColor;
        float _ttl;
    }
}