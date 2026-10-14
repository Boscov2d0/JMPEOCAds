#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif
using UnityEngine;
using UnityEngine.UI;

public static class StorePopup
{
    public const string AppStoreUrl = "https://apps.apple.com/us/app/id6755421287";
    public const string GooglePlayUrl = "https://play.google.com/store/apps/details?id=prison.escape.platformer.runner.obstacle.course.games";

    public static void Open() => StorePopupHost.Show();

    public static void Close() => StorePopupHost.Hide();
}

[DefaultExecutionOrder(32000)]
internal sealed class StorePopupHost : MonoBehaviour
{
    private static StorePopupHost _instance;

    private bool _isOpen;
    private float _savedTimeScale = 1f;
    private bool _savedAudioPause;
    private GameObject _editorOverlay;

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void StorePopupJs_Open(string appStoreUrl, string googlePlayUrl);

    [DllImport("__Internal")]
    private static extern void StorePopupJs_Close();

    [DllImport("__Internal")]
    private static extern int StorePopupJs_IsOpen();
#endif

    public static void Show()
    {
        Ensure().OpenInternal();
    }

    public static void Hide()
    {
        if (_instance != null)
            _instance.CloseInternal();
    }

    private static StorePopupHost Ensure()
    {
        if (_instance != null)
            return _instance;

        var host = new GameObject("StorePopupHost");
        DontDestroyOnLoad(host);
        _instance = host.AddComponent<StorePopupHost>();
        return _instance;
    }

    private void OpenInternal()
    {
        if (_isOpen)
            return;

        _isOpen = true;
        _savedTimeScale = Time.timeScale;
        Time.timeScale = 0f;

#if UNITY_WEBGL && !UNITY_EDITOR
        StorePopupJs_Open(StorePopup.AppStoreUrl, StorePopup.GooglePlayUrl);
#else
        //SetEditorOverlayVisible(true);
#endif
    }

    private void CloseInternal()
    {
        if (_isOpen == false)
            return;

        _isOpen = false;

#if UNITY_WEBGL && !UNITY_EDITOR
        StorePopupJs_Close();
#else
        SetEditorOverlayVisible(false);
#endif
        Time.timeScale = _savedTimeScale;
    }

    private void Update()
    {
        if (_isOpen == false)
            return;

#if UNITY_WEBGL && !UNITY_EDITOR
        if (StorePopupJs_IsOpen() == 0)
            CloseInternal();
#else
        if (Input.GetKeyDown(KeyCode.Escape))
            CloseInternal();
#endif
    }

    private void OnDestroy()
    {
        if (_isOpen)
        {
            Time.timeScale = _savedTimeScale;
#if UNITY_WEBGL && !UNITY_EDITOR
            StorePopupJs_Close();
#endif
        }

        if (_instance == this)
            _instance = null;
    }

    private void SetEditorOverlayVisible(bool visible)
    {
        if (visible)
            BuildEditorOverlay();

        if (_editorOverlay != null)
            _editorOverlay.SetActive(visible);
    }

    private void BuildEditorOverlay()
    {
        if (_editorOverlay != null)
            return;

        _editorOverlay = new GameObject("StorePopupOverlay", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        _editorOverlay.transform.SetParent(transform, false);

        var canvas = _editorOverlay.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 32767;

        var scaler = _editorOverlay.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);
        scaler.matchWidthOrHeight = 0.5f;

        var dimmer = CreateUiObject("Dimmer", _editorOverlay.transform);
        Stretch(dimmer);
        var dimmerImage = dimmer.AddComponent<Image>();
        dimmerImage.color = new Color(0f, 0f, 0f, 0.72f);
        var dimmerButton = dimmer.AddComponent<Button>();
        dimmerButton.transition = Selectable.Transition.None;
        dimmerButton.onClick.AddListener(CloseInternal);

        var panel = CreateUiObject("Panel", _editorOverlay.transform);
        var panelRect = panel.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 0.5f);
        panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.pivot = new Vector2(0.5f, 0.5f);
        panelRect.sizeDelta = new Vector2(720f, 960f);
        var panelImage = panel.AddComponent<Image>();
        panelImage.color = Color.white;

        var header = CreateUiObject("Header", panel.transform);
        var headerRect = header.GetComponent<RectTransform>();
        headerRect.anchorMin = new Vector2(0f, 1f);
        headerRect.anchorMax = new Vector2(1f, 1f);
        headerRect.pivot = new Vector2(0.5f, 1f);
        headerRect.sizeDelta = new Vector2(0f, 72f);
        headerRect.anchoredPosition = Vector2.zero;
        header.AddComponent<Image>().color = new Color(0.96f, 0.96f, 0.96f, 1f);

