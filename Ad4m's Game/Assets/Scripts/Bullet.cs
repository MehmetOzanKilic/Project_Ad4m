/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [HideInInspector] public Vector3 move;
    [SerializeField] private float speed;
    private float time=0.0f;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(time>=10)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition((Vector3) transform.position + (move * speed * Time.deltaTime));

        time = time + 0.1f;
    }
}
*/
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Bullet : MonoBehaviour
{
    [HideInInspector] public Vector3 move;
    [SerializeField] private float speed;
    private float time = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (time >= 2)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        // Only move in the x and z axes
        Vector3 movement = new Vector3(move.x, 0f, move.z);
        transform.position += movement * speed * Time.deltaTime;

        time += Time.deltaTime;
    }
}
