using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class ExampleCanvas : MonoBehaviour
{
    public Text deviceName;
    public VrBlocks.Rig rig;
    public Camera mainCam;
    public Text debugText;
    
    void Start()
    {
        deviceName.text = string.Format("Settings Name: {0}, Device Model: {1}", XRSettings.loadedDeviceName, XRDevice.model);
    }

    private void Update()
    {
        debugText.text = string.Format("Camera Pos: {0}, Headset Pos: {1}", mainCam.transform.position.ToString(), rig.headset.transform.position.ToString());
    }
}
