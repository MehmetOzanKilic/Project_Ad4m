using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class EyeController : MonoBehaviour
{
    [SerializeField]private float stopDis = 1f;
    protected NavMeshAgent agent; //variable for the Enemy navMesh.
    protected LayerMask whatIsGround, whatIsPlayer; //to select the ground and player layers.
    protected GameObject playerObject;
    private PlayerAttributeController playerAttributeController;
    protected MovementController movementController;// adam movement controller script
    protected Renderer[] renderers;//renderers of the main body and two eyes
    protected float seeAng;//how wide can adam see, local variable, changed in EnemyAttributeController.
    protected bool adamFound=false;
    protected bool walkingPointSet=false;
    protected Vector3 desPoint;
    [SerializeField] private float movementRange; //what is this used for? should it be moved to EnemyAttributeController?
    protected float distance;
    protected float xDif;
    protected float zDif;
    protected bool seen=false;
    //visibility of the eyes
    [SerializeField] private float sightDistance = 8f;


    void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
        agent = GetComponent<NavMeshAgent>(); //sets the agent.
        whatIsGround = LayerMask.NameToLayer("Ground");
        playerObject = GameObject.FindWithTag("Player");
        movementController = playerObject.GetComponent<MovementController>();
    }
    private void Start()
    {
        playerAttributeController = playerObject.GetComponent<PlayerAttributeController>(); //imports values from playerattribute manager.
    }
    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(playerObject.transform.position, transform.position);

        /*if(distance<stopDis) agent.isStopped=true;
        else agent.isStopped=false;*/

        if (seeAng != playerAttributeController.seeAngle)
        {
            seeAng = playerAttributeController.seeAngle; //updates seeAng if there's change. Maybe implement by using scripts in EnemyAttributeController later?
        }

        transform.LookAt(playerObject.transform.position);
    }

    protected void changeColor(float color)
    {
        distance = Math.Clamp(distance,2,10);
        float sat = 100-(10*distance);

        renderers[1].material.color = Color.HSVToRGB(color,sat/100,1);
        renderers[2].material.color = Color.HSVToRGB(color,sat/100,1);  
    }

    public void getAdam(GameObject temp)
    {
        playerObject = temp;
    }
    public GameObject sendAdam()
    {
        return playerObject;
    }
    protected float sendDistance()
    {
        return distance;
    }
    protected float sendXDif()
    {
        return xDif;
    }
    protected float sendZDif()
    {
        return zDif;
    }

    protected void die()
    {
        Debug.Log("Eye has died.");
        Destroy(gameObject);
    }    

    //navmesh
    protected void chasePlayer()
    {
        agent.destination = playerObject.transform.position;
    
    }
    protected void patrol()
    {
        if(!walkingPointSet) SearchForDes();
        if(walkingPointSet) agent.SetDestination(desPoint);
        if(Vector3.Distance(transform.position,desPoint)<1) walkingPointSet=false;
    }
    private void SearchForDes()
    {
        float x = UnityEngine.Random.Range(-movementRange,movementRange);
        float z = UnityEngine.Random.Range(-movementRange,movementRange);

        desPoint = new Vector3(transform.position.x + x, transform.position.y, transform.position.z +z);

        if(Physics.Raycast(desPoint, Vector3.down, whatIsGround))
        {
            walkingPointSet = true;
        }    
    }

    protected void eyeSight()
    {
        if(distance<sightDistance)
        {
            //speacial case for the body renderer in case we want to be able to see it sooner
            foreach(var r in renderers)
            {
                float angle = Vector3.Angle(playerObject.transform.forward, r.transform.position-playerObject.transform.position);
                //if the eye-adam vector and adam's look direction vector has a angle smaller then the seeAngle eye renderer becomes visible
                //and the eye is stopped when invisible
                if(Math.Abs(angle)<seeAng)
                {
                    r.enabled=true;
                    agent.isStopped = true;
                    seen=true;
                }
                //eye moves if visible
                else
                {   
                    r.enabled=false;
                    agent.isStopped = false;
                }

            }
            
        }
        else
        {
            foreach(var r in renderers)
            {
                r.enabled = false;
                agent.isStopped=false;
            }
        }
    }
}
