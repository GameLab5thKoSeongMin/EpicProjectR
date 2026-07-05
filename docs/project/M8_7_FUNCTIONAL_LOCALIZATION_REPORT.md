# M8.7 Functional Localization Report

Date: 2026-07-05

## 1. Overall Result

Completed as a functional regression, Korean localization, font import, and error-sweep pass. The first playable keeps the M8.6 Presentation architecture split and preserves the C001/C002/C003 loop.

## 2. Root Cause Of Approve/Reject Not Advancing

Confirmed code risk: generated uGUI text objects were left with their default `raycastTarget = true` while filling the same rect as parent buttons and checklist controls. In the runtime-generated layout this can cause the visible label layer to receive pointer hits instead of the intended parent control path, which matches the observed "button appears clickable but flow does not proceed correctly" symptom.

Fix:

- `FirstPlayableUiFactory.CreateText()` now sets `Text.raycastTarget = false`.
- Presenter flow now uses explicit `Reviewing`, `Result`, and `Completed` modes instead of an ambiguous acknowledgement bool.
- View rendering still does not rebuild the bottom action buttons, so listeners are not duplicated on review/result re-render.

## 3. Files Changed

Created:

- `EpicProjectR/Assets/Scripts/Presentation/FirstPlayableKoreanText.cs`
- `docs/project/M8_7_FUNCTIONAL_LOCALIZATION_REPORT.md`
- `docs/project/M8_7_FONT_IMPORT_MANIFEST.md`

Updated:

- `EpicProjectR/Assets/Scripts/Presentation/FirstPlayablePresenter.cs`
- `EpicProjectR/Assets/Scripts/Presentation/FirstPlayableView.cs`
- `EpicProjectR/Assets/Scripts/Presentation/FirstPlayableUiFactory.cs`
- `EpicProjectR/Assets/Scripts/Presentation/FirstPlayableAssetCatalog.cs`
- `EpicProjectR/Assets/Scripts/Presentation/FirstPlayableScreenState.cs`
- `EpicProjectR/Assets/Tests/EditMode/FirstPlayableDomainTests.cs`
- `docs/project/FIRST_PLAYABLE_QA_GUIDE.md`
- `docs/project/M8_6_BUILD_SAFETY_REPORT.md`
- `docs/project/M8_6_IMPORTED_ASSET_MANIFEST.md`

## 4. Flow Fix Summary

- `FirstPlayablePresenter` now exposes explicit methods:
  - `ShowCurrentCase`
  - `SubmitDecision`
  - `AdvanceAfterResult`
  - internal `ShowResult`
  - internal `ShowCompleted`
- Submit is accepted only in `Reviewing` mode.
- Next/Finish acknowledgement is accepted only in `Result` mode.
- `SubmitDecision` still calls `FirstPlayableSession.Submit`, preserving application behavior.
- `AdvanceAfterResult` renders the next current case or completed state; it does not auto-advance before the player clicks.

## 5. Korean Localization Summary

Normal player-facing UI text is centralized in `FirstPlayableKoreanText`.

Localized areas:

- Header/status labels.
- Docket markers.
- Case summary labels.
- Section titles.
- Buttons.
- Document titles/status/field labels/known field values.
- Rule titles and finding text.
- Premium, audit, outcome, and settlement result text.
- Completed-state text.

Source IDs remain exact and untranslated, including `C001`, `C002`, `C003`, `AR01`, `CR01`, `CR02`, and `R0005`.

English remains only in code identifiers, class/method/file names, source IDs, asset paths, old domain fixture data, or internal GameObject names that are not displayed as normal UI text.

## 6. Font Loading Summary

Runtime font loading remains build-safe through `Resources.Load<Font>()`.

Fallback order:

1. Selected Korean-capable runtime font:
   - title: `NotoSerifKR-Bold`
   - body: `NotoSerifKR-SemiBold`
2. Other imported Korean-capable Resources fonts:
   - `Galmuri11-Bold`
   - `KyoboHandwriting2022khn`
   - `KyoboHandwriting2020pdy`
3. OS dynamic font fallback:
   - `Noto Serif KR`
   - `Malgun Gothic`
   - `Arial`
