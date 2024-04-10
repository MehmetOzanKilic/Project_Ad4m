using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class puzzleslotcontroller : MonoBehaviour
{
    public bool hasPuzzlePiece;

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
