using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    float rotationValue = 0;

    void OnMouseDown()
    {
        transform.Rotate(Vector3.up * 90);
        rotationValue = transform.rotation.eulerAngles.y;
        Debug.Log(rotationValue);

        if (rotationValue == rotationAnswer)
        {
            Debug.Log("Correct");
            
            Debug.Log(GameObject.Find("GameComplete").GetComponent<TileGameComplete>().totalCorrectTiles));

        }
        else
        {
            Debug.Log("Wrong");
        }
    }
    }