        var title = CreateUiObject("Title", header.transform);
        Stretch(title, new Vector2(24f, 0f), new Vector2(-80f, 0f));
        var titleText = title.AddComponent<Text>();
        //titleText.font = ResolveFont();
        titleText.fontSize = 32;
        titleText.fontStyle = FontStyle.Bold;
        titleText.alignment = TextAnchor.MiddleLeft;
        titleText.color = new Color(0.12f, 0.12f, 0.12f, 1f);
        titleText.text = "Store";

        var closeButtonObject = CreateUiObject("Close", header.transform);
        var closeRect = closeButtonObject.GetComponent<RectTransform>();
        closeRect.anchorMin = new Vector2(1f, 0.5f);
        closeRect.anchorMax = new Vector2(1f, 0.5f);
        closeRect.pivot = new Vector2(1f, 0.5f);
        closeRect.sizeDelta = new Vector2(64f, 64f);
        closeRect.anchoredPosition = new Vector2(-8f, 0f);
        closeButtonObject.AddComponent<Image>().color = Color.clear;
        var closeText = CreateUiObject("Label", closeButtonObject.transform);
        Stretch(closeText);
        var closeLabel = closeText.AddComponent<Text>();
        //closeLabel.font = ResolveFont();
        closeLabel.fontSize = 48;
        closeLabel.alignment = TextAnchor.MiddleCenter;
        closeLabel.color = new Color(0.2f, 0.2f, 0.2f, 1f);
        closeLabel.text = "×";
        var closeButton = closeButtonObject.AddComponent<Button>();
        closeButton.transition = Selectable.Transition.None;
        closeButton.onClick.AddListener(CloseInternal);

        var body = CreateUiObject("Body", panel.transform);
        Stretch(body, new Vector2(32f, 32f), new Vector2(-32f, -88f));

        var bodyText = CreateUiObject("Info", body.transform);
        var bodyTextRect = bodyText.GetComponent<RectTransform>();
        bodyTextRect.anchorMin = new Vector2(0f, 1f);
        bodyTextRect.anchorMax = new Vector2(1f, 1f);
        bodyTextRect.pivot = new Vector2(0.5f, 1f);
        bodyTextRect.sizeDelta = new Vector2(0f, 220f);
        bodyTextRect.anchoredPosition = Vector2.zero;
        var info = bodyText.AddComponent<Text>();
        //info.font = ResolveFont();
        info.fontSize = 28;
        info.alignment = TextAnchor.UpperLeft;
        info.color = new Color(0.2f, 0.2f, 0.2f, 1f);
        info.horizontalOverflow = HorizontalWrapMode.Wrap;
        info.verticalOverflow = VerticalWrapMode.Overflow;
        info.text = "Prison Escape - Obby Challenge\n\nOpen the store page, then close this window to keep playing.";

        var openButtonObject = CreateUiObject("OpenStore", body.transform);
        var openRect = openButtonObject.GetComponent<RectTransform>();
        openRect.anchorMin = new Vector2(0.5f, 1f);
        openRect.anchorMax = new Vector2(0.5f, 1f);
        openRect.pivot = new Vector2(0.5f, 1f);
        openRect.sizeDelta = new Vector2(420f, 80f);
        openRect.anchoredPosition = new Vector2(0f, -260f);
        openButtonObject.AddComponent<Image>().color = new Color(0.13f, 0.59f, 0.95f, 1f);
        var openLabelObject = CreateUiObject("Label", openButtonObject.transform);
        Stretch(openLabelObject);
        var openLabel = openLabelObject.AddComponent<Text>();
        //openLabel.font = ResolveFont();
        openLabel.fontSize = 30;
        openLabel.fontStyle = FontStyle.Bold;
        openLabel.alignment = TextAnchor.MiddleCenter;
        openLabel.color = Color.white;
        openLabel.text = "Open Store";
        var openButton = openButtonObject.AddComponent<Button>();
        openButton.onClick.AddListener(OpenStoreUrl);
    }

    private static void OpenStoreUrl()
    {
#if UNITY_LUNA
#if UNITY_IOS
        Luna.Unity.Playable.InstallFullGame(StorePopup.AppStoreUrl, StorePopup.AppStoreUrl);
#else
        Luna.Unity.Playable.InstallFullGame(StorePopup.GooglePlayUrl);
#endif
#elif UNITY_IOS
        Application.OpenURL(StorePopup.AppStoreUrl);
#else
        Application.OpenURL(StorePopup.GooglePlayUrl);
#endif
    }

    private static GameObject CreateUiObject(string name, Transform parent)
    {
        var go = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        return go;
    }

    private static void Stretch(GameObject go, Vector2? offsetMin = null, Vector2? offsetMax = null)
    {
        var rect = go.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.offsetMin = offsetMin ?? Vector2.zero;
        rect.offsetMax = offsetMax ?? Vector2.zero;
    }
}