using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision) //checks for collision with bullets and deals appropriate damage. 
    {
        Transform hitTransform = collision.transform;
        if (hitTransform.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
        /*else if (hitTransform.CompareTag("anything else")
         * {
         *  whatever you want
         * }*/
    }

    
    


}
