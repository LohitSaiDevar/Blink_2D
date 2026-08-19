using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] GameObject checkPointMainMenu;
    [SerializeField] GameObject fastTravelMenu;
    [SerializeField] GameObject gameOverMenu;
    [SerializeField] TMP_Text gameSavedtext;

    //Tutorial Screens
    [SerializeField] GameObject tutorialMenu_Controls;
    [SerializeField] GameObject tutorialMenu_Checkpoints;
    [SerializeField] GameObject tutorialMenu_BlinkOrb;
    [SerializeField] GameObject tutorialMenu_OrbOfPower;
    [SerializeField] GameObject tutorialMenu_LedgeHang;
    [SerializeField] GameObject tutorialMenu_Ricochet;
    [SerializeField] GameObject tutorialMenu_DashThroughBarrier;
    [SerializeField] GameObject tutorialMenu_Dash;
    [SerializeField] GameObject tutorialMenu_CurvedRicochet;
    [SerializeField] GameObject tutorialMenu_DarkVision;
    [SerializeField] TMP_Text interactText;

    bool isTutorialActive = false;

    public bool IsTutorialActive
    {
        get { return isTutorialActive; }
        set { isTutorialActive = value; }
    }

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

    private void OnEnable()
    {
        //DEATH
        PlayerController.OnPlayerDeath += ShowDeathMenu;

        //CHECKPOINT
        GameManager.OnReachingCheckpoint += ShowCheckPointReached;
        GameManager.OnReachingCheckpoint += SaveGame;
    }

    private void OnDisable()
    {
        //DEATH
        PlayerController.OnPlayerDeath -= ShowDeathMenu;

        //CHECKPOINT
        GameManager.OnReachingCheckpoint -= ShowCheckPointReached;
        GameManager.OnReachingCheckpoint -= SaveGame;
    }

    void ShowOrHideMenu(GameObject menu, bool value)
    {
        menu.SetActive(value);
    }
    #region CheckPointMainMenu
    public void ShowCheckPointReached()
    {
        StartCoroutine(CheckpointReached());
    }

    IEnumerator CheckpointReached()
    {
        Checkpoint checkpoint = GameManager.Instance.GetCurrentCheckpoint();
        GameObject checkpointText = checkpoint.GetCheckPointText();

        checkpointText.SetActive(true);
        Animator anim = checkpointText.GetComponent<Animator>();
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        checkpointText.SetActive(false);
    }

    public void SaveGame()
    {
        StartCoroutine(ShowGameSaved());
    }

    IEnumerator ShowGameSaved()
    {
        gameSavedtext.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        gameSavedtext.gameObject.SetActive(false);
    }
    #endregion

    #region FastTravelMenu
    public void ShowFastTravelMenu()
    {
        fastTravelMenu.SetActive(true);
    }
    public void HideFastTravelMenu()
    {
        fastTravelMenu.SetActive(false);
    }
    #endregion

    #region GameOverMenu

    public void ShowDeathMenu()
    {
        Cursor.visible = true;
        gameOverMenu.SetActive(true);
    }

    public void HideDeathMenu()
    {
        Cursor.visible = false;
        gameOverMenu.SetActive(false);
    }

    #endregion

    #region Death Screen

    public void ReturnToCheckPoint()
    {
        GameManager.OnPlayerRespawn?.Invoke();
        HideDeathMenu();
    }

    #endregion

    #region Tutorial

    public void ShowInteractText()
    {
        interactText.gameObject.SetActive(true);
    }

    public void HideInteractText()
    {
        interactText.gameObject.SetActive(false);
    }

    public void ToggleTutorialScreen(Tutorial tutorial, bool value)
    {
        Cursor.visible = value;
        HideInteractText();
        GameManager.Instance.SetCurrentTutorial(tutorial);
        Debug.Log($"Current tutorial: {GameManager.Instance.GetCurrentTutorial()}");
        switch (tutorial.tutorialType)
        {
            case TutorialType.Controls:
                tutorialMenu_Controls.SetActive(value);
                return;

            case TutorialType.Checkpoints:
                tutorialMenu_Checkpoints.SetActive(value);
                return;

            case TutorialType.OrbOfPower:
                tutorialMenu_OrbOfPower.SetActive(value);
                return;

            case TutorialType.BlinkOrb:
                tutorialMenu_BlinkOrb.SetActive(value);
                return;

            case TutorialType.LedgeHanging:
                tutorialMenu_LedgeHang.SetActive(value);
                return;

            case TutorialType.Ricochet:
                tutorialMenu_Ricochet.SetActive(value);
                return;

            case TutorialType.Dash:
                tutorialMenu_Dash.SetActive(value);
                return;

            case TutorialType.DashThroughBarrier:
                tutorialMenu_DashThroughBarrier.SetActive(value);
                return;

            case TutorialType.CurvedRicochet:
                tutorialMenu_CurvedRicochet.SetActive(value);
                return;

            case TutorialType.DarkVision:
                tutorialMenu_DarkVision.SetActive(value);
                return;

            case TutorialType.None:
                return;
        }
    }

    public void CloseTutorialScreen()
    {
        Debug.Log("Button was clicked!");
        Tutorial currentTutorial = GameManager.Instance.GetCurrentTutorial();
        if (currentTutorial != null)
        {
            if(currentTutorial.AlreadyInteracted)
                currentTutorial.HideExclaimationMark();
            else
            {
                currentTutorial.AlreadyInteracted = true;
                currentTutorial.HideExclaimationMark();
            }

            ToggleTutorialScreen(currentTutorial, false);
            Debug.Log("Tutorial closed");
        }
        else
        {
            Debug.Log("No tutorial is currently active.");
        }
    }

    public void NextButton()
    {
        Tutorial current = GameManager.Instance.GetCurrentTutorial();

        ToggleTutorialScreen(current, false);

        if (current != null)
        {
            ToggleTutorialScreen(current.GetNextTutorial(), true);
        }
    }
    #endregion
}
