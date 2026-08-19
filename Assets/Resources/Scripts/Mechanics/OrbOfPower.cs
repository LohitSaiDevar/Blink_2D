using Unity.VisualScripting;
using UnityEngine;

public enum OrbType
{
    Blink,
    Dash,
    DarkVision
}

public class OrbOfPower : MonoBehaviour
{
    [SerializeField] OrbType orbType;


    public OrbType OrbType => orbType;
    public void GrantPower(PlayerController player)
    {
        AudioManager.Instance.PlayOrbPowerSFX();
        switch (orbType)
        {
            case OrbType.Blink:
                //Player selected Blink orb
                player.UnlockBlink();
                break;

            case OrbType.Dash:
                //Player selected Dash orb
                player.UnlockDash();
                break;

            case OrbType.DarkVision:
                //Player selected Dark orb
                player.UnlockDarkVision();
                break;
        }

        gameObject.SetActive(false);
    }
}
