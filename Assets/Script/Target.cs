using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public GameObject[] barriers;
    private Coroutine rotateCor;

    public void CreatBarriers(int gameLevel) //0 easy - 1 normal - 2hard
    {
        for (int i = 0; i <= gameLevel; i++)
        {
            barriers[i].SetActive(true);
        }

        rotateCor = StartCoroutine(RotateBarriers(gameLevel));
    }
    
    IEnumerator RotateBarriers(int gameLevel)
    {
        if (gameLevel == 0)
            while (true)
            {
                barriers[0].transform.Rotate(0, 0, 1, Space.World);
                yield return new WaitForFixedUpdate();
            }
        else if (gameLevel == 1)
            while (true)
            {
                barriers[0].transform.Rotate(0, 0, 1, Space.World);
                barriers[1].transform.Rotate(0, 0, 1, Space.World);
                yield return new WaitForFixedUpdate();
            }

        else if (gameLevel == 2)
            while (true)
            {
                barriers[0].transform.Rotate(0, 0, 1, Space.World);
                barriers[1].transform.Rotate(0, 0, 1, Space.World);
                barriers[2].transform.Rotate(0, 0, 1, Space.World);
                yield return new WaitForFixedUpdate();
            }
    }

    public void DisableBarriers(int gameLevel)
    {
        StopCoroutine(rotateCor);
        for (int i = 0; i <= gameLevel; i++)
        {
            barriers[i].SetActive(false);
        }
    }
}
