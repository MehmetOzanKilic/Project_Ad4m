using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
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
    private bool endTimerFlag=false;
    
    private int tempLevel;
    void Start()
    {   
        //levelEndTimer=500f;
        levelNo=SelectedSections.returnCount();
        if(levelNo==0)
        {levelNo=2;}
        mainCam = Camera.main;
        camPersFlag=false;
        tempLevel = levelNo;
        controllCamera();
        horrorSection=SelectedSections.isHorrorPresent;
        shooterSection=SelectedSections.isShooterPresent;
        dodgerSection=SelectedSections.isDodgerPresent;
        puzzleSection=SelectedSections.isPuzzlePresent;

        if(!puzzleSection)
        {
            GameObject.Find("Grid").SetActive(false);
            GameObject.Find("PuzzlePiecePool").SetActive(false);
            GameObject.Find("Ad4m").GetComponent<PuzzlePiece>().enabled=false;
            endTimerFlag=true;
            print("endTimerFlag: " + endTimerFlag);
        }
        else
        {
            waveTimeText.enabled=false;
        }

        eyesPresent = GameObject.FindGameObjectsWithTag("Eye");
        mobsPresent = GameObject.FindGameObjectsWithTag("Mobs");
        spawnerEyesPresent = GameObject.FindGameObjectsWithTag("SpawnerEye");
        projectileEyesPresent = GameObject.FindGameObjectsWithTag("ProjectileEye");
        projectileShootersPresent = GameObject.FindGameObjectsWithTag("ProjectileShooter");
    }
    
    private float enemyCountTimer=0f;
    void Update()
    {
        if(prepareStage)
        {
            StateController.gamePhase = "PlayerTurn";
            SceneManager.LoadScene("Card Game");
        }


        if(spawn)
        {
            if(horrorSection)
            {
                HorrorSpawn();
            }
            if(shooterSection)
            {
                ShooterSpawn();
            }
            if(dodgerSection)
            {
                DodgerSpawn();
            }
        }

        //being able to switch levels in runtime for testing. Can be removed later.
        if(tempLevel!=levelNo)
        {
            tempLevel=levelNo;
            controllCamera();
        }
        
        if(endTimerFlag)
        {
            levelEndTimer -= Time.deltaTime;
            waveTimeText.text=((int)levelEndTimer).ToString();

            if(levelEndTimer <= 0)
            {
                Debug.LogError("wave ended");
                if(SelectedSections.isCardPresent)
                {
                    StateController.gamePhase = "PlayerTurn";
                    SceneManager.LoadScene("Card Game");
                }
                else
                {
                    //more waves or end level
                    print("wave end");
                    SelectedSections.gameWon=true;
                    SceneManager.LoadScene("GameEndScreen");
                }
            }
        }

        if(enemyCountTimer>10f)
        {
            eyesPresent = GameObject.FindGameObjectsWithTag("Eye");
            mobsPresent = GameObject.FindGameObjectsWithTag("Mobs");
            spawnerEyesPresent = GameObject.FindGameObjectsWithTag("SpawnerEye");
            projectileEyesPresent = GameObject.FindGameObjectsWithTag("ProjectileEye");
            projectileShootersPresent = GameObject.FindGameObjectsWithTag("ProjectileShooter");
            enemyCountTimer = 0;
        }

        else 
        {
            enemyCountTimer+= Time.deltaTime;
        }
    }

    public GameObject SendAdam()
    {
        return adam;
    }

    float eyeTimer=0f;
    float spawnerEyeTimer=0f;
    float projectileEyeTimer=0f;
    //spawn mechanics for horror section
    private void HorrorSpawn()
    {
        //if shooter section is present spawner eyes are instantiated
        if(shooterSection)
        {   
            //only one spawnerEye is allowed
            if(spawnerEyesPresent.Length==0)
            {
                //spawn time for spawnerEyes are 3 times the normal amount
                if(spawnerEyeTimer>=eyeSpawnTime*3)
                {
                    spawner.insSpawnerEye();
                    spawnerEyesPresent = GameObject.FindGameObjectsWithTag("SpawnerEye");
                    spawnerEyeTimer=0f;
                }
                else spawnerEyeTimer+=Time.deltaTime;
            }

            else if(spawnerEyesPresent.Length!=0)
            {   
                if(mobsPresent.Length<10)
                {
                    if(mobTimer>=mobSpawnTime)
                    {
                        spawner.insMobs(dodgerSection,GameObject.FindGameObjectWithTag("SpawnerEye").transform.position);
                        mobsPresent = GameObject.FindGameObjectsWithTag("Mobs");
                        mobTimer=0.0f;
                    }
                    else mobTimer+=Time.deltaTime;
                }
            }
            //if dodger section is not present regular red eyes are also instatiated
            if(!dodgerSection)
            {
                if(eyesPresent.Length<4)
                {
                    if(eyeTimer>=eyeSpawnTime)
                    {
                        spawner.insRedEye();
                        eyesPresent = GameObject.FindGameObjectsWithTag("Eye");
                        eyeTimer=0f;
                    }
                    else eyeTimer+=Time.deltaTime;
                }

            }
        }
        //if dodger section is present spawner eyes are instantiated
        if(dodgerSection)
        {
            //only max 2 projectileEyes are allowed.
            if(projectileEyesPresent.Length<4)
            {   
                //spawn time for spawnerEyes are 3 times the normal amount
                if(projectileEyeTimer>=eyeSpawnTime*2)
                {
                    spawner.insProjectileEye();
                    projectileEyesPresent = GameObject.FindGameObjectsWithTag("ProjectileEye");
                    projectileEyeTimer=0f;
                }
                else projectileEyeTimer+=Time.deltaTime;
            }

            //if shooter section is not present regular red eyes are also instatiated
            if(!shooterSection)
            {
                if(eyesPresent.Length<4)
                {
                    if(eyeTimer>=eyeSpawnTime)
                    {
                        spawner.insWhiteEye();
                        eyesPresent = GameObject.FindGameObjectsWithTag("Eye");
                        eyeTimer=0f;
                    }
                    else eyeTimer+=Time.deltaTime;
                }

            }
        }
        //if both shooter and dodger are not present.
        if(!shooterSection && !dodgerSection)
        {
            if(eyesPresent.Length<7 && eyeTimer>=eyeSpawnTime)
            {
                spawner.insEyes();
                eyesPresent = GameObject.FindGameObjectsWithTag("Eye");
                eyeTimer=0.0f;
            }
            else eyeTimer+=Time.deltaTime;
        }
    }
    float mobTimer=0f;
    private void ShooterSpawn()
    {
        if(horrorSection==false && dodgerSection==false)
        {
            if(mobsPresent.Length<10)
            {
                if(mobTimer>=mobSpawnTime)
                {
                    spawner.insMobs(dodgerSection);
                    mobsPresent = GameObject.FindGameObjectsWithTag("Mobs");
                    mobTimer=0.0f;
                }
                else mobTimer+=Time.deltaTime;
            }
        }
        if(horrorSection==false && dodgerSection==true)
        {
            if(mobsPresent.Length<10)
            {
                if(mobTimer>=mobSpawnTime)
                {
                    spawner.insMobs(dodgerSection);
                    mobsPresent = GameObject.FindGameObjectsWithTag("Mobs");
                    mobTimer=0f;
                }
                else mobTimer+=Time.deltaTime;
            }
        }
    }

    private float projectileShooterTimer=0f;
    private void DodgerSpawn()
    {
        if(!horrorSection && !shooterSection)
        {
            if(projectileShootersPresent.Length<10)
            {
                if(projectileShooterTimer>=projectileShooterTime)
                {
                    spawner.insProjectileShooter();
                    projectileShootersPresent = GameObject.FindGameObjectsWithTag("ProjectileShooter");
                    projectileShooterTimer=0.0f;
                }
                else projectileShooterTimer+=Time.deltaTime;
            }
        }

        if(!horrorSection && shooterSection)
        {
            if(projectileShootersPresent.Length<10)
            {
                if(projectileShooterTimer>=projectileShooterTime*3)
                {
                    spawner.insProjectileShooter();
                    projectileShootersPresent = GameObject.FindGameObjectsWithTag("ProjectileShooter");
                    projectileShooterTimer=0.0f;
                }
                else projectileShooterTimer+=Time.deltaTime;
            }
        }
    }

    //sets every levels spesific parameters and cams
    private void controllCamera()
    {
        setCamsFalse();
        switch(levelNo)
        {
            case 1:
            vCams[0].SetActive(true);
            mainCam.orthographic = true;
            adam.GetComponent<MovementController>().enabled=true;
            adam.GetComponent<MovementController3D>().enabled=false;
            adam.GetComponent<DashController2D>().enabled=true;
            adam.GetComponent<DashController3D>().enabled=false;
            break;
            
            case 2:
            vCams[1].SetActive(true);
            mainCam.orthographic = true;
            adam.GetComponent<MovementController>().enabled=true;
            adam.GetComponent<MovementController3D>().enabled=false;
            adam.GetComponent<DashController2D>().enabled=true;
            adam.GetComponent<DashController3D>().enabled=false;
            break;

            case 3:
            vCams[2].SetActive(true);
            mainCam.orthographic = true;
            adam.GetComponent<MovementController>().enabled=true;
            adam.GetComponent<MovementController3D>().enabled=false;
            adam.GetComponent<DashController2D>().enabled=true;
            adam.GetComponent<DashController3D>().enabled=false;
            break;

            case 4:
            vCams[3].SetActive(true);
            mainCam.orthographic = false;
            adam.GetComponent<MovementController>().enabled=false;
            adam.GetComponent<MovementController3D>().enabled=true;
            adam.GetComponent<DashController2D>().enabled=false;
            adam.GetComponent<DashController3D>().enabled=true;
            break;

            case 5:
            vCams[4].SetActive(true);
            mainCam.orthographic = false;
            adam.GetComponent<MovementController>().enabled=false;
            adam.GetComponent<MovementController3D>().enabled=true;
            adam.GetComponent<DashController2D>().enabled=false;
            adam.GetComponent<DashController3D>().enabled=true;
            break;
        }
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
        Debug.LogError("puzzle complete");
        if(SelectedSections.isCardPresent)
        {
            StateController.gamePhase = "PlayerTurn";
            SceneManager.LoadScene("Card Game");
        }
        else
        {
            //more waves or end level
            print("wave end");
            SelectedSections.gameWon=true;
            SceneManager.LoadScene("GameEndScreen");
        }
    }
}
