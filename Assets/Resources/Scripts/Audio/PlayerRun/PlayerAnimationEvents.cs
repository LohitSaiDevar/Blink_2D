using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    public void PlayFootStep()
    {
        AudioManager.Instance.PlayRunSFX();
    }
}
