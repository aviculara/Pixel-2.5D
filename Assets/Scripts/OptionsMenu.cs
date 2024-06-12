using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    
    //[Header("Buttons")]
    public GameObject fxPlus, fxMinus;
    public GameObject musicPlus, musicMinus;
    public TMP_Text fxValue, musicValue;
    public GameObject apply;
    [Header("Audio")]
    public AudioSource musicSource;
    public AudioSource fxSource;
    public AudioClip click;

    private bool haveApplied = true;
    //settings applied = button disabled
    //unapplied settings = button enabled

    // Start is called before the first frame update
    void Start()
    {
        resetSettings();
        //music max = 0.1
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //sound.GetComponent<AudioSource>().volume

    public void fxUp()
    {
        ClickSound();
        float val = float.Parse(fxValue.text);
        if(val<10f)
        {
            changeVolume(fxValue, fxSource, 1, 0.1f);
        }
        checkButtonActive(fxValue, fxPlus, fxMinus);
        /*
        if(fxValue.text == "0")
        {
            fxMinus.SetActive(true);
            fxValue.text = "1";
            fxSource.volume = 0.1f;
        }
        else if(fxValue.text == "9")
        {
            fxPlus.SetActive(false);
            fxValue.text = "10";
            fxSource.volume = 1;
        }
        else
        {
            changeVolume(fxValue, fxSource, 1, 0.1f);
        }
        */
        
    }

    public void fxDown()
    {
        ClickSound();
        float val = float.Parse(fxValue.text);
        if (0f < val)
        {
            changeVolume(fxValue, fxSource, -1, 0.1f);
        }
        checkButtonActive(fxValue, fxPlus, fxMinus);
    }

    public void musicUp()
    {
        ClickSound();
        float val = float.Parse(musicValue.text);
        if (val < 10f)
        {
            changeVolume(musicValue, musicSource, 1, 0.01f);
        }
        checkButtonActive(musicValue, musicPlus, musicMinus);
    }

    public void musicDown()
    {
        ClickSound();
        float val = float.Parse(musicValue.text);
        if (val > 0f)
        {
            changeVolume(musicValue, musicSource, -1, 0.01f);
        }
        checkButtonActive(musicValue, musicPlus, musicMinus);
    }

    public void ClickSound()
    {
        fxSource.PlayOneShot(click);
    }

    private void changeVolume(TMP_Text textObject, AudioSource sourceObject, int add, float multiplier)
    {
        //i think theres a shorter way but im too tired
        float newVal = float.Parse(textObject.text) + add;
        textObject.text = (newVal).ToString();
        sourceObject.volume = newVal * multiplier;
        haveApplied = false;
        apply.GetComponent<Button>().interactable = true;
    }
    
    private void checkButtonActive(TMP_Text textObject, GameObject plusObject, GameObject minusObject)
    {
        if (textObject.text == "0")
        {
            minusObject.SetActive(false);
        }
        else if (textObject.text == "10")
        {
            plusObject.SetActive(false);
        }
        else if (!plusObject.activeSelf)
        {
            plusObject.SetActive(true);
        }
        else if(!minusObject.activeSelf)
        {
            minusObject.SetActive(true);
        }
        
    }

    public void Apply()
    {
        PlayerPrefs.SetString("fxVolume", fxValue.text);
        PlayerPrefs.SetString("musicVolume", musicValue.text);
        haveApplied = true;
        apply.GetComponent<Button>().interactable = false;
    }
    /*
    public void applyState() //doesnt work
    {
        //settings applied = button disabled
        //unapplied settings = button enabled
        if (haveApplied)
        {
            apply.GetComponent<Button>().interactable = false;
            //apply.GetComponent<Button>().GetComponentInChildren<TextMeshProUGUI>().gameObject.SetActive(false);
        }
        else
        {
            //apply.GetComponent<Button>().GetComponentInChildren<TextMeshProUGUI>().gameObject.SetActive(true);
            apply.GetComponent<Button>().interactable = true;
        }
    }
    */
    public void Quit()
    {
        if(!haveApplied)
        {
            resetSettings();
        }
    }

    public void resetSettings()
    {
        fxValue.text = PlayerPrefs.GetString("fxVolume");
        musicValue.text = PlayerPrefs.GetString("musicVolume");
        if (fxValue.text == "")
        {
            fxValue.text = "10";
        }
        if (musicValue.text == "")
        {
            musicValue.text = "10";
        }
        changeVolume(fxValue, fxSource, 0, 0.1f);
        checkButtonActive(fxValue, fxPlus, fxMinus);
        changeVolume(musicValue, musicSource, 0, 0.01f);
        checkButtonActive(musicValue, musicPlus, musicMinus);
        haveApplied = true;
        apply.GetComponent<Button>().interactable = false;
    }
}
