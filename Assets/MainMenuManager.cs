using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject tutorialMenu;
    private void Start()
    {
        if (PlayerPrefs.GetInt("PlayedBefore") == 0)
        {
            tutorialMenu.SetActive(true);
        }    
    }

    public void DenyTutorial()
    {
        PlayerPrefs.SetInt("PlayedBefore", 1);

        tutorialMenu.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void StartTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
