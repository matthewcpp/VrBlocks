using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VrBlocks
{
    public class Pointer : MonoBehaviour
    {
        public bool Active { get; set; } = true;
        public ControllerButton Trigger { get; } = new ControllerButton();

        private void Start()
        {
            InputModule inputModule = FindObjectOfType<InputModule>();

            if (inputModule)
                inputModule.AddPointer(this);
        }

        private void OnDestroy()
        {
            InputModule inputModule = FindObjectOfType<InputModule>();

            if (inputModule)
                inputModule.RemovePointer(this);
        }
    }
}

