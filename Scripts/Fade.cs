using UnityEngine;
using UnityEngine.UI;

namespace VrBlocks
{
    public class Fade : MonoBehaviour
    {
        public Image fadeImage;

        public enum FadeStatus { None, Fading, Faded, Unfading }
        public float TimeElapsed { get; private set; } = 0.0f;
        public float TotalTime { get; private set; } = 0.0f;

        public FadeStatus Status { get; private set; } = FadeStatus.None;

        public delegate void FadeEvent();

        public FadeEvent OnFadeStart;
        public FadeEvent OnFadeComplete;
        public FadeEvent OnUnfadeStart;
        public FadeEvent OnUnfadeComplete;

        protected Color currentColor;
        protected Color sourceColor;
        protected Color targetColor;

        public bool FadeToColor(Color color, float duration)
        {
            if (Status != FadeStatus.None)
                return false;

            TimeElapsed = 0.0f;
            TotalTime = duration;

            sourceColor = Color.clear;
            targetColor = color;
            currentColor = sourceColor;

            Status = FadeStatus.Fading;
            this.gameObject.SetActive(true);
            OnFadeStart?.Invoke();

            return true;
        }

        public bool Unfade(float duration)
        {
            if (Status != FadeStatus.Faded)
                return false;

            TimeElapsed = 0.0f;
            TotalTime = duration;

            sourceColor = currentColor;
            targetColor = Color.clear;
            currentColor = sourceColor;

            Status = FadeStatus.Unfading;
            OnUnfadeStart?.Invoke();

            return true;
        }

        void Update()
        {

            if (Status == FadeStatus.None)
            {
                this.gameObject.SetActive(false);
            }
            else
            {
                float nearClip = Camera.main.nearClipPlane;
                this.transform.localPosition = new Vector3(0.0f, 0.0f, nearClip + nearClip / 100.0f);

                if (Status == FadeStatus.Fading || Status == FadeStatus.Unfading)
                {
                    TimeElapsed = Mathf.Min(TimeElapsed + Time.deltaTime, TotalTime);
                    float complete = TimeElapsed / TotalTime;

                    if (TimeElapsed == TotalTime)
                    {
                        if (Status == FadeStatus.Fading)
                        {
                            currentColor = targetColor;
                            Status = FadeStatus.Faded;

                            OnFadeComplete?.Invoke();
                        }
                        else
                        {
                            currentColor = Color.clear;
                            Status = FadeStatus.None;
                            OnUnfadeComplete?.Invoke();
                        }
                    }
                    else
                    {
                        currentColor = Color.Lerp(sourceColor, targetColor, complete);
                        fadeImage.color = currentColor;
                    }
                }
            }
        }

    }


}
