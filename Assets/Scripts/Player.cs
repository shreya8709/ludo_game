using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
public class Player : MonoBehaviour
{
    public Dice dice;
    //player name
    public String diceName;
    public int pathMoved;
    public int pathLeft = 57;
    public bool isOut;
    public bool pathCompleted;
    //myturn
    public bool isMyTurn;
    //my starting position
    public Vector3 startingPosition;
    // my dice value
    public int myDiceValue;
    //my index 0 1 2 3 players
    public int myIndex;
    void Awake()
    {
        //my starting postition
        startingPosition = transform.position;
    }
    void Start()
    {
        //my current size
        minscale = transform.localScale;
    }
    void OnMouseDown()
    {
        Move();
    }
    public void Move()
    {
        if (isMyTurn)
        {
            //while clicked deactivate all circles 
            dice.gameManager.DeActivatingAllCircle();
            switch (diceName)
            {
                //for red dice
                case "Reddice":
                    PlayerFunction(dice.gameManager.parentPlayer.redPlayers, dice.gameManager.redPath);
                    break;
                //for qreen dice
                case "Greendice":
                    PlayerFunction(dice.gameManager.parentPlayer.greenPlayers, dice.gameManager.greenPath);
                    break;
                //for yellow dice
                case "Yellowdice":
                    PlayerFunction(dice.gameManager.parentPlayer.yellowPlayers, dice.gameManager.yellowPath);
                    break;
                //for blue dice
                case "Bluedice":
                    PlayerFunction(dice.gameManager.parentPlayer.bluePlayers, dice.gameManager.bluePath);
                    break;
            }
        }
    }
    IEnumerator waitforfirstout(Path_Player[] playerPath)
    {
        yield return new WaitForSeconds(0.4f);
        //replay system
        dice.gameManager.hands[dice.myNum].SetActive(true);
        dice.diceSpriteRenderer.sprite = dice.gameManager.diceDefaultSprites[dice.gameManager.selectedDice];
        //activationing dicesprite animator
        if (dice.diceSpriteRenderer.GetComponent<Animator>().enabled != true)
        {
            dice.diceSpriteRenderer.GetComponent<Animator>().enabled = true;
        }
        //adding path on players
        playerPath[pathMoved - 1].playersOnPath.Add(this);
        if (dice.playerType == 2)
        {
            StartCoroutine(Replay());
        }
    }
    void PlayerFunction(List<Player> playersCount, Path_Player[] playerPath)
    {
        if (playersCount.Count == 0)
        {
            if (dice.diceNum == 6)
            {
                //size of player changing
                for (int i = 0; i < dice.gameManager.parentPlayer.allPlayers.Length; i++)
                {
                    dice.gameManager.parentPlayer.allPlayers[i].repeatable = false;
                    dice.gameManager.parentPlayer.allPlayers[i].transform.localScale = minscale;
                }
                dice.gameManager.musicSound.UpdatePlayerMoveSound();
                //moving player
                transform.DOMove(playerPath[0].transform.position, 0.08f);
                playersCount.Add(this);
                pathMoved = 1;
                pathLeft -= 1;
                isOut = true;
                dice.canRoll = true;
                //calling waitforseconds coroutine
                StartCoroutine(waitforfirstout(playerPath));
            }
        }
        else
        {
            if (isOut)
            {
                StartCoroutine(MoveFurther(playerPath));
            }
            if (isOut == false)
            {
                if (dice.diceNum == 6)
                {
                    //size of player changing
                    for (int i = 0; i < dice.gameManager.parentPlayer.allPlayers.Length; i++)
                    {
                        dice.gameManager.parentPlayer.allPlayers[i].repeatable = false;
                        dice.gameManager.parentPlayer.allPlayers[i].transform.localScale = minscale;
                    }
                    dice.gameManager.musicSound.UpdatePlayerMoveSound();
                    //moving player
                    transform.DOMove(playerPath[0].transform.position, 0.5f);
                    playersCount.Add(this);
                    pathMoved = 1;
                    pathLeft -= 1;
                    isOut = true;
                    dice.canRoll = true;
                    //replay system
                    dice.gameManager.hands[dice.myNum].SetActive(true);
                    dice.diceSpriteRenderer.sprite = dice.gameManager.diceDefaultSprites[dice.gameManager.selectedDice];
                    //activationing dicesprite animator
                    if (dice.diceSpriteRenderer.GetComponent<Animator>().enabled != true)
                    {
                        dice.diceSpriteRenderer.GetComponent<Animator>().enabled = true;
                    }
                    //again adding players on path
                    playerPath[pathMoved - 1].playersOnPath.Add(this);
                    if (dice.playerType == 2)
                    {
                        StartCoroutine(Replay());
                    }
                }
            }
        }
    }
    // ReSharper disable Unity.PerformanceAnalysis
    IEnumerator MoveFurther(Path_Player[] pathOfPlayer)
    {
        if (pathLeft >= dice.diceNum)
        {
            //size of player changing
            for (int i = 0; i < dice.gameManager.parentPlayer.allPlayers.Length; i++)
            {
                dice.gameManager.parentPlayer.allPlayers[i].repeatable = false;
                dice.gameManager.parentPlayer.allPlayers[i].transform.localScale = minscale;
            }
            //removing player from previous path
            for (int i = 0; i < pathOfPlayer.Length; i++)
            {
                pathOfPlayer[i].playersOnPath.Remove(this);
            }
            //moving player to next path
            for (int i = pathMoved; i < (pathMoved + dice.diceNum); i++)
            {
                dice.gameManager.musicSound.UpdatePlayerMoveSound();
                transform.DOMove(pathOfPlayer[i].transform.position, 0.5f);
                yield return new WaitForSeconds(0.2f);
            }
            pathMoved += dice.diceNum;
            pathLeft -= dice.diceNum;
            pathOfPlayer[pathMoved - 1].playersOnPath.Add(this);
            pathOfPlayer[pathMoved - 1].GotiOut();
            if (pathOfPlayer[pathMoved - 1].isOut != true)
            {
                if (pathLeft == 0)
                {
                    pathCompleted = true;
                }
                if (pathCompleted != true)
                {
                    if (dice.diceNum == 6)
                    {
                        //on off animator in dice sprite renderer
                        //activationing dicesprite animator
                        if (dice.diceSpriteRenderer.GetComponent<Animator>().enabled != true)
                        {
                            dice.diceSpriteRenderer.GetComponent<Animator>().enabled = true;
                        }
                        dice.canRoll = true;
                        //replay system
                        dice.diceSpriteRenderer.sprite =
                            dice.gameManager.diceDefaultSprites[dice.gameManager.selectedDice];
                        dice.gameManager.hands[dice.myNum].SetActive(true);
                        if (dice.playerType == 2)
                        {
                            StartCoroutine(Replay());
                        }
                    }
                    else if (dice.diceNum != 6)
                    {
                        dice.diceNum = 0;
                        dice.StartCoroutine(dice.ChangeDice());
                    }
                }
                else
                {
                    if (dice.playerType == 2)
                    {
                        StartCoroutine(Replay());
                    }
                    dice.gameManager.musicSound.UpdateWinSound();
                    if (diceName == "Reddice")
                    {
                        dice.gameManager.parentPlayer.redPlayers.Remove(this);
                    }
                    else if (diceName == "Greendice")
                    {
                        dice.gameManager.parentPlayer.greenPlayers.Remove(this);
                    }
                    else if (diceName == "Yellowdice")
                    {
                        dice.gameManager.parentPlayer.yellowPlayers.Remove(this);
                    }
                    else if (diceName == "Bluedice")
                    {
                        dice.gameManager.parentPlayer.bluePlayers.Remove(this);
                    }
                    dice.completedPlayers += 1;
                    dice.canRoll = true;
                    dice.diceSpriteRenderer.sprite = dice.gameManager.diceDefaultSprites[dice.gameManager.selectedDice];
                    if (dice.completedPlayers == 4)
                    {
                        dice.gameManager.musicSound.UpdateWinSound();
                        dice.gameManager.musicSound.explose();
                        Debug.Log(diceName + " is Completed");
                        dice.gameManager.winnersName.Add(this.diceName);
                        dice.playerType = 0;
                        int winnerCount = dice.gameManager.winnersName.Count;
                        switch (winnerCount)
                        {
                            case 1:
                                dice.gameManager.winningStarsOfDice[myDiceValue].sprite =
                                    dice.gameManager.winningStars[0];
                                break;
                            case 2:
                                dice.gameManager.winningStarsOfDice[myDiceValue].sprite =
                                    dice.gameManager.winningStars[1];
                                break;
                        }
                        //checking winners
                        int noofhumanwins = 0;
                        for (int i = 0; i < dice.gameManager.dices.Length; i++)
                        {
                            if (dice.gameManager.dices[i].playerType == 1 &&
                                dice.gameManager.dices[i].completedPlayers == 4)
                            {
                                noofhumanwins += 1;
                            }
                        }
                        if (dice.gameManager._noOfPlayers == noofhumanwins)
                        {
                            //sending sound when completed
                            dice.gameManager.musicSound.UpdategameOverSound();
                            //showing winners
                            for (int i = 0; i < dice.gameManager._noOfPlayers; i++)
                            {
                                dice.gameManager.winnersNameText[i].text = dice.gameManager.winnersName[i];
                                dice.gameManager.winnersNameText[i].gameObject.SetActive(true);
                            }
                            //adding coins
                            dice.gameManager.coins += 5;
                            dice.gameManager.completed += 1;
                            PlayerPrefs.SetInt("completed", dice.gameManager.completed);
                            //PlayerPrefs.SetInt("coins", dice.gameManager.coins);
                            GameManager.Coins += dice.gameManager.coins;

                        }
                        else if (dice.gameManager.totalPlayers == 2 && winnerCount == 1)
                        {
                            dice.gameManager.musicSound.UpdategameOverSound();
                            dice.gameManager.winnersNameText[0].text = dice.gameManager.winnersName[0];
                            dice.gameManager.winnersNameText[0].gameObject.SetActive(true);

                            //this for all so repeated we can make function too
                            //adding coins
                            dice.gameManager.coins += 5;
                            dice.gameManager.completed += 1;
                            PlayerPrefs.SetInt("completed", dice.gameManager.completed);
                            //PlayerPrefs.SetInt("coins", dice.gameManager.coins);
                            GameManager.Coins += dice.gameManager.coins;
                            //activation game over panel
                            dice.gameManager.canvasGameObjects[8].SetActive(false);
                            dice.gameManager.canvasGameObjects[9].SetActive(true);
                            dice.gameManager.canvasGameObjects[2].SetActive(false);
                            
                            Debug.Log("Game over");
                            dice.gameManager.AddCoins();


                        }
                        else if (dice.gameManager.totalPlayers == 3 && winnerCount == 2)
                        {
                            dice.gameManager.musicSound.UpdategameOverSound();
                            dice.gameManager.winnersNameText[0].text = dice.gameManager.winnersName[0];
                            dice.gameManager.winnersNameText[1].text = dice.gameManager.winnersName[1];
                            dice.gameManager.winnersNameText[0].gameObject.SetActive(true);
                            dice.gameManager.winnersNameText[1].gameObject.SetActive(true);

                            //this for all so repeated we can make function too
                            //adding coins
                            dice.gameManager.coins += 5;
                            dice.gameManager.completed += 1;
                            PlayerPrefs.SetInt("completed", dice.gameManager.completed);
                            //PlayerPrefs.SetInt("coins", dice.gameManager.coins);
                            GameManager.Coins += dice.gameManager.coins;
                            //activation game over panel
                            dice.gameManager.canvasGameObjects[8].SetActive(false);
                            dice.gameManager.canvasGameObjects[9].SetActive(true);
                            dice.gameManager.canvasGameObjects[2].SetActive(false);
                            
                            Debug.Log("Game over");
                            dice.gameManager.AddCoins();

                        }
                        else if (dice.gameManager.totalPlayers == 4 && winnerCount == 3)
                        {

                            dice.gameManager.winnersNameText[0].text = dice.gameManager.winnersName[0];
                            dice.gameManager.winnersNameText[1].text = dice.gameManager.winnersName[1];
                            dice.gameManager.winnersNameText[2].text = dice.gameManager.winnersName[2];
                            dice.gameManager.winnersNameText[0].gameObject.SetActive(true);
                            dice.gameManager.winnersNameText[1].gameObject.SetActive(true);
                            dice.gameManager.winnersNameText[2].gameObject.SetActive(true);

                            //this for all so repeated we can make function too
                            //activation game over panel
                            dice.gameManager.canvasGameObjects[8].SetActive(false);
                            dice.gameManager.canvasGameObjects[9].SetActive(true);
                            dice.gameManager.canvasGameObjects[2].SetActive(false);
                            dice.gameManager.AddCoins();

                        }
                        else
                        {
                            dice.StartCoroutine(dice.ChangeDice());
                        }
                    }
                }
            }
            else
            {
                if (dice.playerType == 2)
                {
                    dice.DiceRoll();
                }
                pathOfPlayer[pathMoved - 1].isOut = false;
            }
        }
    }
    //dice replay by computer function
    IEnumerator Replay()
    {
        yield return new WaitForSeconds(0.5f);
        dice.DiceRoll();
        //activationing dicesprite animator
        if (dice.diceSpriteRenderer.GetComponent<Animator>().enabled != true)
        {
            dice.diceSpriteRenderer.GetComponent<Animator>().enabled = true;
        }
        dice.diceSpriteRenderer.sprite = dice.gameManager.diceDefaultSprites[dice.gameManager.selectedDice];
    }

    //scalling the player
    public Vector3 maxscale = new Vector3(1.1f, 1.1f, 1.1f);
    public bool repeatable;
    public float speed = 1.5f;
    public float duration = 0.5f;
    public Vector3 minscale;

    //size change function
    public void SizeChange()
    {
        StartCoroutine(Size());
    }

    //coroutine of size changes
    IEnumerator Size()
    {
        while (repeatable)
        {
            yield return RepeatLerp(minscale, maxscale, duration);
            yield return RepeatLerp(maxscale, minscale, duration);
        }
    }

    //size change lerp
    public IEnumerator RepeatLerp(Vector3 a, Vector3 b, float time)
    {
        float i = 0.0f;
        float rate = (1.0f / time) * speed;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            transform.localScale = Vector3.Lerp(a, b, i);
            yield return null;
        }
    }
}
