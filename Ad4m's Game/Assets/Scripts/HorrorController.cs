using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HorrorController : MonoBehaviour
{
    private GameObject[] eyesPresent;
    [SerializeField]private GameObject eye;
    private System.Random rnd;

    [SerializeField]private float endTimer=30;
    [SerializeField]private Text counterText;
    [SerializeField]private Text eyeFoundText;
    private int eyeFoundCounter=0;
    [SerializeField]private GameObject escCanvas;
    // Start is called before the first frame update
    void Start()
    {   
        eyeFoundText.text = "0";
        eyesPresent=GameObject.FindGameObjectsWithTag("Eye");
        rnd = new System.Random();
        escCanvas.SetActive(false);
    }

    private float enemyCountTimer = 0;
    private bool escFlag = false;
    // Update is called once per frame
    void Update()
    {
         if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(escFlag==false)
            {
                escCanvas.SetActive(true);
                escFlag=true;
                Time.timeScale=0;
                Cursor.lockState = CursorLockMode.None;
            }

            else if(escFlag==true)
            {
                escCanvas.SetActive(false);
                escFlag=false;
                Time.timeScale=1;
                if(SelectedSections.returnCount()>3)Cursor.lockState = CursorLockMode.Locked;
            }
            
        }
        if(enemyCountTimer>10f)
        {
            eyesPresent = GameObject.FindGameObjectsWithTag("Eye");
            enemyCountTimer = 0;
        }

        else 
        {
            enemyCountTimer+= Time.deltaTime;
        }
        eyeSpawn();

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
    private void eyeSpawn()
    {
        if(spawnTimer>=1.5f && eyesPresent.Length<10)
        {
            Instantiate(eye,randomPoint(),quaternion.identity );
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

    public void retryLevel()
    {
        Time.timeScale=1;
        print("clickclick");
        SelectedSections.gameWon=false;
        gameObject.GetComponent<SectionController>().openGameEnd(); 
    }

    public void returnToComputer()
    {
        Time.timeScale=1;
        SelectedSections.gameWon=false;
        SceneManager.LoadScene("The Computer");
    }

    public void eyeFound()
    {
        eyeFoundCounter+=1;
        eyeFoundText.text=eyeFoundCounter.ToString();
    }
}
