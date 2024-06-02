using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DodgerManager : MonoBehaviour
{
    [SerializeField] private float endTimer = 5;
    [SerializeField] private Text counterText;
    ProjectilePool projectilePool;
    PizzaSlice pizzaSlice;
    WreckingDiscoBall wreckingDiscoBall;

    void Awake()
    {
        projectilePool = FindObjectOfType<ProjectilePool>();
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

    void Update()
    {
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
}
