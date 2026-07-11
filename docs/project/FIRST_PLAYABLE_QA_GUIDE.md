# First Playable QA Guide

Last updated: 2026-07-06.

## How To Run

1. Open the Unity project at `EpicProjectR/`.
2. Open `Assets/Scenes/SampleScene.unity`.
3. Press Play.
4. A runtime-generated `First Playable Canvas` should appear automatically.
5. The UI should open on a bright Main-scene-style bell/document entry composition with no character visible. Click the bell first, wait for the contractor to arrive, then click the presented document to enter review.

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
- Confirm the initial screen has no visible contractor, has a small document on the center-bottom table/blue cloth area, and has the bell to the left of the document.
- Confirm clicking the bell starts contractor arrival, but does not open review.
- Confirm clicking the presented document after arrival opens review.
- Confirm clicking the presented document during review returns to the Main composition for the same pending contract.
- Confirm clicking the workstation document area does not return to Main and does not advance after a decision.
- Confirm the normal entry/review UI does not show a contract list, current-contract detail paper, fixture marker, or internal document IDs.
- Confirm document data appears as paper-style document cards for each contract.
- Confirm missing registration is visible and emphasized for `C002`.
- Confirm rule checklist rows show only applicable reason names for the current case, not all possible reasons and not `AR01`, `AR02`, `AR03`, `AR04`, `CR01`, or other source IDs.
- Confirm empty AR/CR sections are hidden.
- Confirm hidden `AccidentFlag` is not shown in entry, review, document cards, rule checklist, or result UI.
- For `C001`, click `승인` with no checked rules and expect correct approval.
- After submitting `C001`, confirm the contractor gives a short line, exits with the document, and the next contract waits on the Main composition for another bell click.
- For `C002`, check the visible absolute rejection reason for missing registration and click `거절`. Expect no active contract and no regular accident.
- After submitting `C002`, confirm no ducat incentive is awarded and the next contract waits on the Main composition for another bell click.
- For `C003`, check the visible consideration reasons for old hull and accident history, click `승인`, and expect the decision paper right button to change to `조건부 승인` with a 10% premium increase per selected CR.
- After submitting `C003`, confirm the contractor exits, the document disappears, and the completed state appears.
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

## M8.13 Corrected Main Loop QA Addendum

- Confirm the initial composition is bright enough and shows only the small document and bell, with no character visible.
- Confirm bell click starts arrival only: the contractor starts small/dark/faded and settles brighter/larger near the lower-left character area.
- Confirm the bell becomes inactive/semi-transparent after arrival and the presented document remains clickable.
- Confirm only the presented document opens review; the bell does not.
- Confirm the top HUD remains visible and above all review panels.
- Confirm the left review area is split into an upper scrollable chat log and lower character/document/bell composition.
- Confirm dialogue clicks append lines to the chat log instead of replacing one speech bubble.
- Confirm the center workstation is the largest area and documents remain draggable/clamped.
- Confirm the right panel shows only applicable reasons and hides empty sections.
- Confirm `인수 결정서` is a paper-like button, not a bell, and toggles the paper open/closed.
- Confirm the open decision paper shows only `손해보상금`, `보험료`, `거절`, and `승인`/`조건부 승인`.
- Confirm selected CR reasons increase premium by 10% each and do not change compensation.
- Confirm approval/conditional approval awards floor 1% of final premium once per contract; rejection awards none.
- Confirm after approve/reject, the contractor exits, the document disappears, and the next contract waits for another bell click.
- Confirm source IDs remain exact internally but are not displayed in normal document/rule labels.

## M8.14 Main Asset Fidelity QA Addendum

- Confirm all MainSceneUI visuals load from `Resources` without a repository-level `using_image` dependency.
- Confirm the first paper and bell match Main's measured `102x75` and `120x120` proportions and positions.
- Confirm the HUD office title is fully visible at the upper left.
- Confirm the physical trapezoid paper prop is used for presented documents, while broad UI surfaces use the rectangular paper texture.
- Confirm reason rows are readable in normal and selected states.
- Confirm the decision amount area is rectangular paper and does not display the stretched paper-prop silhouette.
- Run `EpicProjectR.Tests.M8_14PlayModeCapture.Run` from Unity command line or an editor invocation to regenerate the C001-C003 screenshot sequence.
- Treat the current automated 1280 request as unverified until a fixed Game View preset produces a true 1280x720 PNG.

## M8.18 1920x1080 Correction QA Addendum

- Confirm the first screen uses the imported Main background from build-safe Resources and no longer appears overly dark.
- Confirm the top HUD title is not clipped, the date is visually centered, and ducats/reputation are cleanly right-aligned at 1920x1080.
- Confirm the contractor arrives from the center-bottom of the Main screen, then settles into the left-side visible position.
- Confirm the review layout reads as left chat/character area, largest center workstation, and narrow right reason/decision panel.
- Confirm the chat panel scrolls with the mouse wheel and still appends one small bubble at a time when clicked.
- Confirm right-panel reason rows fit within the panel and show only applicable reason text, not source IDs.
- Confirm the decision paper opens upward and closes downward when `인수 결정서` is clicked again.
- Confirm rapid open/close clicks do not leave the decision paper hidden while the presenter is in decision-open mode.
