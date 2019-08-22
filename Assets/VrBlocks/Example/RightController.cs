using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightController : MonoBehaviour
{
    public GameObject pointerPrefab;

    private VrBlocks.Controller controller;
    private VrBlocks.Pointer uiPointer;

    private GameObject pointer;

    void Start()
    {
        controller = GetComponent<VrBlocks.Controller>();
        uiPointer = GetComponent<VrBlocks.Pointer>();
        pointer = GameObject.Instantiate(pointerPrefab, this.transform);

        pointer.GetComponent<LineRenderer>().material.color = Color.gray;
    }

    // Update is called once per frame
    void Update()
    {
        uiPointer.Trigger.SetDown(controller.TriggerButton.Down);
    }
}
