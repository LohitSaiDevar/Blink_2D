using UnityEngine;

public class Player_LedgeHangState : IPlayerState
{
    public void Enter(PlayerController player)
    {
        player.IsHanging = true;
        player.ChangeAnimationState(PlayerController.Player_LedgeHang);
        player.ApplyLedgeHang();
        player.SnapToLedge();
        //Debug.Log("Entered Ledge Hang State");
    }

    public void Exit(PlayerController player)
    {
        player.IsHanging = false;
    }

    public void FixedUpdate(PlayerController player)
    {
        
    }

    public PlayerStateID GetID()
    {
        return PlayerStateID.LedgeHang;
    }

    public void Update(PlayerController player)
    {
        
    }
}
