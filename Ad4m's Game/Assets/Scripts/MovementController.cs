using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MovementController : MonoBehaviour
{
    //movement
    [SerializeField] private float moveSpeed;
    private Vector2 inputVector;
    private Vector3 mousePos;
    private Camera mainCamera;
    private bool isDashing;
    public float dashSpeed;
    public float dashTime;
    public Vector3 target;
    [SerializeField] bool movement2D = false;
    private PlayerAttributeController playerAttributeController;
    private GameObject playerObject;
    Animator animator;

    private void Awake()
    {
        playerObject = GameObject.FindWithTag("Player"); //sets the game object
    }

    private int levelCount;
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        mainCamera = Camera.main;
        isDashing = false;
        // Get the PlayerDamageController component from the player GameObject
        playerAttributeController = playerObject.GetComponent<PlayerAttributeController>();
        animator = GetComponent<Animator>();
        levelCount = SelectedSections.returnCount();

    }

    private float h;
    private float v;
    void Update()
    {
        h = Input.GetAxisRaw("Horizontal");
        if (!movement2D) v = Input.GetAxisRaw("Vertical");
        else v = 0;
        inputVector = new Vector2(h, v);

        bool isMoving = inputVector.magnitude > 0.01f;
        animator.SetBool("isRunning", isMoving);
    }
    void FixedUpdate()
    {
        mousePos = Input.mousePosition;
        var targetVector = new Vector3(inputVector.x, 0, inputVector.y).normalized;

        if (!isDashing)
            MoveTowardsTarget(targetVector);
        else if (isDashing)
            DashTowardsTarget(targetVector);

        RotateTowardsMouse();

        if(levelCount>1)
        {
            if (moveSpeed != playerAttributeController.moveSpeed)
            {
                moveSpeed = playerAttributeController.moveSpeed;
            }
            if (dashSpeed != playerAttributeController.dashSpeed)
            {
                dashSpeed = playerAttributeController.dashSpeed;
            }
            if (dashTime != playerAttributeController.dashTime)
            {
                dashTime = playerAttributeController.dashTime;
            }
        }
    }

    private void MoveTowardsTarget(Vector3 targetVector)
    {
        var speed = moveSpeed * Time.deltaTime;

        targetVector = Quaternion.Euler(0, mainCamera.gameObject.transform.eulerAngles.y, 0) * targetVector;
        var targetPosition = transform.position + targetVector * speed;
        transform.position = targetPosition;
    }

    public void Dashing()
    {
        isDashing = true;
        Invoke("NotDashing", dashTime);
    }

    private void NotDashing()
    {
        isDashing = false;
    }

    private void DashTowardsTarget(Vector3 targetVector)
    {
        var speed = dashSpeed * Time.deltaTime;

        targetVector = Quaternion.Euler(0, mainCamera.gameObject.transform.eulerAngles.y, 0) * targetVector;
        var targetPosition = transform.position + targetVector * speed;
        transform.position = targetPosition;
    }

    public void RotateTowardsMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance: 300f))
        {
            target = hitInfo.point;
            //Debug.Log(target);
            target.y = transform.position.y;
            transform.LookAt(target);
        }
    }
}
