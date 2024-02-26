using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MovementController : MonoBehaviour
{
    /*
    //movement
    [SerializeField] private float moveSpeed = 5f;

    private Vector2 inputVector;
    private Vector3 mousePos;

    private Rigidbody rb;

    private Camera mainCamera;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        mainCamera = Camera.main;
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        inputVector = new Vector2(h,v);
        

        mousePos = Input.mousePosition;

    }

    void FixedUpdate()
    {
        var targetVector = new Vector3(inputVector.x, 0, inputVector.y);
        Debug.Log("1:"+targetVector);
        MoveTowardsTarget(targetVector);

        RotateTowardsMouse();
    }


    private void MoveTowardsTarget(Vector3 targetVector)
    {
        var speed=moveSpeed * Time.deltaTime;

        targetVector = Quaternion.Euler(0, mainCamera.gameObject.transform.eulerAngles.y, 0) * targetVector;
        Debug.Log("2:"+targetVector);
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

    */

    public float moveSpeed = 4f; // Speed of player movement
    public float rotationSpeed = 5f; // Speed of player rotation

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Lock cursor to center of screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Player Rotation
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;

        // Rotate the player only around the Y-axis
        transform.Rotate(Vector3.up * mouseX);

        // Player Movement
        if (Input.GetKey(KeyCode.W))
        {
            Vector3 movement = transform.forward * moveSpeed * Time.deltaTime;
            rb.MovePosition(transform.position + movement);
        }
    }
}
