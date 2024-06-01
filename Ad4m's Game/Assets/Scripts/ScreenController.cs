using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenController : MonoBehaviour
{
    [SerializeField] private Text sections;
    [SerializeField] private Canvas winCanvas;
    [SerializeField] private Canvas loseCanvas;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        sections.text = "";
        if(SelectedSections.gameWon)
        {
            winCanvas.gameObject.SetActive(true);
            loseCanvas.gameObject.SetActive(false);
            if(SelectedSections.isHorrorPresent)
            {
                sections.text += " Horror ";
            }
            if(SelectedSections.isCardPresent)
            {
                sections.text += " Card ";
            }
            if(SelectedSections.isShooterPresent)
            {
                sections.text += " Shooter ";
            }
            if(SelectedSections.isDodgerPresent)
            {
                sections.text += " Dodger ";
            }
            if(SelectedSections.isPuzzlePresent)
            {
                sections.text += " Puzzle ";
            }
        }
        else
        {
            winCanvas.gameObject.SetActive(false);
            loseCanvas.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void retryLevel()
    {
        print("clickclick");
        gameObject.GetComponent<SectionController>().openGameEnd(); 
    }

    public void returnToComputer()
    {
        SceneManager.LoadScene("The Computer");
    }
}
