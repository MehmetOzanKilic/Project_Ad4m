using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private float topDownAngle;

    [SerializeField]
    private float distance;
    public Transform target;
    public float smoothSpeed = 0.25f; 
    public Vector3 offset;

    /*public float leftEdge;
    public float rightEdge;
    public float topEdge;
    public float bottomEdge;*/

    void FixedUpdate()
    {
        Vector3 targetPos = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPos, smoothSpeed);
        transform.position = smoothedPosition;

        /*transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, leftEdge, rightEdge),
            Mathf.Clamp(transform.position.y, bottomEdge, topEdge),
            transform.position.z
        );*/
    }

}
