using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Game Objects")]
    public GameObject quitCanvas;
    public GameObject optionsCanvas;
    public GameObject optionsButton;
    public GameObject newGameButton;
    public GameObject loadGameButton;
    public GameObject quitButton;
    public GameObject characters;
    [Header("Audio")]
    public AudioClip click;
    public AudioSource fxSource;
    //public AudioSource musicSource;
    //[Header("Developer")]


    // Start is called before the first frame update
    void Start()
    {
        quitCanvas.SetActive(false);
        optionsCanvas.SetActive(false);
        //optionsButton.GetComponent<Button>().interactable = true;
        /*
        fxSource.volume = float.Parse(PlayerPrefs.GetString("fxVolume")) / 10;
        musicSource.volume = float.Parse(PlayerPrefs.GetString("musicVolume")) / 100;
        */
        LoadSetActive();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickSound()
    {
        fxSource.PlayOneShot(click);
    }

    public void QuitClick()
    {
        ClickSound();
        quitCanvas.SetActive(true);
        Time.timeScale = 0f;
    }

    public void NoClick()
    {
        ClickSound();
        quitCanvas.SetActive(false);
        Time.timeScale = 1f;
    }

    public void YesClick()
    {
        ClickSound();
        PlayerPrefs.SetInt("OpenedOnce", 0);
        Application.Quit();
    }

    public void OptionsClick()
    {
        ClickSound();
        optionsCanvas.SetActive(true);
        optionsButton.GetComponent<Button>().interactable = false;
        characters.SetActive(false);
        newGameButton.SetActive(false);
        quitButton.SetActive(false);
        loadGameButton.SetActive(false);
    }

    public void OptionsQuit()
    {
        ClickSound();
        optionsCanvas.SetActive(false);
        optionsButton.GetComponent<Button>().interactable = true;
        characters.SetActive(true);
        newGameButton.SetActive(true);
        quitButton.SetActive(true);
        LoadSetActive();
    }

    public void NewGame()
    {
        ClickSound();
        PlayerPrefs.SetInt("Existing", 0);
        PlayerPrefs.SetString("Name", "");
        PlayerPrefs.SetInt("Players", 0);
        SceneManager.LoadScene(1);
    }

    public void LoadGame()
    {
        ClickSound();
        SceneManager.LoadScene(3);
    }
    private void LoadSetActive()
    {
        if (PlayerPrefs.GetInt("Existing") > 0)
        {
            loadGameButton.SetActive(true);
        }
    }
}
