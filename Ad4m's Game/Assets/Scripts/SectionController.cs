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

        //("loop ended");
        
    }

    private void findSnap()
    {
        snap = GameObject.Find("Snap").GetComponent<SnapController>();
        if(snap!=null)
        {
            //print("everything fine");
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
        print("opening game level:" + sectionCounter);

        if(sectionCounter == 1)
        {
            //single section loader
            snap.returnSnap();
            printSections();

        }

        else if(sectionCounter > 1)
        {
            if(SelectedSections.isCardPresent)
            {
                //snap.returnSnap();
                printSections();
                Debug.LogError("halt");
                Invoke("loadCard",0.5f);
            }

            else if(SelectedSections.isHorrorPresent || SelectedSections.isShooterPresent || SelectedSections.isDodgerPresent)
            {
                //snap.returnSnap();
                printSections();
                Debug.LogError("halt");
                Invoke("loadLevel1",0.5f);
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
        print("is horror active: " + sectionsPresent[0]);
        print("is card active: " + sectionsPresent[1]);
        print("is shooter active: " + sectionsPresent[2]);
        print("is dodger active: " + sectionsPresent[3]);
        print("is puzzle active: " + sectionsPresent[4]);
    }

    void loadLevel1()
    {
        SceneManager.LoadScene("Level1");
    }

    void loadCard()
    {
        SceneManager.LoadScene("Card Game");
    }
}
