using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BasiEyeController : MonoBehaviour
{
    private MovementController_Level1 adam;
    private Renderer[] renderers;
    private Transform wallBase;
    // Start is called before the first frame update
    void Start()
    {

        adam=GameObject.Find("Ad4m").GetComponent<MovementController_Level1>();
        renderers = GetComponentsInChildren<Renderer>();
        wallBase = GameObject.Find("WallBase").GetComponent<Transform>();
        foreach(Renderer r in renderers)
        {
            r.enabled = false;
        }
    }

    private bool seen=false;
    private float deathCounter=0;
    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, adam.returnTarget());

        if(distance < 4)
        {
            foreach(Renderer r in renderers)
            {
                r.enabled = true;
                seen = true;
            }
        }
        else
        {
            foreach(Renderer r in renderers)
            {
                r.enabled = false;
                seen = false;
            }
        }


        if(distance<1.5f)
        {
            if(deathCounter>1)
            {
                GameObject.Find("GameController").GetComponent<HorrorController>().eyeFound();
                Destroy(gameObject);
            }
            else deathCounter += Time.deltaTime; 
        }
        else deathCounter = 0;
    }

    private void FixedUpdate()
    {
        /*if(!seen)
        {
            transform.position = Vector3.MoveTowards(transform.position,wallBase.position, 2  * Time.deltaTime);
        }*/
    }
}
