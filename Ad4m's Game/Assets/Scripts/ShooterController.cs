using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShooterController : MonoBehaviour
{
    private GameObject[] eyesPresent;
    [SerializeField]private GameObject target;
    private System.Random rnd;

    [SerializeField]private float endTimer=30;
    [SerializeField]private Text counterText;
    // Start is called before the first frame update
    void Start()
    {   
        eyesPresent=GameObject.FindGameObjectsWithTag("Target");
        rnd = new System.Random();
    }

    private float enemyCountTimer = 0;
    
    // Update is called once per frame
    void Update()
    {
        if(enemyCountTimer>10f)
        {
            eyesPresent = GameObject.FindGameObjectsWithTag("Target");
            enemyCountTimer = 0;
        }

        else 
        {
            enemyCountTimer+= Time.deltaTime;
        }
        targetSpawn();

        if(endTimer<=0)
        {
            SelectedSections.gameWon=true;
            SceneManager.LoadScene("GameEndScreen");
        }
        else
        {
            counterText.text=((int)endTimer).ToString();
            endTimer-=Time.deltaTime;
        }
    }

    private float spawnTimer=0;
    private void targetSpawn()
    {
        if(spawnTimer>=3 && eyesPresent.Length<10)
        {
            GameObject temp = (GameObject)Instantiate(target,randomPoint(),quaternion.identity );
            temp.transform.rotation = temp.transform.rotation*Quaternion.Euler(0,180,0);
            spawnTimer=0;
        }
        else
        {
            spawnTimer+= Time.deltaTime;
        }
    }

    
    private Vector3 randomPoint()
    {
        float x = rnd.Next(-13,13);
        float y = rnd.Next(5,14);

        return new Vector3(x,y,22);
    }
}
