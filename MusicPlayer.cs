using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance { get; private set; }
    public AudioSource audioSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 유지
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayClip(AudioClip clip)
    {
        if (clip == null) return;

        audioSource.clip = clip;
        audioSource.time = 0f;
        audioSource.Play();
    }

    public void TogglePlayPause()
    {
        if (audioSource.clip == null) return;

        if (audioSource.isPlaying)
            audioSource.Pause();
        else
            audioSource.Play();
    }

    public void StopMusic()
    {
        if (audioSource.isPlaying)
            audioSource.Stop();

        audioSource.clip = null;
        audioSource.time = 0f;
    }

    public bool IsPlaying => audioSource.isPlaying;
    public float CurrentTime => audioSource.time;
}
