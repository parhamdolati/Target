using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;

public class GameManager : InitManager
{
    [SerializeField] private GameObject[] barriers;
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject cannon;
    [SerializeField] private GameObject gameOver;
    private List<GameObject> Balls = new List<GameObject>();
    private Coroutine rotateCor;
    private int record;
    private bool canFireBall = false;

    private void Start()
    {
        Manager.Instance.StartNewGame += StartNewGame;
        Manager.Instance.GameOver += GameOver;
        Manager.Instance.RecordUp += RecordUp;
        Manager.Instance.GetRecord += GetRecord;
        isInited = true;
    }

    void StartNewGame()
    {
        for (int i = 0; i <= PlayerPrefs.GetInt("gameLevel"); i++)
        {
            barriers[i].SetActive(true);
            barriers[i].transform.eulerAngles = Vector3.zero;
        }
        rotateCor = StartCoroutine(RotateBarriers(Manager.Instance.gameMode));

        record = 0;
        Manager.Instance.UpdateGameRecord(record);
        Manager.Instance.Delay(3,(() => canFireBall = true));
    }

    void GameOver()
    {
        StopCoroutine(rotateCor);
        canFireBall = false;
        gameOver.SetActive(true);
        Manager.Instance.PlayEfx("over");
        Manager.Instance.Delay(1, (() =>
        {
            for (int i = 0; i < Balls.Count; i++)
            {
                PollingSystem.Instance.BackToPool("Ball", Balls[i]);
            }
            Balls.Clear();
            gameOver.SetActive(false);

            switch (Manager.Instance.gameMode)
            {
                case 0:
                    if (record > PlayerPrefs.GetInt("easyRecord"))
                        PlayerPrefs.SetInt("easyRecord", record);
                    break;
                
                case 1:
                    if (record > PlayerPrefs.GetInt("normalRecord"))
                        PlayerPrefs.SetInt("normalRecord", record);
                    break;
                
                case 2:
                    if (record > PlayerPrefs.GetInt("hardRecord"))
                        PlayerPrefs.SetInt("hardRecord", record);
                    break;
            }
            
            Manager.Instance.triggerOnBarrier = false;
            Manager.Instance.GoToMenu();
            
        }));
    }

    void RecordUp(GameObject ball)
    {
        record++;
        PollingSystem.Instance.BackToPool("Ball",ball);
        Balls.Remove(ball);
        Manager.Instance.UpdateGameRecord(record);
    }
    
    IEnumerator RotateBarriers(int gameMode)
    {
        float rotateSpeed = 0.5f;
        if (gameMode == 0)
            while (true)
            {
                barriers[0].transform.Rotate(0, 0, rotateSpeed + record / 50f, Space.World);
                yield return new WaitForFixedUpdate();
            }
        else if (gameMode == 1)
            while (true)
            {
                barriers[0].transform.Rotate(0, 0, rotateSpeed + record / 50f, Space.World);
                barriers[1].transform.Rotate(0, 0, -rotateSpeed + record / 40f, Space.World);
                yield return new WaitForFixedUpdate();
            }
        else if (gameMode == 2)
            while (true)
            {
                barriers[0].transform.Rotate(0, 0, rotateSpeed + record / 50f, Space.World);
                barriers[1].transform.Rotate(0, 0, -rotateSpeed + record / 40f, Space.World);
                barriers[2].transform.Rotate(0, 0, rotateSpeed + record / 30f, Space.World);
                yield return new WaitForFixedUpdate();
            }
    }

    int GetRecord()
    {
        return record;
    }

    private void Update()
    {
        if (canFireBall)
        {
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    var ball = PollingSystem.Instance.GetFromPool("Ball");
                    ball.transform.parent = target.transform;
                    ball.transform.localScale = Vector3.one;
                    ball.transform.position = cannon.transform.parent.Find("Ball").transform.position;
                    ball.SetActive(true);
                    Manager.Instance.PlayEfx("ball");
                    if (cannon.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("idle"))
                        cannon.GetComponent<Animator>().SetTrigger("fire");
                    Balls.Add(ball);
                }
            }
        }
    }
}
