using UnityEngine;

public class HangCheck : MonoBehaviour
{
    public bool CanLedgeHang(Transform ledgeHangTriggerPoint, LayerMask hangLayer)
    {
        return UnityEngine.Physics2D.OverlapCircle(ledgeHangTriggerPoint.position, 0.2f, hangLayer);
    }
}
