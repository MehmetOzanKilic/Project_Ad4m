using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision) //checks for collision with bullets and deals appropriate damage. 
    {
        Transform hitTransform = collision.transform;
        if (hitTransform.CompareTag("Bullet"))
        {
            Destroy(gameObject);
            GameObject.Find("GameController").GetComponent<ShooterController>().enemyShot();
        }
        /*else if (hitTransform.CompareTag("anything else")
         * {
         *  whatever you want
         * }*/
    }

    private float deathTimer=0;
    void Update()
    {
        if(deathTimer>4)
        {
            Destroy(gameObject);
        }
        else
        {
            deathTimer+=Time.deltaTime;
        }
    }

    
    


}
