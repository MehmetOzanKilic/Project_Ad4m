using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour
{
    public MeleeAttackController wp;
    public GameObject hitParticle;

    private void OnCollisionEnter(Collision collision)
    {
        Transform hitTransform = collision.transform;
        //if (hitTransform.CompareTag("Bullet"))
        //{
            Debug.Log("Hello There!");
            //other.GetComponent<Animator>().SetTrigger("Hit");
            hitTransform.GetComponent<PlayerHealth>().TakeDamage(20);
            //Instantiate(hitParticle, new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z), other.transform.rotation);
        //}

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
