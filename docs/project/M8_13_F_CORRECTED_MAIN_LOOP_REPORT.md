# M8.13-F Corrected Main Loop Report

Status: Implemented with static validation. Unity compile and Play Mode screenshot validation remain pending.

## Overall Result

M8.13 corrected the M8.12 loop misunderstanding in the active Presentation architecture:

- bell now starts character arrival,
- character is hidden initially,
- presented document starts review only after arrival,
- workstation clicks no longer return to Main,
- presented document returns from review to the Main composition,
- dialogue accumulates in a scrollable chat log,
- right panel shows only applicable AR/CR reasons,
- empty reason sections are hidden,
- decision paper is paper-like and simplified,
- selected CRs drive 10% premium steps,
- approval/conditional approval awards a once-only ducat incentive,
- post-decision character exit returns to Main/complete without auto-opening review.

## Phases Completed

- Phase A: `M8_13_A_CORRECTED_MAIN_LOOP_VERIFICATION.md`
- Phase B: `M8_13_B_LAYOUT_STATE_IMPLEMENTATION_PLAN.md`
- Phase C: `M8_13_C_ASSET_BRIGHTNESS_CORRECTION_MANIFEST.md`
- Phase D: Presentation implementation completed.
- Phase E: `M8_13_E_PLAYMODE_VALIDATION_REPORT.md`
- Phase F: final report and parity score completed.

## Files Created

- `docs/project/M8_13_A_CORRECTED_MAIN_LOOP_VERIFICATION.md`
- `docs/project/M8_13_B_LAYOUT_STATE_IMPLEMENTATION_PLAN.md`
- `docs/project/M8_13_C_ASSET_BRIGHTNESS_CORRECTION_MANIFEST.md`
- `docs/project/M8_13_E_PLAYMODE_VALIDATION_REPORT.md`
- `docs/project/M8_13_F_CORRECTED_MAIN_LOOP_REPORT.md`
- `docs/project/M8_13_F_CORRECTED_MAIN_LOOP_PARITY_SCORE.md`

## Files Updated

- `EpicProjectR/Assets/Scripts/Presentation/FirstPlayableKoreanText.cs`
- `EpicProjectR/Assets/Scripts/Presentation/FirstPlayableMainSceneRectSpec.cs`
- `EpicProjectR/Assets/Scripts/Presentation/FirstPlayablePresenter.cs`
- `EpicProjectR/Assets/Scripts/Presentation/FirstPlayableScreenState.cs`
- `EpicProjectR/Assets/Scripts/Presentation/FirstPlayableTweenRunner.cs`
- `EpicProjectR/Assets/Scripts/Presentation/FirstPlayableUiTheme.cs`
- `EpicProjectR/Assets/Scripts/Presentation/FirstPlayableView.cs`
- `EpicProjectR/Assets/Tests/EditMode/FirstPlayableDomainTests.cs`
- `docs/project/FIRST_PLAYABLE_QA_GUIDE.md`

## Assets Copied/Reused/Skipped

- Copied: none.
- Reused: existing runtime-safe MainSceneUI Resources background, paper, bell, contractor, speech bubble, and tab/button assets.
- Skipped: broad reference asset import, old scripts, old scenes, prefabs, ScriptableObjects.

## Initial Composition Changes

- Initial state is now `MainWaitingForBell`.
- Contractor is hidden initially.
- Entry document rect is much smaller and placed center-bottom.
- Bell is left of the document.
- Dark overlay alpha reduced to `0.04`.

## Bell And Character Arrival Changes

- Bell click enters `CharacterArriving`.
- Arrival uses move, scale, fade, and tint tweening from small/dark/faded to bright/full-size.
- Arrival completion enters `DocumentPresented`.
- Bell becomes inactive/semi-transparent after arrival.

## Presented Document Behavior

- Entry document is not interactable before arrival.
- Presented document opens review after arrival.
- Presented document in review returns to the Main composition for the same pending contract.
- Workstation document board no longer acts as a return/continue trigger.

## Chat UI Changes

- Left upper panel is now a generated scrollable chat log.
- Clicking the dialogue panel appends the next line instead of replacing a single bubble.
- Older lines remain in the scroll content.

## Workstation Changes

- Center workstation remains the largest review area.
- Drag controller and clamp bounds are preserved.
- Document IDs/status clutter remain hidden in normal UI.

## Right AR/CR Filtering Changes

- Presenter now renders only rules whose IDs are present in `CurrentReview().Triggers`.
- Empty AR or CR sections are hidden.
- Reason rows show reason names only.
- The old `서류에서 확인되는 심사 사유` title was removed from Presentation code.

## Decision Paper Changes

- Lower-right decision trigger uses paper imagery, not bell imagery.
- Open decision paper has three regions: title, compensation/premium, buttons.
- Decision paper shows only `손해보상금`, `보험료`, `거절`, and `승인`/`조건부 승인`.
- Extra premium explanation/result text was removed from the visible paper.

## Ducat Incentive Changes

- Approval/conditional approval incentive uses final premium with 10% per selected CR.
- Incentive remains floor 1% of final premium.
- Incentive remains once-only per contract via `HashSet<ContractId>`.
- Rejection still awards no incentive.

## Exit And Next Contract Loop

- Decision submission hides the presented document and starts character exit.
- The view appends a short contractor line before exit.
- Exit completion returns through the presenter to Main waiting state or completed state.
- Next contract does not auto-enter review.

## Compile And Play Validation

Completed:

- Static Presentation search for forbidden editor APIs.
- Static hidden accident field search in Presentation.
- Static source ID preservation search.
- Static old-script-copy check.
- Static repository boundary checks for `ref_folder`, `Packages`, and `ProjectSettings`.

Skipped:

- Unity compile: exact `6000.3.19f1` editor executable was missing.
- `dotnet build`: `dotnet` command unavailable.
- MSBuild proxy: no generated Unity `.csproj`.
- Play Mode and screenshots: require usable Unity editor.

## Source ID Preservation

Confirmed: no source ID normalization was added. IDs remain exact internally. Normal reason rows no longer display AR/CR IDs.

## AccidentFlag Visibility

Confirmed: no `AccidentFlag` or `WillAccidentIfApproved` strings appear in `EpicProjectR/Assets/Scripts/Presentation`.

## Repository Boundaries

Confirmed not modified:

- `ref_folder/`
- `EpicProjectR/Packages/`
- `EpicProjectR/ProjectSettings/`
- active scenes
- prefabs
- ScriptableObject assets

## Algorithms, Data Structures, Design Patterns

- Algorithms: explicit state transition gating; triggered-rule filtering; smoothstep tweening; 10% per selected CR premium calculation; once-only incentive calculation.
- Data structures: `HashSet<ContractId>` for once-only incentives; dictionaries keyed by `RuleId` for toggles/rows/severities; immutable screen-state DTOs.
- Design patterns: presenter-coordinated state machine, passive view, composition root, UI factory/theme/catalog separation.

## Remaining Risks

- Unity compile must be rerun with exact editor installed.
- Play Mode screenshots are still needed for visual overlap, brightness, and hit-target QA.
- Dialogue scroll/click behavior should be manually checked.
- Exact Main pixel parity may need one more rect-only tuning pass.

## Recommended Next Milestone

M8.14 should be a Unity Play Mode screenshot/interaction QA pass using editor `6000.3.19f1`: capture initial, arrived, review, decision-open, post-decision exit, and next-contract waiting states for C001-C003, then tune only centralized rects and minor UI hit areas.
