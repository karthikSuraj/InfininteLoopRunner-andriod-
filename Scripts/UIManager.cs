using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager sharedInstance;

   
    public int levelDefulty=0;

    public void Awake()
    {
        if (sharedInstance == null)
            sharedInstance = this;
        else if (sharedInstance != null)
            DontDestroyOnLoad(gameObject);

    }
    public void PlayBtn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitBtn()
    {
        Application.Quit();
    }

    public void ReplyGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main menu");
    }
    public void nextLevel()

    {
        levelDefulty+=10;
        SceneManager.LoadScene(1);
    }


}
