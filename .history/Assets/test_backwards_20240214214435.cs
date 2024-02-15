using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_backwards : MonoBehaviour
{
    public float moveSpeed = 10.0f;

    private XRDeviceSimulator xrDeviceSimulator;

    private void Awake()
    {
        xrDeviceSimulator = new XRDeviceSimulator();
    }


    private void OnEnable()
    {
        xrDeviceSimulator.main.MoveBackwards.Enable();
    }

    private void OnDisable()
    {
        xrDeviceSimulator.main.MoveBackwards.Disable();
    }

    private void Update()
    {
        float moveBackwards = xrDeviceSimulator.main.MoveBackwards.ReadValue<float>();
        transform.Translate(Vector3.forward * moveBackwards * moveSpeed * Time.deltaTime);
    }
}
