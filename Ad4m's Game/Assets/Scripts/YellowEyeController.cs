using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public float deneme=80f;
    private bool adamFound=false;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        renderers = GetComponentsInChildren<Renderer>();
        counter=0f;
        looking=false;
    }

    // Update is called once per frame
    void Update()
    {
        if(adam)
        {
            movementController = adam.GetComponent<MovementController>();
            adamFound=true;
        }

        if(adamFound)
        {
            transform.LookAt(adam.transform.position);
            changeColor();

            //Debug.Log(counter);
            var xDif=Math.Abs(movementController.target.x-transform.position.x);
            var zDif=Math.Abs(movementController.target.z-transform.position.z);

            if(xDif<errorMargin && zDif<errorMargin)
            {
                checkDeath();
                counter+=Time.deltaTime;
                looking=true;
            }
            else if(looking)
            {
                attack();
            }


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

    public void getAdam(GameObject temp)
    {
        adam = temp;
    }

    private void death()
    {
        Destroy(gameObject);
    }

    private void changeColor()
    {
        var distance = Vector3.Distance(adam.transform.position, transform.position);
        distance = Math.Clamp(distance,2,10);
        float sat = 100-(10*distance);

        renderers[1].material.color = Color.HSVToRGB(0.14f,sat/100,1);
        renderers[2].material.color = Color.HSVToRGB(0.14f,sat/100,1);  
    }
}
