using UnityEngine;

public class Player_CrouchState : IPlayerState
{
    public void Enter(PlayerController player)
    {
        player.ChangeAnimationState(PlayerController.Player_Crouch);
        player.EnableCrouchCollider();
        player.ApplyFriction();
    }

    public void Exit(PlayerController player)
    {
        
    }

    public void FixedUpdate(PlayerController player)
    {
        
    }

    public PlayerStateID GetID()
    {
        return PlayerStateID.Crouch;
    }

    public void Update(PlayerController player)
    {
        if (player.HorizontalInput != 0)
        {
            // If they start walking, go to move state (you'll need a way to check if the crouch button is released)
            player.stateMachine.ChangeState(PlayerStateID.CrouchWalk);
        }
    }
}
