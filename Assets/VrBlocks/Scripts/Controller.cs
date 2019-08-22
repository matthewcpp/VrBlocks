using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SpatialTracking;
using UnityEngine.XR;

namespace VrBlocks
{
    public class Controller : MonoBehaviour
    {
        public Vector2 Primary2dAxis { get; private set; }

        public ControllerButton TriggerButton { get; } = new ControllerButton();
        public float TriggerValue { get; private set; }
        public ControllerButton Primary2dAxisButton { get; } = new ControllerButton();
        public ControllerButton Primary2dAxisTouch { get; } = new ControllerButton();
        public bool Primary2dAxisHasThumbstick { get; private set; } = false;

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

            try
            {
                if (inputDeviceList.Count > 0) { }
                    InputDevice = inputDeviceList[0];
            }
            catch (System.ArgumentOutOfRangeException e)
            {
                ;
            }


            if (XRSettings.loadedDeviceName.IndexOf("Oculus") != -1)
                Primary2dAxisHasThumbstick = true;

        }

        private void UpdateController(InputDeviceRole role)
        {
            UpdateButton(CommonUsages.triggerButton, TriggerButton);
            UpdateButton(CommonUsages.primary2DAxisClick, Primary2dAxisButton);
            UpdateButton(CommonUsages.primary2DAxisTouch, Primary2dAxisTouch);

            Vector2 axis = Vector2.zero;
            if (Primary2dAxisTouch.Down)
                InputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out axis);

            Primary2dAxis = axis;

            float val = 0.0f;
            if (InputDevice.TryGetFeatureValue(CommonUsages.trigger, out val))
            {
                TriggerValue = val;
            }
        }

        private void UpdateButton(InputFeatureUsage<bool> feature, ControllerButton button)
        {
            bool pressed = false;

            if (InputDevice.TryGetFeatureValue(feature, out pressed))
                button.SetDown(pressed);
        }
    }
}