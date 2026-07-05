# M8.9-B Current Vs Main Behavior Gap Plan

## Gaps Before Implementation

| Gap | Prior state | M8.9 action |
| --- | --- | --- |
| Entry state | Review UI appeared immediately. | Add `Entry` screen mode and clickable incoming contract paper. |
| Opening sequence | Panels were static. | Tween dialogue, contractor, workstation, docket, and right shelf into view. |
| Documents | Cards were vertically scrolled. | Place cards on central board and add mouse drag. |
| Drag bounds | No drag. | Clamp document drag to workstation board. |
| AR/CR split | One combined checklist. | Render AR and CR into separate upper/lower right sections. |
| Decision trigger | Decision paper always visible. | Add compact lower-right decision box. |
| Decision drawer | No bottom rise. | Drawer opens bottom-to-top on decision box click. |
| Conditional approval | Not exposed in normal UI. | Show `조건부 승인` only when a CR row is checked. |
| Main premium | Domain quote showed fixture `125/150`. | Add Main-style selected-CR `+10%` drawer display without changing domain quote. |

## Architecture Plan

- Keep `FirstPlayableBootstrap` as composition root.
- Keep `FirstPlayablePresenter` responsible for screen mode transitions and session submission.
- Keep `FirstPlayableView` passive: it renders state and exposes events.
- Keep `FirstPlayableUiFactory`, `FirstPlayableUiTheme`, and `FirstPlayableAssetCatalog` as creation/style/asset boundaries.
- Add small Presentation helpers only:
  - `FirstPlayableDragController`
  - `FirstPlayableTweenRunner`
  - `FirstPlayableMainLoopState`

## Non-Goals

- No source ID changes.
- No domain/application behavior change.
- No old script copying.
- No scene, prefab, package, project setting, or `ref_folder` modification.
- No normal UI exposure of hidden accident source fields.
