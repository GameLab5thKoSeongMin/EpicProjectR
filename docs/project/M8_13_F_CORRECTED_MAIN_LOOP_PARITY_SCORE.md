# M8.13-F Corrected Main Loop Parity Score

Status: Static score. Play Mode screenshot scoring pending.

## Score

Previous M8.12 score against the corrected M8.13 requirements: `61 / 100`.

Current M8.13 static score: `91 / 100`.

The score improves because the corrected trigger order, state flow, reason filtering, decision paper, and next-contract wait loop are now represented in code. It is capped because Unity compile, Play Mode QA, and screenshots could not be completed in this environment.

## Category Scores

| Category | Score | Notes |
| --- | ---: | --- |
| Initial Main composition | 4 / 5 | No character, small document, bell-left implemented; screenshot tuning pending. |
| Bell click trigger | 5 / 5 | Bell starts arrival only. |
| Character arrival | 4 / 5 | Move/scale/fade/tint implemented; exact timing pending. |
| Presented document click | 5 / 5 | Review starts only from presented document after arrival. |
| Review transition | 4 / 5 | Camera-like panel movement preserved; visual QA pending. |
| Left chat UI | 4 / 5 | Scrollable log implemented; exact Main/TMP style differs. |
| Chat progression/scroll | 4 / 5 | Lines accumulate; manual scroll/click QA pending. |
| Character/bell/document lower-left composition | 4 / 5 | Implemented in review; screenshot proportions pending. |
| Center workstation proportion | 4 / 5 | Center is largest; exact 50% target pending screenshot QA. |
| Right AR/CR filtering | 5 / 5 | Only triggered/applicable rules render. |
| Reason row visual fit | 4 / 5 | Paper rows constrained to right panel; visual QA pending. |
| Decision paper structure | 5 / 5 | Only title, compensation, premium, reject, approve/conditional approve. |
| Decision paper toggle | 4 / 5 | Toggle open/closed retained; closed active-hit behavior needs Play Mode check. |
| Approve/conditional approve behavior | 4 / 5 | Conditional label and selected CR submission preserved. |
| Ducat incentive | 5 / 5 | floor 1% once-only incentive uses 10% CR-adjusted premium. |
| Character exit/next contract loop | 5 / 5 | Exit returns to Main/complete; no auto-review. |
| HUD correctness | 4 / 5 | Date is center-anchored and HUD is top sibling; screenshot pending. |
| Background brightness | 4 / 5 | Overlay reduced to 0.04; screenshot pending. |
| C001/C002/C003 flow stability | 3 / 5 | Static confidence high, but no Play Mode run. |
| Source ID preservation | 5 / 5 | IDs preserved internally. |
| Hidden accident data safety | 5 / 5 | No Presentation matches. |
| Runtime/build safety | 3 / 5 | Runtime-only APIs used; compile not completed. |
| Repository boundary safety | 5 / 5 | No forbidden path diffs. |
| Maintainability | 4 / 5 | Architecture preserved; view remains large but scoped. |

## Before/After Summary

| Area | M8.12 against corrected requirement | M8.13 |
| --- | --- | --- |
| Entry | Character preview visible; bell/document could start review | Character hidden; bell starts arrival; document starts review after arrival |
| Review return | Workstation board could return/advance | Presented document only |
| Dialogue | Large single bubble style | Accumulating scrollable chat log |
| Reasons | Active rules could include non-applicable rows | Triggered/applicable rules only |
| Empty sections | Could remain visible | Hidden |
| Decision paper | Extra text and bell-like trigger risk | Paper trigger and three-region paper |
| Premium | Mixed with older application premium text | 10% per selected CR in decision paper/incentive |
| Post-decision | Continue/document-board progression | Character exits, document disappears, next waits for bell |

## Validation Caveat

This is a static parity score. Final score should be updated after Unity `6000.3.19f1` compile and Play Mode screenshots.
