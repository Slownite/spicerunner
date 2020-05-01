using UnityEngine;
using UnityEngine.UI;

namespace Playniax.Ignition.Framework
{
    [System.Serializable]
    public class AssetBank
    {
        public Object[] assets;

        public Object Get(string name)
        {
            for (int i = 0; i < assets.Length; i++)
            {
                if (assets[i].name == name) return assets[i];
            }
            return null;
        }

        public Font GetFont(string name)
        {
            return Get(name) as Font;
        }

        public Image GetImage(string name)
        {
            return Get(name) as Image;
        }

        public Texture2D GetTexture2D(string name)
        {
            return Get(name) as Texture2D;
        }

        public Sprite GetSprite(string name)
        {
            return Get(name) as Sprite;
        }

        public TextAsset GetTextAsset(string name)
        {
            return Get(name) as TextAsset;
        }
    }
}