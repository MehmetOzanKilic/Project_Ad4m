using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DodgerManager : MonoBehaviour
{
    [SerializeField] private float endTimer = 5;
    [SerializeField] private Text counterText;
    [SerializeField]private GameObject escCanvas;
    ProjectilePool projectilePool;
    PizzaSlice pizzaSlice;
    WreckingDiscoBall wreckingDiscoBall;

    void Awake()
    {
        projectilePool = FindObjectOfType<ProjectilePool>();
        escCanvas.SetActive(false);
        //pizzaSlice = FindObjectOfType<PizzaSlice>();
        //wreckingDiscoBall = FindObjectOfType<WreckingDiscoBall>();
    }

    void Start()
    {
        projectilePool.InitializePool();
        StartCoroutine(projectilePool.ActivateProjectilesPeriodically());
        //wreckingDiscoBall.PrepareDiscoBall();
        //StartCoroutine(pizzaSlice.TakeASlice());
    }
    private bool escFlag = false;
    
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
}
