//this code is pretty much useless until we have animations so ignore!

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunmanDamageController : MonoBehaviour
{
    public MeleeAttackController wp;
    public GameObject hitParticle;
    public float health = 100f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon" || other.tag == "Bullet")
        {
            other.GetComponent<Animator>().SetTrigger("Hit"); 
            Debug.Log(other.name);
            Instantiate(hitParticle, new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z), other.transform.rotation);
        }

    }
}
