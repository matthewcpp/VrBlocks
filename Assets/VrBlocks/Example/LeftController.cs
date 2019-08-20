using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftController : MonoBehaviour
{
    private enum Action { None, Walking, Aiming }
    public GameObject avatar;
    public GameObject headset;
    public GameObject pointerPrefab;
    public float walkSpeed = 4.0f;

    private VrBlocks.Controller controller;
    private GameObject activePointer;

    private Action action = Action.None;

    void Start()
    {
        controller = GetComponent<VrBlocks.Controller>();

        controller.Primary2dAxisButton.OnPress += () => { action = Action.Walking; };
        controller.Primary2dAxisButton.OnRelease += () => { action = Action.Aiming; };

        controller.TriggerButton.OnPress += () => { AimTeleport(); };
        controller.TriggerButton.OnRelease += () => { Teleport(); };
    }

    void Update()
    {
        UpdateWalk();
    }

    private void UpdateWalk()
    {
        if (action != Action.Walking) return;

        Vector3 walkDir = new Vector3(controller.Primary2dAxis.x, 0.0f, controller.Primary2dAxis.y);
        walkDir = Vector3.Normalize(headset.transform.localRotation * walkDir);
        walkDir *= (walkSpeed * Time.deltaTime);
        walkDir.y = 0.0f;

        avatar.transform.localPosition += walkDir;
    }

    private void AimTeleport()
    {
        if (!activePointer)
            activePointer = GameObject.Instantiate(pointerPrefab, this.transform);

        activePointer.SetActive(true);
        action = Action.Aiming;
    }

    private void Teleport()
    {
        activePointer.SetActive(false);
        RaycastHit result;

        Ray ray = new Ray(controller.transform.position, controller.transform.forward);
        if (Physics.Raycast(ray, out result))
        {
            if (result.normal == Vector3.up)
            {
                VrBlocks.Teleport teleporter = avatar.AddComponent<VrBlocks.Teleport>();
                teleporter.OnTeleportComplete += () => { action = Action.None; };

                Vector3 headsetPos = headset.transform.localPosition;
                headsetPos.y = 0.0f;
                teleporter.TeleportToLocation(result.point - headsetPos);
            }
        }
    }
}
