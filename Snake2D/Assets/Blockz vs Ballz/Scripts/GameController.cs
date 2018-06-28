using Assets.Blockz_vs_Ballz.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public enum GameState { MENU, GAME, GAMEOVER }
    public static GameState gameState;

    [Header("Managers")]
    public SnakeMovement SM;
    public BlocksManager BM;

    [Header("Canvas Groups")]
    public CanvasGroup MENU_CG;
    public CanvasGroup GAME_CG;
    public CanvasGroup GAMEOVER_CG;
    public CanvasGroup ADS_CG;
    public Text MoneyGameOver;

    [Header("Score Management")]
    public Text ScoreText;
    public Text MenuScoreText;
    public Text BestScoreText;
    public static int SCORE;
    public static int BESTSCORE;
    public static int MONEY;
    [Header("Some Bool")]
    bool speedAdded;

    public AudioSource MusicBackgroundOut;
    public AudioClip SoundBackgroundOut;

    public AudioSource MusicButton;
    public AudioClip SoundButton;

    public AudioSource MusicPlay;
    public AudioClip SoundButtonPlay;

    public AudioSource MusicBackgroundIn;
    public AudioClip SoundBackgroundIn;

    public GameObject RewardPanel;

    // Use this for initialization
    void Start()
    {

        MusicBackgroundOut.clip = SoundBackgroundOut;
        MusicButton.clip = SoundButton;
        MusicPlay.clip = SoundButtonPlay;
        MusicBackgroundIn.clip = SoundBackgroundIn;
        MusicBackgroundOut.Play();

        //Initially, set the menu and Score is null
        SetMenu();

        //Initialize some booleans
        speedAdded = false;

        //Load data game
        SCORE = 0;
        BESTSCORE = PlayerPrefs.GetInt("BESTSCORE");
        print("BESTSCORE GAME: " + BESTSCORE);
        MONEY = PlayerPrefs.GetInt("MONEY");
        print("MONEY GAME: " + MONEY);

        if (!PlayerPrefs.HasKey("Ads"))
        {
            PlayerPrefs.SetInt("Ads", 0);
        }
        else PlayerPrefs.SetInt("Ads", PlayerPrefs.GetInt("Ads") + 1);

    }

    // Update is called once per frame
    void Update()
    {

        //Update the score text
        ScoreText.text = SCORE + "$";
        MenuScoreText.text = MONEY + "$";

        //Update the Best Score and the text
        if (SCORE > BESTSCORE)
            BESTSCORE = SCORE;

        BestScoreText.text = BESTSCORE + "";

        if (!speedAdded && SCORE > 150)
        {
            SM.speed++;
            speedAdded = true;
        }
    }

    public void SetMenu()
    {
        //Set the GameState
        gameState = GameState.MENU;

        //Manage Canvas Groups
        EnableCG(MENU_CG);
        DisableCG(GAME_CG);
        DisableCG(GAMEOVER_CG);
        DisableCG(ADS_CG);
    }

    public void SetGame()
    {
        //Set the GameState
        gameState = GameState.GAME;

        //Manage Canvas Groups
        EnableCG(GAME_CG);
        DisableCG(MENU_CG);
        DisableCG(GAMEOVER_CG);

        //Reset score
        SCORE = 0;
    }

    public void SetGameover()
    {
        gameState = GameState.GAMEOVER;
        MoneyGameOver.text = SCORE.ToString();
        EnableCG(ADS_CG);
        DisableCG(GAME_CG);
        DisableCG(GAMEOVER_CG);
    }

    public void OnButtonClipClick()
    {
        EnableCG(MENU_CG);
        DisableCG(ADS_CG);
        ShowRewardedAd();
        destroyScence();
    }

    public void OnButtonSkipClick()
    {
        DisableCG(ADS_CG);
        EnableCG(MENU_CG);
        destroyScence();
    }

    void destroyScence()
    {
        //Delete all the objects
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Box"))
        {
            Destroy(g);
        }

        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Snake"))
        {
            Destroy(g);
        }

        foreach (GameObject g in GameObject.FindGameObjectsWithTag("SimpleBox"))
        {
            Destroy(g);
        }

        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Bar"))
        {
            Destroy(g);
        }


        //Spawn the new body parts
        SM.SpawnBodyParts();

        //Reset the previous snake pos to spawn barrier correctly
        BM.SetPreviousSnakePosAfterGameover();

        //Reset the Speed
        speedAdded = false;
        SM.speed = 3;

        // Saving a saved game.
        MONEY += SCORE;
        PlayerPrefs.SetInt("MONEY", MONEY);
        PlayerPrefs.SetInt("BESTSCORE", BESTSCORE);

        //Reset the Simple Blocks List
        BM.SimpleBoxPositions.Clear();
    }

    public void EnableCG(CanvasGroup cg)
    {
        cg.alpha = 1;
        cg.blocksRaycasts = true;
        cg.interactable = true;
    }

    public void DisableCG(CanvasGroup cg)
    {
        cg.alpha = 0;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }

    public void OpenShop()
    {
        SceneManager.LoadScene("Shop");
    }

    public void OnButtonDown()
    {
        MusicButton.PlayOneShot(SoundButton);
    }

    public void OnButtonPlayDown()
    {
        MusicBackgroundOut.Pause();
        MusicButton.PlayOneShot(SoundButton);
        MusicPlay.PlayOneShot(SoundButtonPlay);
        MusicBackgroundIn.playOnAwake = SoundBackgroundIn;
        MusicBackgroundIn.Play();
    }

    public void ShowRewardedAd()
    {
        if (Advertisement.IsReady("rewardedVideo"))
        {
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("rewardedVideo", options);
        }
    }
    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                MONEY += 20;
                PlayerPrefs.SetInt("MONEY", MONEY);
                RewardPanel.SetActive(true);
                SceneManager.LoadScene("Main");
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
        }
    }
    
}
