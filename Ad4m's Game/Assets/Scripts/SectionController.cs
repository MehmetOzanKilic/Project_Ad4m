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
        if(SceneManager.GetActiveScene().ToString() == "The Computer")
        {
            snap = GameObject.Find("Snap").GetComponent<SnapController>();
        }
        
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
    public void openGame()
    {
        int sectionCounter=0;

        foreach(bool temp in sectionsPresent)
        {
            if(temp)sectionCounter++;
        }
        print("opening game level:" + sectionCounter);

        if(sectionCounter == 1)
        {
            printSections();
            if(SelectedSections.isHorrorPresent)
            {
                SceneManager.LoadScene("HorrorLevel");
            }

            if(SelectedSections.isCardPresent)
            {
                SceneManager.LoadScene("Card Game");
            }

            if(SelectedSections.isDodgerPresent)
            {
                SceneManager.LoadScene("DHS");
            }

            if(SelectedSections.isShooterPresent)
            {
                SceneManager.LoadScene("ShooterLevel");
            }

            if(SelectedSections.isPuzzlePresent)
            {
                SceneManager.LoadScene("PuzzleLevel");
            }

        }

        else if(sectionCounter > 1)
        {
            if(SelectedSections.isCardPresent)
            {
                //snap.returnSnap();
                printSections();
                Invoke("loadCard",0.5f);
            }

            else if(SelectedSections.isHorrorPresent || SelectedSections.isShooterPresent || SelectedSections.isDodgerPresent)
            {
                //snap.returnSnap();
                printSections();
                string levelString = "loadLevel" + sectionCounter.ToString();
                Invoke(levelString,0.5f);
            }

        }

        else
        {
            print("error");
            snap.cancelSnap();
            printSections();
        }

    }

    public void openGameEnd()
    {
        int sectionCounter = SelectedSections.returnCount();
        print("opening game level:" + sectionCounter);

        if(sectionCounter == 1)
        {
            if(SelectedSections.isHorrorPresent)
            {
                SceneManager.LoadScene("HorrorLevel");
            }

            if(SelectedSections.isCardPresent)
            {
                SceneManager.LoadScene("Card Game");
            }

            if(SelectedSections.isDodgerPresent)
            {
                SceneManager.LoadScene("DHS");
            }

            if(SelectedSections.isShooterPresent)
            {
                SceneManager.LoadScene("ShooterLevel");
            }

            if(SelectedSections.isPuzzlePresent)
            {
                SceneManager.LoadScene("PuzzleLevel");
            }
            printSections();

        }

        else if(sectionCounter > 1)
        {
            if(SelectedSections.isCardPresent)
            {
                //snap.returnSnap();
                printSections();
                Invoke("loadCard",0.5f);
            }

            else if(SelectedSections.isHorrorPresent || SelectedSections.isShooterPresent || SelectedSections.isDodgerPresent)
            {
                //snap.returnSnap();
                printSections();
                string levelString = "loadLevel" + sectionCounter.ToString();
                Invoke(levelString,0.5f);
            }

        }

        else
        {
            print("error");
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
    void loadLevel2()
    {
        SceneManager.LoadScene("Level2");
    }

    void loadLevel3()
    {
        SceneManager.LoadScene("Level3");
    }

    void loadLevel4()
    {
        SceneManager.LoadScene("Level4");
    }

    void loadLevel5()
    {
        SceneManager.LoadScene("Level5");
    }


    void loadCard()
    {
        SceneManager.LoadScene("Card Game");
    }
}
