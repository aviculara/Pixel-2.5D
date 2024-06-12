using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject settingsPanel;

    // Start is called before the first frame update
    void Start()
    {
        pausePanel.SetActive(false);
        if(settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(Time.timeScale == 0)
            {
                Continue();
            }
            else
            {
                Pause();
            }

        }
    }

    public void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void Continue()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void openSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void closeSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void stopGame()
    {
        Application.Quit();
    }
}
