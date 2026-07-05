// Responsibility: Loads first playable fonts and imported reference sprites through build-safe Resources paths.
using System;
using UnityEngine;

namespace EpicProjectR.Presentation
{
    public sealed class FirstPlayableAssetCatalog
    {
        private const string TitleFontPath = "FirstPlayable/ImportedReference/RuntimeSafe/Fonts/NotoSerifKR-Bold";
        private const string BodyFontPath = "FirstPlayable/ImportedReference/RuntimeSafe/Fonts/NotoSerifKR-SemiBold";
        private const string GalmuriFontPath = "FirstPlayable/ImportedReference/RuntimeSafe/Fonts/Galmuri11-Bold";
        private const string Kyobo2020FontPath = "FirstPlayable/ImportedReference/RuntimeSafe/Fonts/KyoboHandwriting2020pdy";
        private const string Kyobo2022FontPath = "FirstPlayable/ImportedReference/RuntimeSafe/Fonts/KyoboHandwriting2022khn";
        private const string MainKyobo2022FontPath = "FirstPlayable/ImportedReference/RuntimeSafe/MainSceneUI/Fonts/KyoboHandwriting2022khn";
        private const string MainGalmuriFontPath = "FirstPlayable/ImportedReference/RuntimeSafe/MainSceneUI/Fonts/Galmuri11-Bold";
        private const string MainKyobo2020FontPath = "FirstPlayable/ImportedReference/RuntimeSafe/MainSceneUI/Fonts/KyoboHandwriting2020pdy";
        private const string PanelPath = "FirstPlayable/ImportedReference/RuntimeSafe/Sprites/UI/ui_pane_bg";
        private const string ButtonPath = "FirstPlayable/ImportedReference/RuntimeSafe/Sprites/UI/ui_btn";
        private const string PaperPath = "FirstPlayable/ImportedReference/RuntimeSafe/Sprites/Documents/paper_texture_unity_normal";
        private const string PaperTilePath = "FirstPlayable/ImportedReference/RuntimeSafe/Sprites/Documents/paper_tile_dominant";
        private const string SealPath = "FirstPlayable/ImportedReference/RuntimeSafe/Sprites/Documents/seal_stamp";
        private const string RibbonPath = "FirstPlayable/ImportedReference/RuntimeSafe/Sprites/Decorations/ui_ribbon_small";
        private const string ShipPath = "FirstPlayable/ImportedReference/RuntimeSafe/Sprites/Icons/ship_galleon";
        private const string MainBackgroundPath = "FirstPlayable/ImportedReference/RuntimeSafe/MainSceneUI/Background/main_total";
        private const string MainPaperPath = "FirstPlayable/ImportedReference/RuntimeSafe/MainSceneUI/Documents/paper_prop";
        private const string MainPaperTexturePath = "FirstPlayable/ImportedReference/RuntimeSafe/MainSceneUI/Documents/paper_texture_2";
        private const string MainLetterPath = "FirstPlayable/ImportedReference/RuntimeSafe/MainSceneUI/Documents/letter_ui";
        private const string MainApproveTabPath = "FirstPlayable/ImportedReference/RuntimeSafe/MainSceneUI/UI/ui_tab_dim";
        private const string MainRejectTabPath = "FirstPlayable/ImportedReference/RuntimeSafe/MainSceneUI/UI/ui_tab_off";
        private const string MainSmallButtonPath = "FirstPlayable/ImportedReference/RuntimeSafe/MainSceneUI/UI/ui_btn_icon_x";

        private FirstPlayableAssetCatalog(
            Font titleFont,
            Font bodyFont,
            Sprite panelSprite,
            Sprite buttonSprite,
            Sprite documentPaperSprite,
            Sprite documentTileSprite,
            Sprite sealSprite,
            Sprite ribbonSprite,
            Sprite shipSprite,
            Sprite mainBackgroundSprite,
            Sprite mainPaperSprite,
            Sprite mainPaperTextureSprite,
            Sprite mainLetterSprite,
            Sprite mainApproveTabSprite,
            Sprite mainRejectTabSprite,
            Sprite mainSmallButtonSprite)
        {
            TitleFont = titleFont ?? throw new ArgumentNullException(nameof(titleFont));
            BodyFont = bodyFont ?? throw new ArgumentNullException(nameof(bodyFont));
            PanelSprite = panelSprite;
            ButtonSprite = buttonSprite;
            DocumentPaperSprite = documentPaperSprite;
            DocumentTileSprite = documentTileSprite;
            SealSprite = sealSprite;
            RibbonSprite = ribbonSprite;
            ShipSprite = shipSprite;
            MainBackgroundSprite = mainBackgroundSprite;
            MainPaperSprite = mainPaperSprite;
            MainPaperTextureSprite = mainPaperTextureSprite;
            MainLetterSprite = mainLetterSprite;
            MainApproveTabSprite = mainApproveTabSprite;
            MainRejectTabSprite = mainRejectTabSprite;
            MainSmallButtonSprite = mainSmallButtonSprite;
        }

