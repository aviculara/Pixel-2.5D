using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;


public class DailyMenu : MonoBehaviour
{
    [Header("Game Objects")]
    public GameObject chestObject;
    public GameObject dailyCanvas;
    public GameObject contentsObject;
    public GameObject button;
    [Header("Audio")]
    public AudioClip click;
    public AudioClip loot;
    public AudioSource audioSource;
    [Header("Developer")]
    public float growRate = 0.05f;
    public bool openOnce;

    private DateTime lastLogin;
    private Animator chestAnimator;
    private TMP_Text buttonText;


    // Start is called before the first frame update
    void Start()
    {
        dailyCanvas.SetActive(false);
        contentsObject.SetActive(false);
        chestAnimator = chestObject.GetComponent<Animator>();
        buttonText = button.GetComponentInChildren<TMP_Text>();
        lastLogin = DateTime.Parse(PlayerPrefs.GetString("lastLogin", DateTime.Now.ToString()));
        if(PlayerPrefs.GetInt("OpenedOnce")==0)
        {
            dailyCanvas.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenChest()
    {
        if(!chestAnimator.GetBool("Open"))
        {
            audioSource.PlayOneShot(click);
            print("chest clicked");
            chestAnimator.SetBool("Open", true);
            button.GetComponent<Button>().interactable = false;
            PlayerPrefs.SetInt("OpenedOnce", 1);
            StartCoroutine(summonContents(1f));

        }

        else
        {
            print("already open");
            //resetChest(); //for testing
        }
        
    }
    public void resetChest()
    {
        chestAnimator.SetBool("Open", false);
        contentsObject.transform.localScale = new Vector3(80f, 80f, 80f);
        contentsObject.SetActive(false);
        buttonText.text = "Open";

    }

    public void ButtonClick()
    {
        if (!chestAnimator.GetBool("Open"))
        {
            OpenChest();
        }
        else
        {
            dailyCanvas.SetActive(false);
        }
    }

    IEnumerator summonContents(float secs)
    {
        yield return new WaitForSeconds(secs);
        contentsObject.transform.localScale = new Vector3(80f, 80f, 80f);
        contentsObject.SetActive(true);
        audioSource.PlayOneShot(loot);
        InvokeRepeating("growObjects", 0f, 0.025f);
        StartCoroutine(cancelInvoke(0.35f));
    }

    IEnumerator cancelInvoke(float secs)
    {
        yield return new WaitForSeconds(secs);
        CancelInvoke();
        button.GetComponent<Button>().interactable = true;
        buttonText.text = "Close";
    }

    private void growObjects()
    {
        Vector3 newscale = new Vector3(growRate, growRate, growRate);
        contentsObject.transform.localScale += newscale;
    }

    /*StartCoroutine(passiveMe(5));

IEnumerator passiveMe(int secs)
    {
        yield return new WaitForSeconds(secs);
        gameObject.SetActive(false);
    }
    */

    private bool CheckDayPassed()
    {
        DateTime currentDate = DateTime.Now;

        if (currentDate.Date > lastLogin.Date)
        {
            // A day has passed
            // Update the last login date to the current date
            lastLogin = currentDate;

            // Save the new last login date to PlayerPrefs or another storage method
            PlayerPrefs.SetString("lastLogin", lastLogin.ToString());
            PlayerPrefs.Save();

            return true;
        }
        else
        {
            return false;
        }
    }


}
