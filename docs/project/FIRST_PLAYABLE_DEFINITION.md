# First Playable Definition

Last updated: 2026-07-05.

## Definition

The first playable loop is a small, ship-only underwriting loop over fixture data. It proves the player can review contracts, make decisions, see audit feedback, resolve deterministic outcomes, and advance through a tiny sequence.

## Required Capabilities

- Player can start a small turn/session.
- Player can inspect 2-3 ship insurance contract fixtures.
- Player can inspect bundle 1-3 document data.
- Player can see/check representative AR/CR reasons.
- Player can approve or reject.
- Conditional approval is reserved in the model but not required in UI unless confirmed.
- System can audit decision correctness.
- System can calculate CR-based premium recommendation when relevant.
- Approved contracts can become active contracts.
- `AccidentFlag`-driven outcomes resolve deterministically.
- Rejected contracts do not produce regular accidents.
- Minimal settlement/evaluation feedback can be shown.
- Turn or case sequence can advance.

## Intentionally Excluded

- Cargo bundles 4-6.
- Mixed bundle 7.
- Full 24-turn schedule.
- Full 168-contract import.
- Final route map animation.
- Insurance claim sheet detail UI.
- Full settlement economy.
- Special contractor dialogue and after-story branches.
- Full save/load UI.
- Imported reference scenes, prefabs, or scripts.

## Assumptions

| Area | Assumption |
| --- | --- |
| Decisions | Approve/reject first; conditional approval reserved. |
| Premium | CR-driven quote, not freeform player editing. |
| Rules | Small representative AR/CR subset from ship bundle docs. |
| Content | In-memory fixtures before source spreadsheet import. |
| UI | Functional and minimal; no imported reference UI. |
| Outcomes | Deterministic `AccidentFlag`, hidden from player UI. |

## Must Be Confirmed Before Full 24 Turns

- Access and final status of the 8 source workbooks.
- Exact 18 AR and 12 CR rule rows.
- Exact cargo/mixed document fields.
- Route table and coordinates.
- Final conditional approval semantics.
- Final economy parameters from `05_Economy.xlsx`.
- Claim sheet field definitions.
- Special contractor branch/event data.

## Manual QA Checklist

- Start the first playable session.
- Confirm turn/date header displays fixture turn data.
- Confirm 2-3 contracts appear in order.
- Open each contract and inspect bundle documents.
- Confirm submitted/missing documents are visible.
- Confirm AR/CR checklist shows only player-facing reasons.
- Submit a correct rejection and confirm audit feedback.
- Submit a correct approval and confirm active contract creation.
- Submit a CR case and confirm premium recommendation.
- Confirm hidden `AccidentFlag` is not displayed in normal UI.
- Resolve an approved flagged contract and confirm deterministic accident.
- Reject a flagged contract and confirm no regular accident occurs.
- Confirm minimal settlement/evaluation line items are traceable.
- Advance to the next case or end of fixture sequence.

