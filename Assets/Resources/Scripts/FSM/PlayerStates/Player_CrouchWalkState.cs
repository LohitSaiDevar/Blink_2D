using UnityEngine;

public class Player_CrouchWalkState : IPlayerState
{
    public void Enter(PlayerController player)
    {
        player.ChangeAnimationState(PlayerController.Player_CrouchWalk);
    }

    public void Exit(PlayerController player)
    {
        
    }

    public void FixedUpdate(PlayerController player)
    {
        player.StartMoving();
    }

    public PlayerStateID GetID()
    {
        return PlayerStateID.CrouchWalk;
    }

    public void Update(PlayerController player)
    {
        if (Mathf.Abs(player.HorizontalInput) <= 0.05f)
        {
            player.stateMachine.ChangeState(PlayerStateID.Crouch);
        }
    }
}
