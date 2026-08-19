using UnityEngine;

public class Player_ClimbState : IPlayerState
{
    public void Enter(PlayerController player)
    {
        player.IsClimbing = true;
        player.ChangeAnimationState(PlayerController.Player_Climb);
    }

    public void Exit(PlayerController player)
    {
        player.IsClimbing = false;
    }

    public void FixedUpdate(PlayerController player)
    {

    }

    public PlayerStateID GetID()
    {
        return PlayerStateID.Climb;
    }

    public void Update(PlayerController player)
    {
        if(player.HasAnimationEnded(PlayerController.Player_Climb))
        {
            player.FinishClimb();
            player.stateMachine.ChangeState(PlayerStateID.Idle);
        }
    }
}
