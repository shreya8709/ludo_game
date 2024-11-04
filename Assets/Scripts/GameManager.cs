using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    //public Notification Notification;
    //getting sound script
    public MusicSound musicSound;
    //Parent Player Instance
    public ParentPlayer parentPlayer;

    //dices sprites
    public Sprite[] diceNumSprites;

    //path of players
    public Path_Player[] redPath;
    public Path_Player[] greenPath;
    public Path_Player[] yellowPath;
    public Path_Player[] bluePath;

    public Path_Player[][] allPath;
    //all dice outer circle
    public GameObject[] redOuterCircle;
    public GameObject[] greenOuterCircle;
    public GameObject[] yellowOuterCircle;
    public GameObject[] blueOuterCircle;
    public readonly List<GameObject[]> OuterCircles = new List<GameObject[]>();
    public SpriteRenderer[] emojiGotiCut;

    //all dice
    public Dice[] dices; //dices scripts
    public Sprite diceRollSprite;

    //Canvas Gameobject
    public GameObject[] canvasGameObjects;
    public SpriteRenderer[] playingIdentityLogo;

    // Button of local player change
    public Sprite[] localPlayerChangeButtonSprite; // 0 = No one, 1 = human, 2 = computer
    public Image[] localPlayerChangeButton;
    public Sprite[] humanComputer; // 0 = human, 1 = computer

    //Ludo Board Objects
    public GameObject ludoBoard;
    public GameObject[] diceHolders; //dice holders
    public GameObject[] dicesGameObjects; //dices gameobjects

    //About Gameplay system
    public Sprite[] smillingCrying;
    //Local Player System
    public int _noOfPlayers = 4, _noOfHumans = 4, _noOfComputers;    //default system
    public int totalPlayers;

    //Hands
    public GameObject[] hands;

    //Gameplay Winning System
    public List<String> winnersName;
    public List<TMP_Text> winnersNameText;

    //Text of profile   
    public TMP_Text usernameText;
    public TMP_Text completed_coins_text;
    public int coins;
    public int completed;

    //buying system
    public GameObject[] lockDices;

    //Profile System
    public Image profileImage;
    public Sprite[] allProfileImages;
    public GameObject[] profileImagesBack;
    public GameObject[] selectDice;
    public List<int> alreadyHaveDice;
    public int selectedDice = 0;
    public int selectProfile = 3;

    //dices image
    [FormerlySerializedAs("DicesDefaultSpriteRenderers")] public SpriteRenderer[] dicesDefaultSpriteRenderers;
    public Sprite[] diceDefaultSprites;

    //Text of profile name
    public TMP_Text editProfileName;

    //Gameplay systems 
    public SpriteRenderer[] winningStarsOfDice;
    public Sprite[] winningStars;

    //Rate us star
    public Image[] rateUsStars;
    public Image[] settingFun;
    public Sprite[] onOff;
    public int sound = 1;
    public int notification = 1;
    public int vibration = 1;
    void Awake()
    {
        //default username
        PlayerPrefs.SetString("username", "Player");
        //compatible for all phone
        Application.targetFrameRate = 60;
        //adding all outer circles
        OuterCircles.Add(redOuterCircle);
        OuterCircles.Add(greenOuterCircle);
        OuterCircles.Add(yellowOuterCircle);
        OuterCircles.Add(blueOuterCircle);

        allPath = new Path_Player[4][];
        allPath[0] = redPath;
        allPath[1] = greenPath;
        allPath[2] = yellowPath;
        allPath[3] = bluePath;

        for (int i = 0; i < hands.Length; i++)
        {
            hands[i].SetActive(false);
        }
    }

    public void OnCloseSettingButton() 
    {
        
        if (canvasGameObjects[1].activeInHierarchy)
        {
            CrossOfGameSetting();
        }
        else
        {
            SettingHomeShop(1);
        }
    }

    void Start()   // start 
    {
        //returning save value of setting
        sound = PlayerPrefs.GetInt("sound", 1);
        notification = PlayerPrefs.GetInt("notification", 1);
        vibration = PlayerPrefs.GetInt("vibration", 1);
        settingFun[0].sprite = onOff[sound];
        //settingFun[1].sprite = onOff[notification];
        settingFun[2].sprite = onOff[vibration];
        
        //loading saved username
        
        usernameText.text = Username.ToUpper();
        //setting default profile
        for (int i = 0; i < profileImagesBack.Length; i++)
        {
            profileImagesBack[i].SetActive(false);
        }
        selectProfile = PlayerPrefs.GetInt("profile");
        profileImage.sprite = allProfileImages[selectProfile];
        profileImagesBack[selectProfile].SetActive(true);
        //getting selected dice 
        selectedDice = PlayerPrefs.GetInt("selectedDice");
        //setting default saved dice
        for (int i = 0; i < selectDice.Length; i++)
        {
            selectDice[i].SetActive(false);
        }
        selectDice[selectedDice].SetActive(true);
        for (int i = 0; i < dicesDefaultSpriteRenderers.Length; i++)
        {
            dicesDefaultSpriteRenderers[i].sprite = diceDefaultSprites[selectedDice];
        }
        //loading already have dice
        LoadIntList();
        foreach (var number in alreadyHaveDice)
        {
            lockDices[number - 1].SetActive(false);
        }
        //setting default saved dice
        for (int i = 0; i < selectDice.Length; i++)
        {
            selectDice[i].SetActive(false);
        }
        selectDice[selectedDice].SetActive(true);
        for (int i = 0; i < dicesDefaultSpriteRenderers.Length; i++)
        {
            dicesDefaultSpriteRenderers[i].sprite = diceDefaultSprites[selectedDice];
        }
        //activating all outer circles at first
        DeActivatingAllCircle();
        //instantiating winnersName
        for (int i = 0; i < playingIdentityLogo.Length; i++)
        {
            playingIdentityLogo[i].sprite = humanComputer[0];
        }
    }

    void FixedUpdate()
    {
        //about setting info
        musicSound._soundVolume = sound;
        //about coins
        coins = Coins;
        completed = PlayerPrefs.GetInt("completed");
        completed_coins_text.text = "Completed : " + completed.ToString() + "  |  " + "Coins : " + Coins.ToString();
    }

    public static int Coins
    {
        get { return PlayerPrefs.GetInt("Coins", 10); }
        set { PlayerPrefs.SetInt("Coins", value); }
    }

    public void LocalPlayerSelector(Dice dice)
    {
        _noOfPlayers = 0; _noOfHumans = 0; _noOfComputers = 0;
        //changing image of button while change player to computer or human or no player
        if (dice.diceName == "Reddice")
        {
            localPlayerChangeButton[0].sprite = localPlayerChangeButtonSprite[dice.playerType];
            playingIdentityLogo[0].sprite = humanComputer[dice.playerType];
        }
        else if (dice.diceName == "Greendice")
        {
            localPlayerChangeButton[1].sprite = localPlayerChangeButtonSprite[dice.playerType];
            playingIdentityLogo[1].sprite = humanComputer[dice.playerType];
        }
        else if (dice.diceName == "Yellowdice")
        {
            localPlayerChangeButton[2].sprite = localPlayerChangeButtonSprite[dice.playerType];
            playingIdentityLogo[2].sprite = humanComputer[dice.playerType];
        }
        else if (dice.diceName == "Bluedice")
        {
            localPlayerChangeButton[3].sprite = localPlayerChangeButtonSprite[dice.playerType];
            playingIdentityLogo[3].sprite = humanComputer[dice.playerType];
        }
        // 0 = No one, 1 = human, 2 = computer checking player
        if (dice.playerType == 0) { dice.playerType = 1; }
        else if (dice.playerType == 1) { dice.playerType = 2; }
        else { dice.playerType = 0; }
        //checking if all players are selected or not and all player are computer or not
        for (int i = 0; i < dices.Length; i++)
        {
            if (dices[i].playerType == 0 || dices[i].playerType == 2) { _noOfPlayers++; }
            if (dices[i].playerType == 1) { _noOfHumans++; }
            if (dices[i].playerType == 2) { _noOfComputers++; }
        }
        if (_noOfPlayers == 4) { canvasGameObjects[2].SetActive(false); }
        else if (_noOfHumans == 1 && _noOfComputers == 0) { canvasGameObjects[2].SetActive(false); }
        else { canvasGameObjects[2].SetActive(true); }
    }
    public void LocalStart()
    {      
        //gameplay system
        totalPlayers = _noOfHumans + _noOfComputers;
        canvasGameObjects[0].SetActive(false);
        canvasGameObjects[1].SetActive(true);
        ludoBoard.SetActive(true);
        for (int i = 0; i < dices.Length; i++) { if (dices[i].playerType != 0) { diceHolders[i].SetActive(true); } }
        for (int i = 0; i < dices.Length; i++)
        {
            if (dices[i].playerType == 1)
            {
                dicesGameObjects[i].SetActive(true);
                hands[i].SetActive(true); break;
            }
        }
        //getting selected dice 
        selectedDice = PlayerPrefs.GetInt("selectedDice");
        //setting default saved dice
        for (int i = 0; i < selectDice.Length; i++)
        {
            selectDice[i].SetActive(false);
        }
        selectDice[selectedDice].SetActive(true);
        for (int i = 0; i < dicesDefaultSpriteRenderers.Length; i++)
        {
            dicesDefaultSpriteRenderers[i].sprite = diceDefaultSprites[selectedDice];
        }
    }
    public void DeActivatingAllCircle()
    {
        for (int i = 0; i < OuterCircles.Count; i++)
        {
            for (int j = 0; j < OuterCircles[i].Length; j++)
            {
                OuterCircles[i][j].SetActive(false);
            }
        }
        StartCoroutine(relatedtoemoji());
    }
    IEnumerator relatedtoemoji()
    {
        yield return new WaitForSeconds(10f);
        for (int i = 0; i < emojiGotiCut.Length; i++)
        {
            emojiGotiCut[i].gameObject.SetActive(false);
            emojiGotiCut[i].sprite = null;
        }
    }
    //First Scene Bottom Functions
    public void SettingHomeShop(int myValue) // 0 = setting 1 = home 2 = shop
    {
        int randomno = UnityEngine.Random.Range(0, 3);      
        if (myValue == 0)
        {
            canvasGameObjects[3].SetActive(true);           
            canvasGameObjects[5].SetActive(false);
            canvasGameObjects[6].SetActive(false);
        }
        else if (myValue == 1)
        {
            canvasGameObjects[3].SetActive(false);          
            canvasGameObjects[5].SetActive(false);
            canvasGameObjects[6].SetActive(false);
        }
        else if (myValue == 2)
        {
            canvasGameObjects[3].SetActive(false);         
            canvasGameObjects[5].SetActive(true);
            canvasGameObjects[6].SetActive(false);
        }
        else if (myValue == 3)
        {
            canvasGameObjects[3].SetActive(false);        
            canvasGameObjects[5].SetActive(false);
            canvasGameObjects[6].SetActive(true);
        }
    }
    public void BackPanelActive()
    {
        AudioListener.volume = 0;
        canvasGameObjects[8].transform.position = new(10, 10, 0);
        canvasGameObjects[7].SetActive(true);
    }
    public void BackToHomePanel()
    {
        SceneManager.LoadScene(0);
    }
    public void BackToHomeNo()
    {
        AudioListener.volume = 1;
        canvasGameObjects[8].transform.position = new(0, 0, 0);
        canvasGameObjects[7].SetActive(false);
    }

    //buying system
    public void LockDice(int myValue)
    {
        if (coins >= 10)
        {
            coins = coins - 10;
            switch (myValue)
            {
                case 1:
                    lockDices[myValue - 1].SetActive(false);
                    alreadyHaveDice.Add(myValue);
                    SaveIntList();
                    break;
                case 2:
                    lockDices[myValue - 1].SetActive(false);
                    alreadyHaveDice.Add(myValue);
                    SaveIntList();
                    break;
                case 3:
                    lockDices[myValue - 1].SetActive(false);
                    alreadyHaveDice.Add(myValue);
                    SaveIntList();
                    break;
                case 4:
                    lockDices[myValue - 1].SetActive(false);
                    alreadyHaveDice.Add(myValue);
                    SaveIntList();
                    break;
                case 5:
                    lockDices[myValue - 1].SetActive(false);
                    alreadyHaveDice.Add(myValue);
                    SaveIntList();
                    break;
                case 6:
                    lockDices[myValue - 1].SetActive(false);
                    alreadyHaveDice.Add(myValue);
                    SaveIntList();
                    break;
                case 7:
                    lockDices[myValue - 1].SetActive(false);
                    alreadyHaveDice.Add(myValue);
                    SaveIntList();
                    break;
                case 8:
                    lockDices[myValue - 1].SetActive(false);
                    alreadyHaveDice.Add(myValue);
                    SaveIntList();
                    break;
            }
        }
    }
    public void SelectChoose(int myValue)
    {
        for (int i = 0; i < selectDice.Length; i++)
        {
            selectDice[i].SetActive(false);
        }
        switch (myValue)
        {
            case 0:
                selectDice[myValue].SetActive(true);
                PlayerPrefs.SetInt("selectedDice", myValue);
                break;
            case 1:
                selectDice[myValue].SetActive(true);
                PlayerPrefs.SetInt("selectedDice", myValue);
                break;
            case 2:
                selectDice[myValue].SetActive(true);
                PlayerPrefs.SetInt("selectedDice", myValue);
                break;
            case 3:
                selectDice[myValue].SetActive(true);
                PlayerPrefs.SetInt("selectedDice", myValue);
                break;
            case 4:
                selectDice[myValue].SetActive(true);
                PlayerPrefs.SetInt("selectedDice", myValue);
                break;
            case 5:
                selectDice[myValue].SetActive(true);
                PlayerPrefs.SetInt("selectedDice", myValue);
                break;
            case 6:
                selectDice[myValue].SetActive(true);
                PlayerPrefs.SetInt("selectedDice", myValue);
                break;
            case 7:
                selectDice[myValue].SetActive(true);
                PlayerPrefs.SetInt("selectedDice", myValue);
                break;
            case 8:
                selectDice[myValue].SetActive(true);
                PlayerPrefs.SetInt("selectedDice", myValue);
                break;
        }
    }

    public void SaveIntList()
    {
        // Serialize the list to a JSON-like format (e.g., comma-separated string)
        string serializedList = string.Join(",", alreadyHaveDice);

        // Save the serialized list to PlayerPrefs
        PlayerPrefs.SetString("alreadyHaveDiced", serializedList);

        // Save PlayerPrefs to disk (important)
        PlayerPrefs.Save();
    }

    // Function to load the list from PlayerPrefs
    public void LoadIntList()
    {
        // Retrieve the serialized list from PlayerPrefs
        string serializedList = PlayerPrefs.GetString("alreadyHaveDiced");

        // Split the serialized list into individual integers and populate the intList
        string[] intStrings = serializedList.Split(',');
        alreadyHaveDice.Clear(); // Clear the existing list
        foreach (string intString in intStrings)
        {
            int intValue;
            if (int.TryParse(intString, out intValue))
            {
                alreadyHaveDice.Add(intValue);
            }
        }
    }
    public void ChangeProfile()
    {      
        Username = editProfileName.text;
        usernameText.text = Username.ToUpper();
        SettingHomeShop(1);
    }

    public static string Username
    {
        get { return PlayerPrefs.GetString("Username", "Player"); }
        set { PlayerPrefs.SetString("Username", value); }
    }
    public void AddCoins()
    {        
        //coins += 10;
        Coins += 10;
        //PlayerPrefs.SetInt("coins", coins);
    }
    public void ProfileChange(int myValue)
    {
        for (int i = 0; i < profileImagesBack.Length; i++)
        {
            profileImagesBack[i].SetActive(false);
        }
        switch (myValue)
        {
            case 0:
                profileImage.sprite = allProfileImages[myValue];
                profileImagesBack[myValue].SetActive(true);
                PlayerPrefs.SetInt("profile", myValue);
                break;
            case 1:
                profileImage.sprite = allProfileImages[myValue];
                profileImagesBack[myValue].SetActive(true);
                PlayerPrefs.SetInt("profile", myValue);
                break;
            case 2:
                profileImage.sprite = allProfileImages[myValue];
                profileImagesBack[myValue].SetActive(true);
                PlayerPrefs.SetInt("profile", myValue);
                break;
            case 3:
                profileImage.sprite = allProfileImages[myValue];
                profileImagesBack[myValue].SetActive(true);
                PlayerPrefs.SetInt("profile", myValue);
                break;
            case 4:
                profileImage.sprite = allProfileImages[myValue];
                profileImagesBack[myValue].SetActive(true);
                PlayerPrefs.SetInt("profile", myValue);
                break;
            case 5:
                profileImage.sprite = allProfileImages[myValue];
                profileImagesBack[myValue].SetActive(true);
                PlayerPrefs.SetInt("profile", myValue);
                break;
            case 6:
                profileImage.sprite = allProfileImages[myValue];
                profileImagesBack[myValue].SetActive(true);
                PlayerPrefs.SetInt("profile", myValue);
                break;
            case 7:
                profileImage.sprite = allProfileImages[myValue];
                profileImagesBack[myValue].SetActive(true);
                PlayerPrefs.SetInt("profile", myValue);
                break;
            case 8:
                profileImage.sprite = allProfileImages[myValue];
                profileImagesBack[myValue].SetActive(true);
                PlayerPrefs.SetInt("profile", myValue);
                break;
        }
    }
    
    public void SettingOfGame()
    {
        canvasGameObjects[3].SetActive(true);
        AudioListener.volume = 0;
        canvasGameObjects[8].transform.position = new(10, 10, 0);
        //canvasGameObjects[11].SetActive(true);
    }
    public void CrossOfGameSetting()
    {
        canvasGameObjects[3].SetActive(false);
        AudioListener.volume = 1;
        canvasGameObjects[8].transform.position = new(0, 0, 0);
        //canvasGameObjects[11].SetActive(false);
    }
    
    public void SettingFunction(int value)
    {
        switch (value)
        {
            case 0:
                if (settingFun[0].sprite == onOff[1])
                {
                    settingFun[0].sprite = onOff[0];
                    sound = 0;
                    PlayerPrefs.SetInt("sound", sound);
                }
                else
                {
                    settingFun[0].sprite = onOff[1];
                    sound = 1;
                    PlayerPrefs.SetInt("sound", sound);
                }
                break;
            case 1:
                if (settingFun[1].sprite == onOff[1])
                {
                    settingFun[1].sprite = onOff[0];
                    notification = 0;
                    PlayerPrefs.SetInt("notification", notification);
                }
                else
                {
                    notification = 1;
                    settingFun[1].sprite = onOff[1];
                    PlayerPrefs.SetInt("notification", notification);
                }
                break;
            case 2:
                if (settingFun[2].sprite == onOff[1])
                {
                    vibration = 0;
                    settingFun[2].sprite = onOff[0];
                    PlayerPrefs.SetInt("vibration", vibration);
                }
                else
                {
                    vibration = 1;
                    settingFun[2].sprite = onOff[1];
                    PlayerPrefs.SetInt("vibration", vibration);
                }
                break;
        }
    }
}
