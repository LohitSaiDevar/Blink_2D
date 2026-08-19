using UnityEngine;

public class Player_LandState : IPlayerState
{
    public void Enter(PlayerController player)
    {
        player.ChangeAnimationState(PlayerController.Player_Land);
    }

    public void Exit(PlayerController player)
    {
        
    }

    public void FixedUpdate(PlayerController player)
    {
        
    }

    public PlayerStateID GetID()
    {
        return PlayerStateID.Land;
    }

    public void Update(PlayerController player)
    {
        if(player.GroundDetected())
        {
            if(player.HorizontalInput != 0)
            {
                player.stateMachine.ChangeState(PlayerStateID.Move);
            }
            else
            {
                player.stateMachine.ChangeState(PlayerStateID.Idle);
            }
        }
    }
}
