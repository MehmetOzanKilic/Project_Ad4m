using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedSections : MonoBehaviour
{
    public static int sections;
    public static bool isCardPresent;
    public static bool isHorrorPresent;
    public static bool isShooterPresent;
    public static bool isDodgerPresent;
    public static bool isPuzzlePresent;


    public static void reset()
    {
        sections=0;
        print(sections);
    }
}
