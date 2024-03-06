using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SnapController : MonoBehaviour
{
    //snapper = the rectangle in which the sections join(its called snapper because sections snap to it if they are close enough)  
    //snap = joining of the sections

    //to show the sections inside the snapper
    [SerializeField]private Text currentSections;
    //position of the snapper
    [SerializeField]private Transform snapPoint;
    //list of all the sections
    [SerializeField]private List<SectionCarrier> draggableObjects;
    //list of original positions of the sections to use in the cancelSnap function
    private List<Vector3> originalPositions = new List<Vector3>(new Vector3[5]);
    //how far the mouse should be for the section to snap when dragging stops
    [SerializeField]private float snapRange=100.01f;
    //limit of how many sections can be joined 
    [SerializeField]private int sectionLimit=2;
    // how many sections are joined currently
    private int sectionCount;
    //button to cancel snap 
    public GameObject cancelButton;
    
    void Start()
    {
        //connects the section carrier script and this script for each draggable object
        foreach(SectionCarrier draggable in draggableObjects)
        {
            draggable.dragEndedCallBack = OnDragEnded;
        }

        //Call the find function a little bit later to ensure everything is in place first
        Invoke("find",0.1f);
        sectionCount=0;
        currentSections.text = "";
        //button is set inactive while nothing is on it
        cancelButton.SetActive(false);

    }

    private void Update()
    {
        
    }

    //OnMouseUp->dragEndedCallBack->OnDragEnded
    //checks the distance between the snapper and the mouse when dragging ends
    private void OnDragEnded(SectionCarrier draggable)
    {
        float distance = Vector3.Distance(transform.position,Camera.main.ScreenToWorldPoint(Input.mousePosition));
        Debug.Log("snap:" + distance);

        //if the distance is smaller then than the snapRange and 
        //sectionCount does not exceed the sectionLimit,
        //the section is placed in the snapper
        if(distance <= snapRange && sectionLimit > sectionCount)
        {
            char sec = draggable.name[0];
            //to store sections currently in the snapper
            currentSections.text = currentSections.text + sec;
            sectionCount++;
            Debug.Log("in");
            draggable.transform.position = transform.position;
            //cancelButton is set active
            cancelButton.SetActive(true);
        }
    }

    //called when the cancelButton is clicked. Resets everything
    public void cancelSnap()
    {
        int index = 0;
        sectionCount=0;
        foreach(SectionCarrier draggable in draggableObjects)
        {
            draggable.transform.position = originalPositions[index];
            index++;
        }
        currentSections.text = "";
        cancelButton.SetActive(false);
    }

    //to find the originaş position of each section.
    private void find()   
    {
        int index = 0;
        foreach(SectionCarrier draggable in draggableObjects)
            {
                originalPositions[index] = draggable.transform.position;
                index++;
            }

    }
}
