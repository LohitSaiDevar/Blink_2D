using UnityEngine;

public class Player_ThrowState : IPlayerState
{
    public void Enter(PlayerController player)
    {
        player.IsBlinking = true;
        player.ChangeAnimationState(PlayerController.Player_Throw);
        PlayerController.OnBallThrown?.Invoke();
    }

    public void Exit(PlayerController player)
    {
        
    }

    public void FixedUpdate(PlayerController player)
    {

    }

    public PlayerStateID GetID()
    {
        return PlayerStateID.Throw;
    }

    public void Update(PlayerController player)
    {
        if(player.HasAnimationEnded(PlayerController.Player_Throw))
        {
            if (player.GroundDetected())
            {
                player.stateMachine.ChangeState(PlayerStateID.Idle);
            }
            else
            {
                player.stateMachine.ChangeState(PlayerStateID.Fall);
            }
        }
    }
}
