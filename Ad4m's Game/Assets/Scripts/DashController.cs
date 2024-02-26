using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashController : MonoBehaviour
{
    MovementController movementController;
    public float dashCoolDown=1f;
    private float counter;
    void Start()
    {
        movementController = GetComponent<MovementController>();
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
