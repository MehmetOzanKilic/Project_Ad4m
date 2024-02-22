using UnityEngine;

public class MovementController : MonoBehaviour
{
    //movement
    public float moveSpeed = 5f;
    //public float rotationSpeed = 20f;

    //jumping
    public float jumpForce = 2f;
    public bool isOnGround;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
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
        movement = Camera.main.transform.TransformDirection(movement);
        movement.y = 0.0f;

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

}
