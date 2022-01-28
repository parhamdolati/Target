using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : InitManager
{
    [SerializeField] private GameObject[] barriers;
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
        Manager.Instance.PermitionToFire();
    }

    void GameOver()
    {
        Manager.Instance.GoToMenu();
    }

    void RecordUp()
    {
        record++;
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
}
