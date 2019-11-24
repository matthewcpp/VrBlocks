using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class ExampleCanvas : MonoBehaviour
{
    public Text deviceName;
    public VrBlocks.Rig rig;
    public Camera mainCam;
    public Text debugText;

    float debugDecayTime = 0.0f;
    
    void Start()
    {
        deviceName.text = string.Format("Settings Name: {0}, Device Model: {1}", XRSettings.loadedDeviceName, XRDevice.model);
    }

    private void Update()
    {
        if (debugDecayTime > 0.0f)
        {
            debugDecayTime = Mathf.Max(debugDecayTime - Time.deltaTime, 0.0f);

            if (debugDecayTime == 0.0f)
                debugText.text = string.Empty;
        }
    }

    public void OnButtonClick()
    {
        debugText.text = "Button Click!!!";
        debugDecayTime = 1.0f;
    }
}
