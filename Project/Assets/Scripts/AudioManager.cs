using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip backgroundMusic;
    public AudioClip jumpSound;
    public AudioClip deathSound;
    public AudioClip gravityFlipSound;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic();
    }

    public void PlayMusic()
    {
        if (backgroundMusic != null && musicSource.clip != backgroundMusic)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void PlayJump()
    {
        if (jumpSound != null) sfxSource.PlayOneShot(jumpSound);
    }

    public void PlayDeath()
    {
        if (deathSound != null) sfxSource.PlayOneShot(deathSound);
    }

    public void PlayGravityFlip()
    {
        if (gravityFlipSound != null) sfxSource.PlayOneShot(gravityFlipSound);
    }
}