using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Path_Player : MonoBehaviour
{
    public GameObject[] twoPoints;
    public List<Player> playersOnPath = new List<Player>();
    public bool myStar;
    public bool isOut;
    public void GotiOut()
    {
        if (myStar != true)
        {
            if (playersOnPath.Count == 2)
            {
                if (playersOnPath[0].dice.diceName != playersOnPath[1].dice.diceName)
                {
                    if (playersOnPath[0].dice.gameManager.vibration == 1)
                    {
                        Handheld.Vibrate();
                    }
                    playersOnPath[0].dice.gameManager.musicSound.UpdatePlayerCutSound();
                    playersOnPath[0].dice.gameManager.emojiGotiCut[playersOnPath[0].dice.diceCheck].sprite =
                        playersOnPath[0].dice.gameManager.smillingCrying[0];
                    playersOnPath[1].dice.gameManager.emojiGotiCut[playersOnPath[0].dice.diceCheck].sprite =
                        playersOnPath[1].dice.gameManager.smillingCrying[1];
                    playersOnPath[0].dice.gameManager.emojiGotiCut[playersOnPath[0].dice.diceCheck].gameObject.SetActive(true);
                    playersOnPath[1].dice.gameManager.emojiGotiCut[playersOnPath[1].dice.diceCheck].gameObject.SetActive(true);
                    isOut = true;
                    playersOnPath[0].pathLeft = 57;
                    playersOnPath[0].pathMoved = 0;
                    playersOnPath[0].isOut = false;
                    playersOnPath[0].dice.gameManager.parentPlayer.allPlayerList[playersOnPath[0].myDiceValue].Remove(playersOnPath[0]);
                    playersOnPath[0].transform.DOMove(playersOnPath[0].startingPosition, 1);
                    playersOnPath[0] = playersOnPath[1];
                    playersOnPath[0].dice.canRoll = true;
                    playersOnPath[0].dice.diceSpriteRenderer.sprite = playersOnPath[0].dice.gameManager.diceDefaultSprites[playersOnPath[0].dice.gameManager.selectedDice];
                    playersOnPath.Remove(playersOnPath[1]);
                }
                if (playersOnPath.Count == 2)
                {
                    playersOnPath[0].transform.position = twoPoints[0].transform.position;
                    playersOnPath[1].transform.position = twoPoints[1].transform.position;
                }
                if (playersOnPath.Count == 3)
                {
                    playersOnPath[0].transform.position = twoPoints[0].transform.position;
                    playersOnPath[1].transform.position = twoPoints[1].transform.position;
                    playersOnPath[2].transform.position = twoPoints[2].transform.position;
                }
                if (playersOnPath.Count == 4)
                {
                    playersOnPath[0].transform.position = twoPoints[0].transform.position;
                    playersOnPath[1].transform.position = twoPoints[1].transform.position;
                    playersOnPath[2].transform.position = twoPoints[2].transform.position;
                    playersOnPath[3].transform.position = twoPoints[3].transform.position;
                }
            }
        }
    }
    void FixedUpdate()
    {
        if (playersOnPath.Count == 1)
        {
            playersOnPath[0].transform.position = this.transform.position;
        }
        if (playersOnPath.Count == 2)
        {
            playersOnPath[0].transform.position = twoPoints[0].transform.position;
            playersOnPath[1].transform.position = twoPoints[1].transform.position;
        }
        if (playersOnPath.Count == 3)
        {
            playersOnPath[0].transform.position = twoPoints[0].transform.position;
            playersOnPath[1].transform.position = twoPoints[1].transform.position;
            playersOnPath[2].transform.position = twoPoints[2].transform.position;
        }
        if (playersOnPath.Count == 4)
        {
            playersOnPath[0].transform.position = twoPoints[0].transform.position;
            playersOnPath[1].transform.position = twoPoints[1].transform.position;
            playersOnPath[2].transform.position = twoPoints[2].transform.position;
            playersOnPath[3].transform.position = twoPoints[3].transform.position;
        }
    }
}
