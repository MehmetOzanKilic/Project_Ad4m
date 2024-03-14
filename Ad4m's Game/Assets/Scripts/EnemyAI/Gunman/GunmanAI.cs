//this code was first written in 26 February by Alper, please note whenever updated below!
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GunmannAI : MonoBehaviour
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
        player = GameObject.Find("PlayerGameObj").transform; //sets the Player
        agent = GetComponent<NavMeshAgent>(); //sets the agent.
        
    }

    private void Update()
    {
        //Check for sight and attack range every frame
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (playerInAttackRange)
            AttackPlayer();
        else if (!playerInAttackRange && !isDodging)
            ChasePlayer();

    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }
    private void AttackPlayer()
    {
        ///WIP
        ///Make sure enemy doesn't move
        //agent.SetDestination(transform.position);
        
        transform.LookAt(player);

        if (!hasAttacked)
        {
            //Attack code here
            float spawnOffset = 0.0f;

            // Calculate the spawn position
            Vector3 spawnPosition = transform.position + transform.forward * spawnOffset;

            GameObject projectileInstance = Instantiate(projectile, spawnPosition, Quaternion.identity); //spawns a bullet GameObject
            Rigidbody rb = projectileInstance.GetComponent<Rigidbody>(); //rigidbody of the bullet GameObject
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
        agent.SetDestination(transform.position + (Random.insideUnitSphere * 5));
        isDodging = false;
        
    }

    private void ResetAttack()
    {
        hasAttacked = false;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }


}
