// Responsibility: Centralizes first playable UI typography, colors, and selectable interaction styling.
using System;
using UnityEngine;
using UnityEngine.UI;

namespace EpicProjectR.Presentation
{
    public enum FirstPlayableButtonTone
    {
        Primary,
        Reject,
        Neutral
    }

    public sealed class FirstPlayableUiTheme
    {
        private FirstPlayableUiTheme(Font titleFont, Font bodyFont)
        {
            TitleFont = titleFont ?? throw new ArgumentNullException(nameof(titleFont));
            BodyFont = bodyFont ?? throw new ArgumentNullException(nameof(bodyFont));
        }

        public Font TitleFont { get; }
        public Font BodyFont { get; }
        public Color DeskBackground => new Color(0.075f, 0.105f, 0.1f, 1f);
        public Color MainBackdrop => new Color(0.035f, 0.032f, 0.028f, 1f);
        public Color MainOverlay => new Color(0.02f, 0.018f, 0.014f, 0.12f);
        public Color TopStrip => new Color(0.06f, 0.05f, 0.042f, 0.92f);
        public Color HeaderPanel => new Color(0.115f, 0.17f, 0.15f, 0.98f);
        public Color WorkPanel => new Color(0.18f, 0.135f, 0.09f, 0.92f);
        public Color WorkPanelAlt => new Color(0.24f, 0.18f, 0.12f, 0.92f);
        public Color Paper => new Color(0.88f, 0.79f, 0.58f, 0.98f);
        public Color PaperInset => new Color(0.76f, 0.65f, 0.45f, 0.96f);
        public Color PaperShadow => new Color(0.08f, 0.055f, 0.035f, 0.55f);
        public Color Ink => new Color(0.12f, 0.105f, 0.08f, 1f);
        public Color MutedInk => new Color(0.28f, 0.25f, 0.19f, 1f);
        public Color LightText => new Color(0.94f, 0.91f, 0.82f, 1f);
        public Color MutedText => new Color(0.7f, 0.68f, 0.58f, 1f);
        public Color Border => new Color(0.47f, 0.31f, 0.14f, 1f);
        public Color Gold => new Color(0.86f, 0.62f, 0.24f, 1f);
        public Color Approve => new Color(0.25f, 0.42f, 0.28f, 1f);
        public Color Reject => new Color(0.58f, 0.19f, 0.12f, 1f);
        public Color Consideration => new Color(0.18f, 0.34f, 0.52f, 1f);
        public Color DisabledRow => new Color(0.56f, 0.43f, 0.28f, 0.95f);
        public Color SelectedRow => new Color(0.82f, 0.56f, 0.23f, 0.98f);
        public Color Correct => new Color(0.22f, 0.47f, 0.31f, 1f);
        public Color Warning => new Color(0.67f, 0.39f, 0.17f, 1f);

        public static FirstPlayableUiTheme Create(FirstPlayableAssetCatalog assets)
        {
            if (assets == null)
            {
                throw new ArgumentNullException(nameof(assets));
            }

            return new FirstPlayableUiTheme(assets.TitleFont, assets.BodyFont);
        }

        public void ApplyButton(Button button, FirstPlayableButtonTone tone)
        {
            if (button == null)
            {
                throw new ArgumentNullException(nameof(button));
            }

            var baseColor = tone == FirstPlayableButtonTone.Reject ? Reject : tone == FirstPlayableButtonTone.Neutral ? WorkPanelAlt : Approve;
            var colors = button.colors;
            colors.normalColor = baseColor;
            colors.highlightedColor = Lighten(baseColor, 0.16f);
            colors.pressedColor = Darken(baseColor, 0.18f);
            colors.selectedColor = Lighten(baseColor, 0.1f);
            colors.disabledColor = new Color(0.23f, 0.23f, 0.21f, 0.55f);
            colors.fadeDuration = 0.08f;
            button.colors = colors;
            button.transition = Selectable.Transition.ColorTint;
        }

        public void ApplyToggle(Toggle toggle)
        {
            if (toggle == null)
            {
                throw new ArgumentNullException(nameof(toggle));
            }

            var colors = toggle.colors;
            colors.normalColor = DisabledRow;
            colors.highlightedColor = WorkPanelAlt;
            colors.pressedColor = SelectedRow;
            colors.selectedColor = SelectedRow;
            colors.disabledColor = new Color(0.18f, 0.18f, 0.16f, 0.5f);
            colors.fadeDuration = 0.08f;
            toggle.colors = colors;
            toggle.transition = Selectable.Transition.ColorTint;
        }

        private static Color Lighten(Color color, float amount)
        {
            return new Color(
                Mathf.Clamp01(color.r + amount),
                Mathf.Clamp01(color.g + amount),
                Mathf.Clamp01(color.b + amount),
                color.a);
        }

        private static Color Darken(Color color, float amount)
        {
            return new Color(
                Mathf.Clamp01(color.r - amount),
                Mathf.Clamp01(color.g - amount),
                Mathf.Clamp01(color.b - amount),
                color.a);
        }
    }
}
