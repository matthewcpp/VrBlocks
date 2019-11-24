using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.XR;

namespace VrBlocks
{
    public class Rig : MonoBehaviour
    {
        public GameObject Headset { get; private set; }

        [SerializeField]
        private GameObject fadePrefab;

        [SerializeField]
        private GameObject rightControllerAlias;

        [SerializeField]
        private GameObject leftControllerAlias;

        [SerializeField]
        private GameObject headsetAlias;

        private Transform playSpace;

        private void Awake()
        {
            playSpace = this.transform.Find("Play Space");
            Headset = playSpace.transform.Find("Headset").gameObject;

            if (leftControllerAlias != null)
            {
                var leftController = playSpace.Find("LeftController");
                leftControllerAlias.transform.parent = leftController;
            }
            
            if (rightControllerAlias != null)
            {
                var rightController = playSpace.Find("RightController");
                rightControllerAlias.transform.parent = rightController;
            }

            if (headsetAlias != null)
            {
                var headset = playSpace.Find("Headset");
                rightControllerAlias.transform.parent = headset;
            }

        }

        void Start()
        {
            GameObject.Instantiate(fadePrefab, Camera.main.transform);

            if (XRDevice.trackingOriginMode != TrackingOriginMode.Floor)
                InitializeNonFloorOrigin();
        }

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

        public Teleport Teleport(Vector3 position)
        {
            Teleport teleporter = this.gameObject.AddComponent<VrBlocks.Teleport>();

            Vector3 headsetPos = Headset.transform.localPosition;
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
