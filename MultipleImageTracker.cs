using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class MultipleImageTracker : MonoBehaviour
{
    ARTrackedImageManager imageManager;

    void Start()
    {
        imageManager = GetComponent<ARTrackedImageManager>();

        // 이벤트 연결 (원래 방식 유지)
        imageManager.trackablesChanged.AddListener(OnTrackedImage);
    }

    void Update()
    {
        // 필요 없음
    }

    void OnTrackedImage(ARTrackablesChangedEventArgs<ARTrackedImage> args)
    {
        foreach (ARTrackedImage trackedImage in args.added)
        {
            string imageName = trackedImage.referenceImage.name;

            // Band 프리팹만 사용
            GameObject bandPrefab = Resources.Load<GameObject>("Band");
            if (bandPrefab != null)
            {
                if (trackedImage.transform.childCount < 1)
                {
                    GameObject band = Instantiate(bandPrefab, trackedImage.transform.position, trackedImage.transform.rotation);
                    band.transform.SetParent(trackedImage.transform);

                    // BandPlayer에 이미지 이름 전달
                    BandPlayer player = band.GetComponentInChildren<BandPlayer>();
                    if (player != null)
                        player.Setup(imageName);
                }
            }
            else
            {
                Debug.LogWarning("Resources/Band.prefab 를 찾을 수 없습니다.");
            }
        }

        foreach (ARTrackedImage trackedImage in args.updated)
        {
            if (trackedImage.transform.childCount > 0)
            {
                Transform band = trackedImage.transform.GetChild(0);
                band.position = trackedImage.transform.position;
                band.rotation = trackedImage.transform.rotation;
            }
        }
    }
}
