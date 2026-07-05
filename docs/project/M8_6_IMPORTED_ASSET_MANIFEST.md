# M8.6 Imported Asset Manifest

Status: M8.6 runtime-safe assets copied. No quarantine assets imported.

All M8.6 assets were copied into `EpicProjectR/Assets/Resources/FirstPlayable/ImportedReference/RuntimeSafe/` so the runtime UI can load them through `Resources.Load<T>()` without Editor APIs. `.meta` files were intentionally not copied.

| Original `ref_folder` path | New active project path | Type | Reason selected | Safety | Dependency notes | Used in first playable UI | Build-safe loading |
| --- | --- | --- | --- | --- | --- | --- | --- |
| `ref_folder/Assets/Assets/FantasyGrimoireUI/Font/NotoSerifKR-Bold.ttf` | `EpicProjectR/Assets/Resources/FirstPlayable/ImportedReference/RuntimeSafe/Fonts/NotoSerifKR-Bold.ttf` | Font | Title typography consistent with M8.5 style and Korean-capable UI. | Runtime-safe | Standalone `.ttf`; no old script dependency. | Yes | `Resources.Load<Font>()` |
| `ref_folder/Assets/Assets/FantasyGrimoireUI/Font/NotoSerifKR-SemiBold.ttf` | `EpicProjectR/Assets/Resources/FirstPlayable/ImportedReference/RuntimeSafe/Fonts/NotoSerifKR-SemiBold.ttf` | Font | Body typography consistent with M8.5 style and Korean-capable UI. | Runtime-safe | Standalone `.ttf`; no old script dependency. | Yes | `Resources.Load<Font>()` |
| `ref_folder/Assets/paper_texture_unity_normal.png` | `EpicProjectR/Assets/Resources/FirstPlayable/ImportedReference/RuntimeSafe/Sprites/Documents/paper_texture_unity_normal.png` | PNG | Paper surface for submitted document cards. | Runtime-safe | Standalone PNG. | Yes | `Resources.Load<Texture2D>()`, runtime `Sprite.Create()` |
| `ref_folder/Assets/종이_텍스처9_64x64_dominant 1 (1).png` | `EpicProjectR/Assets/Resources/FirstPlayable/ImportedReference/RuntimeSafe/Sprites/Documents/paper_tile_dominant.png` | PNG | Paper tile/inset fallback with stable ASCII runtime path. | Runtime-safe | Standalone PNG; destination renamed only. | Fallback/available | `Resources.Load<Texture2D>()`, runtime `Sprite.Create()` |
| `ref_folder/Assets/Assets/FantasyGrimoireUI/Bonus_Pack/UI_Pane_Bg.png` | `EpicProjectR/Assets/Resources/FirstPlayable/ImportedReference/RuntimeSafe/Sprites/UI/ui_pane_bg.png` | PNG | Existing reference panel treatment for workdesk panels. | Runtime-safe | Standalone PNG. | Yes | `Resources.Load<Texture2D>()`, runtime `Sprite.Create()` |
| `ref_folder/Assets/Assets/FantasyGrimoireUI/Sprites/Btns/UI_btn.png` | `EpicProjectR/Assets/Resources/FirstPlayable/ImportedReference/RuntimeSafe/Sprites/UI/ui_btn.png` | PNG | Existing reference button treatment for decision controls. | Runtime-safe | Standalone PNG. | Yes | `Resources.Load<Texture2D>()`, runtime `Sprite.Create()` |
| `ref_folder/Assets/Assets/FantasyGrimoireUI/Bonus_Pack/UI_Img_ribbon_small.png` | `EpicProjectR/Assets/Resources/FirstPlayable/ImportedReference/RuntimeSafe/Sprites/Decorations/ui_ribbon_small.png` | PNG | Small decoration for header polish without fantasy inventory tone. | Runtime-safe | Standalone PNG. | Yes | `Resources.Load<Texture2D>()`, runtime `Sprite.Create()` |
| `ref_folder/Assets/Images/Ship_Galleon.png` | `EpicProjectR/Assets/Resources/FirstPlayable/ImportedReference/RuntimeSafe/Sprites/Icons/ship_galleon.png` | PNG | Maritime icon for the underwriting office header. | Runtime-safe | Standalone PNG. | Yes | `Resources.Load<Texture2D>()`, runtime `Sprite.Create()` |
| `ref_folder/Assets/Images/로웬 마르크 Stamp.png` | `EpicProjectR/Assets/Resources/FirstPlayable/ImportedReference/RuntimeSafe/Sprites/Documents/seal_stamp.png` | PNG | Stamp/seal treatment for document cards. | Runtime-safe | Standalone PNG; destination renamed only. | Yes | `Resources.Load<Texture2D>()`, runtime `Sprite.Create()` |

## Quarantine

None. No prefabs, ScriptableObjects, materials, animation controllers, scenes, packages, project settings, or old C# scripts were imported.

## Inspected But Not Copied

- TextMeshPro SDF font assets and materials: skipped because Unity UI `Text` can load the `.ttf` directly and TMP assets may bring material dependencies.
- FantasyGrimoire prefabs and demo scenes: skipped due missing-script and old-scene dependency risk.
- Addressables and localization assets/settings: skipped because M8.6 does not add packages or settings.
- Render pipeline/project settings assets: skipped because project settings are forbidden for this milestone.
- Fantasy equipment/currency/material icons: skipped because they do not fit the maritime underwriting workdesk.
- `.mat`, `.anim`, `.controller`, `.asset`, `.prefab`, `.unity`, and `.cs` reference files: skipped unless later milestones explicitly inspect and approve them.

## M8.7 Font Cross-Reference

M8.7 imported the remaining usable `.ttf` / `.otf` reference font files into approved font folders and expanded Resources font fallback. See `docs/project/M8_7_FONT_IMPORT_MANIFEST.md` for the complete font-specific manifest.
