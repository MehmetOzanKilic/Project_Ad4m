using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PuzzleMovementController : MonoBehaviour
{
    // 1 - 360 derece etrada bakabilmek
    // 2 - hareket edebilmek wasd ile
    // 3 - noktanin denk geldigi objeyi tutabilmek



    //movement
    [SerializeField] private float moveSpeed = 5f;
    private Vector2 inputVector;
    private Vector3 mousePos;
    public Camera mainCamera;
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        mainCamera = Camera.main;
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
        

        RotateTowardsMouse();
    }

    private void MoveTowardsTarget(Vector3 targetVector)
    {
        var speed=moveSpeed * Time.deltaTime;

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
