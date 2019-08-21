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
        private Fade fade;

        public bool TeleportToLocation(Vector3 position)
        {
            if (!fade)
                fade = Camera.main.gameObject.GetComponentInChildren<Fade>(true);

            targetPosition = position;

            fade.OnFadeComplete += OnFadeComplete;
            fade.FadeToColor(FadeColor, FadeTime);

            return true;
        }

        void OnFadeComplete()
        {
            fade.OnFadeComplete -= OnFadeComplete;
            this.transform.localPosition = targetPosition;

            Invoke("Unfade", FadedTime);
        }

        void Unfade()
        {
            fade.OnUnfadeComplete += OnUnfadeComplete;
            fade.Unfade(UnfadeTime);
        }

        void OnUnfadeComplete()
        {
            fade.OnUnfadeComplete -= OnUnfadeComplete;
            OnTeleportComplete?.Invoke();

            Destroy(this);
        }
    }

}

