using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RedEyeController : MonoBehaviour
{
    public NavMeshAgent agent; //variable for the Enemy navMesh.
    public LayerMask whatIsGround, whatIsPlayer; //to select the ground and player layers.

    //since you cant look at it to kill it there has to be a to make the red eye dissapear, it will be implemented later
    [SerializeField] private float diesIn=5f;
    private bool seen;//to determine the start of the things written above
    public GameObject adam;
    MovementController movementController;// adam movement controller script

    public float deathTime=1.5f;//how many seconds will it take for the eye to dissapear

    private float counter;

    public float errorMargin = 0.5f;//how far can the mouse can be from the eye so that it still dissapears

    private Renderer[] renderers;//renderers of the main body and two eyes

    public float seeAng=30f;//how wide can adam see * 2
    private bool adamFound=false;
    private bool walkingPointSet=false;
    private Vector3 desPoint;
    [SerializeField] private float range;


    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        counter=0f;
        seen=false;
        agent = GetComponent<NavMeshAgent>(); //sets the agent.
        whatIsGround = LayerMask.NameToLayer("Ground");
        adam = GameObject.Find("Ad4m");
        movementController = adam.GetComponent<MovementController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 currentPos=transform.position;

        currentPos.y = 1;

        transform.position=currentPos;
        
        if(adam)
        {
            adamFound=true;
        }

        if(adamFound)
        {
            //makes the eye look at adam
            transform.LookAt(adam.transform.position);

            //makes the eye change color over distances from white to red
            changeColor();

            //navmesh chaseplayer
            Patrol();

            //visibility of the eyes
            EyeSight();

            if(Vector3.Distance(transform.position,adam.transform.position) < 3)
            {
                death();
            }

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

    [SerializeField]private float damageAmount=30f;

    private void checkDeath()
    {
        //if enough time passes eye is destroyed
        //but since red eye is not suppose to be looked at this will be changed with something to damage the player
        if(counter>deathTime)
        {
            death();
            Debug.Log("You Did Bad!!!!");
        }
    }

    public void getAdam(GameObject temp)
    {
        adam = temp;
    }

    //will be changed
    private void death()
    {
        adam.GetComponent<PlayerHealth>().TakeDamage(damageAmount);
        Destroy(gameObject);
    }

    //calculates the distance betwenn adam and the eye and changes eyes color from white to red according to that distance
    private void changeColor()
    {
        var distance = Vector3.Distance(adam.transform.position, transform.position);
        distance = Mathf.Clamp(distance, 2, 10);
        float sat = 100 - (10 * (distance-2));

        // Calculate the new color
        Color newColor = Color.HSVToRGB(0, sat / 100, 0.3f);

        // Change the color of the material
        renderers[1].material.color = newColor;
        renderers[2].material.color = newColor;

        newColor = Color.HSVToRGB(0, sat / 100, 1f);
        // Enable emission keyword
        renderers[1].material.EnableKeyword("_EMISSION");
        renderers[2].material.EnableKeyword("_EMISSION");

        // Change the emission color of the material
        renderers[1].material.SetColor("_EmissionColor", newColor);
        renderers[2].material.SetColor("_EmissionColor", newColor);
    }

    //navmesh
    private void ChasePlayer()
    {
        agent.destination = adam.transform.position;    
    }
    private void Patrol()
    {
        if(!walkingPointSet) SearchForDes();
        if(walkingPointSet) agent.SetDestination(desPoint);
        if(Vector3.Distance(transform.position,desPoint)<1) walkingPointSet=false;
    }
    private void SearchForDes()
    {
        float x = UnityEngine.Random.Range(-range,range);
        float z = UnityEngine.Random.Range(-range,range);

        desPoint = new Vector3(transform.position.x + x, transform.position.y, transform.position.z +z);
        print(desPoint);

        if(Physics.Raycast(desPoint, Vector3.down, whatIsGround))
        {
            walkingPointSet = true;
        }    
    }
    //visibility of the eyes
    private void EyeSight()
    {
        //counts for body eye1 and eye2
        var loopCounter=0;

            //speacial case for the body renderer in case we want to be able to see it sooner
            foreach(var r in renderers)
            {
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
                    //and the eye is stopped when invisible
                    if(Math.Abs(angle)<seeAng)
                    {
                        renderers[loopCounter].enabled=true;
                        agent.isStopped = true;
                    }

                    else if(Vector3.Distance(transform.position, adam.transform.position)<5)
                    {
                        renderers[loopCounter].enabled=true;
                        agent.isStopped = false;
                    }
                    //eye moves if visible
                    else
                    {   
                        renderers[loopCounter].enabled=false;
                        agent.isStopped = false;
                    }
                }
                loopCounter+=1;

            }
    }
}