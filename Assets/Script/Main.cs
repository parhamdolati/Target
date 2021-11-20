using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    [SerializeField] private Target _target;
    public GameObject menuCanvas, gameCanvas;
    public Button playBtn;
    public TMP_Text menuRecordTxt, gamerecordTxt;
    private int record;
    private int gameLevel;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("record"))
            PlayerPrefs.SetInt("record", 0);
        else
            menuRecordTxt.text = PlayerPrefs.GetInt("record").ToString();

        if(!PlayerPrefs.HasKey("gameLevel"))
            PlayerPrefs.SetInt("gameLevel",0);
        else
            gameLevel = PlayerPrefs.GetInt("gameLevel");
    }

    void Start()
    {
        playBtn.onClick.AddListener(() => { StartCoroutine(PlayGame()); });
    }

    //shroE bazi be tartip amaliat animation
    IEnumerator PlayGame()
    {
        GameObject title = menuCanvas.transform.Find("Title").gameObject;
        title.GetComponent<Animator>().SetTrigger("TitleFadeOff");
        GameObject buttons = menuCanvas.transform.Find("Buttons").gameObject;
        buttons.GetComponent<Animator>().SetTrigger("ButtonsFadeOff");

        float waitSec;
        if (title.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length
            > buttons.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length)
            waitSec = title.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        else waitSec = buttons.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(waitSec);
        gameCanvas.SetActive(true);
        menuCanvas.SetActive(false);
        yield return new WaitForSeconds(gameCanvas.transform.Find("Bottom").GetComponent<Animator>()
            .GetCurrentAnimatorStateInfo(0).length);
        record = 0;
        gamerecordTxt.text = record.ToString();
        _target.CreatBarriers(gameLevel);
        StartCoroutine(FireBall());
    }

    //controll shellik top
    IEnumerator FireBall()
    {
        var cannon = gameCanvas.transform.Find("Bottom").Find("Cannon").transform;
        GameObject ball = gameCanvas.transform.Find("Bottom").Find("Ball").gameObject;
        GameObject target = gameCanvas.transform.Find("Target").gameObject;
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            while (true)
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    GameObject _ball = Instantiate(ball, ball.transform.position, quaternion.identity);
                    _ball.GetComponent<Ball>()._main = this;
                    _ball.transform.parent = target.transform;
                    _ball.SetActive(true);
                    cannon.transform.localScale = new Vector3(1, .9f, 1);
                    yield return new WaitForSeconds(.2f);
                    cannon.localScale = new Vector3(1, 1, 1);
                }
                yield return new WaitForFixedUpdate();
            }
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            while (true)
            {
                if (Input.GetTouch(0).phase==TouchPhase.Began)
                {
                    GameObject _ball = Instantiate(ball, ball.transform.position, quaternion.identity);
                    _ball.transform.parent = target.transform;
                    _ball.SetActive(true);
                    cannon.transform.localScale = new Vector3(1, .9f, 1);
                    yield return new WaitForSeconds(.2f);
                    cannon.localScale = new Vector3(1, 1, 1);
                }
                yield return new WaitForFixedUpdate();
            }
        }
    }

    public void RecordUp()
    {
        record++;
        gamerecordTxt.text = record.ToString();
        StartCoroutine(GameRecordTxtAnimation());
    }
    
    IEnumerator GameRecordTxtAnimation()
    {
        gamerecordTxt.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        yield return new WaitForSeconds(.2f);
        gamerecordTxt.transform.localScale = new Vector3(1, 1, 1);
    }
    
    public void GameOver()
    {
        if (record > PlayerPrefs.GetInt("record"))
        {
            PlayerPrefs.SetInt("record", record);
            menuRecordTxt.text = record.ToString();
        }
        _target.DisableBarriers(gameLevel);
        gameCanvas.SetActive(false);
        menuCanvas.SetActive(true);
    }
}
