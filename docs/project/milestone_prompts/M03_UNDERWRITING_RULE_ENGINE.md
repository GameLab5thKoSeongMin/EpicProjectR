# Milestone M3: Underwriting Rule Engine

Recommended reasoning effort: High

## Goal

Implement deterministic underwriting rule evaluation over the M2 fixtures.

## Required Reading

1. `AGENTS.md`
2. `EpicProjectR/AGENTS.md`
3. `docs/design/05_UNDERWRITING_RULES.md`
4. `docs/design/06_DATA_TABLES_AND_SCHEMA.md`
5. `docs/project/START_TO_END_DEPENDENCY_GRAPH.md`
6. `docs/project/IMPLEMENTATION_RISKS_AND_GATES.md`

## Task Type

Implementation: pure C# domain/application rule engine and tests.

## Allowed Paths

- `EpicProjectR/Assets/Scripts/Domain/`
- `EpicProjectR/Assets/Scripts/Application/`
- `EpicProjectR/Assets/Tests/EditMode/`
- Documentation updates if needed

## Forbidden Paths

- `ref_folder/`
- UI, scenes, prefabs, ScriptableObject assets, packages, project settings
- Full economy
- Full import
- Cargo/mixed implementation beyond preserved extension seams

## Implementation Scope

- Rule condition model and evaluator.
- Ordered deterministic rule evaluation.
- Absolute rejection evaluation.
- Rejection consideration evaluation.
- `ReviewResult` generation.
- Active rule windows for turn/bundle/route where fixture data supports it.
- Rule output with rule ID, severity, affected document/field, and explanation key.
- Preserve extension path for news-driven and discontinuous active windows.

## Test Cases

- Missing document.
- Name/owner mismatch.
- Expired registration date.
- Failed hull inspection.
- Old hull CR.
- Accident history / repair / defect CR.
- Bad weather CR.
- Route/news-ready condition where feasible.

## Non-goals

- Player submission audit.
- Premium recommendation.
- UI.
- Full news timeline.
- Full 18 AR / 12 CR content.

## Architecture Constraints

- Rules are content definitions; evaluator is pure logic.
- Return explainable result objects.
- Avoid hard-coded per-contract behavior.

## Algorithms/Data Structures To Consider

- Algorithms: ordered rule evaluation, predicate matching, active-window filtering.
- Data structures: ordered lists, dictionaries by document/field ID, result collections.

## Design Patterns To Consider

- Policy.
- Specification-like condition objects.
- Result Object.

## Validation Requirements

- Unit/edit-mode tests for each representative rule.
- Determinism test: same input order and fixture produces same result.
- Forbidden path check.

## Final Report Format

Use standard milestone report and include algorithms, data structures, design patterns, validation, and untouched forbidden paths.

## Human Review Triggers

- Need exact final rule rows.
- Need Excel source files.
- Need UI or asset changes.

## Boundary Confirmation

M3 must confirm no Unity UI, scenes, prefabs, assets, packages, project settings, or `ref_folder/` changes.

