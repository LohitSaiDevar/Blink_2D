using UnityEngine;

public class Player_MoveState : IPlayerState
{
    public void Enter(PlayerController player)
    {
        player.ChangeAnimationState(PlayerController.Player_Run);
        player.ResetCanJump();
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
        return PlayerStateID.Move;
    }

    public void Update(PlayerController player)
    {
        
        //player.ResetCameraPan();
        if(!player.GroundDetected())
        {
            player.stateMachine.ChangeState(PlayerStateID.Fall);
        }

        if (Mathf.Abs(player.HorizontalInput) <= 0.05f)
        {
            player.stateMachine.ChangeState(PlayerStateID.Idle);
        }
    }
}
