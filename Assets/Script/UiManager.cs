using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UiManager : InitManager
{
    [SerializeField] private GameObject splashCanvas, menuCanvas, gameCanvas;
    [SerializeField] private Button playBtn, modeBtn, musicBtn, instagramBtn;
    [SerializeField] private TMP_Text menuRecordTxt, gameRecordTxt;
    private int gameLevel;


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
            gameLevel = PlayerPrefs.GetInt("gameLevel");
            switch (gameLevel)
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
    
    IEnumerator GoToGame_IE()
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
        yield return new WaitForSeconds(waitSec);
        
        menuCanvas.SetActive(false);    
        
        gameCanvas.SetActive(true);
    }

    void GoToMenu()
    {
        StartCoroutine(GoToMenu_IE());
    }
    IEnumerator GoToMenu_IE()
    {
        if (gameCanvas.activeInHierarchy)
        {
            gameCanvas.transform.Find("GameOver").gameObject.SetActive(true);
            yield return new WaitForSeconds(1);
            gameCanvas.transform.Find("GameOver").gameObject.SetActive(false);
            gameCanvas.SetActive(false);    
        }
        menuCanvas.SetActive(true);
    }

    void PlaySplash()
    {
        StartCoroutine(PlaySplash_IE());
    }
    IEnumerator PlaySplash_IE()
    {
        splashCanvas.SetActive(true);
        float splashTime = splashCanvas.transform.Find("Splash")
            .GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + 1;
        yield return new WaitForSeconds(splashTime);
        splashCanvas.SetActive(false);
        GoToMenu();
        MenuButtonsListener();
    }

    void MenuButtonsListener()
    {
        playBtn.onClick.AddListener(() =>
        {
            Manager.Instance.PlayEfx("click");
            Manager.Instance.StartNewGame();
            StartCoroutine(GoToGame_IE());
        });
        
        modeBtn.onClick.AddListener(() =>
        {
            Manager.Instance.PlayEfx("click");
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
                Manager.Instance.PlayEfx("click");
            }
        });
        
        instagramBtn.onClick.AddListener(() =>
        {
            Manager.Instance.PlayEfx("click");
            Application.OpenURL("https://www.instagram.com/parham_dolati_/");
        });
    }

    void UpdateGameRecord(int record)
    {
        gameRecordTxt.text = record.ToString();
    }
}
