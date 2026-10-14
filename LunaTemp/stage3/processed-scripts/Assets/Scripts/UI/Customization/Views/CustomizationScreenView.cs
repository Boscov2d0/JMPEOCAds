using Source.Scripts.UI.Customization.Appliers;
using Source.Scripts.UI.Customization.Data;
using Source.Scripts.UI.Customization.Handlers;
using Source.Scripts.UI.Customization.Models;
using Source.Scripts.UI.Customization.Presenters;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Source.Scripts.UI.Customization.Views
{
    public class CustomizationScreenView : MonoBehaviour, ICustomizationScreenView
    {
        private static readonly CategoryBinding[] Bindings =
        {
            new CategoryBinding(CustomizationCategory.Skin, "Button (1)", "SkinPanel"),
            new CategoryBinding(CustomizationCategory.Back, "Button (2)", "BackPanel"),
            new CategoryBinding(CustomizationCategory.Shoes, "Button (3)", "ShoesPanel"),
            new CategoryBinding(CustomizationCategory.Cap, "Button (4)", "CapPanel"),
            new CategoryBinding(CustomizationCategory.Glasses, "Button (5)", "GlassesPanel")
        };

        [SerializeField] private Transform _topPanel;
        [SerializeField] private GameObject _character;
        [SerializeField] private Sprite _activeTabSprite;
        [SerializeField] private Sprite _inactiveTabSprite;
        [SerializeField] private CustomizationCategory _defaultCategory = CustomizationCategory.Skin;
        [SerializeField] private Transform _characterRoot;
        [SerializeField] private CustomizationItemsCatalog _catalog = new CustomizationItemsCatalog();

        private readonly Dictionary<CustomizationCategory, CategoryWidgets> _widgets = new Dictionary<CustomizationCategory, CategoryWidgets>();
        private readonly List<(Button Button, UnityAction Action)> _buttonSubscriptions = new List<(Button Button, UnityAction Action)>();
        private CustomizationScreenPresenter _presenter;

        public event Action<CustomizationCategory, int> CategorySelected;
        public event Action<CustomizationCategory, int> ItemSelected;
        public static event Action SkinSelected;

        private void Awake()
        {
            BindWidgets();
            SubscribeButtons();
            _catalog?.EnsureSize();
            _presenter = new CustomizationScreenPresenter(
                new CustomizationScreenModel(_defaultCategory),
                this,
                new CustomizationItemHandler(_catalog, CharacterMeshApplier.FromCharacter(ResolveCharacterRoot())));
        }

        private void OnValidate() => _catalog?.EnsureSize();

        private Transform ResolveCharacterRoot()
        {
            if (_characterRoot != null)
                return _characterRoot;

            if (_character != null)
                return _character.transform;

            return null;
        }

        private void OnEnable() => _presenter?.Initialize();

        private void OnDisable() => _presenter?.Dispose();

        private void OnDestroy() => UnsubscribeButtons();

        public void SetCategoryPanelActive(CustomizationCategory category, bool isActive)
        {
            if (!TryGetWidgets(category, out CategoryWidgets widgets))
                return;

            if (widgets.Panel != null && widgets.Panel.activeSelf != isActive)
                widgets.Panel.SetActive(isActive);
        }

        public void SetCategoryTabSelected(CustomizationCategory category, bool isSelected)
        {
            if (!TryGetWidgets(category, out CategoryWidgets widgets))
                return;

            Sprite sprite = isSelected ? _activeTabSprite : _inactiveTabSprite;
            if (widgets.TabImage != null && sprite != null)
                widgets.TabImage.sprite = sprite;
        }

        public void SetSelectedItem(CustomizationCategory category, int itemIndex)
        {
            if (!TryGetWidgets(category, out CategoryWidgets widgets))
                return;

            if (widgets.ItemButtons == null || itemIndex < 0 || itemIndex >= widgets.ItemButtons.Length)
                return;

            MoveSelectionOutline(category, widgets.ItemButtons[itemIndex]);
        }

        private void BindWidgets()
        {
            if (_topPanel == null)
                return;

            for (int i = 0; i < Bindings.Length; i++)
            {
                Transform panelTransform = transform.Find(Bindings[i].PanelName);
                if (panelTransform == null)
                    continue;

                Transform tabTransform = _topPanel.Find(Bindings[i].TabButtonName);
                Button tabButton = null;
                if (tabTransform == null || !tabTransform.TryGetComponent(out tabButton))
                    tabButton = null;

                Image tabImage = tabButton != null ? tabButton.targetGraphic as Image : null;
                if (tabImage == null && tabTransform != null)
                    tabTransform.TryGetComponent(out tabImage);

                DisableBackgroundRaycasts(panelTransform);

                _widgets[Bindings[i].Category] = new CategoryWidgets
                {
                    TabButton = tabButton,
                    TabImage = tabImage,
                    Panel = panelTransform.gameObject,
                    ItemButtons = CollectItemButtons(panelTransform),
                    SelectionOutline = FindSelectionOutline(panelTransform),
                    Index = i
                };
            }
        }

        private void SubscribeButtons()
        {
            foreach (KeyValuePair<CustomizationCategory, CategoryWidgets> pair in _widgets)
            {
                CustomizationCategory category = pair.Key;
                CategoryWidgets widgets = pair.Value;

                if (widgets.TabButton != null)
                {
                    UnityAction tabAction = () => CategorySelected?.Invoke(category, widgets.Index);
                    //UnityAction categoryAction = () => OnItemButtonClicked(category, );
                    widgets.TabButton.onClick.AddListener(tabAction);
                    //widgets.TabButton.onClick.AddListener(categoryAction);
                    _buttonSubscriptions.Add((widgets.TabButton, tabAction));
                    //_buttonSubscriptions.Add((widgets.TabButton, categoryAction));
                }

                if (widgets.ItemButtons == null)
                    continue;

                for (int i = 0; i < widgets.ItemButtons.Length; i++)
                {
                    int itemIndex = i;
                    Button itemButton = widgets.ItemButtons[i];
                    if (itemButton == null)
                        continue;

                    UnityAction itemAction = () => OnItemButtonClicked(category, itemIndex, itemButton);
                    itemButton.onClick.AddListener(itemAction);
                    _buttonSubscriptions.Add((itemButton, itemAction));
                }
            }
        }

        private void OnItemButtonClicked(CustomizationCategory category, int itemIndex, Button button)
        {
            MoveSelectionOutline(category, button);
            ItemSelected?.Invoke(category, itemIndex);
        }

        private void MoveSelectionOutline(CustomizationCategory category, Button button)
        {
            if (button == null || !TryGetWidgets(category, out CategoryWidgets widgets))
                return;

            RectTransform outline = widgets.SelectionOutline;
            if (outline == null || outline.parent == button.transform)
                return;

            Vector2 anchorMin = outline.anchorMin;
            Vector2 anchorMax = outline.anchorMax;
            Vector2 anchoredPosition = outline.anchoredPosition;
            Vector2 sizeDelta = outline.sizeDelta;
            Vector2 pivot = outline.pivot;
            Vector3 localScale = outline.localScale;
            Quaternion localRotation = outline.localRotation;

            outline.SetParent(button.transform, false);
            outline.anchorMin = anchorMin;
            outline.anchorMax = anchorMax;
            outline.pivot = pivot;
            outline.sizeDelta = sizeDelta;
            outline.anchoredPosition = anchoredPosition;
            outline.localRotation = localRotation;
            outline.localScale = localScale;
            outline.SetAsLastSibling();
        }

        private static RectTransform FindSelectionOutline(Transform categoryPanel)
        {
            for (int i = 0; i < categoryPanel.childCount; i++)
            {
                Transform row = categoryPanel.GetChild(i);
                if (row.name != "Panel")
                    continue;

                Transform button = row.Find("Button");
                if (button == null)
                    continue;

                Transform outline = button.Find("Image");
                if (outline is RectTransform outlineRect)
                    return outlineRect;
            }

            return null;
        }

        private void UnsubscribeButtons()
        {
            foreach ((Button Button, UnityAction Action) subscription in _buttonSubscriptions)
            {
                if (subscription.Button != null)
                    subscription.Button.onClick.RemoveListener(subscription.Action);
            }

            _buttonSubscriptions.Clear();
        }

        private bool TryGetWidgets(CustomizationCategory category, out CategoryWidgets widgets)
            => _widgets.TryGetValue(category, out widgets);

        private static void DisableBackgroundRaycasts(Transform panel)
        {
            Image[] images = panel.GetComponentsInChildren<Image>(true);
            for (int i = 0; i < images.Length; i++)
            {
                Image image = images[i];
                if (image == null || image.GetComponent<Button>() != null)
                    continue;

                if (image.GetComponentInParent<Button>() != null)
                    continue;

                image.raycastTarget = false;
            }
        }

        private static Button[] CollectItemButtons(Transform panel)
        {
            Button[] buttons = panel.GetComponentsInChildren<Button>(true);
            Array.Sort(buttons, (left, right) => GetItemButtonIndex(left.name).CompareTo(GetItemButtonIndex(right.name)));
            return buttons;
        }

        private static int GetItemButtonIndex(string name)
        {
            if (name == "Button")
                return 0;

            const string prefix = "Button (";
            if (!name.StartsWith(prefix) || !name.EndsWith(")"))
                return int.MaxValue;

            string number = name.Substring(prefix.Length, name.Length - prefix.Length - 1);
            return int.TryParse(number, out int index) ? index : int.MaxValue;
        }

        private readonly struct CategoryBinding
        {
            public readonly CustomizationCategory Category;
            public readonly string TabButtonName;
            public readonly string PanelName;

            public CategoryBinding(CustomizationCategory category, string tabButtonName, string panelName)
            {
                Category = category;
                TabButtonName = tabButtonName;
                PanelName = panelName;
            }
        }

        private sealed class CategoryWidgets
        {
            public Button TabButton;
            public Image TabImage;
            public GameObject Panel;
            public Button[] ItemButtons;
            public RectTransform SelectionOutline;
            public int Index;
        }
    }
}