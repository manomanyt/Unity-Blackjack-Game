using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuScript : MonoBehaviour
{
    public TextMeshProUGUI quitText;
    public void StartGame()
    {
        SceneManager.LoadScene(2);
        
    }

    public void QuitGame()
    {
        quitText.text = "You can't quit, you are addicted!";
    }

    public void QuitGameReal()
    {
        Application.Quit();
    }

    public void BackToGame()
    {
        SceneManager.LoadScene(1);
        
    }

       public void LoadWallet()
    {
        SceneManager.LoadScene(2);
        
    }

       public void LoadLose()
    {
        SceneManager.LoadScene(3);
        
    }

       public void LoadWin()
    {
        SceneManager.LoadScene(4);
        
    }
}
