using System;
using UnityEngine;

namespace Agava.WebUtility.Samples
{
    public class WebBackgroundVolumeSeter : MonoBehaviour
    {
        public static event Action<bool> VolumeSeted;

        private void OnEnable()
        {
            WebApplication.InBackgroundChangeEvent += OnInBackgroundChange;
        }

        private void OnDisable()
        {
            WebApplication.InBackgroundChangeEvent -= OnInBackgroundChange;
        }

        private void OnInBackgroundChange(bool inBackground)
        {
            // Use both pause and volume muting methods at the same time.
            // They're both broken in Web, but work perfect together. Trust me on this.
            AudioListener.pause = inBackground;
            if (inBackground == true)
                AudioListener.volume = 0f;

            VolumeSeted?.Invoke(inBackground);
        }
    }
}
