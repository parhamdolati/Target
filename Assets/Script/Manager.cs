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

    private bool canFire;
    
    public Action<String> PlayEfx;
    public Action PlaySplash;
    public Action StartNewGame;
    public Action RecordUp;
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

    public void PermitionToFire()
    {
        canFire = true;
    }

    private void Update()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            if (canFire)
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    /*GameObject _ball = Instantiate(ball, ball.transform.position, Quaternion.identity);
                    _ball.transform.parent = target.transform;
                    _ball.transform.localScale = Vector3.one;
                    _ball.SetActive(true);
                    PlayEfx("ball");
                    if (cannon.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("idle"))
                        cannon.GetComponent<Animator>().SetTrigger("fire");*/
                    
                }
            }
        }
    }
}
