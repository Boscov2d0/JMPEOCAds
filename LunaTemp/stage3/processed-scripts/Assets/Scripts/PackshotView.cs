using UnityEngine;
using UnityEngine.UI;

public class PackshotView : MonoBehaviour
{
    [SerializeField] private Button _downloadButton;

    public Button DownloadButton { get => _downloadButton; }
}