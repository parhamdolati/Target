using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    [SerializeField] private SoundHandler _soundHandler;
    [SerializeField] private Target _target;
    public GameObject menuCanvas, gameCanvas;
    public GameObject gameOver;
    public GameObject openGameForFirst, splashPanel;
    public Button playBtn, modeBtn, musicBtn, instagramBtn;
    public TMP_Text menuRecordTxt, gamerecordTxt;
    public int record;
    public List<GameObject> lastBall;
    private int gameLevel;
    private bool gameIsStarted;//baraye kontrol kardan inke chand bar play ro nazanim

    private Transform cannon;
    private GameObject ball;
    private GameObject target;
    private int lastRecord;
    private bool canFireBall;
    private bool getNewRecord;

    private void Awake()
    {
        gameIsStarted = false;

        if(!PlayerPrefs.HasKey("gameLevel"))
            PlayerPrefs.SetInt("gameLevel",0);
        else
        {
            gameLevel = PlayerPrefs.GetInt("gameLevel");
            if (gameLevel == 0)
                modeBtn.transform.Find("Mode").GetComponent<Image>().color = new Color32(36, 178, 56, 255);
            else if (gameLevel == 1)
                modeBtn.transform.Find("Mode").GetComponent<Image>().color = new Color32(255, 203, 0, 255);
            else if (gameLevel == 2)
                modeBtn.transform.Find("Mode").GetComponent<Image>().color = new Color32(255, 0, 0, 255);
            
        }
        
        if (!PlayerPrefs.HasKey("easyRecord"))
            PlayerPrefs.SetInt("easyRecord", 0);
        if (!PlayerPrefs.HasKey("normalRecord"))
            PlayerPrefs.SetInt("normalRecord", 0);
        if (!PlayerPrefs.HasKey("hardRecord"))
            PlayerPrefs.SetInt("hardRecord", 0);
        switch (PlayerPrefs.GetInt("gameLevel"))
        {
            case 0:
                menuRecordTxt.text = PlayerPrefs.GetInt("easyRecord").ToString();
                break;
            case 1:
                menuRecordTxt.text = PlayerPrefs.GetInt("normalRecord").ToString();
                break;
            case 2:
                menuRecordTxt.text = PlayerPrefs.GetInt("hardRecord").ToString();
                break;
        }

        if (!PlayerPrefs.HasKey("music"))
            PlayerPrefs.SetInt("music", 1);
        else
        {
            if (PlayerPrefs.GetInt("music") == 1)
                musicBtn.transform.Find("OnOff").GetComponent<Image>().color = new Color32(36, 178, 56, 255);
            else if (PlayerPrefs.GetInt("music") == 0)
                musicBtn.transform.Find("OnOff").GetComponent<Image>().color = new Color32(255, 0, 0, 255);
        }

        if (!PlayerPrefs.HasKey("openGameForFirst"))
            PlayerPrefs.SetInt("openGameForFirst", 0);
    }

    void Start()
    {
        StartCoroutine(Splash());

        lastRecord = 0;
        lastBall = new List<GameObject>();
        cannon = gameCanvas.transform.Find("Bottom").Find("Cannon").transform;
        ball = gameCanvas.transform.Find("Bottom").Find("Ball").gameObject;
        target = gameCanvas.transform.Find("Target").gameObject;
        canFireBall = false;

        playBtn.onClick.AddListener(() =>
        {
            if (!gameIsStarted)
            {
                _soundHandler.PlayEfx("click");
                StartCoroutine(PlayGame());
                gameIsStarted = true;
            }
        });
        modeBtn.onClick.AddListener(() =>
        {
            lastRecord = 0;
            _soundHandler.PlayEfx("click");
            if(menuRecordTxt.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("idle"))
                menuRecordTxt.GetComponent<Animator>().SetTrigger("ChangeMode");
            
            if (gameLevel == 0)
            {
                gameLevel = 1;
                menuRecordTxt.text = PlayerPrefs.GetInt("normalRecord").ToString();
                modeBtn.transform.Find("Mode").GetComponent<Image>().color = new Color32(255, 203, 0, 255);
            }
            else if (gameLevel == 1)
            {
                gameLevel = 2;
                menuRecordTxt.text = PlayerPrefs.GetInt("hardRecord").ToString();
                modeBtn.transform.Find("Mode").GetComponent<Image>().color = new Color32(255, 0, 0, 255);
            }
            else if (gameLevel == 2)
            {
                gameLevel = 0;
                menuRecordTxt.text = PlayerPrefs.GetInt("easyRecord").ToString();
                modeBtn.transform.Find("Mode").GetComponent<Image>().color = new Color32(36, 178, 56, 255);
            }

            PlayerPrefs.SetInt("gameLevel", gameLevel);
        });
        musicBtn.onClick.AddListener(() =>
        {
            if (PlayerPrefs.GetInt("music") == 1)
            {
                PlayerPrefs.SetInt("music", 0);
                musicBtn.transform.Find("OnOff").GetComponent<Image>().color = new Color32(255, 0, 0, 255);
            }
            else if (PlayerPrefs.GetInt("music") == 0)
            {
                PlayerPrefs.SetInt("music", 1);
                musicBtn.transform.Find("OnOff").GetComponent<Image>().color = new Color32(36, 178, 56, 255);
                _soundHandler.PlayEfx("click");
            }
        });
        instagramBtn.onClick.AddListener(() =>
        {
            _soundHandler.PlayEfx("click");
            Application.OpenURL("https://www.instagram.com/parham_dolati_/");
        });
    }

    IEnumerator Splash()
    {
        splashPanel.SetActive(true);
        menuCanvas.transform.Find("Title").GetComponent<Animator>().enabled = false;
        menuCanvas.transform.Find("Buttons").GetComponent<Animator>().enabled = false;

        yield return new WaitForSeconds(splashPanel.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + 1);
        splashPanel.SetActive(false);
        
        menuCanvas.transform.Find("Title").GetComponent<Animator>().enabled = true;
        menuCanvas.transform.Find("Buttons").GetComponent<Animator>().enabled = true;

        if (PlayerPrefs.GetInt("openGameForFirst").Equals(0))
            openGameForFirst.SetActive(true);
    }

    //khroj az menu va shroE bazi be tartip amaliat animation
    IEnumerator PlayGame()
    {
        record = Mathf.RoundToInt(lastRecord * 60 / 100);
        gamerecordTxt.text = record.ToString();
        GameObject title = menuCanvas.transform.Find("Title").gameObject;
        title.GetComponent<Animator>().SetTrigger("TitleFadeOff");
        GameObject buttons = menuCanvas.transform.Find("Buttons").gameObject;
        buttons.GetComponent<Animator>().SetTrigger("ButtonsFadeOff");
        menuRecordTxt.gameObject.SetActive(false);
        _soundHandler.PlayMusic("play");
        
        float waitSec = .5f;
        if (title.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length
            > buttons.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length)
            waitSec += title.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        else waitSec += buttons.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(waitSec);
        gameCanvas.SetActive(true);
        menuCanvas.SetActive(false);
        yield return new WaitForSeconds(gameCanvas.transform.Find("Bottom").GetComponent<Animator>()
            .GetCurrentAnimatorStateInfo(0).length);
        getNewRecord = false;
        _target.CreatBarriers(gameLevel);
        canFireBall = true;
    }

    //controll shellik ball dar har frame
    void Update()
    {
        if (canFireBall)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase.Equals(TouchPhase.Began))
            {
                GameObject _ball = Instantiate(ball, ball.transform.position, quaternion.identity);
                _ball.transform.parent = target.transform;
                _ball.SetActive(true);
                _soundHandler.PlayEfx("ball");
                if (cannon.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("idle"))
                    cannon.GetComponent<Animator>().SetTrigger("fire");
            }
        }
    }

    public void RecordUp()
    {
        record++;
        gamerecordTxt.text = record.ToString();
        StartCoroutine(GameRecordTxtAnimation());
        switch (PlayerPrefs.GetInt("gameLevel"))
        {
            case 0:
                if (record > PlayerPrefs.GetInt("easyRecord") && !getNewRecord)
                {
                    getNewRecord = true;
                    target.transform.Find("NewRecord").gameObject.SetActive(true);
                }
                break;
            case 1:
                if (record > PlayerPrefs.GetInt("normalRecord") && !getNewRecord)
                {
                    getNewRecord = true;
                    target.transform.Find("NewRecord").gameObject.SetActive(true);
                }
                break;
            case 2:
                if (record > PlayerPrefs.GetInt("hardRecord") && !getNewRecord)
                {
                    getNewRecord = true;
                    target.transform.Find("NewRecord").gameObject.SetActive(true);
                }
                break;
        }
    }
    
    IEnumerator GameRecordTxtAnimation()
    {
        gamerecordTxt.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        yield return new WaitForSeconds(.2f);
        gamerecordTxt.transform.localScale = new Vector3(1, 1, 1);
    }
    
    public void GameOver()
    {
        _soundHandler.PlayMusic("stop");
        _soundHandler.PlayEfx("over");
        canFireBall = false;
        lastRecord = record;
        switch (PlayerPrefs.GetInt("gameLevel"))
        {
            case 0:
                if (record > PlayerPrefs.GetInt("easyRecord"))
                {
                    PlayerPrefs.SetInt("easyRecord", record);
                    menuRecordTxt.text = record.ToString();
                }
                break;
            case 1:
                if (record > PlayerPrefs.GetInt("normalRecord"))
                {
                    PlayerPrefs.SetInt("normalRecord", record);
                    menuRecordTxt.text = record.ToString();
                }
                break;
            case 2:
                if (record > PlayerPrefs.GetInt("hardRecord"))
                {
                    PlayerPrefs.SetInt("hardRecord", record);
                    menuRecordTxt.text = record.ToString();
                }
                break;
        }
        
        _target.DisableBarriers();
        gameOver.SetActive(true);
        foreach (GameObject ball in lastBall)
        {
            ball.GetComponent<Animator>().SetTrigger("CollisionOnBarrier");
        }
        menuRecordTxt.gameObject.SetActive(true);
        StartCoroutine(GameOverNumerator());
    }

    IEnumerator GameOverNumerator()
    {
        yield return new WaitForSeconds(2);
        foreach (GameObject ball in lastBall)
        {
            Destroy(ball);
        }
        lastBall.Clear();
        gameOver.SetActive(false);
        //gamerecordTxt.text = "0";
        gameCanvas.SetActive(false);
        menuCanvas.SetActive(true);
        foreach (GameObject barrier in _target.barriers)
        {
            barrier.SetActive(false);
        }

        gameIsStarted = false;
        target.transform.GetChild(0).Find("NewRecord").gameObject.SetActive(false);
    }

    //agar karbar avalin bar bud bazi mikonad nick name ra zakhire mikonim
    public void GetNickname()
    {
        string nickname = openGameForFirst.transform.Find("NicknameBorder").Find("NickNameInput")
            .GetComponent<TMP_InputField>().text;
        if (nickname != "")
        {
            PlayerPrefs.SetString("nickName", nickname);
            PlayerPrefs.SetInt("openGameForFirst", 1);
            openGameForFirst.SetActive(false);
        }
    }
}
