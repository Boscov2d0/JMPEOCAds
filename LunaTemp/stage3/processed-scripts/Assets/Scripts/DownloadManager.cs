using UnityEngine;

public class DownloadManager : MonoBehaviour
{
    [SerializeField] private CanvasSelector _canvasSelector;

    private const string AppStoreUrl = "https://apps.apple.com/us/app/id6755421287";
    private const string GooglePlayUrl = "https://play.google.com/store/apps/details?id=prison.escape.platformer.runner.obstacle.course.games";

    private PackshotView _packshotView;

    private void Start()
    {
        _canvasSelector.ChangeCanvasAction += SetUI;
    }
    private void SetUI()
    {
        if (_packshotView != null)
            _packshotView.DownloadButton.onClick.RemoveListener(OpenStoreUrl);

        _packshotView = _canvasSelector.ActiveCanvasGO.GetComponent<PackshotView>();
        _packshotView.DownloadButton.onClick.RemoveListener(OpenStoreUrl);
    }
    public void OpenStoreUrl()
    {
#if UNITY_LUNA
#if UNITY_IOS
        Luna.Unity.Playable.InstallFullGame(AppStoreUrl, AppStoreUrl);
#else
        Luna.Unity.Playable.InstallFullGame(GooglePlayUrl);
#endif
       Luna.Unity.LifeCycle.GameEnded();
#else
#if UNITY_IOS
        Application.OpenURL(AppStoreUrl);
#else
        Application.OpenURL(GooglePlayUrl);
#endif
#endif
    }
}