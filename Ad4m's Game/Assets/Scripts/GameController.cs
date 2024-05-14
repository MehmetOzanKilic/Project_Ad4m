using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField]private int levelNo=1;
    [SerializeField]private GameObject adam;
    [SerializeField]private Camera mainCam;
    [SerializeField]private SpawnerController spawner;

    private GameObject[] eyesPresent;
    private GameObject[] mobsPresent;
    private GameObject[] spawnerEyesPresent;
    private GameObject[] projectileEyesPresent;

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

    
    
    [SerializeField]private bool camPersFlag;

    
    private int tempLevel;
    void Start()
    {   
        levelNo=SelectedSections.returnCount();
        if(levelNo==0)
        {levelNo=3;}
        mainCam = Camera.main;
        camPersFlag=false;
        tempLevel = levelNo;
        controllCamera();
        horrorSection=SelectedSections.isHorrorPresent;
        shooterSection=SelectedSections.isShooterPresent;
        dodgerSection=SelectedSections.isDodgerPresent;
        puzzleSection=SelectedSections.isPuzzlePresent;
    }
    
    void Update()
    {
        if(prepareStage)
        {
            StateController.gamePhase = "PlayerTurn";
            SceneManager.LoadScene("Card Game");
        }
        eyesPresent = GameObject.FindGameObjectsWithTag("Eye");
        mobsPresent = GameObject.FindGameObjectsWithTag("Mobs");
        spawnerEyesPresent = GameObject.FindGameObjectsWithTag("SpawnerEye");
        projectileEyesPresent = GameObject.FindGameObjectsWithTag("ProjectileEye");

        if(spawn)
        {
            if(horrorSection)
            {
                horrorSpawn();
            }
            if(shooterSection)
            {
                shooterSpawn();
            }
        }

        //being able to switch levels in runtime for testing. Can be removed later.
        if(tempLevel!=levelNo)
        {
            tempLevel=levelNo;
            controllCamera();
        }
        
    }

    public GameObject sendAdam()
    {
        return adam;
    }

    float eyeTimer=0f;
    float spawnerEyeTimer=0f;
    float projectileEyeTimer=0f;
    //spawn mechanics for horror section
    private void horrorSpawn()
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
                eyeTimer=0.0f;
            }
            else eyeTimer+=Time.deltaTime;
        }
    }
    float mobTimer=0f;
    private void shooterSpawn()
    {
        if(horrorSection==false && dodgerSection==false)
        {
            if(mobsPresent.Length<10)
            {
                if(mobTimer>=mobSpawnTime)
                {
                    spawner.insMobs(dodgerSection);
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
                    mobTimer=0f;
                }
                else mobTimer+=Time.deltaTime;
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
}
