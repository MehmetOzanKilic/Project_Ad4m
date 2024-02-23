using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MovementController : MonoBehaviour
{
    //movement
    public float moveSpeed = 5f;
    //public float rotationSpeed = 20f;

    //jumping
    public float jumpForce = 2f;
    public bool isOnGround;

    private Rigidbody rb;

    [SerializeField] private LayerMask groundMask;
    private Camera mainCamera;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        mainCamera = Camera.main;
    }

    void Update()
    {
        Aim();

        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            Jump();
        }

    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;
        Debug.Log(movement);

        movement = Camera.main.transform.TransformDirection(movement);
        movement.y = 0.0f;

        Debug.Log(movement);

        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

        //RotatePlayer(movement);
    }

    /*void RotatePlayer(Vector3 movement)
    {
        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRotation, rotationSpeed * Time.deltaTime));
        }
    }*/

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isOnGround = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }
    }

    private (bool success, Vector3 position) GetMousePosition()
    {
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, groundMask))
        {
            return(success: true, position: hitInfo.point);
        }

        else
        {
            return(success: false, position: Vector3.zero);
        }
    }

    private void Aim()
    {
        var(success,position) = GetMousePosition();
        if(success)
        {
            var direction = position - transform.position;
            
            direction.y=0;

            transform.forward = direction; 
        }
    }

}
