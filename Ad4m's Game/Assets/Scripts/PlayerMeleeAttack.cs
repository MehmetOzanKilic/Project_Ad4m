using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeAttack : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

        Transform hitTransform = other.transform;
        if (hitTransform.CompareTag("Mobs"))
        {
            Debug.Log("player hit mob with melee!");
        }


    }

}
