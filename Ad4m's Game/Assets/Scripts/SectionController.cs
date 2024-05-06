using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SectionController : MonoBehaviour
{
    private SnapController snap;
    bool[] sectionsPresent = new bool[5];
    // Start is called before the first frame update
    void Start()
    {
        Invoke("findSnap",0.4f);
        for(int i = 0;i<5;i++)
        {
            sectionsPresent[i]=false;
        }

        print("loop ended");
        
    }

    private void findSnap()
    {
        snap = GameObject.Find("Snap").GetComponent<SnapController>();
        if(snap!=null)
        {
            print("everything fine");
        }

    }

    [SerializeField]private bool canPrint=false;
    // Update is called once per frame
    void Update()
    { 
        //for debugging
        if(canPrint)
        {
            printSections();
            canPrint=false;
        }
    }
    
    private int[] seperatedSecs = new int[5];
    public void createGame()
    {
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


        sectionsPresent[0] = SelectedSections.isHorrorPresent;
        sectionsPresent[1] = SelectedSections.isCardPresent;
        sectionsPresent[2] = SelectedSections.isShooterPresent;
        sectionsPresent[3] = SelectedSections.isDodgerPresent;
        sectionsPresent[4] = SelectedSections.isPuzzlePresent;

    
        
        
        openGame();

    }


    //tüm senaryoları kontrol et çalışmıyor
    private void openGame()
    {
        int sectionCounter=0;

        foreach(bool temp in sectionsPresent)
        {
            if(temp)sectionCounter++;
        }
        print("opening game:" + sectionCounter);

        if(sectionCounter == 1)
        {
            //single section loader
            print("working");
            snap.returnSnap();
            printSections();

        }

        else if(sectionCounter > 1)
        {
            if(SelectedSections.isCardPresent)
            {
                SceneManager.LoadScene("Card Game");
                snap.returnSnap();
                printSections();
            }

            else if(SelectedSections.isHorrorPresent || SelectedSections.isShooterPresent || SelectedSections.isDodgerPresent)
            {
                SceneManager.LoadScene("Level1");
                snap.returnSnap();
                printSections();
            }

        }

        else
        {
            print("error");
            snap.cancelSnap();
            printSections();
        }

    }


    void printSections()
    {
        foreach(bool temp in sectionsPresent)
        {
            print(temp);
        }
    }

}
