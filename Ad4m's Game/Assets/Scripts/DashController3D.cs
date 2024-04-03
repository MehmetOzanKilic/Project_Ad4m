using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashController3D : MonoBehaviour
{
    MovementController3D movementController;
    public float dashCoolDown=1f;
    private float counter;
    void Start()
    {
        movementController = GetComponent<MovementController3D>();
        counter=dashCoolDown;
    }

    void Update()
    {
        counter+=Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.LeftShift) && counter > dashCoolDown)
        {
            movementController.Dashing();
            counter=0;
        }
    }
}
