using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float speed = 5.0f; //4-5 is okay
    public float boundary = 0.1f; // how far the player needs to be from the edge so that the camera starts moving

    public float delayTime = 0.5f; // delay time for camera movement
    private float timeElapsed = 0f;

    public float minMovementThreshold = 0.05f; // how far the player needs to have moved for the camera follow to trigger
    private Vector3 lastPlayerPosition;
    
    //camera movement limits, this can be adjusted for the level size
    public float minX = -100f;
    public float maxX = 100f;
    public float minY = -100f;
    public float maxY = 100f;

    private bool isFollowing = false;

    void Start()
    {
        if (player != null)
        {
            lastPlayerPosition = player.position;
        }
    }

    void Update()
    {
        if (player != null)
        {
            Vector3 moveDirection = Vector3.zero;

            //operations are done using local positions, because both the camera and the player have parent objects
            Vector3 playerLocalPos = transform.InverseTransformPoint(player.position);

            //check if the player is near the edge of the screen
            if (playerLocalPos.x < boundary && transform.localPosition.x > minX)
            {
                moveDirection += Vector3.left;
            }
            else if (playerLocalPos.x > 1 - boundary && transform.localPosition.x < maxX)
            {
                moveDirection += Vector3.right;
            }

            if (playerLocalPos.y < boundary && transform.localPosition.y > minY)
            {
                moveDirection += Vector3.down;
            }
            else if (playerLocalPos.y > 1 - boundary && transform.localPosition.y < maxY)
            {
                moveDirection += Vector3.up;
            }

            //delay
            if (moveDirection != Vector3.zero)
            {
                if (timeElapsed < delayTime)
                {
                    timeElapsed += Time.deltaTime;
                }
                else if (Vector3.Distance(player.position, lastPlayerPosition) > minMovementThreshold)
                {
                    isFollowing = true;
                }
            }
            else
            {
                timeElapsed = 0f;
                isFollowing = false;
            }

            //update lastPlayerPosition
            lastPlayerPosition = player.position;

            //move the camera, lerp is for smooth movement
            if (isFollowing)
            {
                Vector3 targetPosition = transform.localPosition + moveDirection * speed * Time.deltaTime;
                transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, 0.5f);
            }
        }
    }
}

