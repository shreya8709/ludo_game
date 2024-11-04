using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Dice : MonoBehaviour
{
    public GameManager gameManager;
    //dice information
    public int diceCheck;
    public string diceName;
    public int diceNum;
    public SpriteRenderer diceSpriteRenderer;
    public GameObject diceSpriteObject;
    public GameObject diceAnim;
    //dice is rollable or not checker
    public bool canRoll;
    // player information
    public int playerType; // 0 = No one, 1 = human, 2 = computer
    //value of variable while playing games
    [FormerlySerializedAs("_counterList")] public int counterList;
    int _totalListValue;

    //check my out player
    public int completedPlayers;
    public int myNum;

    //six number in dice
    int _sixCounter;
    public int check;
    public int counterno = 0;
    void OnMouseDown()
    {
        //when dice is clicked, roll the dice
        counterno++;
        DiceRoll();
        //_ads_obj.gameObject.SetActive(false);
    }
    // ReSharper disable Unity.PerformanceAnalysis
    public void DiceRoll()
    {
        //deactivating all dices
        foreach (var t in gameManager.hands)
        {
            t.SetActive(false);
        }
        if (canRoll)
        {
            gameManager.musicSound.UpdateDiceSound();
            canRoll = false;
            //if canroll then rolling start
            diceNum = Random.Range(1, 7);
            _sixCounter += 1;
            if (_sixCounter == 6)
            {
                diceNum = 6;
                _sixCounter = 0;
            }

            Debug.Log("Dice roll " + diceNum);

            diceSpriteObject.SetActive(false);
            diceAnim.SetActive(true);
            //call wait for dice anim function
            StartCoroutine(Waitfordiceanim());
        }
    }

    public void DicerolMy()
    {
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false; 
        foreach (var t in gameManager.hands)
        {
            t.SetActive(false);
        }
        if (canRoll)
        {
            gameManager.musicSound.UpdateDiceSound();
            canRoll = false;
            //if canroll then rolling start
            //diceNum = Random.Range(1, 7);

            diceNum = 6;
            //_sixCounter = 0;
            //}

            Debug.Log("Dice roll " + diceNum);

            diceSpriteObject.SetActive(false);
            diceAnim.SetActive(true);
            //call wait for dice anim function
            StartCoroutine(Waitfordiceanim());
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    IEnumerator Waitfordiceanim()
    {
        //wait for dice animation to finish
        yield return new WaitForSeconds(0.47f);
        if (diceSpriteRenderer.GetComponent<Animator>().enabled != false)
        {
            diceSpriteRenderer.GetComponent<Animator>().enabled = false;
        }
        diceSpriteRenderer.sprite = gameManager.diceNumSprites[diceNum - 1];
        diceSpriteObject.SetActive(true);
        diceAnim.SetActive(false);
        switch (diceName)
        {
            case "Reddice":
                RemainingDiceFunction(gameManager.parentPlayer.redPlayersArray, gameManager.redOuterCircle, gameManager.parentPlayer.redPlayers);
                AfterWait(gameManager.parentPlayer.redPlayers, gameManager.parentPlayer.redPlayersArray, gameManager.redOuterCircle);
                break;
            case "Greendice":
                RemainingDiceFunction(gameManager.parentPlayer.greenPlayersArray, gameManager.greenOuterCircle,
                    gameManager.parentPlayer.greenPlayers);
                AfterWait(gameManager.parentPlayer.greenPlayers, gameManager.parentPlayer.greenPlayersArray, gameManager.greenOuterCircle);
                break;
            case "Yellowdice":
                RemainingDiceFunction(gameManager.parentPlayer.yellowPlayersArray, gameManager.yellowOuterCircle, gameManager.parentPlayer.yellowPlayers);
                AfterWait(gameManager.parentPlayer.yellowPlayers, gameManager.parentPlayer.yellowPlayersArray, gameManager.yellowOuterCircle);
                break;
            case "Bluedice":
                RemainingDiceFunction(gameManager.parentPlayer.bluePlayersArray, gameManager.blueOuterCircle, gameManager.parentPlayer.bluePlayers);
                AfterWait(gameManager.parentPlayer.bluePlayers, gameManager.parentPlayer.bluePlayersArray, gameManager.blueOuterCircle);
                break;
        }
    }
    void RemainingDiceFunction(Player[] playersArray, GameObject[] outerCircles, List<Player> playerCount)     //gameManager.parentPlayer.bluePlayersArray
    {
        for (int i = 0; i < playersArray.Length; i++)
        {
            playersArray[i].isMyTurn = true;
        }
        Debug.Log("Remaining dice function here ");
    }
    // ReSharper disable Unity.PerformanceAnalysis
    void AfterWait(List<Player> playersCount, Player[] playersarray, GameObject[] playerOutCircles)
    {
        if (playersCount.Count == 0)
        {
            if (diceNum != 6)
            {
                StartCoroutine(ChangeDice());
            }
            else
            {
                foreach (var t in playersarray)
                {
                    if (completedPlayers == 0)
                    {
                        t.Move();
                        break;
                    }
                    if (completedPlayers != 0)
                    {
                        if (t.pathLeft >= diceNum)
                        {
                            t.Move();
                            break;
                        }
                    }
                }
            }
        }
        else
        {
            if (diceNum != 6)
            {
                if (playerType == 1 && playersCount.Count == 1)
                {
                    for (int i = 0; i < playersCount.Count; i++)
                    {
                        if (playersCount[i].pathLeft >= diceNum)
                        {
                            playersCount[i].Move();
                            break;
                        }
                        else
                        {
                            StartCoroutine(ChangeDice());
                        }
                    }
                }
                else if (playerType == 1 && playersCount.Count > 1)
                {
                    for (int i = 0; i < playersCount.Count; i++)
                    {
                        if (playersCount[i].pathLeft < diceNum)
                        {
                            counterList++;
                        }
                        _totalListValue = playersCount.Count;
                        if (_totalListValue == counterList)
                        {
                            StartCoroutine(ChangeDice());
                        }
                    }
                    // list 0 = 1       1 = 1
                    for (int i = 0; i < playersCount.Count; i++)
                    {
                        if (playersCount[i].pathLeft >= diceNum && playersCount[i].pathCompleted != true)
                        {
                            playerOutCircles[playersCount[i].myIndex].SetActive(true);
                            playersCount[i].repeatable = true;
                            playersCount[i].SizeChange();
                        }
                    }
                }

                if (playerType == 2 && playersCount.Count == 1)
                {
                    for (int i = 0; i < playersCount.Count; i++)
                    {
                        if (playersCount[i].pathLeft >= diceNum)
                        {
                            playersCount[i].Move();
                            break;
                        }
                        else
                        {
                            StartCoroutine(ChangeDice());
                        }
                    }
                }
                else if (playerType == 2)
                {
                    for (int i = 0; i < playersCount.Count; i++)
                    {
                        if (playersCount[i].pathLeft >= diceNum)
                        {
                            playersCount[i].Move();
                            break;
                        }
                        else
                        {
                            counterList++;
                        }
                        _totalListValue = playersCount.Count;
                        if (_totalListValue == counterList)
                        {
                            StartCoroutine(ChangeDice());
                        }
                    }
                }
            }
            else   //if six come then 
            {
                Debug.Log("if get six on it ");
                //activating all outer circles if dice got 6
                for (int i = 0; i < playersarray.Length; i++)
                {
                    if (playersarray[i].pathLeft >= diceNum && playersarray[i].pathCompleted != true)
                    {
                        playerOutCircles[i].SetActive(true);
                        playersarray[i].repeatable = true;
                        playersarray[i].SizeChange();
                    }
                }
                if (playerType == 1)
                {
                    check = 4 - completedPlayers;
                    if (completedPlayers >= 3 && playersCount.Count == check)
                    {
                        for (int i = 0; i < playersCount.Count; i++)
                        {
                            if (playersCount[i].pathLeft >= diceNum)
                            {
                                playersCount[i].Move();
                                break;
                            }
                            else
                            {
                                StartCoroutine(ChangeDice());
                            }
                        }
                    }
                    else
                    {
                        var count = 0;
                        for (int i = 0; i < playersCount.Count; i++)
                        {
                            if (playersCount[i].pathLeft < diceNum)
                            {
                                count++;
                            }
                        }
                        if (count == playersCount.Count && (4 - completedPlayers - count) == 0)
                        {
                            StartCoroutine(ChangeDice());
                        }
                    }
                }
                if (playerType == 2)
                {
                    check = 4 - completedPlayers;
                    if (playersCount.Count != check)
                    {
                        for (int i = 0; i < playersarray.Length; i++)
                        {
                            if (playersarray[i].pathMoved == 0)
                            {
                                playersarray[i].Move();
                                break;
                            }
                        }
                    }
                    else if (completedPlayers >= 3 && playersCount.Count == check)
                    {
                        for (int i = 0; i < playersCount.Count; i++)
                        {
                            if (playersCount[i].pathLeft >= diceNum)
                            {
                                playersCount[i].Move();
                                break;
                            }
                            else
                            {
                                StartCoroutine(ChangeDice());
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < playersCount.Count; i++)
                        {
                            if (playersCount[i].pathLeft >= diceNum)
                            {
                                playersCount[i].Move();
                                break;
                            }
                        }
                    }
                    if (completedPlayers >= 1)
                    {
                        var count = 0;
                        for (int i = 0; i < playersCount.Count; i++)
                        {
                            if (playersCount[i].pathLeft < diceNum)
                            {
                                count++;
                            }
                        }
                        if (count == playersCount.Count && (4 - completedPlayers - count) == 0)
                        {
                            StartCoroutine(ChangeDice());
                        }
                    }
                }
            }
        }

        
        this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }
    public IEnumerator ChangeDice()
    {
        gameManager.DeActivatingAllCircle();
        //while changing dice other player's player should not be moved
        //foreach (var t in gameManager.parentPlayer.allPlayers)
        //{
        //    t.isMyTurn = false;
        //}
        yield return new WaitForSeconds(0.5f);
        foreach (var t in gameManager.dices)
        {
            t.canRoll = true;
        }
        switch (diceName)
        {
            case "Reddice":
                DiceFunction(0, 1);
                break;
            case "Greendice":
                DiceFunction(1, 2);
                break;
            case "Yellowdice":
                DiceFunction(2, 3);
                break;
            case "Bluedice":
                DiceFunction(3, 0);
                break;
        }     
    }
    // ReSharper disable Unity.PerformanceAnalysis
    void DiceFunction(int me, int other)
    {
        //reseting dice rolling image
        diceSpriteRenderer.sprite = gameManager.diceDefaultSprites[gameManager.selectedDice];
        //first condition
        if (gameManager.dices[other].playerType == 0)
        {
            other++;
            if (other > 3)
            {
                other = 0;
            }
            //second condition
            if (gameManager.dices[other].playerType == 0)
            {
                other++;
                if (other > 3)
                {
                    other = 0;
                }
                //third condition
                if (gameManager.dices[other].playerType == 0)
                {
                    Debug.Log("No Chance !!!");
                }
                else if (gameManager.dices[other].playerType == 1)
                {
                    gameManager.dicesGameObjects[me].SetActive(false);
                    gameManager.dicesGameObjects[other].SetActive(true);
                    gameManager.hands[other].SetActive(true);
                }
                else if (gameManager.dices[other].playerType == 2)
                {
                    gameManager.dicesGameObjects[me].SetActive(false);
                    gameManager.dicesGameObjects[other].SetActive(true);
                    gameManager.hands[other].SetActive(true);
                    gameManager.dices[other].DiceRoll();
                }
            }
            else if (gameManager.dices[other].playerType == 1)
            {
                gameManager.dicesGameObjects[me].SetActive(false);
                gameManager.dicesGameObjects[other].SetActive(true);
                gameManager.hands[other].SetActive(true);
            }
            else if (gameManager.dices[other].playerType == 2)
            {
                gameManager.dicesGameObjects[me].SetActive(false);
                gameManager.dicesGameObjects[other].SetActive(true);
                gameManager.hands[other].SetActive(true);
                gameManager.dices[other].DiceRoll();
            }
        }
        else if (gameManager.dices[other].playerType == 1)
        {
            gameManager.dicesGameObjects[me].SetActive(false);
            gameManager.dicesGameObjects[other].SetActive(true);
            gameManager.hands[other].SetActive(true);
        }
        else if (gameManager.dices[other].playerType == 2)
        {
            gameManager.dicesGameObjects[me].SetActive(false);
            gameManager.dicesGameObjects[other].SetActive(true);
            gameManager.hands[other].SetActive(true);
            gameManager.dices[other].DiceRoll();
        }
        counterList = 0;
        _totalListValue = 0;
        //activationing dicesprite animator
        if (diceSpriteRenderer.GetComponent<Animator>().enabled != true)
        {
            diceSpriteRenderer.GetComponent<Animator>().enabled = true;
        }
    }
}
