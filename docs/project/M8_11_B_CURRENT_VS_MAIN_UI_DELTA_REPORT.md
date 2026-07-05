# M8.11-B Current Vs Main UI Delta Report

## Role Mapping

| Role | Main object | Current generated object | Status after M8.11 | Notes |
| --- | --- | --- | --- | --- |
| Entry document | `BackgroundImage/Paper` | `Clickable Entry Contract` | close | Current is larger for readable Korean fixture summary; uses exact paper source. |
| Dialogue panel | `Character/Ask/Button` and dialogue region | `Dialogue Panel` | close | Uses safe speech bubble image/fallback and measured lower-left area. |
| Contractor visual | `Character` region | `Contractor Visual` | approximate | Uses safe temporary contractor image if available. |
| Workstation | `WorkBenchDisplay/GameObject` | `Central Workbench Surface` | close | Resized to measured `869x1001` area. |
| Draggable document | four Main document prefabs | generated document cards | close | Root sizes now follow Main prefab sizes; old prefabs/scripts not copied. |
| AR panel | `EssentialPanel` | AR scroll section | approximate | Exact `.aseprite` sliced paper skipped; region and row height aligned. |
| CR panel | `ConsideratePanel` | CR scroll section | approximate | Exact `.aseprite` sliced paper skipped; region and row height aligned. |
| Final decision box | decision paper trigger | `Final Decision Box` | close | Moved to lower-right measured decision area. |
| Decision drawer | decision `Image` | `Bottom Right Decision Paper` | close | Size/position corrected to `481x399`, open/closed positions derived from Main. |
| Reject button | `Reject` | reject drawer button | close | Uses exact tab image, `120x65`. |
| Approve button | `Accept` | approve drawer button | close | Uses exact tab image, `120x65`, label changes to conditional approve on CR. |
| Background/decor | `BackgroundImage` | `Main Scene Background` | exact | Uses `Main_Total.png` fallback-safe Resources load. |

## RectTransform Delta Priority

| Role | Previous current | Main target | M8.11 action | Severity |
| --- | --- | --- | --- | --- |
| Shelf | `430x770`, right `-238,-18` | `485x1001`, right `-242,-40` | Updated spec and View. | P0 |
| Workbench | `880x790`, center `-110,-12` | approx `869x1001`, center `44,-39` | Updated spec and View. | P0 |
| Decision drawer | `690x285`, right `-552,156` | `481x399`, right `-241,200`, closed `-241,-94` | Updated spec and View. | P0 |
| Decision buttons | layout-driven `~48h` | `120x65` | Added fixed drawer button size. | P0 |
| Document cards | `292x188` submitted | Main roots `252x300`, `319x333`, `415x449` | Added measured document size array. | P0 |
| Rule rows | `72h` | `~72.62h` | Centralized `73h`. | P1 |
| Entry paper | `760x650` | Main click paper `102x75` | Reduced to `420x310` for readable first playable summary. | P1 |
| Docket | left `190,-18` | no exact Main docket; object region left/background | Moved left as auxiliary paper stack. | P2 |

## Image Delta

| Role | Main image | Current action |
| --- | --- | --- |
| Background | exact PNG | Keep exact. |
| Decision paper | exact PNG | Keep exact. |
| Reject/Accept tabs | exact PNGs | Keep exact and resize buttons. |
| Entry paper | exact PNG | Keep exact. |
| Letter | exact PNG | Keep exact. |
| Shelf/AR/CR papers | `.aseprite` sliced sprites | Skip exact import; use safe generated panels and document limitation. |
| Workbench/table blue art | `.aseprite` | Skip exact import; use paper texture/work panel fallback. |

## Priority Fix List

P0 completed:

- Major panel size/position mismatch for shelf, workstation, decision drawer.
- Wrong decision button hit-area size.
- Document card size mismatch.
- Drag bounds now use larger measured workstation board.

P1 completed:

- Centralized measured specs in `FirstPlayableMainSceneRectSpec`.
- Animation distances updated to Main-like closed/open positions.

P2 remaining:

- Exact `.aseprite` shelf/table art remains skipped.
- TMP SDF font rendering remains approximated with runtime-safe uGUI font files.