4. Unity built-in/default fallback:
   - `Arial.ttf` or `new Font("Arial")`

Missing optional sprites still fall back to generated colors. Missing runtime fonts should not crash the UI.

## 7. Error Sweep Summary

Fixed or hardened:

- Text raycast targets disabled to prevent generated labels from interfering with parent controls.
- Explicit presenter modes prevent result acknowledgement from firing in the wrong state.
- Font fallback chain expanded.
- Korean text avoids displaying hidden accident fixture field names.
- Optional sprite references remain nullable.
- Source IDs are displayed through existing typed ID `ToString()` values and not transformed.

## 8. Tests / Validation Performed

- Pure C# M8.7 smoke passed after approved `dotnet restore/run`.
- Smoke verified:
  - Korean approve/reject labels.
  - `C001` exact ID in header text.
  - `C001` approval result.
  - `C002` `AR01` rejection result.
  - rejected `C002` creates no active contract and no regular accident.
  - `C003` `CR01`/`CR02` approval result.
  - `C003` 150% premium.
  - deterministic accident outcome.
  - session completion after C003.
- Presentation outcome text does not expose `AccidentFlag` or `WillAccidentIfApproved`.
- Unity 6000.3.19f1 batchmode script compile/import reached script compilation and passed:
  - `Tundra build success`
  - no `error CS`
  - no `Scripts have compiler errors`
  - no `NullReferenceException`
  - warnings only: two existing `FindObjectOfType<T>()` obsolete warnings in `FirstPlayableBootstrap`.
- Unity EditMode test runner was attempted with `-runTests -testPlatform EditMode`; the editor compiled successfully and exited with return code 0, but no test result XML was produced, so individual Unity Test Runner test execution could not be confirmed from artifacts.
- `rg -n "UnityEditor|AssetDatabase" EpicProjectR/Assets/Scripts`: no matches.
- `rg -n "AccidentFlag|WillAccidentIfApproved" EpicProjectR/Assets/Scripts/Presentation`: no matches.
- Font folder inspection found copied font assets are `.ttf` / `.otf` only, aside from Unity-generated `.meta` files already present for imported assets.
- Path status check found no active `Packages`, `ProjectSettings`, or scene changes.

## 9. Validation Skipped Or Failed

- Unity Editor Play Mode manual QA was not run in this automated pass.
- Unity EditMode test execution result verification was inconclusive because `Unity_M8_7_EditModeResults.xml` was not generated despite a return code 0 editor exit.
- UI event bubbling can only be fully confirmed in the Unity Editor, but the specific generated text raycast risk was removed.

## 10. Remaining Risks

- Runtime-generated uGUI still needs manual layout QA at target resolutions.
- TMP SDF assets were not imported; future TextMeshPro migration may require package/settings review.
- Some domain fixture data remains English internally, but normal UI presentation maps it into Korean.
- Unity may generate additional `.meta` files for newly copied fonts on next import.

## 11. Manual QA Instructions

In Unity:

1. Open `EpicProjectR/`.
2. Open `Assets/Scenes/SampleScene.unity`.
3. Press Play.
4. Confirm the runtime UI appears without exception.
5. Confirm normal UI labels are Korean while source IDs remain exact.
6. Approve `C001` with no checked rules and confirm result appears.
7. Click `다음 계약` and confirm `C002`.
8. Check `AR01`, click `거절`, and confirm correct rejection/no accident.
9. Click `다음 계약` and confirm `C003`.
10. Check `CR01` and `CR02`, click `승인`, and confirm 150% premium plus deterministic accident.
11. Click `종료` and confirm completed state.

## 12. Preservation Confirmations

- C001/C002/C003 behavior remains intact.
- Source IDs remain exact and untranslated.
- `AccidentFlag` is not exposed in normal Presentation UI.
- No packages were modified.
- No project settings were modified.
- No `ref_folder` files were modified.
- No C# scripts, scenes, prefabs, or non-font ScriptableObjects were copied from `ref_folder`.
- English player-facing UI text was replaced with Korean; remaining English is internal, source ID, asset path, code identifier, or documented fixture data.
