using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordsmanHealth : MonoBehaviour
{
    public float maxHealth; //health of the enemy
    public float health;
    public float bulletDamage;
    public float meleeDamage;
    private PlayerAttributeController playerAttributeController; 
    private GameObject playerObject;
    private GameObject gameController;
    private EnemyAttributeController enemyAttributeController;


    private void Awake()
    {
        playerObject = GameObject.FindWithTag("Player"); //sets the game object
        gameController = GameObject.FindWithTag("GameController"); //sets the game object
    }

    private void Start()
    {
        enemyAttributeController = gameController.GetComponent<EnemyAttributeController>();
        maxHealth = enemyAttributeController.gunmanHealth;
        health = maxHealth;
        Debug.Log(health);
        // Get the PlayerDamageController component from the player GameObject
        playerAttributeController = playerObject.GetComponent<PlayerAttributeController>();
        if (playerObject != null)
        {
            if (playerAttributeController != null)
            {
                // access playerbulletDamage and playermeleeDamage values from the PlayerDamageController 
                bulletDamage = playerAttributeController.playerBulletDamage;
                meleeDamage = playerAttributeController.playerMeleeDamage;
            }
            else
            {
                Debug.LogError("PlayerDamageController component not found on the player GameObject.");
            }
        }
        else
        {
            Debug.LogError("Player GameObject not found.");
        }
    }
    private void Update()
    {
        if (bulletDamage != playerAttributeController.playerBulletDamage)
        {
            bulletDamage = playerAttributeController.playerBulletDamage;
        }
        if (meleeDamage != playerAttributeController.playerMeleeDamage)
        {
            meleeDamage = playerAttributeController.playerMeleeDamage;
        }
        //check for new values of bullet damage for upgrades or debuffs while playing
        if (health < 0.1)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision) //checks for collision with bullets and deals appropriate damage. 
    {
        Transform hitTransform = collision.transform;
        if (hitTransform.CompareTag("Bullet"))
        {
            health -= bulletDamage;
            Debug.Log(bulletDamage);
            Debug.Log(health);
        }
        /*else if (hitTransform.CompareTag("anything else")
         * {
         *  whatever you want
         * }*/
    }

    
    


}
