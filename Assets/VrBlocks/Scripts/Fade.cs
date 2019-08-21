using UnityEngine;

namespace VrBlocks
{
    public class Fade : MonoBehaviour
    {
        public static Fade Instance { get { return GetInstance(); } }
        public enum FadeStatus { None, Fading, Faded, Unfading }
        public float TimeElapsed { get; private set; } = 0.0f;
        public float TotalTime { get; private set; } = 0.0f;

        public FadeStatus Status { get; private set; } = FadeStatus.None;

        public delegate void FadeEvent();

        public FadeEvent OnFadeStart;
        public FadeEvent OnFadeComplete;
        public FadeEvent OnUnfadeStart;
        public FadeEvent OnUnfadeComplete;

        private Color currentColor;
        private Color sourceColor;
        private Color targetColor;
        private Material material;

        private static Fade instance;

        private static Fade GetInstance()
        {
            if (!instance)
                instance = Camera.main.gameObject.AddComponent<Fade>();

            return instance;
        }

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

        private void Start()
        {
            Shader shader = Shader.Find("VrBlocks/HeadsetFade");

            if (shader != null)
                material = new Material(shader);
            else
                Debug.Log("Unable to locate Fade shader.  Ensure it is included with your build.");
        }

        private void Update()
        {
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
                }
            }
        }

        private void OnPostRender()
        {
            if (Status == FadeStatus.None || material == null) return;

            material.color = currentColor;
            material.SetPass(0);
            GL.PushMatrix();
            GL.LoadOrtho();
            GL.Color(currentColor);
            GL.Begin(GL.QUADS);
            GL.Vertex3(0f, 0f, 0.9999f);
            GL.Vertex3(0f, 1f, 0.9999f);
            GL.Vertex3(1f, 1f, 0.9999f);
            GL.Vertex3(1f, 0f, 0.9999f);
            GL.End();
            GL.PopMatrix();
        }
    }
}
