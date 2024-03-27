using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RedEyeController : EyeController
{
    [SerializeField]private float eyeColor=0f;
    [SerializeField]private float errorMargin=1f;
    [SerializeField]private float attackDis=2f;
    [SerializeField]private float deathTime=0.7f;
    [SerializeField]private float diesIn=5f;
    private float diesInCounter=0f;
    private float counter;
    private float dis;
    private bool flag=true;
    private void Start()
    {
        counter=0f;
    }

    private void Update()
    {

    }
    private void FixedUpdate()
    {
        distance = Vector3.Distance(adam.transform.position, transform.position);
        xDif=Math.Abs(movementController.target.x-transform.position.x);
        zDif=Math.Abs(movementController.target.z-transform.position.z);

        if(distance>attackDis)patrol();
        else if(distance<attackDis)
        {
            if(flag)
            {
                Attack();
                flag=false;
            }
        }

        changeColor(eyeColor);

        eyeSight();

        if(seen)
        {
            diesInCounter+=Time.deltaTime;
            if(diesInCounter>diesIn)
            {
                Die();
            }
        }

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
        if(counter>deathTime)
            Attack();
    
    }

    private void Attack()
    {
        Debug.Log("red eye attacked");
        Invoke("Die",0.3f);
    }

    private void Die()
    {
        Debug.Log("red eyec died");
        Destroy(gameObject);
    }
}
