using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowEyeController : MonoBehaviour
{
    public GameObject adam;
    MovementController movementController;

    public float deathTime=1.5f;

    private float counter;

    public float errorMargin = 0.5f;

    private Renderer rend;
    private Renderer[] renderers;
    private Transform[] transforms;

    public float seeAng=50f;

    private bool looking;
    // Start is called before the first frame update
    void Start()
    {
        movementController = adam.GetComponent<MovementController>();
        rend = GetComponent<Renderer>();
        renderers = GetComponentsInChildren<Renderer>();
        counter=0f;
        looking=false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(adam.transform.position);

        //Debug.Log(counter);
        var xDif=Math.Abs(movementController.target.x-transform.position.x);
        var zDif=Math.Abs(movementController.target.z-transform.position.z);

        if(xDif<errorMargin && zDif<errorMargin)
        {
            looking=true;
            checkDeath();
            counter+=Time.deltaTime;
        }
        else if(looking)
            attack();
            


        var loopCounter=0;

        foreach(var r in renderers)
        {
            if(loopCounter==0)
            {
                float angle = Vector3.Angle(adam.transform.forward, transform.position-adam.transform.position);
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
                if(Math.Abs(angle)<seeAng)
                {
                    renderers[loopCounter].enabled=true;
                }
                else
                {   
                    renderers[loopCounter].enabled=false;
                }
            }
            loopCounter+=1;

        }
    }

    private void checkDeath()
    {
        if(counter>deathTime)
        {
            Destroy(gameObject);
        }
    }
    
    private void attack()
    {
        Debug.Log("attacked");
        Destroy(gameObject);
    }
}
