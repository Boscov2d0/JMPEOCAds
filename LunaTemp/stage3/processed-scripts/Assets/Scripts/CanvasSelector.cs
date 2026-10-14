using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasSelector : MonoBehaviour
{
    [SerializeField] private GameObject canvasLandscapeWide;
    [SerializeField] private GameObject canvas16x9;
    [SerializeField] private GameObject canvasPortraitWide;
    [SerializeField] private GameObject canvas9x16;

    [SerializeField, Range(0.01f, 0.5f)] private float landscapeRange = 0.2f;
    [SerializeField, Range(0.01f, 0.15f)] private float portraitRange = 0.08f;

    [SerializeField] private bool autoUpdate = true;

    private int lastWidth;
    private int lastHeight;
    [SerializeField] private GameObject _activeCanvas;

    public event Action<IActiveCanvas> ChangeCanvas;
    public event Action ChangeCanvasAction;

    public GameObject ActiveCanvasGO { get => _activeCanvas; }
    public IActiveCanvas ActiveCanvas =>
        _activeCanvas != null ? _activeCanvas.GetComponent<IActiveCanvas>() : null;

    public void CollectCountdownTexts(List<Text> destination)
    {
        AddCountdownText(canvasLandscapeWide, destination);
        AddCountdownText(canvas16x9, destination);
        AddCountdownText(canvasPortraitWide, destination);
        AddCountdownText(canvas9x16, destination);
    }

    private static void AddCountdownText(GameObject canvas, List<Text> destination)
    {
        if (canvas == null)
            return;

        PlayerHUDView view = canvas.GetComponent<PlayerHUDView>();
        Text timer = view != null ? view.StartGameTimeText : null;
        if (timer == null || destination.Contains(timer))
            return;

        destination.Add(timer);
    }

    private void Start() => UpdateCanvas();

    private void Update()
    {
        if (!autoUpdate) return;

        if (Screen.width != lastWidth || Screen.height != lastHeight)
            UpdateCanvas();
    }

    public void UpdateCanvas()
    {
        lastWidth = Screen.width;
        lastHeight = Screen.height;

        float aspect = (float)Screen.width / Screen.height;
        bool isPortrait = Screen.height >= Screen.width;

        GameObject target;

        if (isPortrait)
        {
            float a9_16 = 9f / 16f;
            target = (Mathf.Abs(aspect - a9_16) < portraitRange)
                ? canvas9x16
                : canvasPortraitWide;
        }
        else
        {
            float a16_9 = 16f / 9f;
            target = (Mathf.Abs(aspect - a16_9) < landscapeRange)
                ? canvas16x9
                : canvasLandscapeWide;
        }

        if (target == null)
            return;

        if (_activeCanvas == target && target.activeSelf)
            return;

        SetAllCanvases(false);
        target.SetActive(true);
        _activeCanvas = target;

        IActiveCanvas view = target.GetComponent<IActiveCanvas>();

        ChangeCanvas?.Invoke(view);
        ChangeCanvasAction?.Invoke();
    }

    private void SetAllCanvases(bool state)
    {
        if (canvasLandscapeWide) canvasLandscapeWide.SetActive(state);
        if (canvas16x9) canvas16x9.SetActive(state);
        if (canvasPortraitWide) canvasPortraitWide.SetActive(state);
        if (canvas9x16) canvas9x16.SetActive(state);
    }
}