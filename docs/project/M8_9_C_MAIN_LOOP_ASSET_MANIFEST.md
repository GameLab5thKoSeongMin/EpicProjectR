# M8.9-C Main Loop Asset Manifest

## Existing M8.8 Runtime-Safe Assets Reused

Path root: `EpicProjectR/Assets/Resources/FirstPlayable/ImportedReference/RuntimeSafe/MainSceneUI/`

| Asset | Use |
| --- | --- |
| `Background/main_total.png` | Full-screen Main-inspired background. |
| `Documents/paper_prop.png` | Paper panel fallback. |
| `Documents/paper_texture_2.png` | Document and decision drawer texture. |
| `Documents/letter_ui.png` | Header/workbench letter prop. |
| `UI/ui_tab_dim.png` | Approve/primary button sprite. |
| `UI/ui_tab_off.png` | Reject/neutral fallback button sprite. |
| `UI/ui_btn_icon_x.png` | Small button fallback. |
| `Fonts/KyoboHandwriting2022khn.ttf` | Preferred Korean title/body font. |
| `Fonts/Galmuri11-Bold.ttf` | Korean font fallback. |
| `Fonts/KyoboHandwriting2020pdy.otf` | Korean font fallback. |

## New M8.9 Runtime-Safe Assets

| Source | Destination | Use |
| --- | --- | --- |
| `ref_folder/Assets/Images/임시_캐릭터 마법사.png` | `EpicProjectR/Assets/Resources/FirstPlayable/ImportedReference/RuntimeSafe/MainSceneUI/Characters/contractor_temp.png` | Lower-left contractor visual. |
| `ref_folder/Assets/Images/Old/말풍선.png` | `EpicProjectR/Assets/Resources/FirstPlayable/ImportedReference/RuntimeSafe/MainSceneUI/UI/speech_bubble.png` | Dialogue panel sprite when import succeeds. |
| `ref_folder/Assets/Images/Old/Bell_Full.png` | `EpicProjectR/Assets/Resources/FirstPlayable/ImportedReference/RuntimeSafe/MainSceneUI/UI/bell_full.png` | Lower-right final decision box image. |

## Fallback Policy

All added asset references are optional `Resources.Load` calls. Missing sprites fall back to generated UI colors or earlier safe sprites. Missing fonts fall back through runtime-safe fonts, OS fonts, and built-in Arial.
