using System.Collections;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private bool collided = false;

    private void Start()
    {
        StartCoroutine(DestroyAfterDelay(5f));
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

}
