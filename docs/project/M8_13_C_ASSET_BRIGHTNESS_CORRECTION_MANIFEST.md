# M8.13-C Asset And Brightness Correction Manifest

Status: Completed before implementation.

## Inspected Reference

Read-only reference inspected:

- `ref_folder/Assets/Scenes/Main.unity`
- Main-linked runtime scripts for bell, dialogue, decision movement, and reason-paper behavior.
- `ref_folder/Assets/Images/Main_Total.png.meta` confirming `Main_Total_0`.

No `ref_folder/` file was modified.

## Runtime-Safe Assets Already Available

Existing active runtime-safe assets under `EpicProjectR/Assets/Resources/FirstPlayable/ImportedReference/RuntimeSafe/MainSceneUI/`:

- `Background/main_total.png`
- `Documents/paper_prop.png`
- `Documents/paper_texture_2.png`
- `Documents/letter_ui.png`
- `UI/bell_full.png`
- `UI/speech_bubble.png`
- `UI/ui_btn_icon_x.png`
- `UI/ui_tab_dim.png`
- `UI/ui_tab_off.png`
- `Characters/contractor_temp.png`
- MainSceneUI fonts

## Asset Decisions

| Need | Decision |
| --- | --- |
| Brighter Main background | Use existing runtime-safe `main_total.png`; reduce overlay alpha further. |
| Initial small document | Use paper image, smaller rect, no large text-heavy overlay. |
| Bell left of document | Use existing `bell_full.png`; semi-transparent after arrival. |
| Decision paper | Use paper texture/image, not bell image. |
| Reason rows | Use paper image/texture rows constrained to right panel width. |
| Broad import from reference | Skipped; no broad asset copying. |
| Old scripts/scenes/prefabs | Skipped; behavior reimplemented in Presentation. |

## Brightness Plan

- Keep `MainBackgroundSprite` full-screen with no preserve-aspect cropping.
- Reduce the dark overlay from M8.12 levels to a minimal alpha that preserves text contrast.
- Use darker panel colors only on workstation/right areas, not over the whole background.
- Keep the top HUD black and readable.

## Boundary Confirmation

No new reference assets are required for M8.13 because the relevant Main background, paper, bell, contractor, and button assets are already present in runtime-safe Resources. If future screenshot QA proves an asset mismatch, import should be requested as a separate narrow asset milestone.
