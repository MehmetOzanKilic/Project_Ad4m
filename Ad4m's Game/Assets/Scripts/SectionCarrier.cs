using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionCarrier : MonoBehaviour
{
    [SerializeField]private Vector3 offset;
    private Vector3 startPos;

    private bool dragging=false;

    private bool inPlace=false;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("find",0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        if(dragging)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
        }
    }

    void OnMouseDown()
    {
        Debug.Log("pressed");
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragging = true;
    }

    void OnMouseUp()
    {
        Debug.Log("let go");
        dragging = false;
        if(!inPlace)
        {
            transform.position = startPos;
        }
    }

    private void find()
    {
        startPos= transform.position;
        Debug.Log(startPos);
    }
}
