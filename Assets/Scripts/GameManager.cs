using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    mainMenu,
    gamePlay,
    gameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    GameState myGameState;
    [SerializeField]
    int myScore;
    EnemyHandler myEnemyHandler;
    DirectorComponent myDirectorComponent;
    CharacterController myCharacterController;
    MusicComponent myMusicComponent;
    float myShipPositionYValue;

    public Text[] myScoreLabels;

    public LevelData[] myLevelDatas;

    public GameObject MainMenu;
    public GameObject GamePlay;
    public GameObject GameOver;


    bool creditsOn = false;
    private void Awake()
    {
        instance = this;
        Init();
    }

    private void Update()
    {
        switch (myGameState)
        {
            case GameState.mainMenu:
                if (!MainMenu.activeInHierarchy)
                {
                    MainMenu.SetActive(true);
                    GamePlay.SetActive(false);
                    GameOver.SetActive(false);
                }
                break;
            case GameState.gamePlay:
                if (!GamePlay.activeInHierarchy)
                {
                    MainMenu.SetActive(false);
                    GamePlay.SetActive(true);
                    GameOver.SetActive(false);
                }
                if (myEnemyHandler.LowestShipPosition < myShipPositionYValue)
                {
                    myGameState = GameState.gameOver;

                    myEnemyHandler.ClearEnemies();
                    myDirectorComponent.DirectorCall(PlayableSelector.gameplayToGameOver);

                }
                else if (myEnemyHandler.AreThereShipsLeft)
                {
                    myGameState = GameState.gameOver;

                    myEnemyHandler.ClearEnemies();
                    myDirectorComponent.DirectorCall(PlayableSelector.gameplayToGameOver);
                }
                break;
            case GameState.gameOver:
                if (!GameOver.activeInHierarchy)
                {
                    MainMenu.SetActive(false);
                    GamePlay.SetActive(false);
                    GameOver.SetActive(true);
                }
                break;
            default:
                break;
        }
    }
    
    /// <summary>
    ///  Gets components at awake
    /// </summary>
    void Init()
    {
        myScore = 0;

        myShipPositionYValue = FindObjectOfType<CharacterController>().myShip.transform.position.y;

        myDirectorComponent = GetComponentInChildren<DirectorComponent>();
        myMusicComponent = GetComponentInChildren<MusicComponent>();

        myCharacterController = FindObjectOfType<CharacterController>();
    }

    /// <summary>
    ///  Gets called to update score values
    /// </summary>
    public void UpdateScore(int value)
    {

        if (myGameState == GameState.gameOver)
            return;
        myScore += value;

        for (int i = 0; i < myScoreLabels.Length; i++)
        {
            myScoreLabels[i].text = "Score: " + myScore;
        }
    }

    /// <summary>
    ///  Sets LevelDatas
    /// </summary>
    public void SelectData(int i)
    {
        UIComponent.instance.ClickSound();
        myDirectorComponent.DirectorCall(PlayableSelector.mainMenuToGameplay);
        myEnemyHandler.currentLevel = i;
        myEnemyHandler.MySpawnValues = myLevelDatas[i].SpawnValue;
        myEnemyHandler.Speed = myLevelDatas[i].speed;
        StartCoroutine(myMusicComponent.StartClips(myLevelDatas[i].introClip, myLevelDatas[i].loopClip));
        StartCoroutine(WaitForDirectorToOutMenu());
    }

    /// <summary>
    ///  Waits for timeline from MainMenu to Gameplay, to spawn the enemies and give control to player
    /// </summary>
    IEnumerator WaitForDirectorToOutMenu()
    {
        yield return null;
        while (myDirectorComponent.MyPlayableDirector.state == UnityEngine.Playables.PlayState.Playing)
        {
            yield return null;
        }
        myEnemyHandler.Init();
        myCharacterController.Init();
        myGameState = GameState.gamePlay;
    }

    /// <summary>
    ///  From gameover, it restart the level
    /// </summary>
    public void Restart()
    {
        UIComponent.instance.ClickSound();
        myDirectorComponent.DirectorCall(PlayableSelector.gameOverToGameplay);
        StartCoroutine(WaitForRestart());
    }
    /// <summary>
    ///  Waits for timeline from game over to Gameplay, to spawn the enemies and give control to player
    /// </summary>
    IEnumerator WaitForRestart()
    {
        yield return null;
        while (myDirectorComponent.MyPlayableDirector.state == UnityEngine.Playables.PlayState.Playing)
        {
            yield return null;
        }
        myEnemyHandler.Init();
        myCharacterController.Init();
        myGameState = GameState.gamePlay;
    }
    /// <summary>
    ///  From gameover to main menu
    /// </summary>
    public void ReturnMainMenu()
    {
        UIComponent.instance.ClickSound();
        myDirectorComponent.DirectorCall(PlayableSelector.gameOverToMainMenu);

        StartCoroutine(WaitForDirectorToInMenu());
    }

    IEnumerator WaitForDirectorToInMenu()
    {
        yield return null;
        while (myDirectorComponent.MyPlayableDirector.state == UnityEngine.Playables.PlayState.Playing)
        {
            yield return null;
        }
        myGameState = GameState.mainMenu;
    }

    /// <summary>
    ///  Shows credits from main menu
    /// </summary>
    public void Credits()
    {
        creditsOn = !creditsOn;

        if (creditsOn)

            myDirectorComponent.DirectorCall(PlayableSelector.mainMenuToCredits);
        else
            myDirectorComponent.DirectorCall(PlayableSelector.creditsToMainMenu);
    }

    public EnemyHandler EnemyHandler { get { return myEnemyHandler; } set { myEnemyHandler = value; } }
    public GameState MyGameState { get { return myGameState; } }
}