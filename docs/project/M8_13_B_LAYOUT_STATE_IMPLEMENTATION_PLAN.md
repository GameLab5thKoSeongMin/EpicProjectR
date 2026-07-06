# M8.13-B Layout And State Implementation Plan

Status: Completed before implementation.

## Allowed Paths

Allowed for this milestone:

- `EpicProjectR/Assets/Scripts/Presentation/`
- `EpicProjectR/Assets/Tests/EditMode/` if needed
- `EpicProjectR/Assets/Tests/PlayMode/` only if safe
- `EpicProjectR/Assets/ImportedReference/RuntimeSafe/MainSceneUI/`
- `EpicProjectR/Assets/Resources/FirstPlayable/ImportedReference/RuntimeSafe/MainSceneUI/`
- `docs/project/`
- `docs/project/screenshots/m8_13/` if generated

Forbidden:

- `ref_folder/` modifications
- active scenes, unrelated prefabs, unrelated ScriptableObjects
- `EpicProjectR/Packages/`
- `EpicProjectR/ProjectSettings/`
- old C# script copies

## Explicit Loop States

The corrected loop should be represented by `FirstPlayableScreenMode` plus view transitions, not scattered booleans.

| State | Purpose | Entry action | Valid next state |
| --- | --- | --- | --- |
| `MainWaitingForBell` | Initial / next-contract Main composition | Show bright background, top HUD, small document, active bell, no character | `CharacterArriving` |
| `CharacterArriving` | Bell was clicked | Disable bell/document, animate character in | `DocumentPresented` |
| `DocumentPresented` | Character has presented document | Keep document clickable, bell inactive/semi-transparent | `OpeningReview` |
| `OpeningReview` | Review panels begin camera-like transition | Show left/center/right review layout | `Reviewing` |
| `Reviewing` | Normal review work | Chat advances, documents drag, reasons select | `DecisionPaperOpen`, `MainWaitingForBell` |
| `DecisionPaperClosed` | Decision paper closed while reviewing | Small paper button visible | `DecisionPaperOpen` |
| `DecisionPaperOpen` | Decision paper raised/open | Reject/approve actions enabled | `DecisionPaperClosed`, `DecisionSubmitted` |
| `DecisionSubmitted` | Decision accepted by session | Apply ledger/result data | `CharacterExiting` |
| `CharacterExiting` | Character thanks/exits with document | Hide decision controls, animate exit | `MainWaitingForBell` or `Completed` |
| `Completed` | C001-C003 fixture session complete | Show completion state | End |

## Layout Plan

Initial Main:

- Top HUD: full width, black strip, highest sibling order.
- Document: small paper on center-bottom table/blue cloth area.
- Bell: left of document.
- Character: hidden.

Review:

- Left band: 30-35% screen width below HUD.
- Left upper 40%: chat log panel with scrollable accumulated bubbles.
- Left lower 60%: character + presented document + inactive bell composition.
- Center: about 50% screen width, largest workstation, draggable documents clamped inside.
- Right: 15-20% screen width, applicable AR/CR rows and decision paper.
- HUD remains independent and above all roots.

## Trigger Plan

1. Main bell click invokes presenter arrival request.
2. Presenter enters `CharacterArriving` and asks the view to play arrival.
3. View notifies presenter when arrival completes.
4. Presenter enters `DocumentPresented`; the document is now the review trigger.
5. Presented document click enters review.
6. During review, presented document click closes review to Main composition for the same pending contract.
7. Workstation clicks do nothing except support document drag.
8. Decision submit invokes session, applies ledger, and starts exit.
9. Exit completion returns to Main composition for the next contract, or completed state after C003.

## C001-C003 Expected State Notes

- C001: no applicable AR/CR rows; both sections hidden; approve creates incentive and returns to Main.
- C002: applicable AR row visible; reject creates no incentive and returns to Main.
- C003: applicable CR rows visible; selecting CR changes approve label to conditional approve, premium increases 10% per selected CR, compensation unchanged, incentive applies after conditional approval.
