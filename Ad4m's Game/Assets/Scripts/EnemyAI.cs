//this code was first written in 26 February by Alper, please note whenever updated below!
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent; //variable for the Enemy navMesh.
    public Transform player; //variable for the Player game object.
    public LayerMask whatIsGround, whatIsPlayer; //to select the ground and player layers.
    public float health; //health of the Enemy. Unused for now as there is no way to deal damage.

    //Patrolling
    public Vector3 walkPoint;  //the point where the enemy will walk to.
    bool walkPointSet;
    public float walkPointRange; //the range of the patrolling area.

    //Attacking
    public float timeBetweenAttacks; 
    bool hasAttacked;
    public GameObject projectile; //the projectile model.

    //States
    public float sightRange, attackRange; 
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("PlayerGameObj").transform; //sets the Player.
        agent = GetComponent<NavMeshAgent>(); //sets the agent.
    }
 
    private void Update()
    {
        //Check for sight and attack range every frame
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    private void Patrolling()
    {
        if (!walkPointSet) SearchWalkPoint();
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ); //walk to the random point.

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) //if the random point is beneath ground don't update walkpoint
            walkPointSet = true;
    }
    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }
    private void AttackPlayer()
    {
        ///WIP
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);
        
        transform.LookAt(player);

        if (!hasAttacked)
        {
            ///Attack code here
            GameObject projectileInstance = Instantiate(projectile, transform.position, Quaternion.identity);
            Rigidbody rb = projectileInstance.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
                rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            }
            else
            {
                Debug.LogError("The instantiated projectile does not have a Rigidbody component.");
            } //this is placeholder code to simulate damage
            ///End of attack code

            hasAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        hasAttacked = false;
    }
    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }


}
