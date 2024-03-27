using UnityEngine;
using Cinemachine;

public class MyPlayer : MonoBehaviour
{
    CinemachineVirtualCamera virtualCamera;
    Camera mainCamera;
    LayerMask layerMask;
    public float moveSpeed = 5f;
    public Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>(); 
        layerMask = LayerMask.GetMask("PlaneGround");
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;

        if (moveDirection != Vector3.zero) { rb.velocity = moveDirection * moveSpeed; }
        else { rb.velocity = Vector3.zero; }
    }

    void HandleRotation()
    {
        Vector3 mousePos = Input.mousePosition;
        Ray ray = mainCamera.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layerMask))
        {
            Vector3 lookDirection = raycastHit.point - transform.position;
            lookDirection.y = 0f;
            Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
            rb.MoveRotation(lookRotation);
        }
    }
}
