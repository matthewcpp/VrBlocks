using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftController : MonoBehaviour
{
    public VrBlocks.Rig rig;
    
    public GameObject pointerPrefab;
    public float walkSpeed = 4.0f;

    private GameObject activePointer;
    private VrBlocks.Controller controller;

    private bool triggerPulled = false;

    void Start()
    {
        controller = GetComponent<VrBlocks.Controller>();

        controller.TriggerButton.OnPress += AimTeleport;
        controller.TriggerButton.OnRelease += Teleport;
    }

    void Update()
    {
        UpdateWalk();

        triggerPulled = triggerPulled || controller.TriggerValue > 0.5f;
    }

    private void UpdateWalk()
    {
        if (PlayerShouldWalk())
        {
            Vector3 walkDir = new Vector3(controller.Primary2dAxis.x, 0.0f, controller.Primary2dAxis.y);

            walkDir = rig.headset.transform.localRotation * walkDir;
            walkDir *= (walkSpeed * Time.deltaTime);
            walkDir.y = 0.0f;

            rig.transform.localPosition += walkDir;
        }
    }

    private bool PlayerShouldWalk()
    {
        if (controller.Primary2dAxisHasThumbstick)
            return controller.Primary2dAxisTouch.Down;
        else
            return controller.Primary2dAxisButton.Down;
    }

    private void AimTeleport()
    {
        if (!activePointer)
            activePointer = GameObject.Instantiate(pointerPrefab, this.transform);

        activePointer.SetActive(true);
        triggerPulled = false;
    }

    private void Teleport()
    {
        activePointer.SetActive(false);
        RaycastHit result;

        Ray ray = new Ray(controller.transform.position, controller.transform.forward);
        if (Physics.Raycast(ray, out result) && triggerPulled)
        {
            if (result.normal == Vector3.up)
            {
                rig.Teleport(result.point);
            }
        }
    }
}
