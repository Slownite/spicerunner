using UnityEngine;
using Playniax.Ignition.Framework;

namespace Playniax.Ignition.SpriteSystem
{
    [AddComponentMenu("Playniax/Ignition/SpriteSystem/AnimationGroup")]
    public class AnimationGroup : MonoBehaviour
    {
        public Animation[] spriteGroup;

        [System.Serializable]
        public class Animation
        {
            public string name;
            public Sprite[] sprites;
            public float speed = 1;

            public float GetFrame()
            {
                //            return Mathf.RoundToInt(_frame) % sprites.Length;

                var frame = MathHelpers.Mod(_frame, sprites.Length - 1);

                return Mathf.RoundToInt(frame);
            }

            public Sprite GetSprite(int frame)
            {
                return sprites[Mathf.RoundToInt(frame) % sprites.Length];
            }

            public void SetFrame(int frame)
            {
                _frame = Mathf.RoundToInt(frame) % sprites.Length;
            }

            public int Once(SpriteRenderer spriteRenderer, float speed)
            {
                _frame += speed * this.speed * Time.deltaTime;

                if (_frame >= 0 && _frame <= sprites.Length - 1)
                {
                    spriteRenderer.sprite = sprites[Mathf.RoundToInt(GetFrame())];

                    return 1;
                }

                //if (_frame < 0) _frame = 0;
                //if (_frame > sprites.Length - 1) _frame = sprites.Length - 1;

                return 0;
            }

            public float Loop(SpriteRenderer spriteRenderer, float speed)
            {
                _frame += speed * this.speed * Time.deltaTime;

                var frame = GetFrame();

                spriteRenderer.sprite = sprites[Mathf.RoundToInt(frame)];

                return frame;
            }

            float _frame;
        }

        public float GetFrame(string name)
        {
            var animation = Find(name);
            if (animation != null) return animation.GetFrame();
            return -1;
        }

        public Sprite GetSprite(string name, int frame)
        {
            var animation = Find(name);
            if (animation != null) return animation.GetSprite(frame);
            return null;
        }

        public void Loop(int index, SpriteRenderer spriteRenderer, float speed = 1)
        {
            spriteGroup[index].Loop(spriteRenderer, speed);
        }

        public void Loop(string name, SpriteRenderer spriteRenderer, float speed = 1)
        {
            var animation = Find(name);
            if (animation != null) animation.Loop(spriteRenderer, speed);
        }

        public int Once(string name, SpriteRenderer spriteRenderer, float speed = 1)
        {
            var animation = Find(name);
            if (animation != null) return animation.Once(spriteRenderer, speed);
            return 0;
        }

        public void SetFrame(string name, int frame)
        {
            var animation = Find(name);
            if (animation != null) animation.SetFrame(frame);
        }

        public Animation Find(string name)
        {
            for (int i = 0; i < spriteGroup.Length; i++)
            {
                if (spriteGroup[i] != null && spriteGroup[i].name == name) return spriteGroup[i];
            }
            return null;
        }
    }
}