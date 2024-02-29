using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class WhiteEyeController : MonoBehaviour
{
    [SerializeField]private float moveSpeed=1f;
    public GameObject adam;
    MovementController movementController;

    public float deathTime=1.5f;

    private float counter;

    public float errorMargin = 0.5f;

    private Renderer rend;
    private Renderer[] renderers;
    private Transform[] transforms;

    public float seeAng=50f;

    private bool adamFound=false;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        renderers = GetComponentsInChildren<Renderer>();
        counter=0f;
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

            //Debug.Log(counter);
            var xDif=Math.Abs(movementController.target.x-transform.position.x);
            var zDif=Math.Abs(movementController.target.z-transform.position.z);

            if(xDif<errorMargin && zDif<errorMargin)
            {
                checkDeath();
                counter+=Time.deltaTime;
            }
            else
                counter=0f;


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

    void FixedUpdate()
    {

    }

    private void checkDeath()
    {
        if(counter>deathTime)
            Destroy(gameObject);
    }

    public void getAdam(GameObject temp)
    {
        adam = temp;
    }
}
