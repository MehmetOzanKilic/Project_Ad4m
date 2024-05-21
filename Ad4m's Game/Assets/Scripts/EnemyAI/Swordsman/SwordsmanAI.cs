//this code was first written in 26 February by Alper, please note whenever updated below!
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SwordsmanAI : MonoBehaviour
{
    protected NavMeshAgent agent; //variable for the Enemy navMesh.
    protected Transform player; //variable for the Player game object
    protected GameObject playerObject;
    protected PlayerAttributeController playerAttributeController;
    public LayerMask whatIsGround, whatIsPlayer; //to select the ground and player layers.

    //Attacking
    public float timeBetweenAttacks;
    public bool canAttack = true;
    public GameObject projectile; //the projectile model.
    public bool isDashing = false;
    protected float timeSlowMultiplier;
    public float oldSpeed;
    public float oldAcc;
    public float dashSpeed = 200f;


    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform; //sets the Player
        playerObject = GameObject.FindWithTag("Player"); //sets the Player
        agent = GetComponent<NavMeshAgent>(); //sets the agent.

    }
    private void Start()
    {
        playerAttributeController = playerObject.GetComponent<PlayerAttributeController>();
        oldSpeed = agent.speed;
        oldAcc = agent.acceleration;
    }

    private void Update()
    {

        ChasePlayer();

    }

    public void ChasePlayer()
    {
        //if the distance between the player and agent is more than 5, move towards the player.
        if (Vector3.Distance(transform.position, player.position) > 0.2 && Vector3.Distance(transform.position, player.position) > 7)
        {
            isDashing = false;
            agent.speed = oldSpeed;
            agent.SetDestination(player.position);
        }
        else if (Vector3.Distance(transform.position, player.position) > 0.2 && Vector3.Distance(transform.position, player.position) < 7)
        {
            agent.isStopped = true;
            agent.isStopped = false;
            agent.speed = 200f;
            isDashing = true;
            agent.SetDestination(player.position);
        }
        else
        {
            DashAway();
        }
    }
    public void DashAway()
    {
        agent.speed = dashSpeed;
        agent.acceleration = dashSpeed;
        isDashing = true;
        Dodge();
        Dodge();
        isDashing = false;
        canAttack = true;
        agent.speed = oldSpeed;
        agent.acceleration = oldAcc;
    }
    protected void Dodge()
    {
        agent.isStopped = true; 
        Vector3 randomDirection = Random.insideUnitSphere;
        randomDirection *= Random.Range(10f, 15f); // Random distance between 10 and 15
        Vector3 destination = transform.position + randomDirection;

        // Ensure the distance from current position to destination is greater than 10
        while (Vector3.Distance(transform.position, destination) <= 10f)
        {
            randomDirection = Random.insideUnitSphere;
            randomDirection *= Random.Range(10f, 15f);
            destination = transform.position + randomDirection;
        }

        agent.isStopped = false;
        agent.SetDestination(destination);
    }

}
