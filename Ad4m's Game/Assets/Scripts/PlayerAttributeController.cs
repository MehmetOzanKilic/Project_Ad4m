//this is for all upgrades, attributes of the player. even upgrades will be kept here then will be used from the scripts using getcomponent.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributeController : MonoBehaviour
{
    public float playerBulletDamage = 80f;
    public float reloadSpeed = 1f;
    public float bulletSpeed = 10f;
    public int shotAmount = 3;
    public bool isMachineGun = false;

    public float playerMeleeDamage = 40f;

    public float timeSlowMultiplier = 1f;

    public float seeAngle = 30f;

    public float meleeAttackCooldown = 1f;

    public float moveSpeed = 5f;

    public float dashSpeed = 30f;
    public float dashTime = 0.1f;
    public float dashCoolDown = 1f;

    public float playerHealth = 100f;
    public bool canBurstHeal = false;
    public bool canSacrifice = false;
    private int sacrificeCount = 3;

    public int[] Upgrades = { -1, -1 };

    private GameObject playerObject;
    private PlayerHealth playerHealthScript;



    private EnemyAttributeController enemyAttributeController;
    private GameObject gameController;
    //upgrades
    private void Awake()
    {
        gameController = GameObject.FindWithTag("GameController"); //sets the game object
        playerObject = GameObject.FindWithTag("Player"); //sets the game object
    }
    private void Start()
    {
        enemyAttributeController = gameController.GetComponent<EnemyAttributeController>();
        playerHealthScript = playerObject.GetComponent<PlayerHealth>();
        UpdateUpgrades();
    }
    private void Update()
    {
        //UpdateUpgrades();
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (canSacrifice)
            {
                if (sacrificeCount > 0)
                {
                    DestroyAllMobs();
                    sacrificeCount--;
                    playerHealthScript.TakeDamage(70f);
                }
            }
        }
    }
    void DestroyAllMobs()
    {
        GameObject[] mobs = GameObject.FindGameObjectsWithTag("Mobs");
        foreach (GameObject mob in mobs)
        {
            Destroy(mob);
        }
    }
    void RemoveDuplicateUpgrades()
    {
        for (int i = 0; i < Upgrades.Length; i++)
        {
            for (int j = i + 1; j < Upgrades.Length; j++)
            {
                if (Upgrades[i] == Upgrades[j] && Upgrades[j] != -1)
                {
                    Upgrades[j] = -1;
                }
            }
        }
    }
    public void UpdateUpgrades()
    {
        ResetAttributes();

        Upgrades[0] = SelectedUpgrades.selectedUpgrades[0];
        Upgrades[1] = SelectedUpgrades.selectedUpgrades[1];
        RemoveDuplicateUpgrades();
        //Upgrade IDs: 0- Direct Damage, 1- Healing, 2- AoE Damage, 3-Buff, 4-Debuff, 5- Sacrifice 6-Counterattack
        for (int i = 0; i < Upgrades.Length; i++)
        {
            switch (Upgrades[i])
            {
                case 0:

                    Debug.Log("Direct Damage");
                    playerBulletDamage = 50f;
                    playerMeleeDamage = 100f;
                    break;

                case 1:
                    Debug.Log("Healing");
                    canBurstHeal = true;
                    break;

                case 2:
                    Debug.Log("AoE Damage");
                    reloadSpeed = 0.5f;
                    shotAmount = 20;
                    isMachineGun = true;
                    break;

                case 3:
                    Debug.Log("Buff");
                    playerHealth = 150f;
                    break;

                case 4:
                    Debug.Log("Debuff");
                    enemyAttributeController.gunmanHealth = 75f;
                    enemyAttributeController.swordsmanHealth = 60f;
                    break;

                case 5:
                    Debug.Log("Sacrifice");
                    canSacrifice = true; 
                    break;

                case 6:
                    moveSpeed=7.5f;
                    break;

                default:
                    Debug.Log("No Upgrade selected for slot " + i);
                    break;
            }
        }
    }
    public void ResetAttributes()
    {
        playerBulletDamage = 80f;
        reloadSpeed = 1f;
        bulletSpeed = 10f;
        shotAmount = 3;
        isMachineGun = false;
        playerMeleeDamage = 40f;
        timeSlowMultiplier = 1f;
        seeAngle = 30f;
        meleeAttackCooldown = 1f;
        moveSpeed = 5f;
        dashSpeed = 30f;
        dashTime = 0.1f;
        dashCoolDown = 1f;
        playerHealth = 100f;
        canBurstHeal = false;
        canSacrifice = false;
        enemyAttributeController.gunmanHealth = 100f;
        enemyAttributeController.swordsmanHealth = 80f;
}
}

