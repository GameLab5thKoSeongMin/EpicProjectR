# M8.13-E Play Mode Validation Report

Status: Static validation completed. Unity compile and Play Mode validation skipped by local tooling limits.

## Validation Attempt Summary

| Check | Result | Evidence |
| --- | --- | --- |
| Unity compile | Skipped | Project requires `6000.3.19f1`; `C:\Program Files\Unity\Hub\Editor\6000.3.19f1` exists but contains no `Unity.exe`. |
| `dotnet build` | Failed before compile | `dotnet` command is not available in this shell. |
| MSBuild proxy compile | Failed before compile | No generated `EpicProjectR/Assembly-CSharp.csproj` exists. |
| Play Mode QA | Skipped | No usable exact Unity editor executable in this environment. |
| Screenshot capture | Skipped | Requires Play Mode/editor session. |
| Static `UnityEditor` / `AssetDatabase` search | Passed | No matches in `EpicProjectR/Assets/Scripts/Presentation`. |
| Static `AccidentFlag` / `WillAccidentIfApproved` Presentation search | Passed | No matches in `EpicProjectR/Assets/Scripts/Presentation`. |
| Source ID preservation check | Passed | Existing tests/content preserve `C001`, `C002`, `C003`, `AR01`, `AR02`, `AR03`, `AR04`, `CR01`, `CR02`, and `R0005`; no source ID normalization added. |
| Old scripts copied check | Passed | No Main-linked old script class names appear in active Presentation code. |
| ProjectSettings changes | Passed | `git diff -- EpicProjectR/ProjectSettings` produced no diff. |
| Packages changes | Passed | `git diff -- EpicProjectR/Packages` produced no diff. |
| `ref_folder` modifications | Passed | `git diff -- ref_folder` produced no diff. |

## Static UI Behavior Validation

Confirmed in code:

- Initial state is `MainWaitingForBell`.
- Bell click transitions to `CharacterArriving`, not review.
- Arrival completion transitions to `DocumentPresented`.
- Presented document click transitions into review.
- Presented document click during review closes back to `DocumentPresented`.
- Workstation document board no longer has a click handler that returns to Main or advances results.
- Decision submit starts character exit and schedules return to Main/complete state after the exit tween.
- Next contract returns through `ShowCurrentCase()` and waits for a new bell click.
- The top HUD is built after entry/review roots so it remains the last sibling and visually above panels.
- The HUD date is anchored to the horizontal center instead of dependent on a layout row.
- Dialogue lines append into generated message rows inside a `ScrollRect`.
- Rule rows are filtered to `CurrentReview().Triggers` only.
- Empty AR/CR sections are hidden.
- Reason rows are named generically and show reason labels only.
- The old right-panel title text is no longer present in Presentation code.
- Decision paper uses paper sprites and has only title, compensation, premium, reject, and approve/conditional approve controls.

## C001 Expected Static Flow

Expected:

- Initial bell/document appears; no character visible.
- Bell click starts arrival.
- Presented document opens review.
- No AR/CR sections are visible because no rules trigger.
- Decision paper opens.
- Approve awards floor 1% of final premium once.
- Character exits; document disappears; C002 waits for bell.

Static confidence: High. Play Mode confirmation still required.

## C002 Expected Static Flow

Expected:

- C002 starts from Main waiting state.
- Applicable AR reason for missing registration is visible.
- Reject creates no ducat incentive.
- Character exits; document disappears; C003 waits for bell.

Static confidence: High. Play Mode confirmation still required.

## C003 Expected Static Flow

Expected:

- C003 starts from Main waiting state.
- Applicable CR reasons are visible.
- Selecting CR reasons changes right button to `조건부 승인`.
- Premium display increases by 10% per selected CR.
- Compensation display remains the original coverage amount.
- Conditional approval awards floor 1% of the adjusted premium once.
- Character exits; document disappears; completed state appears.

Static confidence: Medium-high. Play Mode confirmation still required, especially the number of selected CR rows and visual fit.

## Validation Skipped

Skipped:

- Unity compile, because the exact project editor executable was unavailable and no generated `.csproj` exists.
- Play Mode interaction checks, because they require a usable Unity editor.
- Screenshot capture, because it requires Play Mode.

## Risks

- C# compile errors may remain until Unity regenerates project files or the exact editor is restored.
- Visual proportions and text overlap still require screenshot QA.
- ScrollRect click/scroll interaction should be checked manually because generated uGUI uses a parent Button and child scroll view.
- The generated UI remains a first-playable runtime composition, not Main prefab parity.
