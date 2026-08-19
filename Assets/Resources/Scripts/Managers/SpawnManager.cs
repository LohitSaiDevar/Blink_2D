using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Fireball")]
    [SerializeField] float fireballSpawnFrequency;
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private GameObject fireballCannonPrefab;
    [SerializeField] Vector3 fireballSpawnOffset;
    [SerializeField] float fireballRotationOffset;
    public static SpawnManager Instance;

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

    private void Start()
    {
        StartCoroutine(SpawnFireballTimer(fireballSpawnFrequency));
    }

    public void SpawnFireballs()
    {
        float currentAngle = fireballCannonPrefab.transform.eulerAngles.z - fireballRotationOffset;
        float radians = currentAngle * Mathf.Deg2Rad;

        Vector3 aimDir = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0);

        Vector3 fireballSpawnPos = fireballCannonPrefab.transform.position + fireballSpawnOffset;
        GameObject fireball = Instantiate(fireballPrefab, fireballSpawnPos, Quaternion.Euler(0, 0, currentAngle));

        fireball.GetComponent<Fireball>().StartMovingFireball(aimDir);
    }

    IEnumerator SpawnFireballTimer(float delay)
    {
        SpawnFireballs();
        yield return new WaitForSeconds(delay);
        StartCoroutine(SpawnFireballTimer(delay));
    }
}
