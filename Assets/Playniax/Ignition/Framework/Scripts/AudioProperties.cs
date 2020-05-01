using UnityEngine;

namespace Playniax.Ignition.Framework
{
    [System.Serializable]
    public class AudioProperties
    {
        public static bool mute = false;

        public AudioClip audioClip;
        public float volumeScale = 1f;
        public float panStereo = 0;
        public float pitch = 1;
        public bool enabled = true;

        public static void Copy(AudioProperties source, AudioProperties target)
        {
            target.audioClip = source.audioClip;
            target.volumeScale = source.volumeScale;
            target.panStereo = source.panStereo;
            target.pitch = source.pitch;
            target.enabled = source.enabled;
        }

        public AudioProperties Clone()
        {
            var clone = new AudioProperties();

            clone.audioClip = audioClip;
            clone.volumeScale = volumeScale;
            clone.panStereo = panStereo;
            clone.pitch = pitch;
            clone.enabled = enabled;

            return clone;
        }

        public AudioSource Play()
        {
            if (mute == false && enabled == true) return AudioChannels.Play(audioClip, volumeScale, panStereo, pitch);
            return null;
        }
    }
}
