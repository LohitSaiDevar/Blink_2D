using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class CutsceneStarter : MonoBehaviour
{
    private PlayableDirector director;
    [SerializeField] GameObject dialogueBoxUI;

    [Header("Players")]
    [SerializeField] GameObject realPlayer;     // Drag your REAL player here
    [SerializeField] GameObject cutscenePlayer; // Drag your DUPLICATE player here

    [Header("Keybinds")]
    [SerializeField] private KeyCode startGameKey = KeyCode.Return; // Default is Enter key

    private bool gameStarted = false;

    private void Awake()
    {
        director = GetComponent<PlayableDirector>();
    }

    private void Start()
    {
        if (dialogueBoxUI != null) dialogueBoxUI.SetActive(false);   // Hide dialogue
        if (realPlayer != null) realPlayer.SetActive(false);         // Hide real player during title
          // Show cutscene player/pose
    }

    private void OnEnable()
    {
        director.stopped += OnCutsceneEnded;
    }

    private void OnDisable()
    {
        director.stopped -= OnCutsceneEnded;
    }

    private void Update()
    {
        // Wait for player to press Enter to start the game
        if (!gameStarted && Input.GetKeyDown(startGameKey))
        {
            StartGameSequence();
        }
    }

    private void StartGameSequence()
    {
        gameStarted = true;
        if (cutscenePlayer != null) cutscenePlayer.SetActive(true);

        // 2. Notify your GameManager that the game is officially starting
        // (Make sure your GameManager's OnGameStart delegate/event exists!)
        // GameManager.Instance?.StartGame(); 

        // 3. Begin Cutscene
        PlayCutscene();
    }

    private void PlayCutscene()
    {
        if (realPlayer != null) realPlayer.SetActive(false);
        if (cutscenePlayer != null) cutscenePlayer.SetActive(true);

        director.Play();
    }

    private void OnCutsceneEnded(PlayableDirector dir)
    {
        // Cutscene finished -> Hand control over to actual gameplay
        if (dialogueBoxUI != null) dialogueBoxUI.SetActive(false);
        if (cutscenePlayer != null) cutscenePlayer.SetActive(false);  // Hide dummy player
        if (realPlayer != null) realPlayer.SetActive(true);           // Enable real playable character!
    }
}
