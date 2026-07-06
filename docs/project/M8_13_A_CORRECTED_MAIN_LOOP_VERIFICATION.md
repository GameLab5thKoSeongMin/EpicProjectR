# M8.13-A Corrected Main Loop Verification

Status: Completed before implementation.

## 1. Latest User Clarification

Confirmed: M8.13 corrects the M8.12 misunderstanding. The bell starts contractor arrival. The presented document starts review. During review, only the presented document returns to the Main composition; the workstation document board must not close review. After approval, conditional approval, or rejection, the character exits with the document, the UI returns to Main composition, and the next contract waits for a new bell click.

Confirmed UX details:

- Initial Main composition is bright, with no character visible.
- A small document sits on the center-bottom table / blue cloth area.
- The bell sits left of the document.
- Top black HUD remains visible above all review UI.
- Review layout is left chat/character, large center workstation, narrow right AR/CR and decision paper.
- Dialogue accumulates in a scrollable chat log.
- Right panel shows only applicable rejection reasons for the current case.
- Decision paper is paper-like, toggles open/closed, and contains only title, compensation, premium, reject, and approve/conditional approve controls.

## 2. M8.12 Behaviors That Were Wrong

Wrong in M8.12:

- The initial screen could show a contractor preview.
- Bell or document both started review directly.
- Review entry did not require the character to finish arriving first.
- Dialogue was closer to one large bubble than an accumulated chat log.
- The workstation board could close review or advance after result.
- The right panel rendered all active AR/CR rules instead of only case-applicable findings.
- Empty AR/CR sections remained visible.
- The decision button/paper could use bell-like imagery.
- Decision paper contained extra premium/explanation/result text.
- Post-decision flow waited on a continue/document-board action instead of returning to Main composition after character exit.

## 3. Main.unity Behavior That Supports The Corrected Loop

Confirmed from `ref_folder/Assets/Scenes/Main.unity` and directly attached scripts:

- `GameLoopController.RingBell()` only acts while waiting for the bell and opens the next visitor.
- Main has a top HUD with date/money/performance style fields.
- Main has a `DialogueLogBubblePanelView` and `DialogueBubbleClickArea`; dialogue is accumulated into scrollable message rows and advanced by clicking the dialogue area.
- Main uses `RectTransformToggleMover` and `UnderwritingDecisionToggleMoveGate` for decision-paper movement after decisions.
- Main has a `MissedRejectionReasonPaperSpawner` that filters reasons before spawning paper rows.
- Main has background image `Main_Total.png`, already mirrored as runtime-safe `main_total.png`.

## 4. Project Decisions Not Fully Verifiable In Main

Project decisions for this first playable:

- Keep the current active scene as runtime host only; do not copy Main scene objects.
- Use generated uGUI Presentation objects instead of copied prefabs/scripts.
- Use C001-C003 fixture reviews for applicable reason filtering.
- Use 10% premium increase per selected CR in Presentation decision paper.
- Apply ducat incentive as floor 1% of final premium once per approved/conditionally approved contract.
- Hide normal UI source IDs while preserving exact IDs internally.

## 5. Updated Implementation Plan

1. Add explicit first playable loop states for waiting, arrival, presented document, review, drawer, submitted, exiting, and next waiting.
2. Update entry hit targets so bell starts arrival and the document starts review only after arrival.
3. Hide the entry character until arrival and animate it from small/dark/faded to lower-left/character area.
4. Rework review layout to preserve HUD and use left 30-35%, center about 50%, right 15-20%.
5. Replace single dialogue text with scrollable accumulating chat bubbles.
6. Filter rule rows to `CurrentReview().Triggers` only.
7. Hide AR/CR sections that have no applicable rows.
8. Make decision box/drawer use paper imagery and only the required text/actions.
9. Remove workstation board close/continue behavior.
10. After decision, show a short character line, animate exit, return to Main composition, and wait for the next bell click.

## 6. Risk Table

| Risk | Level | Mitigation |
| --- | --- | --- |
| Exact Main pixel parity remains screenshot-dependent | Medium | Centralize rects in `FirstPlayableMainSceneRectSpec` and document skipped Play Mode screenshot validation if unavailable. |
| Unity compile may be blocked by local editor/tooling environment | Medium | Run static checks and attempt Unity/dotnet validation; record exact failures. |
| Current first playable text is mojibake Korean source text | Medium | Preserve existing labels requested by the milestone and avoid broad localization rewrites. |
| Generated uGUI chat log may differ from TMP Main implementation | Low | Match behavior: accumulated rows, scrolling, click-anywhere dialogue advance. |
| Filtering only triggered rules changes C001/C002/C003 visibility | Low | Validate expected C001 no reasons, C002 AR reason, C003 CR reasons. |
| Post-decision auto-return timing may need tuning | Low | Keep transition short and confined to Presentation tween code. |
