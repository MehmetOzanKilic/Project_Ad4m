using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class WhiteEyeController : MonoBehaviour
{
    public NavMeshAgent agent; //variable for the Enemy navMesh.
    public LayerMask whatIsGround, whatIsPlayer; //to select the ground and player layers.
    [SerializeField]private float moveSpeed=1f;
    public GameObject adam;
    private MovementController movementController;//adam movement controller script

    public float deathTime=1.5f;//how many seconds will it take for the eye to dissapear 

    private float counter;

    public float errorMargin = 1f;//how far can the mouse can be from the eye so that it still dissapears

    private Renderer[] renderers;//renderers of the main body and two eyes

    public float seeAng=30f;//how wide can adam see * 2

    private bool adamFound=false;

    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        counter=0f;
        agent = GetComponent<NavMeshAgent>(); //sets the agent.
    }

    void Update()
    {
        if(adam)
        {
            movementController = adam.GetComponent<MovementController>();
            adamFound=true;
        }

        if(adamFound)
        {
            //makes the eye look at adam
            transform.LookAt(adam.transform.position);

            //navmesh chaseplayer  
            ChasePlayer();

            //visibility of the eyes
            EyeSight();

            //differences between mouse position and eye position
            var xDif=Math.Abs(movementController.target.x-transform.position.x);
            var zDif=Math.Abs(movementController.target.z-transform.position.z);

            //if the differences are small enough scripts checks death 
            if(xDif<errorMargin && zDif<errorMargin)
            {
                checkDeath();
                counter+=Time.deltaTime;
            }
            else
                counter=0f;
        }
        

    }

    void FixedUpdate()
    {

    }

    private void checkDeath()
    {
        //if enough time passes eye is destroyed
        if(counter>deathTime)
            Destroy(gameObject);
    }

    public void getAdam(GameObject temp)
    {
        adam = temp;
    }

    //navmesh
    private void ChasePlayer()
    {
        agent.destination = adam.transform.position;    
    }

    //visibility of the eyes
    private void EyeSight()
    {   
        //counts for body eye1 and eye2
        var loopCounter=0;

            foreach(var r in renderers)
            {   
                //speacial case for the body renderer in case we want to be able to see it sooner
                if(loopCounter==0)
                {
                    float angle = Vector3.Angle(adam.transform.forward, transform.position-adam.transform.position);
                    //seeAngle is increased here for body
                    if(Math.Abs(angle)<seeAng+70d)
                    {
                        renderers[0].enabled=true;
                    }
                    else
                    {   
                        renderers[0].enabled=false;
                    }
                }

                else
                {
                    float angle = Vector3.Angle(adam.transform.forward, transform.GetChild(loopCounter-1).position-adam.transform.position);
                    //if the eye-adam vector and adam's look direction vector has a angle smaller then the seeAngle eye renderer becomes visible
                    //and the eye is stopped when visible  
                    if(Math.Abs(angle)<seeAng)
                    {
                        renderers[loopCounter].enabled=true;
                        agent.isStopped=true;

                    }
                    //eye moves if not visible
                    else
                    {   
                        renderers[loopCounter].enabled=false;
                        agent.isStopped=false;
                    }
                }
                loopCounter+=1;

            }
    }

}
