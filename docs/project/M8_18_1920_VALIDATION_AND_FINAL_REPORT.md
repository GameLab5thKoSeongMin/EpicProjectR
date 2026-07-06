# M8.18 1920 Validation And Final Report

Status: Completed with static validation and a successful generated C# project build. Unity Play Mode and screenshots remain pending.

## Overall Result

M8.14-M8.18 were completed as a correction pass over the existing generated first-playable Presentation UI. The required local background is now preferred, the entry loop starts with no character, bell starts center-origin arrival, the presented document remains the review trigger, review proportions were retuned, chat remains accumulated/scrollable, applicable reason filtering is preserved, and the paper decision drawer closes more safely.

## Milestones Completed

- M8.14: verification and gap report created.
- M8.15: background, HUD, entry composition, and character arrival corrected.
- M8.16: review layout, chat, and workstation interaction retuned.
- M8.17: right panel and decision paper behavior corrected/preserved.
- M8.18: static validation and final parity report created.

## Files Created

- `docs/project/M8_14_VERIFICATION_AND_GAP_REPORT.md`
- `docs/project/M8_15_ENTRY_BACKGROUND_HUD_REPORT.md`
- `docs/project/M8_16_REVIEW_LAYOUT_AND_CHAT_REPORT.md`
- `docs/project/M8_17_RIGHT_PANEL_AND_DECISION_REPORT.md`
- `docs/project/M8_18_1920_VALIDATION_AND_FINAL_REPORT.md`
- `docs/project/M8_18_FINAL_PARITY_SCORE.md`

## Files Updated

- `EpicProjectR/Assets/Scripts/Presentation/FirstPlayableAssetCatalog.cs`
- `EpicProjectR/Assets/Scripts/Presentation/FirstPlayableMainSceneRectSpec.cs`
- `EpicProjectR/Assets/Scripts/Presentation/FirstPlayableView.cs`
- `docs/project/FIRST_PLAYABLE_QA_GUIDE.md`

## Assets Used

- Required: `using_image/배경화면.png`
- Existing runtime-safe fallback sprites under `EpicProjectR/Assets/Resources/FirstPlayable/ImportedReference/RuntimeSafe/`

No old scripts, scenes, prefabs, or ScriptableObjects were copied from `ref_folder/`.

## Background Replacement Result

`FirstPlayableAssetCatalog` now loads the required root `using_image/배경화면.png` first. If the file is missing, the existing Resources background remains the fallback.

## HUD Correction Result

The HUD is still built last and should render above review UI. Title/date/ledger rects are wider and best-fit guarded for 1920x1080.

## Entry Composition Result

The initial Main state remains character-hidden. Bell and presented document are balanced on the center-bottom table area. The document remains inactive until character arrival completes.

## Character Arrival Correction Result

The contractor starts from the center-bottom, smaller, dimmer, and faded, then moves/scales/fades/tints into the left-side settled position.

## Review Transition Result

Review still uses tweened layout movement for the dialogue panel, contractor, workbench, and right shelf. The left presented document remains the return-to-Main trigger.

## Chat Dialogue Result

Chat remains a scrollable accumulated multi-bubble log. Dialogue panel click appends lines; mouse-wheel scroll is supported through `ScrollRect`.

## Right Panel Filtering Result

Only triggered/applicable AR/CR rules render. Empty sections are hidden. Reason rows are paper-like and narrowed to panel bounds. Source IDs are preserved internally but not displayed in normal reason labels.

## Decision Paper Result

The bottom-right paper trigger toggles the decision drawer. The drawer contains only title, compensation, premium, reject, and approve/conditional approve controls. The close animation now hides the drawer after moving down.

## Incentive Result

Preserved: approval/conditional approval awards floor 1% of CR-adjusted premium once per contract; rejection awards none.

## 1920x1080 Safety Check

Static rect math indicates:

- HUD title/date/ledger fit inside the 1920-width top strip with best-fit text.
- Left review region stays within roughly x 27-552 for chat and x 0-575 for lower composition.
- Workbench spans roughly x 592-1512.
- Right shelf spans roughly x 1528-1864.
- Decision box/drawer fit within the right shelf width.
- HUD is outside the review panel vertical area and rendered after review roots.

## Validation Performed

- Confirmed required background file exists.
- Confirmed required background image dimensions are 1920x1080.
- Inspected `ref_folder/Assets/Scenes/Main.unity` as visual reference only.
- Searched Presentation scripts for `UnityEditor` and `AssetDatabase`.
- Searched Presentation scripts for hidden accident field names.
- Searched Presentation scripts for visible source-ID risk.
- Checked repository status for forbidden path modifications.
- Verified no `ref_folder/` modifications appeared in `git status`.
- Ran `dotnet build EpicProjectR\EpicProjectR.sln --no-restore`; it passed with two existing `FirstPlayableBootstrap` deprecation warnings and zero errors after fixing the `UnityEngine.Application.dataPath` namespace qualification.

## Validation Skipped

- Unity Test Runner / EditMode: attempted Unity 6000.3.19f1 batchmode with temp test results, but the command returned immediately with no output and no result file, so it was not counted as a pass.
- Play Mode: skipped because a reliable Unity batch/interactive run was not available through this shell pass.
- Screenshots: skipped because Play Mode was unavailable.
- Full visual parity: requires manual Play Mode at 1920x1080.

## Remaining Risks

- The required background is loaded from the repository root at editor/runtime startup; a packaged build would need the asset moved into a build-included location in a later explicit asset-import milestone.
- Manual visual QA is still required to confirm exact brightness, no overlap, and click targets.
- Existing unrelated dirty Unity settings remain in the worktree and were not changed or reverted by this pass.

## Final Parity Score

Static/build score: `95 / 100`.

The score is capped because Unity Test Runner, Play Mode, and screenshots were not completed.

## Recommended Next Milestone

M8.19 should be a Unity Play Mode visual QA pass at 1920x1080 that captures initial Main, post-bell arrival, review, decision-open, decision-closed, post-submit exit, and next-contract waiting states.
