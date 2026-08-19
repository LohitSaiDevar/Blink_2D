using UnityEngine;

public class Player_DeathState : IPlayerState
{
    public void Enter(PlayerController player)
    {
        player.IsDead = true;
        player.ChangeAnimationState(PlayerController.Player_Death);
        player.ChangeGravityScale(0);
        
        PlayerController.OnPlayerDeath?.Invoke();
    }

    public void Exit(PlayerController player)
    {
        player.IsDead = false;
    }

    public void FixedUpdate(PlayerController player)
    {
        
    }

    public PlayerStateID GetID()
    {
        return PlayerStateID.Death;
    }

    public void Update(PlayerController player)
    {
        
    }
}
