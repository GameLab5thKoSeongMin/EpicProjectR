# M8.12-C UI Image And Asset Correction Manifest

Status: Completed without new asset copies.

## Runtime-Safe Asset Status

| Main UI need | Current runtime asset | Status |
| --- | --- | --- |
| Full background | `MainSceneUI/Background/main_total.png` | Used |
| Bell | `MainSceneUI/UI/bell_full.png` | Used |
| Entry document/paper prop | `MainSceneUI/Documents/paper_prop.png` | Used |
| Decision paper/document texture | `MainSceneUI/Documents/paper_texture_2.png` | Used |
| Letter prop | `MainSceneUI/Documents/letter_ui.png` | Used |
| Contractor placeholder | `MainSceneUI/Characters/contractor_temp.png` | Used |
| Speech bubble | `MainSceneUI/UI/speech_bubble.png` | Used |
| Approve tab | `MainSceneUI/UI/ui_tab_dim.png` | Used |
| Reject tab | `MainSceneUI/UI/ui_tab_off.png` | Used |
| Small icon/button | `MainSceneUI/UI/ui_btn_icon_x.png` | Available fallback |
| Korean fonts | `MainSceneUI/Fonts/*.ttf`, `*.otf` | Used by catalog fallback chain |
| Shelf/table/right panel `.aseprite` art | Not imported as PNG | Skipped |
| Top HUD image | None needed | Built with UI panel/color |

## Asset Decisions

- No new files were copied from `ref_folder`.
- Existing runtime-safe imported PNG/font assets were reused through `FirstPlayableAssetCatalog`.
- If a referenced Resource fails to load, the catalog/factory/theme path still falls back to generated colors, built-in/dynamic fonts, and generic sprites where available.
- `.aseprite` files remain skipped because importing/exporting them would require asset pipeline work outside this milestone.

## Changed Asset Usage

- The entry screen now uses bell, document paper, and contractor assets.
- The dialogue panel prefers the Main speech bubble sprite.
- The decision summary lower area uses a white paper-like panel with the existing paper sprite fallback.
- The dark overlay remains present but reduced to low alpha for readability.
