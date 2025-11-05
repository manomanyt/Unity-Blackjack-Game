using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource sfxSource;        

    public AudioClip cardSound;
    public AudioClip winSound;
    public AudioClip coinSound;

    public void PlayCardSound()
    {
        sfxSource.PlayOneShot(cardSound);
    }

    public void PlayWinSound()
    {
        sfxSource.PlayOneShot(winSound);
    }

    public void PlayCoinSound()
    {
        sfxSource.PlayOneShot(coinSound);
    }
}
