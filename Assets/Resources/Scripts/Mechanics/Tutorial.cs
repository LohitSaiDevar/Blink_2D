using UnityEngine;
using UnityEngine.UI;

public enum TutorialType
{
    None,
    Controls,
    Checkpoints,
    OrbOfPower,
    BlinkOrb,
    LedgeHanging,
    Ricochet,
    CurvedRicochet,
    Dash,
    DashThroughBarrier,
    DarkVision
}

public class Tutorial : MonoBehaviour
{
    public TutorialType tutorialType;
    [SerializeField]Tutorial NextTutorial;
    [SerializeField] Image exclaimationMark;
    bool alreadyInteracted = false;

    public bool AlreadyInteracted
    {
        get { return alreadyInteracted; }
        set { alreadyInteracted = value; }
    }

    public Tutorial GetNextTutorial()
    {
        return NextTutorial;
    }
    public Tutorial GetTutorialGameObj()
    {
        return this;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        UIManager.Instance.ShowInteractText();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        UIManager.Instance.HideInteractText();
    }

    public void HideExclaimationMark()
    {
        exclaimationMark.gameObject.SetActive(false);
    }
}
