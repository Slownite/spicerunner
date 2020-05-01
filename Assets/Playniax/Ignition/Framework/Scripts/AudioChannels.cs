using UnityEngine;

namespace Playniax.Ignition.Framework
{
    [AddComponentMenu("Playniax/Ignition/Framework/AudioChannels")]
    public class AudioChannels : MonoBehaviour
    {
        public static bool mute = false;

        public static AudioSource[] channels;

        public static AudioSource GetAvailableChannel()
        {
            if (channels == null) return null;

            if (channels.Length == 0) return null;

            for (int i = 0; i < channels.Length; i++)
            {
                if (channels[i] && channels[i].isPlaying == false) return channels[i];
            }

            return null;
        }

        public static AudioSource Play(AudioClip audioClip, float volumeScale = 1f, float panStereo = 0, float pitch = 1)
        {
            if (mute) return null;

            if (audioClip == null) return null;

            var channel = GetAvailableChannel();

            if (channel)
            {
                channel.panStereo = panStereo;
                channel.pitch = pitch;
                channel.PlayOneShot(audioClip, volumeScale);
            }

            return channel;
        }

        void OnDestroy()
        {
            channels = null;
        }

        void Awake()
        {
            channels = GetComponents<AudioSource>();
        }
    }
}