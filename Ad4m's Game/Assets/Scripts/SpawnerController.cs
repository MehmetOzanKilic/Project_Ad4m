using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    private GameController gameController;
    [SerializeField]private GameObject adam;
    [SerializeField]private GameObject whiteEye;
    [SerializeField]private GameObject redEye;
    [SerializeField]private GameObject yellowEye;

    [SerializeField]private int eyesAllowed=5;
    [SerializeField]private int[] eyeProb;

    private System.Random rnd;
    private GameObject[]  eyesPresent;
    // Start is called before the first frame update
    void Start()
    {
        rnd = new System.Random();
    }

    // Update is called once per frame
    void Update()
    {   
        eyesPresent = GameObject.FindGameObjectsWithTag("Eye");
        
        if(eyesPresent.Length < eyesAllowed)
        {
            var random = rnd.Next(0,100);

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
        float distance = rnd.Next(5,7);
        float angle = rnd.Next(0,360) * Mathf.Deg2Rad;

        return new Vector3(Mathf.Cos(angle) * distance, 0.5f, Mathf.Sin(angle) * distance);
    }
}
