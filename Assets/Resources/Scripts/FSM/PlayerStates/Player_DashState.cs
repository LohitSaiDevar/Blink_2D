using UnityEngine;

public class Player_DashState : IPlayerState
{
    public void Enter(PlayerController player)
    {
        player.IsDashing = true;
        player.CanDash = false;
        player.ChangeAnimationState(PlayerController.Player_Dash);

        EnableDashFor(player);
    }

    public void Exit(PlayerController player)
    {
        player.IsDashing = false;
    }

    public void FixedUpdate(PlayerController player)
    {
        
    }

    public PlayerStateID GetID()
    {
        return PlayerStateID.Dash;
    }

    public void Update(PlayerController player)
    {
        if (!player.IsDashing)
        {
            if (player.VerticalVelocity <= -0.1f)
            {
                player.stateMachine.ChangeState(PlayerStateID.Fall);
            }
            else if(player.GroundDetected())
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

    void EnableDashFor(PlayerController player)
    {
        Collider2D barrier = player.GetDetectedBarrier();

        if (barrier != null)
        {
            //Debug.Log("Barrier Dash");
            PlayerController.OnPlayerBarrierDash?.Invoke(barrier);
        }
        else
        {
            //Debug.Log("Normal Dash");
            PlayerController.OnPlayerDash?.Invoke();
        }
    }
}
