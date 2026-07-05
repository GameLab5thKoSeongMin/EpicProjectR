# M8.12-F Main Loop Playmode Parity Score

Status: Static estimate. Play Mode screenshot scoring pending.

## Score

Previous static estimate after M8.11: 88 / 100.

Current static estimate after M8.12: 92 / 100.

The score improves because the runtime loop now matches Main's entry, HUD, dialogue, decision paper, and cleaned player-facing labels more closely. It is not scored higher because interactive Play Mode/screenshot validation could not be completed in this environment.

## Category Scores

| Category | Score | Notes |
| --- | ---: | --- |
| Top HUD fidelity | 4 / 5 | Main-like title/date/ledger implemented; exact pixel spacing pending. |
| Date/ducat/reputation display | 5 / 5 | Korean date and stacked ledger added. |
| Initial bell/document/character composition | 4 / 5 | Implemented with available assets; exact old positions need screenshot check. |
| Bell click interaction | 5 / 5 | Bell/document start review through presenter event. |
| Character arrival/exit behavior | 4 / 5 | Fade/scale/tint/move implemented; timing approximate. |
| Dialogue UI fidelity | 4 / 5 | Large upper-left panel implemented; exact old log bubble style not fully replicated. |
| Dialogue progression | 5 / 5 | Click advances local lines. |
| Contract list/current contract removal | 5 / 5 | Normal docket/current case paper removed. |
| Document text cleanup | 5 / 5 | Visible document IDs/status removed. |
| AR/CR text cleanup | 5 / 5 | Section labels fixed; row titles hide IDs. |
| UI area proportions | 4 / 5 | Workbench largest, left second, right smaller; screenshot tuning pending. |
| Decision paper toggle behavior | 5 / 5 | Toggle open/close added through presenter state. |
| Approval/rejection flow | 4 / 5 | Existing session flow preserved; manual C001/C002/C003 still pending. |
| Ducat incentive behavior | 5 / 5 | Once-only floor 1% ledger implemented. |
| Background brightness | 4 / 5 | Overlay greatly reduced; visual confirmation pending. |
| Main runtime loop similarity | 4 / 5 | Main-like loop improved; exact old board/document close behavior partly inferred. |
| C001/C002/C003 stability | 3 / 5 | Code path preserved, but compile/play validation inconclusive. |
| Source ID preservation | 5 / 5 | IDs still used internally in typed IDs and mappings. |
| Hidden accident data safety | 5 / 5 | Static search confirms no Presentation exposure. |
| Runtime/build safety | 3 / 5 | Runtime-only APIs used; compile evidence incomplete due environment. |
| Repository boundary safety | 5 / 5 | No forbidden path modifications intended. |
| Code maintainability | 4 / 5 | Architecture preserved; View grew but remains scoped to passive UI. |

## Before/After Summary

| Area | M8.11 | M8.12 |
| --- | --- | --- |
| Entry | Clickable contract paper with dashboard text | Bell/document/contractor entry composition |
| HUD | Cluttered single meta text | Main-like title/date/ledger strip |
| Left UI | Contract list/current case paper | Large dialogue and character area |
| Rules | IDs visible in row titles | Reason text only |
| Documents | IDs/status visible | Player-facing document content only |
| Decision | Open-only drawer | `인수 결정서` toggle drawer |
| Economy | Settlement text only | HUD ledger with approval incentive |

## Validation Caveat

This score is a static parity estimate. Final score should be updated after Unity Play Mode screenshots validate layout, brightness, and click progression.
