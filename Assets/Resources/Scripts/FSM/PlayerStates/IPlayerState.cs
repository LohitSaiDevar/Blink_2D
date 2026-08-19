using UnityEngine;

public enum PlayerStateID
{
    Idle,
    Move,
    Jump,
    Fall,
    Land,
    Crouch,
    CrouchWalk,
    Dash,
    Attack,
    Aim,
    Throw,
    LedgeHang,
    Climb,
    Hurt,
    Death
}

public interface IPlayerState
{
    PlayerStateID GetID();
    void Enter(PlayerController player);
    void Update(PlayerController player);
    void FixedUpdate(PlayerController player);
    void Exit(PlayerController player);
}
