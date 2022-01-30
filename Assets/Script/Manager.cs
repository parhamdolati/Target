using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager Instance;
    [SerializeField] private UiManager UiManager;
    [SerializeField] private SFXManager SfxManager;
    [SerializeField] private GameManager GameManager;
    [SerializeField] private PollingSystem PollingSystem;

    public Action<String> PlayEfx;
    public Action PlaySplash;
    public Action StartNewGame;
    public Action<GameObject> RecordUp;
    public Action<int> UpdateGameRecord;
    public Action GameOver;
    public Action GoToMenu;

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
}
