using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionCarrier : MonoBehaviour
{
    //makes it easier to call functions in between scripts
    public delegate void DragEndedDelegate(SectionCarrier draggableObject);
    public DragEndedDelegate dragEndedCallBack;

    //how far the mouse is from the center of the section
    private Vector3 offset;

    private Vector3 startPos;

    private bool dragging=false;

    // Start is called before the first frame update
    void Start()
    {   
        //Call the find function a little bit later to ensure everything is in place first
        Invoke("find",0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        //updates the posiion of the section according to the mouse position
        if(dragging)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
        }
    }

    void OnMouseDown()
    {
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragging = true;
    }

    void OnMouseUp()
    {
        dragging = false;
        //send the section back to the original position when the mouse button is up
        transform.position = startPos;
        //will send this object and script to the snapController to check if the section will snap or not
        dragEndedCallBack(this);
    }

    private void find()
    {
        //seperate function to designate starting positions after some time
        startPos= transform.position;
    }
}
