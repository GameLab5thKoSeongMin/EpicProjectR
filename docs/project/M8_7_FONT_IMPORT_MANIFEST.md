# M8.7 Font Import Manifest

Date: 2026-07-05

## Runtime Strategy

The first playable uses uGUI `Text`, so M8.7 imports original font files (`.ttf`, `.otf`) and keeps TextMeshPro assets out of runtime. Runtime-used fonts live under `Resources` for `Resources.Load<Font>()`. Fonts imported for future use live outside `Resources`.

No `.meta` files were copied from `ref_folder`; Unity may generate new `.meta` files on import.

## Fonts Copied From `ref_folder`

| Original path | New active project path | Type | Korean support | Usage | Resources-loaded | Dependency notes |
| --- | --- | --- | --- | --- | --- | --- |
| `ref_folder/Assets/Assets/FantasyGrimoireUI/Font/NotoSerifKR-Bold.ttf` | `EpicProjectR/Assets/ImportedReference/RuntimeSafe/Fonts/NotoSerifKR-Bold.ttf` | `.ttf` | Yes, inferred from KR family name. | Imported for future and duplicated in runtime Resources. | No | Standalone font. |
| `ref_folder/Assets/Assets/FantasyGrimoireUI/Font/NotoSerifKR-Bold.ttf` | `EpicProjectR/Assets/Resources/FirstPlayable/ImportedReference/RuntimeSafe/Fonts/NotoSerifKR-Bold.ttf` | `.ttf` | Yes. | First playable title font. | Yes | Standalone font. |
| `ref_folder/Assets/Assets/FantasyGrimoireUI/Font/NotoSerifKR-SemiBold.ttf` | `EpicProjectR/Assets/ImportedReference/RuntimeSafe/Fonts/NotoSerifKR-SemiBold.ttf` | `.ttf` | Yes, inferred from KR family name. | Imported for future and duplicated in runtime Resources. | No | Standalone font. |
| `ref_folder/Assets/Assets/FantasyGrimoireUI/Font/NotoSerifKR-SemiBold.ttf` | `EpicProjectR/Assets/Resources/FirstPlayable/ImportedReference/RuntimeSafe/Fonts/NotoSerifKR-SemiBold.ttf` | `.ttf` | Yes. | First playable body font. | Yes | Standalone font. |
| `ref_folder/Assets/Assets/IndigoLay/PixelAdventureBookUI/Font/NotoSerif-Bold.ttf` | `EpicProjectR/Assets/ImportedReference/RuntimeSafe/Fonts/NotoSerif-Bold.ttf` | `.ttf` | Not inferred from file name. | Imported for future Latin serif use. | No | Standalone font. |
| `ref_folder/Assets/Assets/IndigoLay/PixelAdventureBookUI/Font/NotoSerif-Medium.ttf` | `EpicProjectR/Assets/ImportedReference/RuntimeSafe/Fonts/NotoSerif-Medium.ttf` | `.ttf` | Not inferred from file name. | Imported for future Latin serif use. | No | Standalone font. |
| `ref_folder/Assets/Assets/IndigoLay/PixelAdventureBookUI/Font/NotoSerif-Regular.ttf` | `EpicProjectR/Assets/ImportedReference/RuntimeSafe/Fonts/NotoSerif-Regular.ttf` | `.ttf` | Not inferred from file name. | Imported for future Latin serif use. | No | Standalone font. |
| `ref_folder/Assets/TextMesh Pro/Fonts/LiberationSans.ttf` | `EpicProjectR/Assets/ImportedReference/RuntimeSafe/Fonts/LiberationSans.ttf` | `.ttf` | No Korean support inferred. | Imported for future fallback/reference. | No | Standalone font. |
| `ref_folder/Assets/TextMesh Pro/Resources/Fonts & Materials/Galmuri11-Bold.ttf` | `EpicProjectR/Assets/ImportedReference/RuntimeSafe/Fonts/Galmuri11-Bold.ttf` | `.ttf` | Likely yes, Korean font family. | Imported for future and duplicated as runtime fallback. | No | Standalone font. |
| `ref_folder/Assets/TextMesh Pro/Resources/Fonts & Materials/Galmuri11-Bold.ttf` | `EpicProjectR/Assets/Resources/FirstPlayable/ImportedReference/RuntimeSafe/Fonts/Galmuri11-Bold.ttf` | `.ttf` | Likely yes. | Runtime fallback font. | Yes | Standalone font. |
| `ref_folder/Assets/TextMesh Pro/Resources/Fonts & Materials/KyoboHandwriting2020pdy.otf` | `EpicProjectR/Assets/ImportedReference/RuntimeSafe/Fonts/KyoboHandwriting2020pdy.otf` | `.otf` | Likely yes, Korean font family. | Imported for future and duplicated as runtime fallback. | No | Standalone font. |
| `ref_folder/Assets/TextMesh Pro/Resources/Fonts & Materials/KyoboHandwriting2020pdy.otf` | `EpicProjectR/Assets/Resources/FirstPlayable/ImportedReference/RuntimeSafe/Fonts/KyoboHandwriting2020pdy.otf` | `.otf` | Likely yes. | Runtime fallback font. | Yes | Standalone font. |
| `ref_folder/Assets/TextMesh Pro/Resources/Fonts & Materials/KyoboHandwriting2022khn.ttf` | `EpicProjectR/Assets/ImportedReference/RuntimeSafe/Fonts/KyoboHandwriting2022khn.ttf` | `.ttf` | Likely yes, Korean font family. | Imported for future and duplicated as runtime fallback. | No | Standalone font. |
| `ref_folder/Assets/TextMesh Pro/Resources/Fonts & Materials/KyoboHandwriting2022khn.ttf` | `EpicProjectR/Assets/Resources/FirstPlayable/ImportedReference/RuntimeSafe/Fonts/KyoboHandwriting2022khn.ttf` | `.ttf` | Likely yes. | Runtime fallback font. | Yes | Standalone font. |

## Font-Related Assets Skipped

| Asset group | Examples | Reason skipped |
| --- | --- | --- |
| TextMeshPro SDF font assets | `NotoSerifKR-Bold SDF.asset`, `NotoSerifKR-SemiBold SDF.asset`, `Galmuri11 SDF.asset`, Liberation/Kyobo TMP font assets | uGUI `Text` does not need them; copying may require TMP package/material dependencies and import settings. |
| TextMeshPro material presets | TMP `.mat` files under `TextMesh Pro/Resources/Fonts & Materials/` | Material presets depend on TMP shader/import setup; not needed for this pass. |
| TextMeshPro settings/style/sprite assets | `TMP Settings.asset`, default style sheet, EmojiOne sprite asset | These are settings or non-font dependencies and would risk package/project setting coupling. |

## Boundary Confirmation

- No C# scripts copied from `ref_folder`.
- No scenes copied from `ref_folder`.
- No prefabs copied from `ref_folder`.
- No ProjectSettings or Packages copied.
- No TextMeshPro migration performed.
