using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpecificCamera : MonoBehaviour
{
    Transform target;
    Vector3 offset;
    private int currentMode = 0;

    void Awake()
    {
        target = FindObjectOfType<MyPlayer>().transform;
    }

    void Start()
    {
        offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        transform.position = desiredPosition;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentMode = 0; // top-down
            SetCameraMode(currentMode);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentMode = 1; // isometric
            SetCameraMode(currentMode);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentMode = 2; // tps
            SetCameraMode(currentMode);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentMode = 3; // fps
            SetCameraMode(currentMode);
        }
    }

    void SetCameraMode(int mode)
    {
        switch (mode)
        {
            case 0:
                transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                offset = new Vector3(0f, 20f, 0f);
                break;
            case 1:
                transform.rotation = Quaternion.Euler(45f, -45f, 0f);
                offset = new Vector3(8, 15f, -6f);
                break;
            case 2:
                transform.rotation = Quaternion.Euler(30f, 0f, 0f);
                offset = new Vector3(0f, 5f, -5f);
                break;
            case 3:
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                offset = new Vector3(0f, 2.5f, 1.5f);
                break;
        }
    }
}
