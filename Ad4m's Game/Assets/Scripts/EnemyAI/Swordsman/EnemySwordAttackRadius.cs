using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(Collider))]

public class EnemySwordAttackRadius : MonoBehaviour
{
    private float meleeDamage;
    protected GameObject playerObject;
    private EnemyAttributeController enemyAttributeController;
    private PlayerHealth playerHealth;
    private GameObject gameController;
    private SwordsmanAI swordsmanAI; 
    GameObject parentObject;
    protected NavMeshAgent agent; //variable for the Enemy navMesh.

    private void Awake()
    {
        playerObject = GameObject.FindWithTag("Player"); //sets the Player
        gameController = GameObject.FindWithTag("GameController"); //sets the game object
        agent = transform.parent.GetComponent<NavMeshAgent>(); //sets the agent.
        parentObject = transform.parent.gameObject;
    }
    private void Start()
    {
        enemyAttributeController = gameController.GetComponent<EnemyAttributeController>();
        playerHealth = playerObject.GetComponent<PlayerHealth>();
        swordsmanAI = parentObject.GetComponent<SwordsmanAI>();
        meleeDamage = enemyAttributeController.meleeDamage;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Transform hitTransform = other.transform;
        if (hitTransform.CompareTag("Player"))
        {
            Debug.Log("Player hit by melee attack");
            playerHealth.TakeDamage(meleeDamage);
            agent.isStopped = true;
            swordsmanAI.DashAway();
        }
    }
}
