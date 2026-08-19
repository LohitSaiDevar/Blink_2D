using UnityEngine;

public class Player_FallState : IPlayerState
{
    public void Enter(PlayerController player)
    {
        player.ChangeAnimationState(PlayerController.Player_Fall);
    }

    public void Exit(PlayerController player)
    {
        
    }

    public void FixedUpdate(PlayerController player)
    {
        player.StartMoving();
        if(player.GroundDetected())
        {
            player.stateMachine.ChangeState(PlayerStateID.Land);
        }
    }

    public PlayerStateID GetID()
    {
        return PlayerStateID.Fall;
    }

    public void Update(PlayerController player)
    {
        
    }
}
