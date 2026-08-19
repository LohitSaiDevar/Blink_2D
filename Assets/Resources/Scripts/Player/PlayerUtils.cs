using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUtils : MonoBehaviour
{
    public static PlayerUtils Instance;

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

    public Vector3 GetMouseWorldPos()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        worldPos.z = 0f;
        return worldPos;
    }
}
