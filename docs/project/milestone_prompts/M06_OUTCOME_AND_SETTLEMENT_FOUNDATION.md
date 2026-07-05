# Milestone M6: Outcome And Settlement Foundation

Recommended reasoning effort: High

## Goal

Implement deterministic outcome resolution and minimal traceable settlement/evaluation result structures.

## Required Reading

1. `AGENTS.md`
2. `EpicProjectR/AGENTS.md`
3. `docs/design/02_CORE_LOOP.md`
4. `docs/design/05_UNDERWRITING_RULES.md`
5. `docs/design/08_OUTCOMES_ECONOMY_AND_AFTERSTORY.md`
6. `docs/project/FIRST_PLAYABLE_DEFINITION.md`
7. `docs/project/IMPLEMENTATION_RISKS_AND_GATES.md`

## Task Type

Implementation: pure C# outcome/settlement foundations and tests.

## Allowed Paths

- `EpicProjectR/Assets/Scripts/Domain/`
- `EpicProjectR/Assets/Scripts/Application/`
- `EpicProjectR/Assets/Tests/EditMode/`
- Documentation updates if needed

## Forbidden Paths

- `ref_folder/`
- Unity UI, scenes, prefabs, ScriptableObject assets, packages, project settings
- Final economy formulas from inaccessible data
- Claim detail UI
- After-story channels

## Implementation Scope

- `AccidentFlag`-driven outcome resolver.
- Approved or conditional flagged contract creates deterministic accident.
- Rejected flagged contract does not create regular accident.
- Return/due date handling if fixture data supports it.
- `OutcomeResult`.
- Minimal `SettlementResult` / `EvaluationResult` line-item structure.
- Economy parameter placeholders isolated from formulas requiring `05_Economy.xlsx`.
- Tests for deterministic accident, rejected flagged no accident, and traceable settlement line items.

## Non-goals

- Full economy balancing.
- Final salary/commission simulation.
- Claim sheet UI.
- Letters, board posts, gifts, inspections.
- Save/load.

## Architecture Constraints

- Outcome services consume active contract state.
- Settlement results should be line-item based and traceable.
- Economy formulas must remain replaceable.

## Algorithms/Data Structures To Consider

- Algorithms: date/due outcome selection, deterministic flag resolution, line-item aggregation.
- Data structures: active contract list, outcome result, settlement line item list.

## Design Patterns To Consider

- Policy.
- Result Object.
- State Transition.

## Validation Requirements

- Tests for approved flagged accident.
- Tests for rejected flagged no accident.
- Tests for unflagged no accident.
- Tests for traceable line items.
- Forbidden path check.

## Final Report Format

Use standard milestone report and include algorithms, data structures, design patterns, validation, and untouched forbidden paths.

## Human Review Triggers

- Need exact economy rows.
- Need claim detail fields.
- Need UI/asset changes.

## Boundary Confirmation

M6 must confirm no UI, scenes, prefabs, ScriptableObject assets, packages, project settings, or `ref_folder/` changes.

