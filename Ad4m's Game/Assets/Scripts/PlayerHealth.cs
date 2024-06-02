using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    // Start is called before the first frame update
    private float lerpTimer;
    public float chipSpeed = 1f;
    private float health;
    public float maxHealth; //max health of player, can be changed with upgrades
    public Image frontHealthBar;
    public Image backHealthBar;
    private PlayerAttributeController playerAttributeController;
    private GameObject playerObject;
    private float burstHealCount =3;

    private float timerInterval = 2f; // Interval in seconds

    private void Awake()
    {
        playerObject = GameObject.FindWithTag("Player"); //sets the game object
    }
    void Start()
    {
        playerAttributeController = playerObject.GetComponent<PlayerAttributeController>();
        maxHealth = playerAttributeController.playerHealth;
        health = maxHealth; //sets health to maxhealth

        StartCoroutine(StartTimer());
    }

    // Update is called once per frame
    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth); //makes sure health never exceeds 0 or maxhealth
        UpdateHealthUI();
        
        if (Input.GetKeyDown(KeyCode.H))
        {
            BurstHeal();
        }
        //placeholder code to test healthbar!
        //////////////////////////////////////////////////////////////
        if(Input.GetKeyDown(KeyCode.P))
        {
            TakeDamage(Random.Range(5, 10));
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            RestoreHealth(Random.Range(5, 10));
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log(health);
        }
        
        /////////////////////////////////////////////////////////////*/
        
    }
    private void BurstHeal()
    {
        if (playerAttributeController.canBurstHeal && burstHealCount > 0)
        {
            burstHealCount--;
            RestoreHealth(maxHealth/2);
        }
    }
    public void UpdateHealthUI()
    {
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFraction = health / maxHealth;
        if(fillB > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete *= percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);

        }
        if(fillF < hFraction)
        {
            backHealthBar.color = Color.green;
            backHealthBar.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete *= percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, backHealthBar.fillAmount, percentComplete);

        }
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        lerpTimer = 0f;

        if(health <= 0f)
        {
            EndGameScreen();
        }
    }
    public void RestoreHealth(float healAmount)
    {
        health += healAmount;

        lerpTimer = 0f;
    }
    private IEnumerator StartTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(timerInterval);
            RestoreHealth(1f);
        }
    }

    private void EndGameScreen()
    {
        SceneManager.LoadScene("GameEndScreen");
        SelectedSections.gameWon=false;
    }
}
