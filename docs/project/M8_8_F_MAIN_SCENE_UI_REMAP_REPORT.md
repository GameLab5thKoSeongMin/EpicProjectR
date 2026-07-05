# M8.8-F Main Scene UI Remap Report

Date: 2026-07-06

## 1. Overall Result

Completed M8.8-B through M8.8-F in one controlled run.

The first playable UI now moves significantly closer to `ref_folder/Assets/Scenes/Main.unity` while preserving the M8.6/M8.7 Presentation architecture and the Korean C001/C002/C003 first playable flow.

Before parity estimate: **46 / 100**.

After parity estimate: **69 / 100**.

## 2. What Was Wrong With The Previous UI Direction

The previous M8.7 UI was functional and readable, but it still felt like a generated QA dashboard:

- top header,
- left docket,
- center document viewer,
- right checklist,
- bottom full-width result band.

M8.8-A showed that the old Main scene's actual visual language is different: a full illustrated background, workbench, paper props, right shelf/check area, and a bottom-right decision panel.

## 3. Main Scene Hierarchy / Style Findings

Main scene findings used:

- one main Screen Space Overlay Canvas at `1920 x 1080`,
- `Main_Total.png` background,
- `WorkBench`,
- `WorkBenchDisplay`,
- right `Shelf`,
- `CheckView`,
- `EssentialPanel`,
- `ConsideratePanel`,
- bottom-right `Accept` / `Reject` decision area,
- direct document prefabs for ship application, registration, hull inspection, and route declaration.

## 4. Main-Scene-Only Compliance

Only `ref_folder/Assets/Scenes/Main.unity` was used as scene-based visual/layout reference.

Direct Main-referenced prefabs were inspected only for dependency classification:

- `ref_folder/Assets/Prefabs/ShipInsuranceDocument.prefab`
- `ref_folder/Assets/Prefabs/ShipRegistDocument.prefab`
- `ref_folder/Assets/Prefabs/HullInspectionDocument.prefab`
- `ref_folder/Assets/Prefabs/RouteDocument.prefab`
- `ref_folder/Assets/Prefabs_KSM/ConversationPanel.prefab`

No forbidden scene was used as visual/layout reference.

## 5. Assets Copied

Copied runtime-loaded MainSceneUI assets:

- `main_total.png`
- `paper_prop.png`
- `paper_texture_2.png`
- `letter_ui.png`
- `ui_tab_dim.png`
- `ui_tab_off.png`
- `ui_btn_icon_x.png`
- `KyoboHandwriting2022khn.ttf`
- `Galmuri11-Bold.ttf`
- `KyoboHandwriting2020pdy.otf`

Destination:

- `EpicProjectR/Assets/Resources/FirstPlayable/ImportedReference/RuntimeSafe/MainSceneUI/`

Unity generated `.meta` files during compile/import. Source `.meta` files were not copied.

## 6. Assets Inspected But Not Copied

Not copied:

- old document prefabs because they reference old scripts,
- ConversationPanel prefab because it is old UI prefab structure,
- old C# scripts,
- TMP SDF `.asset` files,
- `.aseprite` UI art,
- ScriptableObject case content,
- timelines/playables,
- materials,
- scenes,
- Packages,
- ProjectSettings.

## 7. UI Remap Summary

Updated:

- `FirstPlayableAssetCatalog.cs`
  - Added MainSceneUI Resources paths.
  - Added Main background/paper/letter/tab sprite properties.
  - Prioritized MainSceneUI Kyobo/Galmuri font equivalents.

- `FirstPlayableUiTheme.cs`
  - Shifted palette toward Main scene paper, overlay, shelf, and tab-button tones.

- `FirstPlayableUiFactory.cs`
  - Added anchored panel helper.
  - Centralized Main-style button sprite selection.

- `FirstPlayableView.cs`
  - Replaced the generic 3-column root composition with:
    - full-screen Main background,
    - dark overlay,
    - top date/status strip,
    - left contract paper stack,
    - central workbench document area,
    - right shelf checklist,
    - bottom-right decision/result paper.
  - Kept view passive and event-only.

- `FirstPlayableScreenState.cs`
  - Added null guards.

## 8. Code Quality Changes

Quality changes are documented in:

- `docs/project/M8_8_E_PRESENTATION_CODE_QUALITY_REPORT.md`

Summary:

