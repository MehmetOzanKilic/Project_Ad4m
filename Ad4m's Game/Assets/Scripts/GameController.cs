using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]private GameObject adam;
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


    [SerializeField]private float eyeSpawnTime=2f;
    [SerializeField]private float mobSpawnTime=2f;

    
    
    
    void Start()
    {   


    }
    
    void Update()
    {
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
}
