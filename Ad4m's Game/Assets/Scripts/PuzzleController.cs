using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PuzzleController : MonoBehaviour
{
    [SerializeField]private int levelNo;
    [SerializeField]private GameObject adam;
    [SerializeField]private Camera mainCam;
    [SerializeField]private SpawnerController spawner;

    private GameObject[] eyesPresent;
    private GameObject[] mobsPresent;
    private GameObject[] spawnerEyesPresent;
    private GameObject[] projectileEyesPresent;
    private GameObject[] projectileShootersPresent;

    //SerilizeFields will be removed later.
    [SerializeField]private bool spawn=true;
    [SerializeField]private bool horrorSection;
    [SerializeField]private bool shooterSection;
    [SerializeField]private bool dodgerSection;
    [SerializeField]private bool puzzleSection;
    [SerializeField]private bool prepareStage=false;
    [SerializeField]private GameObject[] vCams; 


    [SerializeField]private float eyeSpawnTime=2f;
    [SerializeField]private float mobSpawnTime=2f;
    [SerializeField]private float projectileShooterTime=1f;

    
    
    [SerializeField]private bool camPersFlag;
    [SerializeField]private float levelEndTimer=100f;
    [SerializeField]private Text waveTimeText;
    private bool endTimerFlag=true;
    
    private int tempLevel;
    void Start()
    {   
        levelNo=2;
        mainCam = Camera.main;
    }
    
    private float enemyCountTimer=0f;
    void Update()
    {

        if(endTimerFlag)
        {
            levelEndTimer -= Time.deltaTime;
            waveTimeText.text=((int)levelEndTimer).ToString();

            if(levelEndTimer <= 0)
            {

                //more waves or end level
                print("wave end");
                SelectedSections.gameWon=false;
                SceneManager.LoadScene("GameEndScreen");
            
            }
        }

    }

    public GameObject SendAdam()
    {
        return adam;
    }

    private void setCamsFalse()
    {
        foreach(var v in vCams)
        {
            v.SetActive(false);
        }
    }

    public void puzzleFinished()
    {
        //more waves or end level
        print("wave end");
        SelectedSections.gameWon=true;
        SceneManager.LoadScene("GameEndScreen");
    }
}
