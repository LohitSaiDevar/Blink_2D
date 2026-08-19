using UnityEngine;

public class Blink : MonoBehaviour
{
    [SerializeField] LayerMask groundLayer;
    public ContactPoint2D ContactPoint;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            if (collision.gameObject.CompareTag("RicochetAngled"))
            {
                ContactPoint2D contactPoint = collision.GetContact(0);

                Vector2 reflectedDir = Vector2.Reflect(rb.linearVelocity.normalized, contactPoint.normal);
                float speed = rb.linearVelocity.magnitude;
                

                float bounceAngle = Mathf.Atan2(reflectedDir.y, reflectedDir.x) * Mathf.Rad2Deg;

                if (bounceAngle > 45 && bounceAngle < 135)
                {
                    reflectedDir = Vector2.up;
                }
                else if (bounceAngle >= 135)
                {
                    reflectedDir = new Vector2(-1, 1).normalized;
                }
                else if (bounceAngle <= 45)
                {
                    reflectedDir = new Vector2(1, 1).normalized;
                }

                rb.linearVelocity = reflectedDir * speed;
            }
            

            if (!collision.gameObject.CompareTag("Ricochet") && !collision.gameObject.CompareTag("RicochetAngled"))
            {
                ContactPoint = collision.GetContact(0);
                PlayerController.OnBlinkUsed?.Invoke(transform);
            }
        }
    }

    public Vector2 BlinkOffset(float blinkOffsetDistance)
    {
        Vector2 pointToCenter = ContactPoint.point - (Vector2)transform.position;
        Vector2 offsetDirection = -pointToCenter.normalized;

        Vector2 blinkOffset = offsetDirection * blinkOffsetDistance;
        //Debug.Log("Ball Pos: " + transform.position);
        //Debug.Log("Contact Pos: " + ContactPoint.point);
        return blinkOffset;
    }


}
