using UnityEngine;

public class Player_AimState : IPlayerState
{
    public void Enter(PlayerController player)
    {
        player.IsAiming = true;
        player.ReadyToThrow = true;
        player.ChangeAnimationState(PlayerController.Player_Idle);
    }

    public void Exit(PlayerController player)
    {
        player.ReadyToThrow = false;
        player.StopAiming();
    }

    public void FixedUpdate(PlayerController player)
    {
        
    }

    public PlayerStateID GetID()
    {
        return PlayerStateID.Aim;
    }

    public void Update(PlayerController player)
    {
        player.PerformAim();
    }
}
