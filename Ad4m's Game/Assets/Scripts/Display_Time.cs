using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class Display_Time : MonoBehaviour
{
    public TextMeshProUGUI TMP;

    void Start()
    {
        UpdateDateTime();
    }

    
    void Update()
    {
        UpdateDateTime();
    }

    void UpdateDateTime()
    {
        DateTime currentDateTime = DateTime.Now;

        string dateTimeString = currentDateTime.ToString("HH:mm:ss  dd/MM/yyyy");

        TMP.text = dateTimeString;
    }
}
