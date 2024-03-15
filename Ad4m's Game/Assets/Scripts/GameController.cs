using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]private GameObject adam;
    [SerializeField]private bool isHorrorActive=false;
    [SerializeField]private GameObject eyeSpawner; 

    
    
    // Start is called before the first frame update
    void Start()
    {
        eyeSpawner.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if(isHorrorActive)
        {
            eyeSpawner.SetActive(isHorrorActive);
        }
    }

    public GameObject sendAdam()
    {
        return adam;
    }

}