        public Font TitleFont { get; }
        public Font BodyFont { get; }
        public Sprite PanelSprite { get; }
        public Sprite ButtonSprite { get; }
        public Sprite DocumentPaperSprite { get; }
        public Sprite DocumentTileSprite { get; }
        public Sprite SealSprite { get; }
        public Sprite RibbonSprite { get; }
        public Sprite ShipSprite { get; }
        public Sprite MainBackgroundSprite { get; }
        public Sprite MainPaperSprite { get; }
        public Sprite MainPaperTextureSprite { get; }
        public Sprite MainLetterSprite { get; }
        public Sprite MainApproveTabSprite { get; }
        public Sprite MainRejectTabSprite { get; }
        public Sprite MainSmallButtonSprite { get; }

        public static FirstPlayableAssetCatalog Load()
        {
            return new FirstPlayableAssetCatalog(
                LoadFont(MainKyobo2022FontPath, new[] { MainGalmuriFontPath, MainKyobo2020FontPath, TitleFontPath, GalmuriFontPath, Kyobo2022FontPath, Kyobo2020FontPath }, new[] { "KyoboHandwriting2022khn", "Malgun Gothic", "Arial" }, 18),
                LoadFont(MainKyobo2022FontPath, new[] { MainGalmuriFontPath, MainKyobo2020FontPath, BodyFontPath, Kyobo2022FontPath, GalmuriFontPath, Kyobo2020FontPath }, new[] { "KyoboHandwriting2022khn", "Malgun Gothic", "Arial" }, 16),
                LoadSprite(PanelPath, "first-playable-panel"),
                LoadSprite(ButtonPath, "first-playable-button"),
                LoadSprite(PaperPath, "first-playable-paper"),
                LoadSprite(PaperTilePath, "first-playable-paper-tile"),
                LoadSprite(SealPath, "first-playable-seal"),
                LoadSprite(RibbonPath, "first-playable-ribbon"),
                LoadSprite(ShipPath, "first-playable-ship"),
                LoadSprite(MainBackgroundPath, "main-scene-background"),
                LoadSprite(MainPaperPath, "main-scene-paper"),
                LoadSprite(MainPaperTexturePath, "main-scene-paper-texture"),
                LoadSprite(MainLetterPath, "main-scene-letter"),
                LoadSprite(MainApproveTabPath, "main-scene-approve-tab"),
                LoadSprite(MainRejectTabPath, "main-scene-reject-tab"),
                LoadSprite(MainSmallButtonPath, "main-scene-small-button"));
        }

        private static Font LoadFont(string resourcesPath, string[] fallbackResourcePaths, string[] fallbackNames, int fallbackSize)
        {
            var font = Resources.Load<Font>(resourcesPath);
            if (font != null)
            {
                return font;
            }

            foreach (var fallbackResourcePath in fallbackResourcePaths)
            {
                font = Resources.Load<Font>(fallbackResourcePath);
                if (font != null)
                {
                    return font;
                }
            }

            var dynamicFont = Font.CreateDynamicFontFromOSFont(fallbackNames, fallbackSize);
            if (dynamicFont != null)
            {
                return dynamicFont;
            }

            var builtInFont = Resources.GetBuiltinResource<Font>("Arial.ttf");
            return builtInFont != null ? builtInFont : new Font("Arial");
        }

        private static Sprite LoadSprite(string resourcesPath, string spriteName)
        {
            var texture = Resources.Load<Texture2D>(resourcesPath);
            if (texture == null)
            {
                return null;
            }

            var rect = new Rect(0f, 0f, texture.width, texture.height);
            var sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f), 100f);
            sprite.name = spriteName;
            return sprite;
        }
    }
}
