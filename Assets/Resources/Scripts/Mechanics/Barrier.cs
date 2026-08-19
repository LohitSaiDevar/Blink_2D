using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    Collider2D barrierCollider;
    Collider2D ignoredPlayer;
    private void Awake()
    {
        barrierCollider = GetComponent<Collider2D>();
    }
    private void OnEnable()
    {
        PlayerController.OnPlayerBarrierDash += DisableBarrier;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerBarrierDash -= DisableBarrier;
    }
    void DisableBarrier(Collider2D collider)
    {
        if(collider != barrierCollider) return;

        PlayerController player =
        FindFirstObjectByType<PlayerController>();

        ignoredPlayer = player.GetComponent<Collider2D>();

        Physics2D.IgnoreCollision(
            ignoredPlayer,
            barrierCollider,
            true);
        StartCoroutine(ReEnableCollision(0.5f));
    }

    IEnumerator ReEnableCollision(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (ignoredPlayer != null)
        {
            Physics2D.IgnoreCollision(
                ignoredPlayer,
                barrierCollider,
                false);
        }
    }
}
