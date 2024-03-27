using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class WhiteEyeController : EyeController
{
    [SerializeField]private float moveSpeed=1f;
    [SerializeField]private float chaseDis=5f;
    [SerializeField]private float attackDis=1.5f;
    [SerializeField]private float deathTime=10f;//how many seconds will it take for the eye to dissapear 

    private float counter;

    [SerializeField]private float errorMargin = 1f;//how far can the mouse can be from the eye so that it still dissapears

    private bool flag=true;
    void Start()
    {
        counter=0f;
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        distance = Vector3.Distance(adam.transform.position, transform.position);
        xDif=Math.Abs(movementController.target.x-transform.position.x);
        zDif=Math.Abs(movementController.target.z-transform.position.z);
        
        //navmesh patrol or chaseplayer
        if(distance>chaseDis)  patrol();
        else if(distance<chaseDis && distance>attackDis) chasePlayer();
        else if(distance<attackDis)
        {
            if(flag)
            {
                Attack();
                flag=false;
            }
        }

        //visibility of the eyes
        eyeSight();

        //if the differences are small enough scripts checks death 
        if(xDif<errorMargin && zDif<errorMargin)
        {
            CheckDeath();
            counter+=Time.deltaTime;
        }
        else
            counter=0f;
    }

    private void CheckDeath()
    {
        //if enough time passes eye is destroyed
        if(counter>deathTime)
            Die();
    }

    private void Attack()
    {
        Debug.Log("white eye attacked");
        Invoke("Die",0.3f);
    }
    
    private void Die()
    {
        
        Debug.Log("white eye died");
        Destroy(gameObject);
    }

}
