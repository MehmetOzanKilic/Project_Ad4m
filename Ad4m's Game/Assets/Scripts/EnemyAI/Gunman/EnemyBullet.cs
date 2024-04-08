using System.Collections;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    protected float damageAmount = 10f; //damageamount for generic gunman. maybe could add diferent classes later?
    private void Start()
    {
        StartCoroutine(DestroyAfterDelay(5f)); //destroys bullet if it doesn't hit within 5 seconds
    }
    private void OnCollisionEnter(Collision collision)
    {
        Transform hitTransform = collision.transform;
        if (hitTransform.CompareTag("Player")) //checks for the Player tag to launch takeDamage function in PlayerHealth
        {
            Debug.Log("hit player!");
            hitTransform.GetComponent<PlayerHealth>().TakeDamage(damageAmount);
            Destroy(gameObject);
        }
        /*else if (hitTransform.CompareTag("Mobs") || hitTransform.CompareTag("Ground"))
        {
            //Debug.Log("hit mob!");
        }
        else { Destroy(gameObject); }*/
        
    }
    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);        
        Debug.Log("Bullet destroyed due to timeout.");
        Destroy(gameObject);
    }

}
