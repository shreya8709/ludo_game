using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentPlayer : MonoBehaviour
{
    public List<Player> redPlayers;
    public List<Player> greenPlayers;
    public List<Player> yellowPlayers;
    public List<Player> bluePlayers;

    public Player[] redPlayersArray;
    public Player[] greenPlayersArray;
    public Player[] yellowPlayersArray;
    public Player[] bluePlayersArray;

    public Player[] allPlayers;
    public List<List<Player>> allPlayerList;
    void Awake()
    {
        allPlayerList = new List<List<Player>>();
        allPlayerList.Add(redPlayers);
        allPlayerList.Add(greenPlayers);
        allPlayerList.Add(yellowPlayers);
        allPlayerList.Add(bluePlayers);
    }
}
