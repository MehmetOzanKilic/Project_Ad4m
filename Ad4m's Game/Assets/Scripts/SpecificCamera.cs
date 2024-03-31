using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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
            //side view
            SetCameraMode(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //top-down
            SetCameraMode(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            //isometric
            SetCameraMode(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            //tps
            SetCameraMode(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            //fps
            SetCameraMode(5);
        }
    }

    void SetCameraMode(int mode)
    {
        switch (mode)
        {
            case 1:
                transform.rotation = Quaternion.Euler(20f, 0f, 0f);
                offset = new Vector3(0f, 10f, -10f);
                break;
            case 2:
                transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                offset = new Vector3(0f, 20f, 0f);
                break;
            case 3:
                transform.rotation = Quaternion.Euler(45f, -45f, 0f);
                offset = new Vector3(8, 15f, -6f);
                break;
            case 4:
                transform.rotation = Quaternion.Euler(30f, 0f, 0f);
                offset = new Vector3(0f, 5f, -5f);
                break;
            case 5:
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                offset = new Vector3(0f, 2.5f, 1.5f);
                break;
        }
    }
}
