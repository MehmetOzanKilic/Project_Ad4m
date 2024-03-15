//this code was first written in 26 February by Alper, please note whenever updated below!
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EyesAI : MonoBehaviour
{
    public NavMeshAgent agent; //variable for the Enemy navMesh.
    public Transform player; //variable for the Player game object.
    public LayerMask whatIsGround, whatIsPlayer; //to select the ground and player layers.

    private void Awake()
    {
        player = GameObject.Find("Ad4m").transform; //sets the Player.
        agent = GetComponent<NavMeshAgent>(); //sets the agent.
    }

    private void Update()
    {
        ChasePlayer();
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

}

