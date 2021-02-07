using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi.SavedGame;


public class GameManager : MonoBehaviour
{

    private const int COIN_SCORE_AMOUNT = 5;
    private const int MONEY_SCORE_AMOUNT = 50;
    public static GameManager Instance { set; get; }

    public bool IsDead { set; get; }
    private bool isGamestarted = false;
    private PlayerMotor motor;
    public Animator enimyHitPlayer;


    // UI and UI field
    public Animator gameCanvas;
    public Animator TapToStart;
    public Animator StartMenu;
   
    public Text scoreText, coinText, modifierText, moneyText, HighScoreText, coinBank;
    private float score, coinScore, modifierScore, moneyScore, coinB;
    private int lastScore;

    //Death menu

    public Animator deathMenuAnim;
    public Text deadCoinText, deadScoreText, deadMoneyText;

    // google play Services menu 
    


    private void Awake()
    {
        Instance = this;
        modifierScore = 1;
        motor = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMotor>();
        modifierText.text = "x" + modifierScore.ToString("0.0");
        coinText.text = coinScore.ToString("0");
        scoreText.text = scoreText.text = score.ToString("0");
        HighScoreText.text = PlayerPrefs.GetInt("HiScore").ToString();
        coinBank.text = PlayerPrefs.GetInt("coinsAmount").ToString();
        
    }

    private void Update()
    {
        if(mobileInput.Instance.Tap && !isGamestarted)
        {
            isGamestarted = true;
            motor.StartRunning();

            FindObjectOfType<CameraMotor>().IsMoving = true;

            gameCanvas.SetTrigger("Show");
            TapToStart.SetTrigger("Hide");
            StartMenu.SetTrigger("HideStartMenu");
        }
        if (isGamestarted && !IsDead)
        {
            //bump up score
            score += (Time.deltaTime * modifierScore);
            scoreText.text = score.ToString("0");

        }
    }

    public void GetCoin()
    {
        coinScore ++;
        coinText.text = coinScore.ToString("0");
        score += COIN_SCORE_AMOUNT;
        scoreText.text = scoreText.text = score.ToString("0");
    }
    public void GetMoney()
    {
        moneyScore++;
        moneyText.text = moneyScore.ToString("0");
        score += MONEY_SCORE_AMOUNT;
        scoreText.text = scoreText.text = score.ToString("0");
    }

    public void UpdateModifier(float modifierAmount)
    {
        modifierScore = 1.0f + modifierAmount;
      
        modifierText.text = "x" + modifierScore.ToString("0.0");
    }

    public void OnPlayButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");

        Time.timeScale = 1f;

    }

    public void OnDeath()
    {
        IsDead = true;
        deadScoreText.text = score.ToString("0");
        deadCoinText.text = coinScore.ToString("0");
        deathMenuAnim.SetTrigger("Dead");
        gameCanvas.SetTrigger("Hide");
        enimyHitPlayer.SetTrigger("hit");

        coinB = PlayerPrefs.GetInt("coinsAmount") + coinScore;

        PlayerPrefs.SetInt("coinsAmount", (int)coinB);

        // check if this is high score
        if (score > PlayerPrefs.GetInt("HiScore"))
        {
            float s = score;
            if (s % 1 == 0)
                s += 1;
            PlayerPrefs.SetInt("HiScore", (int)s);
        }
        
    }

    public void Mute()
    {
        AudioListener.pause = !AudioListener.pause;
    }
               
    
 }
