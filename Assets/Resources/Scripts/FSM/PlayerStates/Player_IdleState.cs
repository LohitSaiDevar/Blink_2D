using UnityEngine;

public class Player_IdleState : IPlayerState
{
    public void Enter(PlayerController player)
    {
        player.ApplyFriction();
        player.ChangeAnimationState(PlayerController.Player_Idle);
        player.ResetCanJump();
    }

    public void Exit(PlayerController player)
    {
        
    }

    public void FixedUpdate(PlayerController player)
    {
        
    }

    public PlayerStateID GetID()
    {
        return PlayerStateID.Idle;
    }

    public void Update(PlayerController player)
    {
        //player.ResetCameraPan();
        if (Mathf.Abs(player.HorizontalInput) > 0.05f)
        {
            player.stateMachine.ChangeState(PlayerStateID.Move);
        }

        if (!player.GroundDetected())
        {
            player.stateMachine.ChangeState(PlayerStateID.Fall);
        }
    }
}
