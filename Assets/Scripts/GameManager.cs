using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using TMPro;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public enum gameState { start, play, end }
    
    public static GameManager instance;

    [Header("Basic Elements")]
    public CameraFollow camera;
    public PlayerControl playerObject;
    PlayerControl player;

    [Header("Start UI Elements")]
    public GameObject uiStart;

    [Header("Game UI Elements")]
    public GameObject uiGame;
    public GameObject uiGameWorldspace;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI lifeText;
    public Image cooldownBar;

    [Header("End UI Elements")]
    public GameObject uiEnd;
    public TextMeshProUGUI scoreTextEnd;

    [Header("Audio Elements")]
    public AudioSource bgm;
    public AudioClip bgmNotGame;
    public AudioClip bgmInGame;
    public AudioClip sfxLaserPlayer;
    public AudioClip sfxLaserEnemy;
    public AudioClip sfxHitPlayer;
    public AudioClip sfxExplodePlayer;
    public AudioClip sfxExplodeEnemy;
    public AudioClip sfxAddLife;

    [Header("Events")]
    public UnityEvent onClockTick;

    public bool gameStarted { get; private set; }
    float clock;
    public uint fib { get; private set; }
    uint fibA, fibB;
    uint currentScore;
    uint currentLife;
    gameState currentState;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    private void OnEnable()
    {
        currentState = gameState.start;
        refreshState();
    }

    private void Update()
    {
        if (currentState == gameState.play && gameStarted)
        {
            if (clock > 0f)
            {
                clock -= Time.deltaTime;
            }
            else
            {
                currentScore += fibB * 10;

                fibB = fibA;
                fibA = fib;
                fib = fibA + fibB;

                onClockTick.Invoke();
                clock = 10f;
            }

            if (scoreText != null)
                scoreText.text = currentScore.ToString("00000000");

            if (lifeText != null)
                lifeText.text = currentLife.ToString();
        }
        else if (currentState != gameState.play && Keyboard.current.anyKey.wasPressedThisFrame)
            StartGame();
    }

    public void StartGame()
    {
        if (gameStarted)
            return;

        clock = 0f;
        gameStarted = true;
        currentState = gameState.play;
        refreshState();
    }

    public void ResetGame()
    {
        if (!gameStarted)
            return;

        gameStarted = false;
        ResetPlayer();
        currentState = gameState.end;
        refreshState();
    }

    public void refreshState()
    {
        uiStart?.SetActive(currentState == gameState.start);
        uiGame?.SetActive(currentState == gameState.play);
        uiGameWorldspace?.SetActive(currentState == gameState.play);
        uiEnd?.SetActive(currentState == gameState.end);

        switch (currentState)
        {
            case gameState.start:
                ResetPlayer();
                if (bgm != null)
                {
                    bgm.clip = bgmNotGame;
                    bgm.loop = true;
                    bgm.Play();
                }    
                break;
            case gameState.play:
                currentScore = 0;
                currentLife = 10;
                fibA = 1;
                fibB = 0;
                fib = fibA + fibB;
                SpawnPlayer();
                if (bgm != null)
                {
                    bgm.clip = bgmInGame;
                    bgm.loop = true;
                    bgm.Play();
                }
                break;
            case gameState.end:
                ResetCamera();
                if (scoreTextEnd != null)
                    scoreTextEnd.text = currentScore.ToString("00000000");
                GameObject[] npc = GameObject.FindGameObjectsWithTag("Enemy");
                GameObject[] ammo = GameObject.FindGameObjectsWithTag("Bullet");
                foreach (GameObject g in npc)
                    Destroy(g);
                foreach (GameObject b in ammo)
                    Destroy(b);
                if (bgm != null)
                {
                    bgm.clip = bgmNotGame;
                    bgm.loop = true;
                    bgm.Play();
                }
                break;
        }
    }

    void SpawnPlayer()
    {
        player = Instantiate(playerObject, Vector3.zero, Quaternion.identity);
        if (camera != null)
            camera.followObject = player.transform;
    }

    void ResetPlayer()
    {
        if (player == null)
            return;

        player.DestroyShip();
    }

    void ResetCamera()
    {
        if (camera != null && camera.followObject != null)
            camera.followObject = null;
    }

    public void AddPoint() 
    {
        currentScore += fib;

        if (currentScore >= 100000000)
        {
            currentScore = 99999999;
            ResetGame();
        }
    }

    public void AddLife()
    {
        if (currentLife >= 10)
            return;
        playSFXAddLife();
        currentLife++;
    }

    public void ReduceLife()
    {
        currentLife--;
        playSFXPlayerHit();
        if (currentLife == 0)
            ResetGame();
    }

    public void SetCooldownBarValue(float value) => cooldownBar.fillAmount = value;

    public void playSFXPlayerLaser() => bgm?.PlayOneShot(sfxLaserPlayer);

    public void playSFXPlayerHit() => bgm?.PlayOneShot(sfxHitPlayer);

    public void playSFXPlayerDie() => bgm?.PlayOneShot(sfxExplodePlayer);

    public void playSFXEnemyLaser() => bgm?.PlayOneShot(sfxLaserEnemy);

    public void playSFXEnemyDie() => bgm?.PlayOneShot(sfxExplodeEnemy);

    public void playSFXAddLife() => bgm?.PlayOneShot(sfxAddLife);
}
