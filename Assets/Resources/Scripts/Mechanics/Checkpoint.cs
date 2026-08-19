using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    bool isCheckPointReached;
    [SerializeField] GameObject checkpointText;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isCheckPointReached)
        {
            GameManager.Instance.SetCheckpoint(this);
            GameManager.OnReachingCheckpoint?.Invoke();
            isCheckPointReached = true;
        }
    }

    public GameObject GetCheckPointText()
    {
        return checkpointText;
    }
    void FastTravel()
    {

    }
}
