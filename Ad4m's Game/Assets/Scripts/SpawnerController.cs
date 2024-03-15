using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class SpawnerController : MonoBehaviour
{
    private GameController gameController;
    private GameObject adam;
    [SerializeField]private GameObject whiteEye;
    [SerializeField]private GameObject redEye;
    [SerializeField]private GameObject yellowEye;

    [SerializeField]private int eyesAllowed=5;
    [SerializeField]private int[] eyeProb;

    private System.Random rnd;
    private GameObject[]  eyesPresent;

    [SerializeField]private float spawnTime=2f;
    private float spawnTimer;
    private bool puzzlePresent=false;

    private void Awake()
    {
        adam = GameObject.Find("Ad4m"); //sets the Player.
    }

    void Start()
    {
        rnd = new System.Random();
        spawnTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {   
        spawnTimer+=Time.deltaTime;
        eyesPresent = GameObject.FindGameObjectsWithTag("Eye");
        
        insEyes();  
    }

    private void insWhiteEye()
    {
        var tempAdam=(GameObject)Instantiate(whiteEye,randomPos(),Quaternion.identity);
        tempAdam.GetComponent<WhiteEyeController>().getAdam(adam);
    }
    private void insRedEye()
    {
        var tempAdam=(GameObject)Instantiate(redEye,randomPos(),Quaternion.identity);
        tempAdam.GetComponent<RedEyeController>().getAdam(adam);
    }
    private void insYellowEye()
    {
        var tempAdam=(GameObject)Instantiate(yellowEye,randomPos(),Quaternion.identity);
        tempAdam.GetComponent<YellowEyeController>().getAdam(adam);
    }
    private Vector3 randomPos()
    {
        float distance = rnd.Next(7,10);
        float angle = rnd.Next(0,360) * Mathf.Deg2Rad;

        return new Vector3(Mathf.Cos(angle) * distance, 0.5f, Mathf.Sin(angle) * distance)+adam.transform.position;
    }
    private void insEyes()
    {
        if(!puzzlePresent)
        {
            if(eyesPresent.Length < eyesAllowed && spawnTimer>spawnTime)
            {
                spawnTimer = 0f;
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
        }

        if(puzzlePresent)
        {
            if(eyesPresent.Length < 1/*eyesAllowed*/ && spawnTimer>spawnTime)
            {
                spawnTimer = 0f;
                var random = rnd.Next(0,eyeProb[2]);
                insRedEye();

                if(0<=random && random<=eyeProb[0])
                {
                    insWhiteEye();
                }
                else if(eyeProb[0]<random && random<=eyeProb[1])
                {
                    insRedEye();
                }
                else if(eyeProb[1]<random && random<=eyeProb[2])
                {
                    insYellowEye();
                }
                else
                    Debug.Log("sıkıntı");

            }
        }
    }
}
