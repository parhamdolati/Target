using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : InitManager
{
    [SerializeField] private GameObject[] barriers;
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject cannon;
    private List<GameObject> Balls = new List<GameObject>();
    private Coroutine rotateCor;
    private int record;
    private void Start()
    {
        Manager.Instance.StartNewGame += StartNewGame;
        Manager.Instance.GameOver += GameOver;
        Manager.Instance.RecordUp += RecordUp;
        isInited = true;
    }

    void StartNewGame()
    {
        for (int i = 0; i <= PlayerPrefs.GetInt("gameLevel"); i++)
        {
            barriers[i].SetActive(true);
            barriers[i].transform.eulerAngles = Vector3.zero;
        }
        rotateCor = StartCoroutine(RotateBarriers(PlayerPrefs.GetInt("gameLevel")));

        record = 0;
        Manager.Instance.UpdateGameRecord(record);
    }

    void GameOver()
    {
        for (int i = 0; i < Balls.Count; i++)
        {
            PollingSystem.Instance.BackToPool("Ball",Balls[i]);
            Balls.Remove(Balls[i]);
        }
        Manager.Instance.GoToMenu();
    }

    void RecordUp(GameObject ball)
    {
        record++;
        PollingSystem.Instance.BackToPool("Ball",ball);
        Balls.Remove(ball);
        Manager.Instance.UpdateGameRecord(record);
    }
    
    IEnumerator RotateBarriers(int gameLevel)
    {
        float rotateSpeed = 0.5f;
        if (gameLevel == 0)
            while (true)
            {
                barriers[0].transform.Rotate(0, 0, rotateSpeed + record / 50f, Space.World);
                yield return new WaitForFixedUpdate();
            }
        else if (gameLevel == 1)
            while (true)
            {
                barriers[0].transform.Rotate(0, 0, rotateSpeed + record / 50f, Space.World);
                barriers[1].transform.Rotate(0, 0, -rotateSpeed + record / 40f, Space.World);
                yield return new WaitForFixedUpdate();
            }
        else if (gameLevel == 2)
            while (true)
            {
                barriers[0].transform.Rotate(0, 0, rotateSpeed + record / 50f, Space.World);
                barriers[1].transform.Rotate(0, 0, -rotateSpeed + record / 40f, Space.World);
                barriers[2].transform.Rotate(0, 0, rotateSpeed + record / 30f, Space.World);
                yield return new WaitForFixedUpdate();
            }
    }

    public void StopRotateBarriers()
    {
        StopCoroutine(rotateCor);
    }
    
    private void Update()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                var ball = PollingSystem.Instance.GetFromPool("Ball");
                ball.transform.parent = target.transform;
                ball.transform.localScale = Vector3.one * 2;
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
