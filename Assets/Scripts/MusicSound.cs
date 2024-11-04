using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSound : MonoBehaviour
{
    public AudioSource diceAudio;
    public AudioSource playerMoveAudio;
    public AudioSource playerCutAudio;
    public AudioSource winAudio;
    public AudioSource gameOverAudio;
    public float _soundVolume = 1f;
    //about explosion
    public GameObject explosion;

    // Update is called once per frame
    void FixedUpdate()
    {
        diceAudio.volume = _soundVolume;
        playerMoveAudio.volume = _soundVolume;
        playerCutAudio.volume = _soundVolume;
        winAudio.volume = _soundVolume;
    }
    public void UpdateDiceSound()
    {
        diceAudio.Play();
    }
    public void UpdatePlayerMoveSound()
    {
        playerMoveAudio.Play();
    }
    public void UpdatePlayerCutSound()
    {
        playerCutAudio.Play();
    }
    public void UpdateWinSound()
    {
        winAudio.Play();
    }
    public void UpdategameOverSound()
    {
        gameOverAudio.Play();
    }
    public void explose()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
    }
}
