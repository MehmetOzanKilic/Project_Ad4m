using System.Collections;
using UnityEngine;

public class ProjectileBullet : MonoBehaviour
{
    private bool collided = false;
    [SerializeField] private float speed;
    private Vector3 shootDirection = new Vector3(0,0,0);
    private GameObject adam;

    private void Start()
    {
        StartCoroutine(DestroyAfterDelay(5f));
        adam=GameObject.Find("Ad4m");
        if(adam!=null)print("olması lazım");
    }

    public void setObjTransform(Vector3 temp)
    {
        transform.position = temp;
    }
    private void OnCollisionEnter(Collision collision)
    {
        Transform hitTransform = collision.transform;
        if (hitTransform.CompareTag("Player"))
        {
            Debug.Log("hit player!");
            hitTransform.GetComponent<PlayerHealth>().TakeDamage(10f);
        }
        if (hitTransform.CompareTag("Mobs") || hitTransform.CompareTag("Ground"))
        {
            //Debug.Log("hit mob!");
        }
        else { Destroy(gameObject); }
        
    }
    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);        
        Debug.Log("Bullet destroyed due to timeout.");
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

}
