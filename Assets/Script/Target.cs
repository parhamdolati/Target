using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private Main _main;
    public GameObject[] barriers;
    private Coroutine rotateCor;

    public void CreatBarriers(int gameLevel) //0 easy - 1 normal - 2hard
    {
        for (int i = 0; i <= gameLevel; i++)
        {
            barriers[i].SetActive(true);
            barriers[i].transform.eulerAngles = Vector3.zero;
        }

        rotateCor = StartCoroutine(RotateBarriers(gameLevel));
    }
    
    IEnumerator RotateBarriers(int gameLevel)
    {
        float rotateSpeed = 0.5f;
        if (gameLevel == 0)
            while (true)
            {
                barriers[0].transform.Rotate(0, 0, rotateSpeed + _main.record / 50f, Space.World);
                yield return new WaitForFixedUpdate();
            }
        else if (gameLevel == 1)
            while (true)
            {
                barriers[0].transform.Rotate(0, 0, rotateSpeed + _main.record / 50f, Space.World);
                barriers[1].transform.Rotate(0, 0, -rotateSpeed + _main.record / 40f, Space.World);
                yield return new WaitForFixedUpdate();
            }
        else if (gameLevel == 2)
            while (true)
            {
                barriers[0].transform.Rotate(0, 0, rotateSpeed + _main.record / 50f, Space.World);
                barriers[1].transform.Rotate(0, 0, -rotateSpeed + _main.record / 40f, Space.World);
                barriers[2].transform.Rotate(0, 0, rotateSpeed + _main.record / 30f, Space.World);
                yield return new WaitForFixedUpdate();
            }
    }

    public void DisableBarriers()
    {
        StopCoroutine(rotateCor);
    }
}
