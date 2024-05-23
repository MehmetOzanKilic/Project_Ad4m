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
    public static bool isPuzzlePresent=true;


    public static void reset()
    {
        sections=0;
        print(sections);

        isCardPresent=false;
        isHorrorPresent=false;
        isShooterPresent=false;
        isPuzzlePresent=false;
        isDodgerPresent=false;

        print("is horror active: false");
        print("is card active: false");
        print("is shooter active: false");
        print("is dodger active: false");
        print("is puzzle active: false");
    
    }

    public static bool fightingStagePresent()
    {
        if(isHorrorPresent || isShooterPresent || isDodgerPresent)
        {
            return true;
        }
        else return false;
    }

    public static void printAllSections()
    {
        print(isHorrorPresent);
        print(isCardPresent);
        print(isShooterPresent);
        print(isDodgerPresent);
        print(isPuzzlePresent);
    }

    public static int returnCount()
    {
        int counter=0;

        if(isHorrorPresent) counter++;
        if(isCardPresent) counter++;
        if(isShooterPresent) counter++;
        if(isDodgerPresent) counter++;
        if(isPuzzlePresent) counter++;

        return counter;

    } 
}
