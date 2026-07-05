# M8.6 Reference Asset Migration Plan

Status: created before copying any M8.6 assets.

## Strategy

Use `ref_folder/` as reference evidence only. Copy a small set of dependency-free runtime assets into the active Unity project and load them with `Resources.Load<T>()`. Do not copy old C# scripts, full folders, metadata, settings, packages, active scenes, prefabs, or ScriptableObjects into runtime.

Approved runtime destination:

- `EpicProjectR/Assets/Resources/FirstPlayable/ImportedReference/RuntimeSafe/Fonts/`
- `EpicProjectR/Assets/Resources/FirstPlayable/ImportedReference/RuntimeSafe/Sprites/UI/`
- `EpicProjectR/Assets/Resources/FirstPlayable/ImportedReference/RuntimeSafe/Sprites/Documents/`
- `EpicProjectR/Assets/Resources/FirstPlayable/ImportedReference/RuntimeSafe/Sprites/Decorations/`
- `EpicProjectR/Assets/Resources/FirstPlayable/ImportedReference/RuntimeSafe/Sprites/Icons/`

No quarantine import is planned for M8.6.

## Selected Runtime-Safe Assets

| Group | Original path | Planned new path | Reason | Dependency notes |
| --- | --- | --- | --- | --- |
| Fonts | `ref_folder/Assets/Assets/FantasyGrimoireUI/Font/NotoSerifKR-Bold.ttf` | `EpicProjectR/Assets/Resources/FirstPlayable/ImportedReference/RuntimeSafe/Fonts/NotoSerifKR-Bold.ttf` | Korean-capable title font already used by M8.5 style. | Standalone font file; no old script dependency. |
| Fonts | `ref_folder/Assets/Assets/FantasyGrimoireUI/Font/NotoSerifKR-SemiBold.ttf` | `EpicProjectR/Assets/Resources/FirstPlayable/ImportedReference/RuntimeSafe/Fonts/NotoSerifKR-SemiBold.ttf` | Korean-capable body font already used by M8.5 style. | Standalone font file; no old script dependency. |
| Paper/document sprites | `ref_folder/Assets/paper_texture_unity_normal.png` | `EpicProjectR/Assets/Resources/FirstPlayable/ImportedReference/RuntimeSafe/Sprites/Documents/paper_texture_unity_normal.png` | Paper surface for document cards. | Standalone PNG. |
| Paper/document sprites | `ref_folder/Assets/종이_텍스처9_64x64_dominant 1 (1).png` | `EpicProjectR/Assets/Resources/FirstPlayable/ImportedReference/RuntimeSafe/Sprites/Documents/paper_tile_dominant.png` | Small paper tile for insets/document texture variation. | Standalone PNG; renamed to ASCII destination for code path stability. |
| UI sprites | `ref_folder/Assets/Assets/FantasyGrimoireUI/Bonus_Pack/UI_Pane_Bg.png` | `EpicProjectR/Assets/Resources/FirstPlayable/ImportedReference/RuntimeSafe/Sprites/UI/ui_pane_bg.png` | Existing UI panel treatment suitable for workdesk panels. | Standalone PNG. |
| Button sprites | `ref_folder/Assets/Assets/FantasyGrimoireUI/Sprites/Btns/UI_btn.png` | `EpicProjectR/Assets/Resources/FirstPlayable/ImportedReference/RuntimeSafe/Sprites/UI/ui_btn.png` | Existing button treatment suitable for decision buttons. | Standalone PNG. |
| Decorative sprites | `ref_folder/Assets/Assets/FantasyGrimoireUI/Bonus_Pack/UI_Img_ribbon_small.png` | `EpicProjectR/Assets/Resources/FirstPlayable/ImportedReference/RuntimeSafe/Sprites/Decorations/ui_ribbon_small.png` | Small official-looking decoration for the header/workdesk. | Standalone PNG. |
| Icons | `ref_folder/Assets/Images/Ship_Galleon.png` | `EpicProjectR/Assets/Resources/FirstPlayable/ImportedReference/RuntimeSafe/Sprites/Icons/ship_galleon.png` | Maritime signal for ship contract cases. | Standalone PNG. |
| Paper/document sprites | `ref_folder/Assets/Images/로웬 마르크 Stamp.png` | `EpicProjectR/Assets/Resources/FirstPlayable/ImportedReference/RuntimeSafe/Sprites/Documents/seal_stamp.png` | Stamp/seal treatment for document cards; used as visual texture, not as source data. | Standalone PNG; renamed to ASCII destination for code path stability. |

## Required Classification

| Group | Finding | M8.6 action |
| --- | --- | --- |
| 1. Fonts | `.ttf` and `.otf` files exist, including NotoSerifKR and TextMesh Pro sample fonts. | Copy only NotoSerifKR Bold/SemiBold as standalone runtime fonts. |
| 2. TextMeshPro font assets or materials | SDF `.asset` files and TMP font materials exist. | Do not copy; TextMeshPro asset dependencies are not needed for Unity UI `Text` and may carry material references. |
| 3. UI sprites | FantasyGrimoire UI sprite sheets/assets include panel, button, bookmark, and decorative sprites. | Copy selected standalone PNGs only. |
| 4. Paper/document sprites | Paper texture PNGs and document-preview images exist. | Copy selected paper textures and stamp PNG. |
| 5. Button sprites | `UI_btn.png` and close/arrow buttons exist. | Copy `UI_btn.png` only. |
| 6. Panel/background sprites | `UI_Pane_Bg.png`, `UI_Pane_Bg2.png`, and background images exist. | Copy one panel PNG; keep generated color panels as fallback. |
| 7. Decorative sprites | Ribbons, bookmarks, feathers, and other fantasy decorations exist. | Copy only `UI_Img_ribbon_small.png`; skip fantasy-heavy items. |
| 8. Icons | Many fantasy equipment/material/currency icons and a ship image exist. | Copy only `Ship_Galleon.png`; skip fantasy inventory icons. |
| 9. Materials | `.mat` files exist. | Do not copy; material shader dependencies are not needed for this runtime UI pass. |
| 10. Animation clips/controllers | `.anim` and `.controller` files exist. | Do not copy; no animation dependency is required for first playable QA. |
| 11. Audio assets | No first-playable-required audio selected. | Do not copy. |
| 12. Prefabs | Multiple prefabs exist, including FantasyGrimoire demo UI. | Do not copy; likely references old scripts/components and not needed. |
| 13. ScriptableObjects | `.asset` files exist, including settings and content-like assets. | Do not copy; dependencies are not fully understood for this milestone. |
| 14. Localization assets | Addressables/localization asset groups exist. | Do not copy; would require Addressables/settings handling. |
| 15. Scenes | Old `.unity` scenes exist. | Inspect only if needed; do not import as active scenes. |
| 16. Scripts | Old `.cs` scripts exist. | Forbidden to copy. |
| 17. Packages | Reference packages/settings may exist outside active project. | Do not copy or modify packages. |
| 18. ProjectSettings | Reference render pipeline and project settings exist. | Do not copy or modify project settings. |

## Metadata Plan

Do not copy `.meta` files. Unity should generate new GUIDs in the active project on import. This avoids GUID collision and unintended references back to old project assets.

## Build Loading Plan

Create a runtime `FirstPlayableAssetCatalog` that loads the selected files from `Assets/Resources/FirstPlayable/ImportedReference/RuntimeSafe/` using minimal constant paths. Presentation code should not use `UnityEditor` or `AssetDatabase`.
