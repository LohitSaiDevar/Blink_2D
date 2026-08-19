using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    //START GAME
    public static Action OnGameStart;

    //CHECKPOINT
    Checkpoint currentCheckPoint;
    public static Action OnReachingCheckpoint;

    //DEATH STATE
    public static Action OnPlayerRespawn;

    //TUTORIAL
    Tutorial currentTutorial;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        StartGame();
    }

    private void OnEnable()
    {
        OnPlayerRespawn += RespawnPlayer;
        OnGameStart += CursorInvisible;
    }

    private void OnDisable()
    {
        OnPlayerRespawn -= RespawnPlayer;
        OnGameStart -= CursorInvisible;
    }

    #region CheckPoint
    public void SetCheckpoint(Checkpoint checkPoint)
    {
        currentCheckPoint = checkPoint;
    }
    
    public Checkpoint GetCurrentCheckpoint()
    {
        return currentCheckPoint;
    }

    public void RespawnPlayer()
    {
        PlayerController player = FindFirstObjectByType<PlayerController>();
        if (player == null || currentCheckPoint == null) return;

        player.transform.position = currentCheckPoint.transform.position;
    }
    #endregion

    #region Tutorial
    public void SetCurrentTutorial(Tutorial tutorial)
    {
        currentTutorial = tutorial;
    }

    public Tutorial GetCurrentTutorial()
    {
        return currentTutorial;
    }
    #endregion

    public void StartGame()
    {
        OnGameStart?.Invoke();
    }

    public void SetCursorVisibility(bool isVisible)
    {
        Cursor.visible = isVisible;
    }

    void CursorInvisible()
    {
        Cursor.visible = false;
    }
}
