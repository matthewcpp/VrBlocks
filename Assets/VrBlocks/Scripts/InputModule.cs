using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace VrBlocks
{
    class InputModule : PointerInputModule
    {
        // each registered pointer will have its own instance of Pointer event data.
        private Dictionary<Pointer, PointerEventData> pointerCache = new Dictionary<Pointer, PointerEventData>();

        public void AddPointer(Pointer pointer)
        {
            if (pointerCache.ContainsKey(pointer)) return;

            var pointerEventData = new PointerEventData(eventSystem);

            pointerCache[pointer] = pointerEventData;
        }

        public void RemovePointer(Pointer pointer)
        {
            pointerCache.Remove(pointer);
        }

        protected override void Awake()
        {
            base.Awake();
        }

        public override void Process()
        {
            foreach (var item in pointerCache)
            {
                Pointer pointer = item.Key;
                PointerEventData pointerEventData = item.Value;

                List<RaycastResult> results = GetRaycastResults(pointer, pointerEventData);

                Process(pointer, pointerEventData, results);
            }
        }

        void Process(Pointer pointer, PointerEventData pointerEventData, List<RaycastResult> raycastResults)
        {
            if (pointer.Trigger.Pressed)
            {
                foreach (var raycastResult in raycastResults)
                {
                    GameObject target = ExecuteEvents.ExecuteHierarchy(raycastResult.gameObject, pointerEventData, ExecuteEvents.pointerDownHandler);
                    if (target != null)
                    {
                        pointerEventData.pressPosition = pointerEventData.position;
                        pointerEventData.pointerPressRaycast = raycastResult;
                        pointerEventData.pointerPress = target;
                        pointerEventData.eligibleForClick = true;

                        return;
                    }
                }
            }
            else if (pointer.Trigger.Released)
            {
                if (pointerEventData.pointerPress != null)
                {
                    ExecuteEvents.ExecuteHierarchy(pointerEventData.pointerPress, pointerEventData, ExecuteEvents.pointerClickHandler);
                    ExecuteEvents.ExecuteHierarchy(pointerEventData.pointerPress, pointerEventData, ExecuteEvents.pointerUpHandler);

                    pointerEventData.pointerPress = null;
                    pointerEventData.eligibleForClick = false;
                }
            }
        }

        protected virtual List<RaycastResult> GetRaycastResults(Pointer pointer, PointerEventData pointerEventData)
        {
            List<RaycastResult> raycasts = new List<RaycastResult>();

            if (pointer.Active)
            {
                RaycastResult raycastResult = new RaycastResult();
                raycastResult.worldPosition = pointer.transform.position;
                raycastResult.worldNormal = pointer.transform.forward;
                pointerEventData.pointerCurrentRaycast = raycastResult;

                eventSystem.RaycastAll(pointerEventData, raycasts);
            }

            return raycasts;
        }
    }
}