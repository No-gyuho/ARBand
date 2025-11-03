using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BandPlayer : MonoBehaviour
{
    public TMP_Text titleText;
    public TMP_Text artistText;
    public TMP_Text timerText;
    public TMP_Text statusText;

    public Button playPauseButton;
    public Button stopButton;
    public Image playPauseImage;
    public Sprite playIcon;
    public Sprite pauseIcon;

    private string currentTrackName;

    void Start()
    {
        playPauseButton?.onClick.AddListener(OnPlayPausePressed);
        stopButton?.onClick.AddListener(OnStopPressed);
    }

    public void Setup(string imageName)
    {
        currentTrackName = imageName;

        var clip = Resources.Load<AudioClip>($"Audio/{imageName}");
        MusicPlayer.Instance.PlayClip(clip);

        switch (imageName.ToLower())
        {
            case "breakthebeat":
                titleText.text = "Break the Beat";
                artistText.text = "Digital Pulse";
                break;
            case "yesterday":
                titleText.text = "Yesterday";
                artistText.text = "AJ";
                break;
            default:
                titleText.text = "Unknown";
                artistText.text = "???";
                break;
        }

        timerText.text = "00:00";
        statusText.text = "Playing";
        UpdateButtonIcon();
    }

    void Update()
    {
        if (MusicPlayer.Instance != null && MusicPlayer.Instance.IsPlaying)
        {
            float t = MusicPlayer.Instance.CurrentTime;
            timerText.text = $"{(int)t / 60:00}:{(int)t % 60:00}";
        }
    }

    void OnPlayPausePressed()
    {
        MusicPlayer.Instance.TogglePlayPause();
        statusText.text = MusicPlayer.Instance.IsPlaying ? "Playing" : "Paused";
        UpdateButtonIcon();
    }

    void OnStopPressed()
    {
        MusicPlayer.Instance.StopMusic();
        titleText.text = "";
        artistText.text = "";
        timerText.text = "00:00";
        statusText.text = "";

        Destroy(transform.parent.gameObject); // ¹êµå ÇÁ¸®ÆÕ Á¦°Å
    }

    void UpdateButtonIcon()
    {
        playPauseImage.sprite = MusicPlayer.Instance.IsPlaying ? pauseIcon : playIcon;
    }
}
