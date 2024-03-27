using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHandler : MonoBehaviour
{
    Camera mainCamera;
    LayerMask layerMask;

    void Awake()
    {
        mainCamera = Camera.main;
        layerMask = LayerMask.GetMask("PlaneGround");
    }

    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layerMask))
        {
            transform.position = raycastHit.point;
        }
    }
}
