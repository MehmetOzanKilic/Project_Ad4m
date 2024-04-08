//this code was first written in 26 February by Alper, please note whenever updated below!
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GunmanAI  : MonoBehaviour
{
    private NavMeshAgent agent; //variable for the Enemy navMesh.
    private Transform player; //variable for the Player game object.
    public LayerMask whatIsGround, whatIsPlayer; //to select the ground and player layers.

    //Attacking
    public float timeBetweenAttacks;
    bool hasAttacked;
    public GameObject projectile; //the projectile model.
    public bool isDodging;


    //States
    public float  attackRange;
    public bool  playerInAttackRange;

    private void Start()
    {
        player = GameObject.Find("Ad4m").transform; //sets the Player
        agent = GetComponent<NavMeshAgent>(); //sets the agent.
        
    }

    private void Update()
    {
        //Check for sight and attack range every frame
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer); //checks for attack range in a sphere around the agent with the radius attackRange

        if (playerInAttackRange)
            AttackPlayer();
        else if (!playerInAttackRange && !isDodging)
            ChasePlayer();
    }

    private void ChasePlayer()
    {   
        //if the distance between the player and agent is more than 5, move towards the player.
        if(Vector3.Distance(transform.position,player.position)>5)
        {
            agent.isStopped=false;
            agent.SetDestination(player.position);
        }
        else
            agent.isStopped=true; 
    }
    private void AttackPlayer()
    {
        transform.LookAt(player);

        if (!hasAttacked)
        {
            float spawnOffset = 0.0f;

            // Calculate the spawn position
            //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
            Vector3 spawnPosition = transform.position + transform.forward * spawnOffset;

            GameObject projectileInstance = Instantiate(projectile, spawnPosition, Quaternion.identity); //spawns a bullet GameObject
            Rigidbody rb = projectileInstance.GetComponent<Rigidbody>(); //rigidbody of the bullet GameObject
            //||DO NOT DO IT LIKE THIS! THIS IS PLACEHOLDER CODE! CHANGE THIS IF YOU HAVE TIME! THIS WILL FUCK UP OPTIMIZATION SO MUCH! MOVE GETCOMPONENT TO START() ASAP! -ALPER
            //||DO NOT DO IT LIKE THIS! THIS IS PLACEHOLDER CODE! CHANGE THIS IF YOU HAVE TIME! THIS WILL FUCK UP OPTIMIZATION SO MUCH! MOVE GETCOMPONENT TO START() ASAP! -ALPER
            //||DO NOT DO IT LIKE THIS! THIS IS PLACEHOLDER CODE! CHANGE THIS IF YOU HAVE TIME! THIS WILL FUCK UP OPTIMIZATION SO MUCH! MOVE GETCOMPONENT TO START() ASAP! -ALPER
            //||maybe use awake()?
            //||\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

            if (rb != null)
            {
                rb.AddForce(transform.forward * 32f, ForceMode.Impulse); //speed of the spawned projectile
                rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            }
            else
            {
                Debug.LogError("The instantiated projectile does not have a Rigidbody component.");
            }
            Dodge();

            //End of attack code

            hasAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void Dodge()
    {
        isDodging = true;
        agent.SetDestination(transform.position + (Random.insideUnitSphere * 5)); //randomly selects a point in a range of 5 to make hard to hit
        isDodging = false;
        
    }

    private void ResetAttack()
    {
        hasAttacked = false;
    }


    private void OnDrawGizmosSelected() //draws attack range in editor to make it easier to adjust (not important)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }


}
