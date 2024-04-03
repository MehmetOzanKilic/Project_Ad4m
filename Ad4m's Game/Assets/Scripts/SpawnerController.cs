using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;


public class SpawnerController : MonoBehaviour
{
    private GameController gameController;
    private GameObject adam;
    [SerializeField]private GameObject whiteEye;
    [SerializeField]private GameObject spawnerEye;
    [SerializeField]private GameObject redEye;
    [SerializeField]private GameObject projectileEye;
    [SerializeField]private GameObject yellowEye;
    [SerializeField]private GameObject gunMan;
    [SerializeField]private GameObject swordMan; 
    [SerializeField]private int eyesAllowed=5;
    [SerializeField]private int[] eyeProb;
    [SerializeField]private int[] mobProb;

    private System.Random rnd;
    private GameObject[]  eyesPresent;
    private GameObject[]  mobsPresent;

    [SerializeField]private float eyeSpawnTime=2f;
    [SerializeField]private float mobSpawnTime=2f;
    private float eyeSpawnTimer;
    private float mobSpawnTimer;
    private bool puzzlePresent=false;
    public bool horrorPresent=false;
    public bool shootingPresent=false;
    public bool dodgerPresent=false;


    private void Awake()
    {
        adam = GameObject.Find("Ad4m"); //sets the Player.
    }

    void Start()
    {
        rnd = new System.Random();
        eyeSpawnTimer = 0f;
        mobSpawnTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {   

    }

    private Vector3 randomPos()
    {
        float distance = rnd.Next(12,16);
        float angle = rnd.Next(0,360) * Mathf.Deg2Rad;

        return new Vector3(Mathf.Cos(angle) * distance, 0.5f, Mathf.Sin(angle) * distance)+adam.transform.position;
    }
    private Vector3 randomPos(int a)
    {
        float distance = rnd.Next(12,16)*a;
        float angle = rnd.Next(0,360) * Mathf.Deg2Rad;

        return new Vector3(Mathf.Cos(angle) * distance, 0.5f, Mathf.Sin(angle) * distance)+adam.transform.position;
    }
    private Vector3 randomPos(Vector3 pos)
    {
        float distance = rnd.Next(1,2);
        float angle = rnd.Next(0,360) * Mathf.Deg2Rad;

        return new Vector3(Mathf.Cos(angle) * distance, 0.5f, Mathf.Sin(angle) * distance)+pos;
    }
    public void insEyes()
    {
        var random = rnd.Next(0,eyeProb[1]);

        if(0<=random && random<=eyeProb[0])
        {
            insWhiteEye();
        }
        else if(eyeProb[0]<random && random<=eyeProb[1])
        {
            insRedEye();
        }
        else
            Debug.Log("sıkıntı");
    }
    public void insMobs(bool temp)
    {   
        int upperLimit;

        if(!temp) upperLimit=mobProb[0];
        else upperLimit=mobProb[1];

        var random = rnd.Next(0,upperLimit);

        if(0<=random && random<=mobProb[0])
        {
            insSwordMan();
        }
        else if(mobProb[0]<random && random<=mobProb[1])
        {
            insGunMan();
        }
    }
    public void insMobs(bool temp,Vector3 pos)
    {   
        int upperLimit;

        if(!temp) upperLimit=mobProb[0];
        else upperLimit=mobProb[1];

        var random = rnd.Next(0,upperLimit);

        if(0<=random && random<=mobProb[0])
        {
            insSwordMan(pos);
        }
        else if(mobProb[0]<random && random<=mobProb[1])
        {
            insGunMan(pos);
        }
    }
    public void insWhiteEye()
    {
        var temp=(GameObject)Instantiate(whiteEye,randomPos(),Quaternion.identity);
    }
    public void insRedEye()
    {
        var temp=(GameObject)Instantiate(redEye,randomPos(),Quaternion.identity);
    }
    public void insYellowEye()
    {
        var temp=(GameObject)Instantiate(yellowEye,randomPos(),Quaternion.identity);
        temp.GetComponent<RedEyeController>().getAdam(adam);
    }

    public void insGunMan()
    {
        Instantiate(gunMan,randomPos(),Quaternion.identity);
    }
     public void insGunMan(Vector3 pos)
    {
        Instantiate(gunMan,randomPos(pos),Quaternion.identity);
    }

    public void insSwordMan()
    {
        Instantiate(swordMan,randomPos(),Quaternion.identity);
    }
    public void insSwordMan(Vector3 pos)
    {
        Instantiate(swordMan,randomPos(pos),Quaternion.identity);
    }
    public void     insSpawnerEye()
    {
        Instantiate(spawnerEye,randomPos(),Quaternion.identity);
    }
    public void insProjectileEye()
    {
        Instantiate(projectileEye,randomPos(),Quaternion.identity);
    }
}
