// Responsibility: Creates reusable Unity UI controls for the runtime first playable workdesk.
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace EpicProjectR.Presentation
{
    public sealed class FirstPlayableUiFactory
    {
        private readonly FirstPlayableUiTheme theme;
        private readonly FirstPlayableAssetCatalog assets;

        public FirstPlayableUiFactory(FirstPlayableUiTheme theme, FirstPlayableAssetCatalog assets)
        {
            this.theme = theme ?? throw new ArgumentNullException(nameof(theme));
            this.assets = assets ?? throw new ArgumentNullException(nameof(assets));
        }

        public FirstPlayableAssetCatalog Assets => assets;

        public GameObject CreatePanel(Transform parent, string name, Color color, bool outlined, Sprite sprite = null)
        {
            if (parent == null)
            {
                throw new ArgumentNullException(nameof(parent));
            }

            var panel = new GameObject(name);
            panel.transform.SetParent(parent, false);
            var image = panel.AddComponent<Image>();
            image.color = color;
            image.sprite = sprite;
            image.type = sprite != null ? Image.Type.Simple : Image.Type.Simple;

            if (outlined)
            {
                var outline = panel.AddComponent<Outline>();
                outline.effectColor = theme.Border;
                outline.effectDistance = new Vector2(1f, -1f);
            }

            Stretch(panel);
            return panel;
        }

        public GameObject CreateAnchoredPanel(
            Transform parent,
            string name,
            Vector2 anchorMin,
            Vector2 anchorMax,
            Vector2 anchoredPosition,
            Vector2 sizeDelta,
            Color color,
            bool outlined,
            Sprite sprite = null)
        {
            var panel = CreatePanel(parent, name, color, outlined, sprite);
            SetRect(panel, anchorMin, anchorMax, anchoredPosition, sizeDelta);
            return panel;
        }

        public GameObject CreateWorkPanel(Transform parent, string name, float preferredWidth, float flexibleWidth)
        {
            var panel = CreatePanel(parent, name, theme.WorkPanel, true, assets.PanelSprite);
            var layout = panel.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(12, 12, 10, 12);
            layout.spacing = 8;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;
            var element = panel.AddComponent<LayoutElement>();
            element.preferredWidth = preferredWidth;
            element.flexibleWidth = flexibleWidth;
            return panel;
        }

        public Text CreateText(Transform parent, string name, Font font, int size, FontStyle style, Color color, TextAnchor alignment)
        {
            if (parent == null)
            {
                throw new ArgumentNullException(nameof(parent));
            }

            var textObject = new GameObject(name);
            textObject.transform.SetParent(parent, false);
            var text = textObject.AddComponent<Text>();
            text.font = font;
            text.raycastTarget = false;
            text.fontSize = size;
            text.fontStyle = style;
            text.color = color;
            text.alignment = alignment;
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            Stretch(textObject);
            textObject.AddComponent<LayoutElement>().minHeight = size + 8;
            return text;
        }

        public ScrollRect CreateScroll(Transform parent, string name, Color viewportColor, out Transform content)
        {
            if (parent == null)
            {
                throw new ArgumentNullException(nameof(parent));
            }

            var scrollObject = new GameObject(name);
            scrollObject.transform.SetParent(parent, false);
            var scrollRect = scrollObject.AddComponent<ScrollRect>();
            scrollRect.horizontal = false;

            var viewport = CreatePanel(scrollObject.transform, "Viewport", viewportColor, true);
            viewport.AddComponent<Mask>().showMaskGraphic = false;
            var viewportRect = viewport.GetComponent<RectTransform>();
            viewportRect.anchorMin = Vector2.zero;
            viewportRect.anchorMax = Vector2.one;
            viewportRect.offsetMin = Vector2.zero;
            viewportRect.offsetMax = Vector2.zero;

            var contentObject = new GameObject("Content");
            contentObject.transform.SetParent(viewport.transform, false);
            var layout = contentObject.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(8, 8, 8, 8);
            layout.spacing = 8;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;
            contentObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            var contentRect = contentObject.GetComponent<RectTransform>();
            contentRect.anchorMin = new Vector2(0, 1);
            contentRect.anchorMax = new Vector2(1, 1);
            contentRect.pivot = new Vector2(0.5f, 1);
            contentRect.offsetMin = Vector2.zero;
            contentRect.offsetMax = Vector2.zero;

            scrollRect.viewport = viewportRect;
            scrollRect.content = contentRect;
            content = contentObject.transform;
            return scrollRect;
        }

        public Button CreateButton(Transform parent, string label, FirstPlayableButtonTone tone, UnityAction action)
        {
            if (parent == null)
            {
                throw new ArgumentNullException(nameof(parent));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var buttonObject = CreatePanel(parent, label, Color.white, true, ButtonSpriteFor(tone));
            var button = buttonObject.AddComponent<Button>();
            button.targetGraphic = buttonObject.GetComponent<Image>();
            theme.ApplyButton(button, tone);
            button.onClick.AddListener(action);
            var text = CreateText(buttonObject.transform, "Text", theme.TitleFont, 17, FontStyle.Bold, theme.LightText, TextAnchor.MiddleCenter);
            text.text = label;
            buttonObject.AddComponent<LayoutElement>().preferredHeight = 48;
            return button;
        }

        public void CreateSectionTitle(Transform parent, string text)
        {
            var title = CreateText(parent, text, theme.TitleFont, 18, FontStyle.Bold, theme.Gold, TextAnchor.MiddleLeft);
            title.text = text;
            title.GetComponent<LayoutElement>().preferredHeight = 28;
        }

        public void CreateDivider(Transform parent)
        {
            var divider = CreatePanel(parent, "Divider", theme.Border, false);
            divider.AddComponent<LayoutElement>().preferredHeight = 2;
        }

        public Image CreateSpriteImage(Transform parent, string name, Sprite sprite, Color fallbackColor, bool preserveAspect)
        {
            var imageObject = CreatePanel(parent, name, fallbackColor, false, sprite);
            imageObject.AddComponent<LayoutElement>();
            var image = imageObject.GetComponent<Image>();
            image.preserveAspect = preserveAspect;
            return image;
        }

        private static void Stretch(GameObject target)
        {
            var rect = target.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }

        private Sprite ButtonSpriteFor(FirstPlayableButtonTone tone)
        {
            if (tone == FirstPlayableButtonTone.Primary && assets.MainApproveTabSprite != null)
            {
                return assets.MainApproveTabSprite;
            }

            if (tone == FirstPlayableButtonTone.Reject && assets.MainRejectTabSprite != null)
            {
                return assets.MainRejectTabSprite;
            }

            return assets.MainRejectTabSprite ?? assets.ButtonSprite;
        }

        private static void SetRect(GameObject target, Vector2 anchorMin, Vector2 anchorMax, Vector2 anchoredPosition, Vector2 sizeDelta)
        {
            var rect = target.GetComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.anchoredPosition = anchoredPosition;
            rect.sizeDelta = sizeDelta;
            rect.offsetMin = rect.offsetMin;
            rect.offsetMax = rect.offsetMax;
        }
    }
}
