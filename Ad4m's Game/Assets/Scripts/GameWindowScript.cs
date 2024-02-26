using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class GameWindowScript : MonoBehaviour
{
    public GameManager gameManager;

    public bool isOnWindow = false;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void OnPointerClick()
    {
        Debug.Log("Clicked on the GameObject: " + gameObject.name);

        // You can perform actions specific to when the GameObject is clicked here
    }

    public void OnPointerEnter()
    {
        Debug.Log("Cursor is over the GameObject: " + gameObject.name);
        isOnWindow = true;
        gameManager.isCursorChecking = false;

    }

    public void OnPointerExit()
    {
        gameManager.isCursorChecking = true;

    }

}
