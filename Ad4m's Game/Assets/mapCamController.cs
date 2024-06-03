using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapCamController : MonoBehaviour
{
    private GameObject adam;
    // Start is called before the first frame update
    void Start()
    {
        adam = GameObject.Find("Ad4m"); 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 newPos=adam.transform.position;

        newPos.y=30;

        transform.position=newPos;
        Quaternion desiredRotation = Quaternion.Euler(90, 0, 0);

        transform.rotation=desiredRotation;
    }
}
