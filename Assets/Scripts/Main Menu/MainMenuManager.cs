using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
 
    public void LoadGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }


}
