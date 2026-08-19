using UnityEngine;

public enum Area
{
    Dungeon,
    Forest,
    Lava
}

public class BGMTrigger : MonoBehaviour
{
    [SerializeField] Area area;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayBGM(area);
        }
    }
    public void PlayBGM(Area type)
    {
        switch (type)
        {
            case Area.Dungeon:
                AudioManager.Instance.PlayDungeonBGM();
                break;

            case Area.Forest:
                AudioManager.Instance.PlayForestBGM();
                break;

            case Area.Lava:
                AudioManager.Instance.PlayLavaBGM();
                break;
        }
    }
}
