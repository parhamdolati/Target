using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UiManager : InitManager
{
    [SerializeField] private GameObject splashCanvas, menuCanvas, gameCanvas;
    [SerializeField] private GameObject leaderboardPanel, openGameForFirst;
    [SerializeField] private Button playBtn, modeBtn, leaderboardBtn, musicBtn, instagramBtn;
    [SerializeField] private Button setNicknameBtn;
    [SerializeField] private TMP_Text menuRecordTxt, gameRecordTxt;
    [SerializeField] private GameObject newRecord;

    private bool getNewRecord = false;
    private bool playGame = false;

    private void Start()
    {
        #region Init PlayerPrefs

        if (!PlayerPrefs.HasKey("easyRecord"))
            PlayerPrefs.SetInt("easyRecord", 0);
        if (!PlayerPrefs.HasKey("normalRecord"))
            PlayerPrefs.SetInt("normalRecord", 0);
        if (!PlayerPrefs.HasKey("hardRecord"))
            PlayerPrefs.SetInt("hardRecord", 0);
        
        if(!PlayerPrefs.HasKey("gameLevel"))
            PlayerPrefs.SetInt("gameLevel",0);
        else
        {
            int gameMode = PlayerPrefs.GetInt("gameLevel");
            Manager.Instance.gameMode = gameMode;
            switch (gameMode)
            {
                case 0:
                    modeBtn.transform.Find("Mode").GetComponent<Image>().color = new Color32(36, 178, 56, 255);
                    menuRecordTxt.text = PlayerPrefs.GetInt("easyRecord").ToString();
                    break;
                case 1:
                    modeBtn.transform.Find("Mode").GetComponent<Image>().color = new Color32(255, 203, 0, 255);
                    menuRecordTxt.text = PlayerPrefs.GetInt("normalRecord").ToString();
                    break;
                case 2:
                    modeBtn.transform.Find("Mode").GetComponent<Image>().color = new Color32(255, 0, 0, 255);
                    menuRecordTxt.text = PlayerPrefs.GetInt("hardRecord").ToString();
                    break;
            }
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
        

        #endregion
        
        Manager.Instance.GoToMenu += GoToMenu;
        Manager.Instance.PlaySplash += PlaySplash;
        Manager.Instance.UpdateGameRecord = UpdateGameRecord;
        
        isInited = true;
    }
    
    void PlaySplash()
    {
        splashCanvas.SetActive(true);
        float splashTime = splashCanvas.transform.Find("Splash")
            .GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + 1;
        Manager.Instance.Delay(splashTime, () =>
        {
            splashCanvas.SetActive(false);
            GoToMenu();
            MenuButtonsListener();
        });
    }

    void GoToMenu()
    {
        playGame = false;
        newRecord.SetActive(false);
        if (gameCanvas.activeInHierarchy)
            gameCanvas.SetActive(false);
        switch (Manager.Instance.gameMode)
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
        menuCanvas.SetActive(true);
        Manager.Instance.Delay(1,(() =>
        {
            if (PlayerPrefs.GetInt("openGameForFirst").Equals(0))
                openGameForFirst.SetActive(true);
        }));
        
    }
    
    void GoToGame()
    {
        Animator titleAnim = menuCanvas.transform.Find("Title").GetComponent<Animator>();
        titleAnim.SetTrigger("TitleFadeOff");
        Animator buttonsAnim = menuCanvas.transform.Find("Buttons").GetComponent<Animator>();
        buttonsAnim.SetTrigger("ButtonsFadeOff");
        
        float waitSec = .5f;
        float titleAnimTime = titleAnim.GetCurrentAnimatorStateInfo(0).length;
        float buttonsAnimTime = buttonsAnim.GetCurrentAnimatorStateInfo(0).length;
        
        if (titleAnimTime > buttonsAnimTime)
            waitSec += titleAnimTime;
        else waitSec += buttonsAnimTime;
        Manager.Instance.Delay(waitSec,(() =>
        {
            menuCanvas.SetActive(false);
            gameCanvas.SetActive(true);
        }));
    }
    
    void MenuButtonsListener()
    {
        playBtn.onClick.AddListener(() =>
        {
            if (!playGame)
            {
                playGame = true;
                Manager.Instance.PlayEfx("click");
                Manager.Instance.StartNewGame();
                GoToGame();
            }
        });

        modeBtn.onClick.AddListener(() =>
        {
            Manager.Instance.PlayEfx("click");
            if(menuRecordTxt.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("idle"))
                menuRecordTxt.GetComponent<Animator>().SetTrigger("ChangeMode");

            Manager.Instance.ResetLastRecord();
            
            int gameMode = Manager.Instance.gameMode;
            switch (gameMode)
            {
                case 0:
                    gameMode = 1;
                    menuRecordTxt.text = PlayerPrefs.GetInt("normalRecord").ToString();
                    modeBtn.transform.Find("Mode").GetComponent<Image>().color = new Color32(255, 203, 0, 255);
                    break;
                case 1:
                    gameMode = 2;
                    menuRecordTxt.text = PlayerPrefs.GetInt("hardRecord").ToString();
                    modeBtn.transform.Find("Mode").GetComponent<Image>().color = new Color32(255, 0, 0, 255);
                    break;
                case 2:
                    gameMode = 0;
                    menuRecordTxt.text = PlayerPrefs.GetInt("easyRecord").ToString();
                    modeBtn.transform.Find("Mode").GetComponent<Image>().color = new Color32(36, 178, 56, 255);
                    break;
            }

            PlayerPrefs.SetInt("gameLevel", gameMode);
            Manager.Instance.gameMode = gameMode;
        });
        
        leaderboardBtn.onClick.AddListener((() =>
        {
            Manager.Instance.PlayEfx("click");
            leaderboardPanel.SetActive(true);
            Manager.Instance.ConnectToServer();
        }));
        
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
                Manager.Instance.PlayEfx("click");
            }
        });
        
        instagramBtn.onClick.AddListener(() =>
        {
            Manager.Instance.PlayEfx("click");
            Application.OpenURL("https://www.instagram.com/parham_dolati_/");
        });
        
        setNicknameBtn.onClick.AddListener(() => GetNickname());
    }

    void UpdateGameRecord(int record)
    {
        gameRecordTxt.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        gameRecordTxt.text = record.ToString();
        Manager.Instance.Delay(.2f,(() =>
        {
            gameRecordTxt.transform.localScale = new Vector3(1, 1, 1);
        }));
        
        if (!getNewRecord)
        {
            switch (Manager.Instance.gameMode)
            {
                case 0:
                    if (record > PlayerPrefs.GetInt("easyRecord"))
                    {
                        getNewRecord = true;
                        newRecord.SetActive(true);
                    }
                    break;
                case 1:
                    if (record > PlayerPrefs.GetInt("normalRecord"))
                    {
                        getNewRecord = true;
                        newRecord.SetActive(true);
                    }
                    break;
                case 2:
                    if (record > PlayerPrefs.GetInt("hardRecord"))
                    {
                        getNewRecord = true;
                        newRecord.SetActive(true);
                    }
                    break;
            }
        }
    }
    
    void GetNickname()
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
