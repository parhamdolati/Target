using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

public class Manager : MonoBehaviour
{
    public static Manager Instance;
    
    [SerializeField] private UiManager UiManager;
    [SerializeField] private SFXManager SfxManager;
    [SerializeField] private GameManager GameManager;
    [SerializeField] private PollingSystem PollingSystem;
    [SerializeField] private LeaderboardManager LeaderboardManager;

    public Action<String> PlayEfx;
    public Action PlaySplash;
    public Action StartNewGame;
    public Action<GameObject> RecordUp;
    public Action<int> UpdateGameRecord;
    public Action GameOver;
    public Action GoToMenu;
    public Func<int> GetRecord;
    public Func<Task> ConnectToServer;

    public bool triggerOnBarrier = false;
    public int gameMode; // 0: easy --- 1: normal --- 2: hard 

    private void Awake()
    {
        Instance = this;
        StartCoroutine(ChekAllChildsBeReady());
    }

    IEnumerator ChekAllChildsBeReady()
    {
        yield return new WaitUntil((() => AllChildsIsReady()));
        PlaySplash();
    }

    bool AllChildsIsReady()
    {
        var childs = GetComponentsInChildren<InitManager>();
        for (int i = 0; i < childs.Length; i++)
        {
            if (!childs[i].isInited) return false;
        }

        return true;
    }

    public void Delay(float timer,Action OnComplete)
    {
        StartCoroutine(DelayIE(timer, OnComplete));
    }
    
    IEnumerator DelayIE(float timer,Action OnComplete)
    {
        yield return new WaitForSeconds(timer);
        OnComplete?.Invoke();
    }
}

public enum GameMode
{
    easy,
    normal,
    hard
}
