using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashController : MonoBehaviour
{
    MovementController movementController;//movement script
    public float dashCoolDown;//how many second before the next dash
    private float counter;//time passage

    void Start()
    {
        movementController = GetComponent<MovementController>();
        counter = dashCoolDown;
    }

    // Update is called once per frame
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
