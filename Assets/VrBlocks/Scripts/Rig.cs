using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.XR;

namespace VrBlocks
{
    public class Rig : MonoBehaviour
    {
        public GameObject headset;
        public Transform playSpace;

        public float PlayerHeightAdjustment { get
            {
                return playSpace.transform.localPosition.y;
            }

            set
            {
                Vector3 position = playSpace.localPosition;
                position.y = value;
                playSpace.localPosition = position;
            }
        }

        void Start()
        {
            if (XRDevice.trackingOriginMode != TrackingOriginMode.Floor)
                InitializeNonFloorOrigin();
        }

        public Teleport Teleport(Vector3 position)
        {
            Teleport teleporter = this.gameObject.AddComponent<VrBlocks.Teleport>();

            Vector3 headsetPos = headset.transform.localPosition;
            headsetPos.y = 0.0f;
            teleporter.TeleportToLocation(position - headsetPos);

            return teleporter;
        }

        private void InitializeNonFloorOrigin()
        {
#if VR_BLOCKS_OCULUS
            Debug.Log("VrBlocks: Setting Tracking Origin to Floor via OVRManager");
            OVRManager manager = this.gameObject.AddComponent<OVRManager>();
            manager.trackingOriginType = OVRManager.TrackingOrigin.FloorLevel;
#else
            Debug.Log("VrBlocks: Setting default height for non-floor tracking origin.");
            PlayerHeightAdjustment = 1.754f;
#endif
        }
    }

}
