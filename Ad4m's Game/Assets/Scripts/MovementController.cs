using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MovementController : MonoBehaviour
{
    //movement
    [SerializeField] private float moveSpeed = 5f;
    private Vector2 inputVector;
    private Vector3 mousePos;
    private Camera mainCamera;
    private bool isDashing;
    public float dashSpeed = 30f;
    public float dashTime = 0.1f;

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        mainCamera = Camera.main;
        isDashing = false;
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        inputVector = new Vector2(h,v);
        

        mousePos = Input.mousePosition;
    }
    void FixedUpdate()
    {
        var targetVector = new Vector3(inputVector.x, 0, inputVector.y).normalized;
        
        if(!isDashing)
            MoveTowardsTarget(targetVector);
        else if(isDashing)
            DashTowardsTarget(targetVector);

        RotateTowardsMouse();
    }

    private void MoveTowardsTarget(Vector3 targetVector)
    {
        var speed=moveSpeed * Time.deltaTime;

        targetVector = Quaternion.Euler(0, mainCamera.gameObject.transform.eulerAngles.y, 0) * targetVector;
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

        targetVector = Quaternion.Euler(0, mainCamera.gameObject.transform.eulerAngles.y, 0) * targetVector;
        var targetPosition = transform.position + targetVector * speed;
        transform.position = targetPosition;
    }
    private void RotateTowardsMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(mousePos);

        if(Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance: 300f))
        {
            var target = hitInfo.point;
            target.y = transform.position.y;
            transform.LookAt(target);
        }
    }
}