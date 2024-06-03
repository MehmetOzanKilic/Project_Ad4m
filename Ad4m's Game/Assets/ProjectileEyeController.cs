using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ProjectileEyeController : MonoBehaviour
{
    public NavMeshAgent agent; //variable for the Enemy navMesh.
    public LayerMask whatIsGround, whatIsPlayer; //to select the ground and player layers.

    [SerializeField]private GameObject eyeProjectile;

    [SerializeField] private ProjectilePool projectilePool;
    //since you cant look at it to kill it there has to be a to make the red eye dissapear, it will be implemented later
    [SerializeField] private float diesIn=3f;
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
        print(projectilePool);
        renderers = GetComponentsInChildren<Renderer>();
        counter=0f;
        seen=false;
        agent = GetComponent<NavMeshAgent>(); //sets the agent.
        whatIsGround = LayerMask.NameToLayer("Ground");
        adam = GameObject.Find("Ad4m");
        movementController = adam.GetComponent<MovementController>();
        print(projectilePool);
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 currentPos=transform.position;

        currentPos.y = 1.3f;

        transform.position=currentPos;

        if(adam)
        {
            adamFound=true;
        }

        if(adamFound)
        {
            //makes the eye look at adam
            transform.LookAt(adam.transform.position);

            if(Vector3.Distance(transform.position, adam.transform.position) < 3)
            {
                death();
            }

            //makes the eye change color over distances from white to red
            changeColor();

            //navmesh chaseplayer
            Patrol();

            //visibility of the eyes
            EyeSight();

            //differences between mouse position and eye position
            var xDif=Math.Abs(movementController.target.x-transform.position.x);
            var zDif=Math.Abs(movementController.target.z-transform.position.z);

            //if the differences are small enough scripts checks death
            if(SelectedSections.returnCount() <4)
            {
                if(xDif<errorMargin && zDif<errorMargin)
                {
                    checkDeath();
                    counter+=Time.deltaTime;
                }
                else
                    counter=0f;
            }
            else
            {
                if(seenFlag)
                {
                    checkDeath();
                    counter+=Time.deltaTime;
                }
                else
                    counter=0f;
            }


        }

    }

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
        Debug.Log("death");
        GameObject temp=(GameObject)Instantiate(eyeProjectile, transform.position, transform.rotation);
        Destroy(gameObject);
    }
    

    //calculates the distance betwenn adam and the eye and changes eyes color from white to red according to that distance
    private void changeColor()
    {
        var distance = Vector3.Distance(adam.transform.position, transform.position);
        distance = Math.Clamp(distance,2,10);
        float sat = 100-(10*(distance-5));

        Color newColor = Color.HSVToRGB(0.9f,sat/100,0.5f);

        renderers[1].material.color = newColor;
        renderers[2].material.color = newColor;

        renderers[1].material.EnableKeyword("_EMISSION");
        renderers[2].material.EnableKeyword("_EMISSION");

        newColor = Color.HSVToRGB(0.9f,sat/100,1f);

        // Change the emission color of the material
        renderers[1].material.SetColor("_EmissionColor", newColor);
        renderers[2].material.SetColor("_EmissionColor", newColor);  
    }

    //navmesh
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

        if(Physics.Raycast(desPoint, Vector3.down, whatIsGround))
        {
            walkingPointSet = true;
        }    
    }
    //visibility of the eyes
    private bool seenFlag = false;
    private void EyeSight()
    {
        //counts for body eye1 and eye2
        var loopCounter=0;

            //speacial case for the body renderer in case we want to be able to see it sooner
            foreach(var r in renderers)
            {
                if(loopCounter==0)
                {
                    // Get the player's forward direction, ignoring the y component
                    Vector3 playerForward = new Vector3(adam.transform.forward.x, 0, adam.transform.forward.z);

                    // Get the direction to the target object, ignoring the y component
                    Vector3 targetDirection = new Vector3(transform.position.x - adam.transform.position.x, 0, transform.position.z - adam.transform.position.z);

                    // Calculate the angle between the player's forward direction and the target direction
                    float angle = Vector3.Angle(playerForward, targetDirection);
                    if(SelectedSections.returnCount()==4)angle-=5;
                    print(angle);

                    //seeAngle is increased here for body
                    if(Math.Abs(angle)<seeAng+70d)
                    {
                        renderers[0].enabled=true;
                        if(Math.Abs(angle)<(seeAng-26f))
                        {
                            seenFlag=true;
                        }
                        else seenFlag=false;
                        
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

                    else if(Vector3.Distance(transform.position, adam.transform.position) < 5)
                    {
                        renderers[loopCounter].enabled=true;
                        agent.isStopped = true;
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