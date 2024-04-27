using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private int[] seperatedSecs = new int[5];
    public void createGame()
    {
        print("create game");
        int final = SelectedSections.sections;
        int counter=0;
        

        while(final>10)
        {
            seperatedSecs[counter] = final%10;
            print(seperatedSecs[counter]);
            final /=10;
            if(final<10)
            {   
                counter++;
                seperatedSecs[counter] = final;
            }
            counter++;
        }

        print(seperatedSecs[0]);
        print(seperatedSecs[1]);
        print(seperatedSecs[2]);
        print(seperatedSecs[3]);
        print(seperatedSecs[4]);

        manageSectionSelection();
    }

    private void manageSectionSelection()
    {
        SelectedSections.isHorrorPresent = false;
        SelectedSections.isCardPresent = false;
        SelectedSections.isShooterPresent = false;
        SelectedSections.isDodgerPresent = false;
        SelectedSections.isPuzzlePresent = false;

        for(int i=0; i<5; i++)
        {
            if(seperatedSecs[i]==1)
            {
                SelectedSections.isHorrorPresent = true;
            }
            
            if(seperatedSecs[i]==2)
            {
                SelectedSections.isCardPresent = true;
            }

            if(seperatedSecs[i]==3)
            {
                SelectedSections.isShooterPresent = true;
            }

            if(seperatedSecs[i]==4)
            {
                SelectedSections.isDodgerPresent = true;
            }

            if(seperatedSecs[i]==5)
            {
                SelectedSections.isPuzzlePresent = true;
            }
        }

        print(SelectedSections.isHorrorPresent);
        print(SelectedSections.isCardPresent);
        print(SelectedSections.isShooterPresent);
        print(SelectedSections.isDodgerPresent);
        print(SelectedSections.isPuzzlePresent);

    }
}
