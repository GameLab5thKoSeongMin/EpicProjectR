# M8.12-F Entry HUD Dialogue Decision Report

Status: Implemented with static validation. Play screenshot validation remains pending.

## Overall Result

M8.12 replaced the remaining dashboard-like first playable UI with a Main-like entry and review loop:

- top HUD with title/date/ducats/reputation,
- centered bell/document/contractor entry,
- bell/document start review,
- large upper-left click-advance dialogue,
- central workstation as largest area,
- smaller right AR/CR shelf,
- cleaned document/rule labels,
- `인수 결정서` decision drawer toggle,
- character arrival/exit tween,
- one-time approval incentive.

## Files Created

- `docs/project/M8_12_A_MAIN_LOOP_HUD_DIALOGUE_VERIFICATION.md`
- `docs/project/M8_12_B_ENTRY_HUD_DIALOGUE_DECISION_IMPLEMENTATION_PLAN.md`
- `docs/project/M8_12_C_UI_IMAGE_AND_ASSET_CORRECTION_MANIFEST.md`
- `docs/project/M8_12_F_ENTRY_HUD_DIALOGUE_DECISION_REPORT.md`
- `docs/project/M8_12_F_MAIN_LOOP_PLAYMODE_PARITY_SCORE.md`

## Files Updated

- `EpicProjectR/Assets/Scripts/Presentation/FirstPlayableView.cs`
- `EpicProjectR/Assets/Scripts/Presentation/FirstPlayablePresenter.cs`
- `EpicProjectR/Assets/Scripts/Presentation/FirstPlayableScreenState.cs`
- `EpicProjectR/Assets/Scripts/Presentation/FirstPlayableKoreanText.cs`
- `EpicProjectR/Assets/Scripts/Presentation/FirstPlayableMainSceneRectSpec.cs`
- `EpicProjectR/Assets/Scripts/Presentation/FirstPlayableTweenRunner.cs`
- `EpicProjectR/Assets/Scripts/Presentation/FirstPlayableUiTheme.cs`
- `docs/project/FIRST_PLAYABLE_QA_GUIDE.md`

## Assets Copied/Replaced/Skipped

- Copied: none.
- Replaced: none.
- Reused: existing runtime-safe MainSceneUI PNG/font assets in `Assets/Resources/FirstPlayable/ImportedReference/RuntimeSafe/MainSceneUI/`.
- Skipped: `.aseprite` shelf/table/panel art, because safe export/import is outside this milestone.

## Main Verification Summary

Confirmed in `Main.unity` and Main-linked scripts:

- Bell button drives `GameLoopController.RingBell()`.
- Date/money/performance HUD fields exist.
- Visitor image and dialogue are bound during visitor opening.
- Workbench and decision paper movement use `RectTransformToggleMover`.
- Dialogue progresses by click through `DialogueBubbleClickArea`.
- CR checked reason count adjusts premium in Main by 10% per checked reason.

## HUD Changes

- Removed the cluttered `턴 | 날짜 | 계약 | 상태` top text.
- Added left `리세브라 해상 보험 심사국`, centered `1599년 01월 15일`, right ducats/reputation.
- Presenter now renders HUD from a data-only `FirstPlayableHudState`.

## Entry, Bell, Character Changes

- Entry no longer shows a contract list or current-case summary paper.
- Entry shows centered bell, document, and contractor preview.
- Bell and document both start review.
- Character arrival uses coordinated movement, fade, tint, and scale.
- Result state plays reverse-like exit animation.

## Dialogue Changes

- Added upper-left large dialogue panel.
- Dialogue lines live in `FirstPlayableKoreanText`.
- Clicking the dialogue panel advances fixture-safe Korean lines.

## Removed UI Elements

- Normal contract list panel.
- Normal current contract detail panel.
- Internal fixture markers in entry UI.
- Document ID/status clutter.
- Top `AR / CR 심사 항목` title.

## Document Text Cleanup

- Removed visible `DocumentId` suffixes such as `C001-APP`.
- Removed `제출됨 / 심사 가능`.
- Kept player-facing document kind and fields.

## AR/CR Cleanup

- Section titles are now `절대 거절 사유` and `거절 고려 사유`.
- Rule row titles show reason text only.
- Rule IDs remain internal in rule mappings, dictionaries, and event submissions.

## Decision Paper Changes

- Decision box is labeled `인수 결정서`.
- Presenter toggles drawer open/closed.
- Drawer rises from the lower-right closed position.
- Drawer includes left reject, right approve/conditional approve, and a white selected-reason summary paper.

## Ducat Incentive Implementation

- Presenter owns a first playable office ledger.
- On approved or conditional approved submission, it awards `floor(finalPremium / 100)` once per contract.
- Final premium uses fixture/application `BasePremium * MultiplierPercent / 100`.
- Reputation follows settlement total score delta.
- Domain/application/content behavior was not changed.

## Brightness/Layout Changes

- Dark overlay alpha reduced from `0.42` to `0.12`.
- Central workstation remains the largest area.
- Left dialogue/character is second-largest.
- Right AR/CR shelf width reduced.

## Validation

Performed:

- Static search: no `UnityEditor` or `AssetDatabase` in runtime scripts.
- Static search: no `AccidentFlag` or `WillAccidentIfApproved` in Presentation scripts.
- Static search: visible document/status label builders removed from normal UI path.
- Main scene/script inspection for bell, HUD, dialogue, toggle movement, decision paper, and CR premium behavior.

Attempted but inconclusive:

- Unity batch command with project version `6000.3.19f1` returned exit code 0 but did not emit a readable log file in this environment.
- `dotnet build EpicProjectR/Assembly-CSharp.csproj --no-restore` failed before C# compilation because access to `C:\Users\rhtjd\AppData\Local\Microsoft SDKs` was denied.
- Escalated retry was automatically rejected by Codex usage-limit policy.

Skipped:

- Play Mode screenshot/manual validation, because this environment did not provide an interactive Unity Play session.

## Repository Boundaries

Confirmed:

- No intentional changes to `ref_folder/`.
- No intentional changes to active scenes/prefabs/ScriptableObject assets.
- No intentional changes to `EpicProjectR/Packages/`.
- No intentional changes to `EpicProjectR/ProjectSettings/`.
- No old C# scripts copied.

## Algorithms, Data Structures, Design Patterns

- Algorithms: floor integer incentive calculation; UI tween interpolation with smoothstep; CR selected-count label update.
- Data structures: `HashSet<ContractId>` for once-only incentive protection; dictionaries keyed by `RuleId` for rule toggles/rows/severity; data-only screen state DTOs.
- Design patterns: composition root, passive view, presenter state coordinator, factory-created UI controls, theme/catalog separation.

## Remaining Risks

- Play Mode visual parity and click behavior need manual confirmation.
- Unity compile should be rerun in a normal editor session because this environment did not produce a log.
- Exact old character arrival timing remains approximated.
- Document-board click to advance after result should be tested against drag/click interactions.

## Recommended Next Milestone

M8.13 should be a short Play Mode visual QA pass with screenshots: validate entry composition, arrival/exit timing, decision drawer toggling, C001/C002/C003 flow, and adjust rects only where screenshots show mismatch.
