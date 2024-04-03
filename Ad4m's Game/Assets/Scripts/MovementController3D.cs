using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class MovementController3D : MonoBehaviour
{
    //movement
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private GameObject followTransform;
    private Vector3 inputVector;
    private Vector3 mousePos;
    private bool isDashing;
    public float dashSpeed = 30f;
    public float dashTime = 0.1f;
    public Vector3 target;
    [SerializeField] bool movement2D=false;

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        isDashing = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private float h;
    private float v;
    private float mouseX;
    private float mouseY;
    [SerializeField] private float cameraSensivity = 1f;
    [SerializeField] private Transform cam;
    void Update()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;

        camForward.y=0;
        camRight.y=0;

        Vector3 forwardRel = v * camForward;
        Vector3 rightRel = h * camRight;

        Vector3 movementDir = forwardRel + rightRel; 
        
        mouseX = Input.GetAxis("Mouse Y");
        mouseY = Input.GetAxis("Mouse X");
        target = new Vector3(-mouseX,mouseY*cameraSensivity,0);
        transform.eulerAngles = transform.eulerAngles + target;

        var angles = transform.localEulerAngles;
        angles.z = 0;

        var angle = transform.localEulerAngles.x;    

        if(angle>180 && angle<353) angles.x=353;
        else if (angle<160 && angle>20) angles.x = 20;

        transform.localEulerAngles = angles;

        inputVector = movementDir;
        

        mousePos = Input.mousePosition;
    }
    void FixedUpdate()
    {
        var targetVector = new Vector3(inputVector.x, 0, inputVector.z).normalized;
        
        if(!isDashing)
            MoveTowardsTarget(targetVector);
        else if(isDashing)
            DashTowardsTarget(targetVector);


    }

    private void MoveTowardsTarget(Vector3 targetVector)
    {
        var speed=moveSpeed * Time.deltaTime;
        var targetPosition = transform.position + targetVector * speed;
        transform.position = targetPosition;
    }

    public void Dashing()
    {
        isDashing=true;
        Invoke("NotDashing", dashTime);
    }

    private void NotDashing()
    {
        isDashing=false;
    }

    private void DashTowardsTarget(Vector3 targetVector)
    {
        var speed=dashSpeed * Time.deltaTime;
        var targetPosition = transform.position + targetVector * speed;
        transform.position = targetPosition;
    }
    
}
