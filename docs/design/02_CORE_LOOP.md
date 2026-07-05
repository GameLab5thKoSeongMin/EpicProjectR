# Core Loop

Last inspected: 2026-07-05.

## Confirmed Loop

1. **Turn start**
   - A workday begins on the scheduled date.
   - The full game uses 24 monthly workdays.

2. **News / rule review**
   - News can add, strengthen, or remove underwriting criteria.
   - The active rule list and UI criteria must reflect the current turn.

3. **Contract application intake**
   - The player receives a sequence of contract cases for the turn.
   - The full game target is 7 contracts per turn.

4. **Document bundle inspection**
   - The player inspects the contract's active document bundle.
   - Early ship bundles focus on application, registration, hull inspection, and route declaration.
   - Later bundles add cargo and mixed contract documents.

5. **Rule / checklist comparison**
   - The player compares document data against absolute rejection rules and rejection consideration rules.
   - Triggered objective reasons should be auditable from stable rule IDs.

6. **Subjective information handling**
   - Subjective information may appear as rumors, dialogue, pressure, or other clues.
   - It should not directly modify objective document data.
   - It should not directly score the player. It affects outcomes/narrative responsibility.

7. **Decision**
   - Approval and rejection are confirmed.
   - Conditional approval is referenced in design data and after-story grammar, but exact first-implementation semantics still need confirmation.

8. **Approved contract tracking**
   - Approved contracts enter a contract management map / route tracking surface.
   - RouteID determines path behavior once route data is available.

9. **Accident / outcome resolution**
   - If an approved or conditionally approved contract has `AccidentFlag = 1`, accident occurs deterministically on the return/due date.
   - If rejected, regular accident resolution does not occur; special contractors may still branch through after-stories.

10. **Settlement / evaluation**
    - Objective evaluation score and money are separate tracks.
    - Settlement should explain line items clearly.

11. **After-story feedback**
    - Feedback can arrive through port board, letters, gifts, and inspections.
    - Accident/story reports should appear before financial summary when possible.
    - Board/letter feedback should be throttled to avoid overload.

12. **Turn transition**
    - The game advances to the next scheduled turn, preserving active contracts, queued events, seen news, and player career state.

## Inferred Loop

- First playable should run a small deterministic ship-insurance subset before full 24-turn content exists.
- News should be represented as a scheduled content entry that changes active rule windows, not as random newspaper effects.
- The UI should show only player-facing criteria, never `AccidentFlag`.
- The core loop can be implemented without final dialogue, final map art, or full after-story channels.

## Unclear Loop Points

| Point | Why unclear | Safe default |
| --- | --- | --- |
| Conditional approval | It appears in data guide and after-story trigger grammar, but UI/action semantics are not fully specified. | Model as a possible decision state; implement approve/reject first unless the team confirms details. |
| Manual premium editing | CR pricing is described, but direct player premium editing is not confirmed. | Use CR-driven premium quote only. |
| Subjective information UI | High-level behavior is clear, exact input interaction is not. | Treat as display/narrative input, not rule predicate input. |
| Claim document details | Claim detail page exists but is empty. | Track claims as outcome data; postpone final claim UI. |
| Full dialogue integration | Excel dialogue file is inaccessible. | Use placeholder dialogue IDs or no final dialogue in M0/M1. |

## First Playable Focus

The safest first playable should focus on:

- Ship insurance only.
- Bundle 1, then bundle 2, then bundle 3.
- A small set of deterministic sample cases.
- Absolute rejection checks for missing document, name mismatch, expired registration, departure date mismatch, failed hull inspection.
- Rejection consideration checks for old hull, accident history, repair history, hull defect, bad weather, uncharted route.
- Approval/rejection submission with objective audit result.
- Hidden `AccidentFlag` and deterministic outcome in a test harness.

Do not implement cargo, mixed contracts, full special contractor arcs, final after-story UI, or full economy in the first playable unless the team explicitly changes priority.
