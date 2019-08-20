using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SpatialTracking;
using UnityEngine.XR;

namespace VrBlocks
{
    class Controller : MonoBehaviour
    {
        public Vector2 Primary2dAxis { get; private set; }

        public ControllerButton TriggerButton { get; } = new ControllerButton();
        public ControllerButton Primary2dAxisButton { get; } = new ControllerButton();

        public InputDevice InputDevice { get; private set; }
        private TrackedPoseDriver driver;


        void Start()
        {
            driver = GetComponent<TrackedPoseDriver>();
        }

        // Update is called once per frame
        void Update()
        {
            if (InputDevice.isValid)
                UpdateController(InputDevice.role);
            else
                InitController();
        }

        private void InitController()
        {
            List<InputDevice> inputDeviceList = new List<InputDevice>();

            InputDeviceRole role = driver.poseSource == TrackedPoseDriver.TrackedPose.LeftPose ? InputDeviceRole.LeftHanded : InputDeviceRole.RightHanded;
            InputDevices.GetDevicesWithRole(role, inputDeviceList);

            if (inputDeviceList.Count > 0)
                InputDevice = inputDeviceList[0];

        }

        private void UpdateController(InputDeviceRole role)
        {
            UpdateButton(CommonUsages.triggerButton, TriggerButton);
            UpdateButton(CommonUsages.primary2DAxisClick, Primary2dAxisButton);

            Vector2 axis;
            if (InputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out axis))
                Primary2dAxis = axis;
        }

        private void UpdateButton(InputFeatureUsage<bool> feature, ControllerButton button)
        {
            bool pressed = false;

            if (InputDevice.TryGetFeatureValue(feature, out pressed))
                button.SetDown(pressed);
        }
    }
}