using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using TMPro;

public class ARBandSceneManagerWithOverlay : MonoBehaviour
{
    public ARTrackedImageManager imageManager;

    public GameObject confirmPanel;
    public TMP_Text infoText;
    public Button confirmButton;
    public Button cancelButton;

    private GameObject currentBand;
    private string currentImageName;
    private ARTrackedImage pendingImage;

    void Start()
    {
        confirmPanel?.SetActive(false);
    }

    void OnEnable()
    {
        imageManager.trackablesChanged.AddListener(OnTrackedImagesChanged);
        confirmButton.onClick.AddListener(OnConfirmPlay);
        cancelButton.onClick.AddListener(OnCancelPlay);
    }

    void OnDisable()
    {
        imageManager.trackablesChanged.RemoveListener(OnTrackedImagesChanged);
        confirmButton.onClick.RemoveListener(OnConfirmPlay);
        cancelButton.onClick.RemoveListener(OnCancelPlay);
    }

    void OnTrackedImagesChanged(ARTrackablesChangedEventArgs<ARTrackedImage> args)
    {
        foreach (var trackedImage in args.added)
        {
            string imageName = trackedImage.referenceImage.name;

            if (currentBand == null)
            {
                pendingImage = trackedImage;
                currentImageName = imageName;
                confirmPanel.SetActive(true);
                return;
            }
        }

        foreach (var trackedImage in args.updated)
        {
            if (currentBand != null && trackedImage.referenceImage.name == currentImageName)
            {
                currentBand.transform.position = trackedImage.transform.position;
                currentBand.transform.rotation = trackedImage.transform.rotation;
            }
        }
    }

    public void OnConfirmPlay()
    {
        confirmPanel.SetActive(false);

        if (currentBand != null)
            Destroy(currentBand);

        GameObject prefab = Resources.Load<GameObject>(currentImageName);
        if (prefab != null)
        {
            currentBand = Instantiate(prefab, pendingImage.transform.position, pendingImage.transform.rotation);
            currentBand.transform.SetParent(pendingImage.transform);

            BandPlayer player = currentBand.GetComponentInChildren<BandPlayer>();
            if (player != null)
            {
                player.Setup(currentImageName);
            }
        }

        pendingImage = null;
    }

    public void OnCancelPlay()
    {
        confirmPanel.SetActive(false);
        pendingImage = null;
    }
}
