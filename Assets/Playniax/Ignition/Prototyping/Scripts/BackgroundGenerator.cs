using System.Collections.Generic;
using UnityEngine;

namespace Playniax.Ignition.Prototyping
{
    public class BackgroundGenerator : LevelGeneratorBase
    {
        public Sprite[] sprites;
        public int count;
        public int orderInLayer = 0;

        public bool randomRotation;

        public override void Awake()
        {
            base.Awake();

            if (count == 0) count = sprites.Length;

            for (int i = 0; i < count; i++)
            {
                var sprite = _GetSprite();

                var background = new GameObject(sprite.name);

                var spriteRenderer = background.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = sprite;
                spriteRenderer.sortingOrder = orderInLayer;

                Position(background);

                background.transform.SetParent(transform);

                if (randomRotation) background.transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 359));

                _index += 1;
                if (_index >= sprites.Length) _index = 0;

            }
        }

        Sprite _GetSprite()
        {
            if (_sprites == null) _sprites = new List<Sprite>();

            if (_sprites.Count == 0)
            {
                for (int i = 0; i < sprites.Length; i++)
                {
                    _sprites.Add(sprites[i]);
                }
            }

            var index = Random.Range(0, _sprites.Count);

            var sprite = _sprites[index];

            _sprites.Remove(sprite);

            return sprite;
        }

        int _index;
        List<Sprite> _sprites;
    }
}
