using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : InitManager
{
    [SerializeField] private GameObject[] barriers;
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject cannon;
    [SerializeField] private GameObject gameOver;
    private List<GameObject> Balls = new List<GameObject>();
    private Coroutine rotateCor;
    private int record;
    private int lastRecord;
    private bool canFireBall = false;
    private float touchBeginTime;

    private void Start()
    {
        Manager.Instance.StartNewGame += StartNewGame;
        Manager.Instance.GameOver += GameOver;
        Manager.Instance.RecordUp += RecordUp;
        Manager.Instance.GetRecord += GetRecord;
        Manager.Instance.ResetLastRecord += ResetLastRecord;
        lastRecord = 0;
        isInited = true;
    }

    void StartNewGame()
    {
        for (int i = 0; i <= Manager.Instance.gameMode; i++)
        {
            barriers[i].SetActive(true);
            barriers[i].transform.eulerAngles = Vector3.zero;
        }
        rotateCor = StartCoroutine(RotateBarriers(Manager.Instance.gameMode));

        record = (lastRecord * 60) / 100;
        Manager.Instance.UpdateGameRecord(record);
        Manager.Instance.Delay(3,(() => canFireBall = true));
    }

    void GameOver()
    {
        lastRecord = record;
        StopCoroutine(rotateCor);
        canFireBall = false;
        gameOver.SetActive(true);
        Manager.Instance.PlayEfx("over");
        Manager.Instance.Delay(1, (() =>
        {
            for (int i = 0; i < Balls.Count; i++)
            {
                Balls[i].GetComponent<Animator>().enabled = false;
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
            for (int i = 0; i <= barriers.Length; i++)
            {
                barriers[i].SetActive(false);
            }
        }));
    }

    void RecordUp(GameObject ball)
    {
        record++;
        ball.GetComponent<Animator>().enabled = false;
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

    void ResetLastRecord()
    {
        lastRecord = 0;
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
            else if (Application.platform == RuntimePlatform.Android)
            {
                if (Input.touchCount > 0 && Input.GetTouch(0).phase.Equals(TouchPhase.Began))
                {
                    touchBeginTime = Time.time;
                }

                else if (Input.GetTouch(0).phase.Equals(TouchPhase.Ended))
                {
                    if (Time.time - touchBeginTime < 0.5f)
                    {
                        var ball = PollingSystem.Instance.GetFromPool("Ball");
                        ball.transform.parent = target.transform;
                        ball.transform.localScale = Vector3.one;
                        ball.transform.position = cannon.transform.parent.Find("Ball").transform.position;
                        ball.GetComponent<Image>().color = Color.white;
                        ball.SetActive(true);
                        Manager.Instance.PlayEfx("ball");
                        if (cannon.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("idle"))
                            cannon.GetComponent<Animator>().SetTrigger("fire");
                        Balls.Add(ball);
                    }

                    touchBeginTime = 0;
                }
            }
        }
    }
}
