# First Playable QA Guide

Last updated: 2026-07-06.

## How To Run

1. Open the Unity project at `EpicProjectR/`.
2. Open `Assets/Scenes/SampleScene.unity`.
3. Press Play.
4. A runtime-generated `First Playable Canvas` should appear automatically.
5. The UI should open on a Main-scene-style bell/document/contractor entry composition. After clicking the bell or document, the review layout should appear with a full-screen background, top HUD, upper-left dialogue/contractor area, central draggable document workbench, right split AR/CR checklist, lower-right `인수 결정서`, and bottom decision drawer.

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
- Document section title: `제출 서류`
- Absolute rejection section title: `절대 거절 사유`
- Consideration section title: `거절 고려 사유`
- Decision box title: `인수 결정서`

## Manual Checks

- Confirm the top HUD shows `리세브라 해상 보험 심사국`, `1599년 01월 15일`, ducats, and reputation.
- Confirm normal player-facing UI text is Korean.
- Confirm the imported Noto Serif KR font is used for title/body text, or that the UI falls back to an OS font if Unity has not imported the font yet.
- Confirm MainSceneUI background, bell, paper, tab button, contractor, speech bubble, and letter visuals appear where imported Resources assets are available.
- Confirm the normal entry/review UI does not show a contract list, current-contract detail paper, fixture marker, or internal document IDs.
- Confirm document data appears as paper-style document cards for each contract.
- Confirm missing registration is visible and emphasized for `C002`.
- Confirm rule checklist rows show reason names only, not `AR01`, `AR02`, `AR03`, `AR04`, `CR01`, or other source IDs.
- Confirm hidden `AccidentFlag` is not shown in entry, review, document cards, rule checklist, or result UI.
- For `C001`, click `승인` with no checked rules and expect correct approval.
- After submitting `C001`, confirm the UI does not auto-advance and accepts either the visible continue control or document-board click for progression.
- For `C002`, check the visible absolute rejection reason for missing registration and click `거절`. Expect no active contract and no regular accident.
- After submitting `C002`, confirm the result panel says no active contract was created.
- For `C003`, check the visible consideration reasons for old hull and accident history, click `승인`, and expect a 150 percent premium quote plus deterministic accident outcome.
- After submitting `C003`, confirm the continue button changes to `종료`.
- Confirm settlement/evaluation feedback appears after each submit.
- Confirm the session reaches a complete state after the third contract.

## Known Limitations

- UI is a QA-ready generated game draft, not final art.
- UI is generated at runtime instead of stored as a prefab or scene hierarchy.
- Imported reference sprites are loaded build-safely from `Assets/Resources/FirstPlayable/ImportedReference/RuntimeSafe/`, but final sprite import settings and slicing have not been visually tuned.
- Contract map, claim sheet, after-story, cargo, mixed contracts, and full economy are not implemented.
- The three contracts are fixtures, not final 168-contract data.
- Conditional approval is exposed only through the decision drawer when one or more CR rows are checked.
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

## M8.9 Main Runtime Loop QA Addendum

- Confirm the initial screen shows a clickable contract/document entry state before the review panels appear.
- Confirm clicking the entry paper starts the review sequence.
- Confirm the dialogue/contractor, workstation, docket, and AR/CR shelf slide into view.
- Confirm generated documents can be dragged with the mouse and stay inside the workstation bounds.
- Confirm the right shelf is split into AR and CR sections.
- Confirm clicking the lower-right final decision box raises the decision drawer from the bottom.
- Confirm the drawer places `거절` on the left and `승인`/`조건부 승인` on the right.
- For `C003`, check the visible old-hull and accident-history consideration reasons; confirm the drawer approve label changes to `조건부 승인` and the Main-style drawer premium reads `120%`.
- Confirm the submitted result still reports the application fixture premium quote and C001/C002/C003 flow remains correct.

## M8.11 Main UI Size/Image QA Addendum

- Confirm the right shelf is taller and narrower, matching the old Main shelf region rather than the previous compact panel.
- Confirm the central workstation occupies the measured Main display area and document drag feels bounded to that work surface.
- Confirm document cards are taller, closer to old document prefab proportions.
- Confirm the decision drawer is lower-right, narrower, taller, and uses `120x65` reject/approve tab buttons.
- Confirm the decision drawer opens from the lower-right Main-like closed position and does not cover the AR/CR shelf.
- Confirm exact Main PNG assets appear where available; `.aseprite` shelf/table art is still an acknowledged fallback.

## M8.12 Entry HUD Dialogue Decision QA Addendum

- Confirm the initial composition is bright enough and shows bell, document, and contractor preview near the center.
- Confirm clicking the bell or document starts the contractor/review sequence.
- Confirm the contractor starts smaller/darker/faded and settles brighter/larger.
- Confirm the dialogue panel is large, upper-left, and advances lines when clicked.
- Confirm clicking `인수 결정서` opens the lower-right decision paper and clicking it again closes the paper.
- Confirm the decision paper has `거절` on the left and `승인` or `조건부 승인` on the right.
- Confirm selected reason summary appears inside a lower white paper-like area.
- Confirm approving a contract increases HUD ducats by floor 1% of the submitted premium and updates reputation from settlement score.
- Confirm after approve/reject, the contractor exits; then document-board click progresses to the next contract.
- Confirm source IDs remain exact internally but are not displayed in normal document/rule labels.
