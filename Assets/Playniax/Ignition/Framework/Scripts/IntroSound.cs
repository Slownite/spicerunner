using UnityEngine;

namespace Playniax.Ignition.Framework
{
    [AddComponentMenu("Playniax/Ignition/Framework/IntroSound")]
    public class IntroSound : MonoBehaviour
    {
        public AudioProperties audioProperties;

        void OnEnable()
        {
            _audioProperties = audioProperties;
        }

        void Update()
        {
            if(_audioProperties != null && AudioChannels.channels != null)
            {
                _audioProperties.Play();

                _audioProperties = null;
            }
        }

        AudioProperties _audioProperties;
    }
}