# M8.12-A Main Loop, HUD, Dialogue Verification

Status: Completed from static Main scene/script inspection.

Reference scene: `ref_folder/Assets/Scenes/Main.unity` only.

## Main Evidence

| Area | Verification |
| --- | --- |
| Top HUD | `GameLoopController` has `dateText`, `moneyText`, and `performanceText`; `RefreshViews()` updates date/player status. |
| Bell | `bellButton` is wired to `RingBell()`, which opens the next visitor only from `WaitingForBell`. |
| Visitor flow | `OpenNextVisitor()` binds visitor image, starts the review, and can run greeting dialogue before entering workbench. |
| Workbench transition | `workbenchToggleMovers` call `RectTransformToggleMover.ToggleMove()`, confirming panel/rect movement rather than camera logic. |
| Dialogue | `DialogueManager.Advance()` is driven by `DialogueBubbleClickArea`, confirming click-to-progress dialogue behavior. |
| Decision paper | Main `Paper`/decision buttons use `RectTransformToggleMover` and `UnderwritingDecisionToggleMoveGate`; the decision paper rises from a closed to an opened position. |
| AR/CR premium | `considerRejectPremiumIncreaseRate` is `0.1`; checked CR reasons increase premium by 10% each in old Main. |
| AR/CR panel | `MarineInsuranceRejectionReasonListView` separates reject and consider-reject roots. |
| Document/board toggles | Main has board/document buttons wired to `GameObjectToggle`/`RectTransformToggleMover`. |
| Brightness | Main background image is direct full-screen art; current first playable dark overlay was additional and too heavy. |

## Observation Table

| User observation | Main verification result | Implementation decision | Risk |
| --- | --- | --- | --- |
| Top black HUD incomplete | Confirmed by `dateText`, `moneyText`, `performanceText` fields | Add title/date/ducat/reputation HUD; remove cluttered meta string | Styling not pixel-perfect without screenshot tooling |
| Background too dark | Confirmed as current overlay issue, not Main requirement | Reduce overlay alpha from 0.42 to 0.12 | Could still need visual tuning |
| Bell/document centered entry | Partly confirmed: Main has bell/board/paper entry objects | Add centered bell, paper, contractor preview | Exact hierarchy not copied |
| Bell starts arrival | Confirmed by `RingBell()` -> `OpenNextVisitor()` | Entry bell and document both start review | Arrival animation is approximation |
| Character grows/brights on arrival | Partly confirmed by visitor image binding; exact tween not fully encoded in inspected scripts | Add scale/fade/tint arrival | Needs Play screenshot confirmation |
| Dialogue upper-left and clickable | Confirmed by dialogue manager/click area | Add large upper-left dialogue panel with click advancement | Uses fixture-safe lines |
| Contract list/current detail are not Main loop | Confirmed by Main using incoming visitor/document loop | Remove normal left docket/current contract paper from UI | None |
| AR/CR IDs exposed | Current UI issue; Main list uses item views, not source IDs as primary labels | Hide IDs from row title; keep keys internal | Static search still sees IDs in mapping code |
| Decision label should be `인수 결정서` | User decision; Main decision paper confirmed | Rename decision box and drawer | None |
| Decision paper toggles | Confirmed by `RectTransformToggleMover` | Presenter toggles open/closed via view methods | Drawer remains active after close animation |
| Next after result via document click | Main flow uses visitor resolution and toggles; exact click target partly inferred | Document board click advances after result; next button remains fallback | Click-on-card behavior needs Play verification |
| Ducat incentive 1% | User-requested project decision, not found in inspected Main snippet | Add Presentation ledger reward once per approved contract | Economy final source remains unavailable |

## Removed Current UI Elements

- Left contract list paper.
- Current contract detail paper.
- Header meta string containing turn/date/contract/status.
- Document ID suffixes and submitted status labels.
- Top `AR / CR 심사 항목` title.

## Asset Notes

Already available runtime-safe Main assets include background, bell, paper prop, paper texture, letter, contractor placeholder, speech bubble, and decision tab sprites. `.aseprite` shelf/table/panel assets remain skipped because they are not PNG runtime imports in this milestone.
