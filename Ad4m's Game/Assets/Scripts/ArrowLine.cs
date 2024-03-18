using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowLine : MonoBehaviour
{
    Transform arena;
    List<GameObject> arrows = new List<GameObject>();
    bool isCoroutineRunning = false;

    void Awake()
    {
        arena = GameObject.FindWithTag("Arena").transform;

        foreach (Transform childTransform in transform)
        {
            childTransform.gameObject.SetActive(false);
            arrows.Add(childTransform.gameObject);
        }

        arrows.Reverse();
    }

    void Start()
    {
        SetArrowLineDirection();
    }

    void Update()
    {
        int randomNumber = Random.Range(0, 2000);
        if (randomNumber == 54 && !isCoroutineRunning)
        {
            StartCoroutine(ActivateArrowLine());
        }
    }

    void SetArrowLineDirection()
    {
        float yRotation = Random.Range(0, 361);
        transform.Rotate(new Vector3(0f, yRotation, 0f));
    }

    IEnumerator ActivateArrowLine()
    {
        isCoroutineRunning = true;

        foreach (GameObject arrow in arrows)
        {
            arrow.SetActive(true);
            yield return new WaitForSeconds(1f);
        }

        Debug.Log("ArrowLine is active!!!");

        isCoroutineRunning = false;
    }
}
