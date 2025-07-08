using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource sfxSource;

    public AudioClip stepSound;
    public AudioClip shootSound;
    public AudioClip jumpSound;
    public AudioClip shardSound;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
else
        {
            Destroy(gameObject); // Prevent duplicates
        }
    }

    public void PlayStep() => sfxSource.PlayOneShot(stepSound);
    public void PlayShoot() => sfxSource.PlayOneShot(shootSound);
    public void PlayJump() => sfxSource.PlayOneShot(jumpSound);
    public void PlayShard() => sfxSource.PlayOneShot(shardSound);
}

