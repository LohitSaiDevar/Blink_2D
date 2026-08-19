using System.Collections;
using UnityEngine;

public class PlayerPhysics2D : MonoBehaviour
{
    public static PlayerPhysics2D Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void Move(
            Rigidbody2D rb,
            float inputX,
            float moveSpeed,
            bool isGrounded,
            float groundAccel,
            float airAccel
        )
    {
        float accel = isGrounded ? groundAccel : airAccel;
        rb.linearVelocity = new Vector2(inputX * accel * moveSpeed, rb.linearVelocity.y);
    }

    public void Jump(Rigidbody2D rb, float jumpForce)
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
    }

    public IEnumerator Dash(
            Rigidbody2D rb,
            TrailRenderer tr,
            float dashSpeed, 
            float dashTime, 
            float dashMoveCooldown, 
            float dashDirection,
            System.Action onDashEnd,
            System.Action onDashReset
        )
    {
        
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;
        rb.linearVelocity = new Vector2(dashDirection * dashSpeed, 0);
        tr.emitting = true;
        yield return new WaitForSeconds(dashTime);

        tr.emitting = false;
        rb.gravityScale = originalGravity;
        onDashEnd?.Invoke();

        yield return new WaitForSeconds(dashMoveCooldown);
        onDashReset?.Invoke();
    }



    public void ApplyFriction(Rigidbody2D rb, float deceleration, bool isGrounded)
    {
        if(isGrounded)
        {
            /*Vector2 velocity = rb.linearVelocity;
            velocity.x *= deceleration;
            rb.linearVelocity = velocity;*/

            float targetVelocity = Mathf.MoveTowards(rb.linearVelocity.x, 0, deceleration * Time.fixedDeltaTime);
            rb.linearVelocity = new Vector2(targetVelocity, rb.linearVelocity.y);
        }
    }

    
}
