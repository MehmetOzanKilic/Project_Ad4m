using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunmanHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float health;
    public float bulletDamage;
    public float meleeDamage;
    private PlayerAttributeController playerAttributeController;
    private GameObject playerObject;


    private void OnCollisionEnter(Collision collision)
    {
        Transform hitTransform = collision.transform;
        if (hitTransform.CompareTag("Bullet"))
        { 
            health = health - bulletDamage;
            Debug.Log(bulletDamage);
            Debug.Log(health);

        }

    }
    private void Awake()
    {
        playerObject = GameObject.Find("Ad4m");
    }

    private void Start()
    {
        
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
        bulletDamage = playerAttributeController.playerBulletDamage;
        meleeDamage = playerAttributeController.playerMeleeDamage;
        if (health < 1)
        {
            Destroy(gameObject);
        }
    }


}
