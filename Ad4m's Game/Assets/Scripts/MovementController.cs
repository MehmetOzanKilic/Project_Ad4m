using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MovementController : MonoBehaviour
{
<<<<<<< Updated upstream
    //movement
    [SerializeField] private float moveSpeed = 5f;

    private Vector2 inputVector;
=======
    private Vector2 inputVector;//wasd
>>>>>>> Stashed changes
    private Vector3 mousePos;
    private Rigidbody rb;
    private Camera mainCamera;
    public float moveSpeed = 3f;
    public float dashSpeed = 5f;//how fast the dash will go
    public float dashTime;//how long the dash will last
    public Vector3 targetVector;
    private bool isDashing;//flag to stop moving and start dashing

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        mainCamera = Camera.main;
        isDashing = false;
    }

    void Update()
    {
        mousePos = Input.mousePosition;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        inputVector = new Vector2(h,v);

    }

    void FixedUpdate()
    {
        targetVector = new Vector3(inputVector.x, 0, inputVector.y).normalized;

        RotateTowardsMouse();

        if(!isDashing)
            MoveTowardsTarget(targetVector);
        
        if(isDashing)
            DashTowardsTarget(targetVector);

    }

    private void MoveTowardsTarget(Vector3 targetVector)
    {
        var speed=moveSpeed * Time.deltaTime;

        //adjust the trajectory according to the cameras angle.
        targetVector = Quaternion.Euler(0, mainCamera.gameObject.transform.eulerAngles.y, 0) * targetVector;

        transform.position += targetVector*speed;
    }

    public void Dashing()
    {
        isDashing = true;

        //Calls the NotDashing() after dashTime seconds.Determines how long the dash lasts.
        Invoke("NotDashing", dashTime);
    }

    private void NotDashing()
    {
        isDashing = false;
    }

    private void DashTowardsTarget(Vector3 targetVector)
    {    
        var speed=dashSpeed * Time.deltaTime;

        targetVector = Quaternion.Euler(0, mainCamera.gameObject.transform.eulerAngles.y, 0) * targetVector;
        transform.position += targetVector*speed;
    }

    //tracks the players rotation by finding the point that the mouse is on and looking at it
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
<<<<<<< Updated upstream


=======
>>>>>>> Stashed changes
}
