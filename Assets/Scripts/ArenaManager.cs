using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ArenaManager : MonoBehaviour
{
    public Transform player1pos, player2pos; //can i find it from tag
    public TextMeshProUGUI timerTMP, startTMPw, startTMPb;
    public int timer = 90;
    public GameObject retryButton;
    public GameObject startButton;

    private PlayerMove p1Script;
    private PlayerMove p2Script;
    private Coroutine timerRoutine;
    // Start is called before the first frame update
    void Start()
    {
        timerTMP.text = timer.ToString();
        p1Script = player1pos.gameObject.GetComponent<PlayerMove>();
        p2Script = player2pos.gameObject.GetComponent<PlayerMove>();
        Time.timeScale = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if (player1pos.position.x < player2pos.position.x) //default
        {
            //change x scale
            p1Script.orientation = 1;
            p2Script.orientation = 1;
        }
        else
        {
            p1Script.orientation = -1;
            p2Script.orientation = -1;
        }

    }

    IEnumerator Countdown()
    {
        turnOffInputs();
        startTMPw.text = startTMPb.text = "3";
        startTMPw.gameObject.SetActive(true);
        startTMPb.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        startTMPw.text = startTMPb.text = "2";
        yield return new WaitForSeconds(1f);
        startTMPw.text = startTMPb.text = "1";
        yield return new WaitForSeconds(1f);
        startTMPb.text = startTMPw.text = "START!";
        turnOnInputs();
        yield return new WaitForSeconds(0.5f);
        startTMPw.gameObject.SetActive(false);
        startTMPb.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);

        timerRoutine = StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        timer -= 1;
        timerTMP.text = timer.ToString();
        
        if(timer<=5)
        {
            timerTMP.color = new Color(0.835f, 0, 0, 1);
        }
        yield return new WaitForSeconds(1f);
        if (timer>0)
        {
            timerRoutine = StartCoroutine(Timer());
        }
        else
        {
            timeOver();
        }
    }

    public void timeOver()
    {
        int p1HP = p1Script.hp;
        int p2HP = p2Script.hp;
        StopCoroutine(timerRoutine);
        turnOffInputs();
        if (p1HP > p2HP)
        {
            startTMPw.text = startTMPb.text = "Player 1 Wins!";
            p2Script.animator.SetTrigger("Death");
            //p1Script.animator.SetTrigger("Jump");
            p1Script.animator.SetBool("Win",true);
        }
        else if(p1HP < p2HP)
        {
            startTMPw.text = startTMPb.text = "Player 2 Wins!";
            p1Script.animator.SetTrigger("Death");
            //p2Script.animator.SetTrigger("Jump");
            p2Script.animator.SetBool("Win", true);
        }
        else
        {
            startTMPw.text = startTMPb.text = "Tie!";
        }
        startTMPw.gameObject.SetActive(true);
        startTMPb.gameObject.SetActive(true);
        retryButton.SetActive(true);
        //StartCoroutine(endGame());
    }

    IEnumerator endGame()
    {
        yield return new WaitForSeconds(0.75f);
        Time.timeScale = 0;
    }

    public void turnOffInputs()
    {
        p1Script.inputOn = false;
        p2Script.inputOn = false;
    }

    public void turnOnInputs()
    {
        p1Script.inputOn = true;
        p2Script.inputOn = true;
    }

    public void newGame()
    {
        //SceneManager.LoadScene(4);
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void startGame()
    {
        Time.timeScale = 1;
        StartCoroutine(Countdown());
        startButton.SetActive(false);
    }
}
