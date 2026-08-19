using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource bgm;
    [SerializeField] AudioSource sfx;


    [Header("Audio clips")]

    //BGM
    [SerializeField] AudioClip dungeonBGM;
    [SerializeField] AudioClip forestBGM;
    [SerializeField] AudioClip lavaBGM;

    //SFX
    [SerializeField] AudioClip jumpSFX;
    [SerializeField] AudioClip dashSFX;
    [SerializeField] AudioClip deathSFX;
    [SerializeField] AudioClip throwSFX;
    [SerializeField] AudioClip checkpointSFX;
    [SerializeField] AudioClip runningSFX;
    [SerializeField] AudioClip orbPowerSFX;

    public static AudioManager Instance;
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
        PlayBGM(forestBGM);
    }
    private void OnEnable()
    {
        PlayerController.OnPlayerJump += PlayJumpSFX;
        PlayerController.OnPlayerDash += PlayDashSFX;
        PlayerController.OnPlayerDeath += PlayDeathSFX;
        PlayerController.OnBallThrown += PlayThrowSFX;
        GameManager.OnReachingCheckpoint += PlayCheckPointSFX;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerJump -= PlayJumpSFX;
        PlayerController.OnPlayerDash -= PlayDashSFX;
        PlayerController.OnPlayerDeath -= PlayDeathSFX;
        PlayerController.OnBallThrown -= PlayThrowSFX;
        GameManager.OnReachingCheckpoint -= PlayCheckPointSFX;
    }

    public void PlaySFX(AudioClip clip)
    {
        sfx.PlayOneShot(clip);
    }

    public void PlayBGM(AudioClip clip)
    {
        sfx.PlayOneShot(clip);
    }

    public void StopBGMs()
    {
        sfx.Stop();
    }

    #region SFX
    void PlayJumpSFX()
    {
        PlaySFX(jumpSFX);
    }

    void PlayDashSFX()
    {
        PlaySFX(dashSFX);
    }

    void PlayDeathSFX()
    {
        PlaySFX(deathSFX);
    }

    void PlayThrowSFX()
    {
        PlaySFX(throwSFX);
    }

    void PlayCheckPointSFX()
    {
        PlaySFX(checkpointSFX);
    }

    public void PlayRunSFX()
    {
        PlaySFX(runningSFX);
    }

    public void PlayOrbPowerSFX()
    {
        PlaySFX(orbPowerSFX);
    }
    #endregion

    #region BGM

    public void PlayForestBGM()
    {
        StopBGMs();
        PlayBGM(forestBGM);
    }

    public void PlayDungeonBGM()
    {
        StopBGMs();
        PlayBGM(dungeonBGM);
    }

    public void PlayLavaBGM()
    {
        StopBGMs();
        PlayBGM(lavaBGM);
    }

    #endregion
}
