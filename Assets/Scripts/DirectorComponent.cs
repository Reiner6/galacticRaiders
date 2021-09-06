using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
public enum PlayableSelector
{
    mainMenuToGameplay,
    gameOverToGameplay,
    gameOverToMainMenu,
    gameplayToGameOver,
    mainMenuToCredits,
    creditsToMainMenu

}
public class DirectorComponent : MonoBehaviour
{
    public PlayableAsset mainMenuIn;
    public PlayableAsset mainMenuToGameplay;
    public PlayableAsset gameOverToGameplay;
    public PlayableAsset gameplayToGameOver;
    public PlayableAsset gameOverToMainMenu;
    public PlayableAsset mainMenuToCredits;
    public PlayableAsset creditsToMainMenu;
    PlayableDirector myPlayableDirector;

    private void Awake()
    {
        Init();
    }
    /// <summary>
    /// Gets components and plays initial timeline
    /// </summary>
    void Init()
    {
        myPlayableDirector = GetComponent<PlayableDirector>();

        myPlayableDirector.playableAsset = mainMenuIn;

        myPlayableDirector.Play();
    }
    /// <summary>
    ///when called plays a timeline
    /// </summary>
    public void DirectorCall(PlayableSelector value)
    {
        switch (value)
        {
            case PlayableSelector.mainMenuToGameplay:
                myPlayableDirector.playableAsset = mainMenuToGameplay;
                break;
            case PlayableSelector.gameOverToGameplay:

                myPlayableDirector.playableAsset = gameOverToGameplay;
                break;
            case PlayableSelector.gameOverToMainMenu:
                myPlayableDirector.playableAsset = gameOverToMainMenu;
                break;
            case PlayableSelector.gameplayToGameOver:
                myPlayableDirector.playableAsset = gameplayToGameOver;
                break;
               case PlayableSelector.mainMenuToCredits:
                myPlayableDirector.playableAsset = mainMenuToCredits;
                break;
               case PlayableSelector.creditsToMainMenu:
                myPlayableDirector.playableAsset = creditsToMainMenu;
                break;
            default:
                break;
        }
        myPlayableDirector.Play();
    }

    public PlayableDirector MyPlayableDirector { get { return myPlayableDirector; } }
}