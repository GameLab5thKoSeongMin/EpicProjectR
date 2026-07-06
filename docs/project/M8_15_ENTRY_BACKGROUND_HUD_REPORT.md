# M8.15 Entry Background HUD Report

Status: Implemented with static validation.

## Background

Implemented: `FirstPlayableAssetCatalog` now tries to load `using_image/배경화면.png` directly from the repository root before falling back to the existing runtime Resources background. This avoids copying or importing reference assets and keeps the mandatory background as the first choice.

Confirmed: the dark overlay remains very low alpha through `FirstPlayableUiTheme.MainOverlay`, so the screen should no longer read as overly dark when the required background loads.

## HUD

Implemented: the top strip remains built last, preserving top rendering order above entry and review UI.

Implemented: the title, date, and ledger rects were widened and use Unity Text best-fit:

- title width increased from 680 to 760 px with a smaller max font,
- date width increased from 360 to 420 px and remains center-anchored,
- ledger width increased from 260 to 320 px and remains right-aligned.

## Entry Composition

Implemented: the small presented document and bell remain center-bottom on the table area, with the bell to the left of the document.

Implemented: no character is visible in the initial `MainWaitingForBell` state.

## Character Arrival

Implemented: the entry contractor now starts from the screen center-bottom rather than from the left side. The animation uses movement, scale, fade, and tint from a smaller/dimmer state into the left-side settled position.

Preserved: bell click starts arrival only, disables/semi-transparently fades the bell, and does not open review.

Preserved: when arrival completes, the document remains and becomes the review entry trigger.

## Files Updated

- `EpicProjectR/Assets/Scripts/Presentation/FirstPlayableAssetCatalog.cs`
- `EpicProjectR/Assets/Scripts/Presentation/FirstPlayableMainSceneRectSpec.cs`
- `EpicProjectR/Assets/Scripts/Presentation/FirstPlayableView.cs`
