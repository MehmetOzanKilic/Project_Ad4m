using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashController : MonoBehaviour
{
    MovementController3D movementController;
    public float dashCoolDown=1f;
    private float counter;
    private PlayerAttributeController playerAttributeController;

    void Start()
    {
        movementController = GetComponent<MovementController3D>();
        playerAttributeController = GetComponent<PlayerAttributeController>();
        dashCoolDown = playerAttributeController.dashCoolDown;
        counter = dashCoolDown;
    }

    void Update()
    {
        if (dashCoolDown != playerAttributeController.dashCoolDown) {
            dashCoolDown = playerAttributeController.dashCoolDown;
            counter = dashCoolDown;
        }

        counter +=Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.LeftShift) && counter > dashCoolDown)
        {
            movementController.Dashing();
            counter=0;
        }
    }
}
