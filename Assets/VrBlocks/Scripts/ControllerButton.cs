using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VrBlocks
{
    class ControllerButton
    {
        public delegate void Event();

        public Event OnPress;
        public Event OnRelease;

        public bool Down { get; private set; }
        public bool Pressed { get { return !previouslyDown && Down; } }
        public bool Released { get { return previouslyDown && !Down; } }

        private bool previouslyDown = false;

        public void SetDown(bool down)
        {
            previouslyDown = Down;
            Down = down;

            if (Pressed)
                OnPress?.Invoke();

            if (Released)
                OnRelease?.Invoke();
        }
    }
}