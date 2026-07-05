# First Playable QA Guide

Last updated: 2026-07-05.

## How To Run

1. Open the Unity project at `EpicProjectR/`.
2. Open `Assets/Scenes/SampleScene.unity`.
3. Press Play.
4. A runtime-generated `First Playable Canvas` should appear automatically.
5. The UI should use a Main-scene-inspired layout with a full-screen background, top date/status strip, left paper contract docket, central workbench document area, right shelf-style AR/CR checklist, and bottom-right decision/result paper.

No scene or prefab needs to be manually wired for this pass.

## Expected Flow

The fixture session contains 3 ship insurance contracts:

| Contract | Expected purpose | Expected good decision |
| --- | --- | --- |
| `C001` | Clean bundle 1 approval case | Approve |
| `C002` | Missing registration absolute rejection case | Reject |
| `C003` | Consideration case with old hull and recent accident history | Approve with CR premium quote |

## Expected Korean Labels

- Approve button: `승인`
- Reject button: `거절`
- Next button: `다음 계약`
- Finish button: `종료`
- Contract docket title: `계약 목록`
- Current case title: `현재 계약`
- Document section title: `제출 서류`
- Rule section title: `AR / CR 심사 항목`

## Manual Checks

- Confirm the turn/date header shows turn 1 and `1599-01-15`.
- Confirm normal player-facing UI text is Korean.
- Confirm the imported Noto Serif KR font is used for title/body text, or that the UI falls back to an OS font if Unity has not imported the font yet.
- Confirm MainSceneUI background, paper, tab button, ship, seal, and letter visuals appear where imported Resources assets are available.
- Confirm contract IDs are displayed exactly as `C001`, `C002`, and `C003`.
- Confirm a fixture marker is visible for each current case.
- Confirm document data appears as paper-style document cards for each contract.
- Confirm missing registration is visible and emphasized for `C002`.
- Confirm rule checklist uses exact rule IDs such as `AR01` and `CR01`.
- Confirm hidden `AccidentFlag` is not shown in the normal contract panel.
- For `C001`, click `승인` with no checked rules and expect correct approval.
- After submitting `C001`, confirm the UI does not auto-advance and requires `다음 계약`.
- For `C002`, check `AR01` and click `거절`. Expect no active contract and no regular accident.
- After submitting `C002`, confirm the result panel says no active contract was created.
- For `C003`, check `CR01` and `CR02`, click `승인`, and expect a 150 percent premium quote plus deterministic accident outcome.
- After submitting `C003`, confirm the continue button changes to `종료`.
- Confirm settlement/evaluation feedback appears after each submit.
- Confirm the session reaches a complete state after the third contract.

## Known Limitations

- UI is a QA-ready generated game draft, not final art.
- UI is generated at runtime instead of stored as a prefab or scene hierarchy.
- Imported reference sprites are loaded build-safely from `Assets/Resources/FirstPlayable/ImportedReference/RuntimeSafe/`, but final sprite import settings and slicing have not been visually tuned.
- Contract map, claim sheet, after-story, cargo, mixed contracts, and full economy are not implemented.
- The three contracts are fixtures, not final 168-contract data.
- Conditional approval is modeled but not exposed in the first UI.
- Some internal fixture data remains English in source code, but normal UI presentation should map it into Korean.

## Bug Report Notes For Next Pass

When reporting bugs, include:

- Contract ID.
- Rule IDs checked.
- Decision submitted.
- Expected result.
- Actual result.
- Screenshot if UI layout is involved.

## M8.6 Visual QA Addendum

Use this in the next manual visual QA pass:

- Verify the runtime UI still appears automatically without scene wiring.
- Verify imported sprites do not stretch badly at 1920x1080 and common laptop aspect ratios.
- Verify document cards remain readable with the paper texture and seal treatment.
- Verify button hit areas and labels remain clear over the imported button sprite.
- Verify the ship/ribbon header art supports the maritime underwriting tone and does not make the screen feel like a fantasy inventory UI.
- Verify no hidden fixture accident field is visible in the contract summary, document cards, rule checklist, or result panel.

## M8.7 Functional QA Addendum

- Confirm clicking text on top of `승인`, `거절`, `다음 계약`, and `종료` still activates the button.
- Confirm clicking rule row text still lets the row/toggle respond as expected.
- Confirm source IDs remain exact even inside Korean sentences.
- Confirm no normal UI text displays `AccidentFlag` or `WillAccidentIfApproved`.

## M8.8 Main Scene Parity QA Addendum

- Confirm the first screen no longer reads as a generic 3-column QA dashboard.
- Confirm the background and composition resemble the old Main scene's workbench, paper, shelf, and bottom-right decision panel structure.
- Confirm `승인` and `거절` use compact tab-like button art when MainSceneUI Resources assets import successfully.
- Confirm missing optional MainSceneUI sprites fall back to readable generated colors rather than throwing exceptions.
- Confirm Korean text remains readable over the darker background and paper panels.
