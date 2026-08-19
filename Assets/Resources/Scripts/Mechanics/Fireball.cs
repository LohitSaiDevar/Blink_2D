using System.Collections;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    Rigidbody2D rb;
    Animator animator;
    Collider2D fireballCollider;
    public const string Fireball_Collision = "Fireball_Collision";
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        fireballCollider = GetComponent<Collider2D>();
        // Initialize fireball properties here (e.g., speed, direction)
    }

    public void StartMovingFireball(Vector3 aimDir)
    {
        rb.linearVelocity = aimDir * moveSpeed;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Ground"))
            return; // Ignore collisions with non-player and non-ground objects

        rb.linearVelocity = Vector2.zero; // Stop the fireball's movement
        fireballCollider.enabled = false; // Disable the collider to prevent further collisions

        animator.Play(Fireball_Collision); // Play the collision animation
        StartCoroutine(DestroyAfterAnimation()); // Start coroutine to destroy the fireball after the animation
    }

    IEnumerator DestroyAfterAnimation()
    {
        yield return null; // Wait for the next frame to ensure the animation has started
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length); // Wait for the animation to finish
        Destroy(gameObject); // Destroy the fireball after the animation is complete
    }
}
