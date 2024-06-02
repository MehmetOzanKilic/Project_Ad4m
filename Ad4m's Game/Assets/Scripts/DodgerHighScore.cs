using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DodgerHighScore : MonoBehaviour
{
    int currentHits = 0;
    [SerializeField] TextMeshProUGUI highScoreText;
    [SerializeField] TextMeshProUGUI hitCountText;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float countdownTime = 10f;

    void Start()
    {
        StartCoroutine(TimerCoroutine());
        HandleTexts();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            currentHits++;
            hitCountText.text = "Hit Count: " + currentHits;
        }
    }

    void HandleTexts()
    {
        hitCountText.text = "Hit Count: " + currentHits;
        highScoreText.gameObject.SetActive(false);

        if (PlayerPrefs.HasKey("HighScore"))
        {

        }
        else
        {
            PlayerPrefs.SetInt("HighScore", 9999);
        }
    }

    void UpdateHighScoreText()
    {
        highScoreText.text = "Highscore: " + PlayerPrefs.GetInt("HighScore");
    }

    public void EndGame()
    {
        if (currentHits <= PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", currentHits);
            PlayerPrefs.Save();
        }

        currentHits = 0;
        UpdateHighScoreText();
    }

    IEnumerator TimerCoroutine()
    {
        while (countdownTime > 0)
        {
            timerText.text = "Time Remaining: " + Mathf.CeilToInt(countdownTime).ToString();
            yield return null;
            countdownTime -= Time.deltaTime;
        }

        timerText.text = "Time Remaining: 0";

        highScoreText.gameObject.SetActive(true);
        EndGame();
    }
}