- resource paths centralized,
- optional asset fallback preserved,
- state DTO null guards added,
- render entry null guards added,
- factory owns anchored panel creation and button sprite choice,
- presenter and gameplay logic unchanged.

## 9. Korean Text Status

Korean player-facing text remains centralized in `FirstPlayableKoreanText`.

Source IDs remain exact and untranslated:

- `C001`
- `C002`
- `C003`
- `AR01`
- `CR01`
- `CR02`
- `R0005`

## 10. Runtime Asset Loading Strategy

Runtime assets are loaded through `FirstPlayableAssetCatalog`.

Fonts:

- MainSceneUI Kyobo font first,
- MainSceneUI Galmuri/Kyobo fallback,
- previous M8.7 font fallback,
- OS/default fallback.

Sprites:

- `Resources.Load<Texture2D>()`,
- runtime `Sprite.Create()`,
- nullable optional sprite properties,
- generated color fallbacks when sprites are missing.

No `UnityEditor` or `AssetDatabase` runtime loading was introduced.

## 11. First Playable Flow Validation

Pure C# smoke passed:

- `C001` approve with no checked rules is correct.
- Result can be acknowledged manually by UI flow design; no auto-advance was added.
- `C002` reject with `AR01` is correct.
- Rejected `C002` creates no active contract and no regular accident.
- `C003` approve with `CR01` / `CR02` is correct.
- `C003` premium quote is 150%.
- `C003` deterministic accident outcome occurs.
- Session completes.

Unity manual Play Mode was not run.

## 12. Source ID Preservation Confirmation

Static search confirmed source IDs still exist as exact strings in content/tests/presentation text where expected.

No source ID normalization, localization, or renumbering was introduced.

## 13. AccidentFlag Visibility Confirmation

Static search:

- `rg -n "AccidentFlag|WillAccidentIfApproved" EpicProjectR/Assets/Scripts/Presentation`

Result:

- no matches.

Hidden accident fixture names are not exposed in normal Presentation UI.

## 14. Repository Boundary Confirmation

Not modified:

- `ref_folder/`
- `EpicProjectR/Packages/`
- `EpicProjectR/ProjectSettings/`
- active scenes
- active prefabs
- ScriptableObject assets

Copied MainSceneUI folder contains no `.cs`, `.prefab`, `.unity`, `.asset`, `.mat`, `.anim`, or `.controller` runtime imports.

## 15. Validation Performed

Performed:

- git status before and after scoped checks,
- Unity 6000.3.19f1 batchmode script compilation,
- Unity EditMode test command attempt,
- pure C# smoke test,
- static `UnityEditor` / `AssetDatabase` search,
- static `AccidentFlag` / `WillAccidentIfApproved` search,
- source ID static search,
- copied asset extension check,
- Packages / ProjectSettings / active scene scoped status check,
- Main-scene-only compliance review.

Unity compile result:

- exit code 0,
- `Tundra build success`,
- no `error CS`,
- no `Scripts have compiler errors`,
- no `NullReferenceException`,
- warnings only for existing obsolete `FindObjectOfType<T>()` calls in `FirstPlayableBootstrap`.

Pure C# smoke:

- passed after the temporary project was corrected to copy source files into the temp project.
- nullable warnings remain pre-existing; no errors.

## 16. Validation Skipped Or Inconclusive

- Manual Unity Play Mode QA was skipped because this automated run did not open interactive Unity.
- Unity EditMode test command exited 0 and compiled, but did not generate `Unity_M8_8_EditModeResults.xml`; individual Unity Test Runner results are inconclusive.
- Visual parity was scored from implemented structure and static inspection, not screenshot comparison.

## 17. Remaining Visual Gaps

- Exact Main scene object positions need manual screenshot QA.
- Right shelf line art from `.aseprite` assets is still approximated.
- Document cards emulate old prefabs but are not draggable old document prefabs.
- TMP SDF rendering is approximated with uGUI fonts.
- Main scene dialogue, timeline, map board, and month-end systems are out of scope.

## 18. Recommended Next Milestone

M8.9: Manual Play Mode Visual QA and Layout Tuning.

Recommended focus:

- capture 1920x1080 screenshot,
- check text overflow,
- tune fixed panel positions,
- tune background opacity,
- evaluate whether selected `.aseprite` art should be converted/exported safely to PNG in a future approved asset pass,
- keep the same Presentation architecture and C001/C002/C003 flow.
