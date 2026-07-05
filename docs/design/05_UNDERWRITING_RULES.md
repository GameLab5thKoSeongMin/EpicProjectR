# Underwriting Rules

Last inspected: 2026-07-05.

## Rule Model

The game separates underwriting findings into two families:

| Rule family | Meaning | Player consequence | Premium consequence | Implementation note |
| --- | --- | --- | --- | --- |
| Absolute Rejection (`AR`) | A contract must be rejected if this condition is present. | Approval is objectively wrong. | No premium should be accepted. | Use for missing critical documents, critical mismatches, expired documents, hard news bans, and must-accident blockers. |
| Rejection Consideration (`CR`) | A contract is risky but can still be underwritten. | Approval can be objectively valid if priced correctly. | 1 CR = 125 percent, 2 CR = 150 percent, 3+ CR = reject recommended. | The player may still underwrite 3+ CR cases at 150 percent, but accident responsibility remains with the player. |

`AR` and `CR` rules are content data. Rule behavior should be implemented by reusable condition evaluators instead of bespoke per-contract scripts.

## Objective Rules Versus Subjective Information

Objective rules are derived from documents, date windows, route/news state, and table values. These drive correct action, evaluation score, and rule feedback.

Subjective information is tied to one contract's accident outcome. It does not connect to other data and does not modify objective score. It can make a rule-correct approval emotionally painful, or make a risky approval narratively meaningful, but it should not silently change `AR` or `CR` findings.

## Accident Rule

Accidents are deterministic:

- If `AccidentFlag = 1` and the player approves or conditionally approves, an accident occurs on the contract's return/accident date.
- If the player rejects the contract, no accident occurs for that contract.
- Accident chance is not a probability roll.
- `AccidentFlag` must remain hidden from normal player UI.

## News-Driven Rule Windows

News is scheduled by turn and can activate, strengthen, or deactivate rules.

| Confirmed news effect | Turn information | Rule impact |
| --- | --- | --- |
| Banned reef / route zone | Turn 4 | Turns relevant route use into absolute rejection. |
| Cargo criteria begin | Turn 7 | Cargo bundle criteria become available. |
| Loading proof criteria | Turn 8 | Loading certificate risk/absence becomes active. |
| Market spike | Turn 10 | Cargo value and market-value comparison become more important. |
| Winter surcharge | Turn 11 | Winter route risk becomes visible. |
| Winter absolute rule | Turn 12-13 and 23-24 | Winter rule can become absolute rejection for affected routes. |
| Pirate warning | Turn 14 | Pirate route risk becomes a consideration. |
| Pirate escalation | Turn 17 | Pirate route risk can become absolute rejection. |
| Plague / quarantine | Turn 19-22 | Quarantine certificate required for Baleca-bound cargo-relevant contracts. |
| Pirate warning ends | Turn 21 | Prior pirate warning no longer applies after its end window. |
| Cargo overvaluation threshold strengthens | Turn 21 | Overvaluation rejection threshold changes from above 200 percent to above 150 percent. |

Exact `RuleID`, `NewsID`, route filters, and date filters should come from `04_UnderwritingRules.xlsx` when available.

## First Implementation Rule Set

M1 should use a small representative ship-only subset:

| Rule type | Suggested first fixtures | Source confidence |
| --- | --- | --- |
| AR | Missing ship registration, ship name mismatch, owner mismatch, expired registration, missing hull inspection, failed hull inspection, missing route declaration, departure date mismatch | Confirmed from ship document pages. |
| CR | Hull age 15 years or older, recent accident history, major repair history, hull defect, bad weather forecast, uncharted route | Confirmed from ship document pages. |
| News AR | Banned reef/zone after turn 4 | Confirmed high level, exact route data unresolved. |

Do not implement cargo value-ratio rules in M1 unless final data tables are available.

## Scoring Relationship

Confirmed scoring model:

- Checklist inspection: +2 per checked item.
- Correct action: +5.
- Objective wrong action: -10.
- Subjective deviation: no score penalty.
- Accident consequences affect money/outcome, not hidden probability.

The design intent is that players can be objectively correct and still experience narrative harm. Tests should verify that subjective information never changes objective scoring.

## Implementation Assumptions For M1-M3

Use these assumptions until the inaccessible spreadsheets are available:

| Area | Assumption |
| --- | --- |
| Decision enum | Support approve and reject first. Reserve conditional approval without requiring UI in M1. |
| Premium editing | Use rule-driven premium adjustment only. Do not add freeform premium editing. |
| Rule activation | Rules can have optional `StartTurn`, `EndTurn`, `RouteID`, `BundleID`, and document-field conditions. |
| Rule output | Rule checks should return rule ID, severity, affected document/field, and player-facing explanation key. |
| Debugging | Hidden fixture data can expose `AccidentFlag` and expected decision only in tests/debug tools. |

