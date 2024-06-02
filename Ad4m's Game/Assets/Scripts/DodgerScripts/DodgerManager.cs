using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgerManager : MonoBehaviour
{
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
}
