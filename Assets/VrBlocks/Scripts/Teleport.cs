using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VrBlocks
{
    public class Teleport : MonoBehaviour
    {
        public float FadeTime { get; set; } = 0.3f;
        public float FadedTime { get; set; } = 0.15f;
        public float UnfadeTime { get; set; } = 0.3f;

        public Color FadeColor { get; set; } = Color.black;

        public delegate void Event();

        public Event OnTeleportComplete;

        private Vector3 targetPosition;


        public bool TeleportToLocation(Vector3 position)
        {
            targetPosition = position;

            var fade = Fade.Instance;
            fade.OnFadeComplete += OnFadeComplete;
            fade.FadeToColor(FadeColor, FadeTime);

            return true;
        }

        void OnFadeComplete()
        {
            Fade.Instance.OnFadeComplete -= OnFadeComplete;
            this.transform.localPosition = targetPosition;

            Invoke("Unfade", FadedTime);
        }

        void Unfade()
        {
            var fade = Fade.Instance;
            fade.OnUnfadeComplete += OnUnfadeComplete;
            fade.Unfade(UnfadeTime);
        }

        void OnUnfadeComplete()
        {
            Fade.Instance.OnUnfadeComplete -= OnUnfadeComplete;
            OnTeleportComplete?.Invoke();

            Destroy(this);
        }
    }

}

