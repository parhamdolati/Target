using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    public AudioSource music, efx;
    public AudioClip[] efxs;//0 releas ball - 1 click - 2 game over
    
    public void PlayEfx(string situation)
    {
        if (PlayerPrefs.GetInt("music").Equals(1))
        {
            switch (situation)
            {
                case "ball":
                    efx.clip = efxs[0];
                    efx.Play();
                    break;
                case "click":
                    efx.clip = efxs[1];
                    efx.Play();
                    break;
                case "over":
                    efx.clip = efxs[2];
                    efx.Play();
                    break;
            }
        }
    }
}
