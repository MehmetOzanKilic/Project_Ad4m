using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class puzzlepiececontroller : MonoBehaviour
{
    public bool isSelected = false;
    public GameObject correctSlot;

    public puzzlegamecontroller puzzlegamecontroller;

    // Start is called before the first frame update
    void Start()
    {
        puzzlegamecontroller = GameObject.Find("PuzzleGameController").GetComponent<puzzlegamecontroller>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
