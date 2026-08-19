using UnityEngine;

public class Player_JumpState : IPlayerState
{
    public void Enter(PlayerController player)
    {
        player.ChangeAnimationState(PlayerController.Player_Jump);
        PlayerController.OnPlayerJump?.Invoke();
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
        return PlayerStateID.Jump;
    }

    public void Update(PlayerController player)
    {
        if(player.VerticalVelocity < -0.1f)
        {
            player.stateMachine.ChangeState(PlayerStateID.Fall);
        }
    }
}
