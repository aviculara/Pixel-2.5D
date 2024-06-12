using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{
    public GameObject playersPlus, playersMinus;
    public TMP_Text playersValue;
    public AudioSource fxSource;
    public AudioSource musicSource;
    public AudioClip click;

    public TMP_Text title1, title2;
    public TMP_InputField worldName;

    private int players;

    // Start is called before the first frame update
    void Start()
    {
        players = int.Parse(playersValue.text);
        checkButtonActive(players, playersPlus, playersMinus);
        fxSource.volume = float.Parse(PlayerPrefs.GetString("fxVolume")) / 10;
        musicSource.volume = float.Parse(PlayerPrefs.GetString("musicVolume")) / 100;

        if (PlayerPrefs.GetInt("Existing") > 0)
        {
            players = PlayerPrefs.GetInt("Players");
            playersValue.text = players.ToString();
            title1.text = title2.text = PlayerPrefs.GetString("Name");
            worldName.text = PlayerPrefs.GetString("Name");
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ClickSound()
    {
        fxSource.PlayOneShot(click);
    }
    public void playersUp()
    {
        ClickSound();
        int val = int.Parse(playersValue.text);
        if (val < 6)
        {
            players = val + 1;
            playersValue.text = players.ToString();
        }
        checkButtonActive(players, playersPlus, playersMinus);
    }

    public void playersDown()
    {
        ClickSound();
        int val = int.Parse(playersValue.text);
        if (val > 1)
        {
            players = val - 1;
            playersValue.text = players.ToString();
        }
        checkButtonActive(players, playersPlus, playersMinus);
    }
    private void checkButtonActive(int val, GameObject plusObject, GameObject minusObject)
    {
        if (val == 1)
        {
            minusObject.SetActive(false);
        }
        else if (val == 6)
        {
            plusObject.SetActive(false);
        }
        else if (!plusObject.activeSelf)
        {
            plusObject.SetActive(true);
        }
        else if (!minusObject.activeSelf)
        {
            minusObject.SetActive(true);
        }
    }

    public void changeName()
    {
        if (worldName.text != "")
        {
            title1.text = title2.text = worldName.text + "'s World";
        }
        else
        {
            title1.text = title2.text = "New World";
        }
    }

    public void Back()
    {
        SceneManager.LoadScene(0);
    }

    public void Create()
    {
        SceneManager.LoadScene(2);
        PlayerPrefs.SetInt("Existing", 1);
        PlayerPrefs.SetString("Name", title1.text);
        PlayerPrefs.SetInt("Players", players);
    }

    public void resetPrefs()
    {
        PlayerPrefs.SetInt("Existing", 0);
        PlayerPrefs.SetString("Name", "");
        PlayerPrefs.SetInt("Players", 0);
    }
}
