# Milestone M2: Content Fixtures And Validation Foundation

Recommended reasoning effort: High

## Goal

Create representative in-memory content fixtures and validation foundations for the ship-only technical slice.

## Required Reading

1. `AGENTS.md`
2. `EpicProjectR/AGENTS.md`
3. `docs/design/04_DOCUMENT_BUNDLES.md`
4. `docs/design/05_UNDERWRITING_RULES.md`
5. `docs/design/06_DATA_TABLES_AND_SCHEMA.md`
6. `docs/project/START_TO_END_IMPLEMENTATION_PLAN.md`
7. `docs/project/IMPLEMENTATION_RISKS_AND_GATES.md`
8. `docs/project/MILESTONE_CHECKLIST.md`

## Task Type

Implementation: pure C# content fixtures, repositories, validators, and tests.

## Allowed Paths

- `EpicProjectR/Assets/Scripts/Domain/`
- `EpicProjectR/Assets/Scripts/Content/`
- `EpicProjectR/Assets/Tests/EditMode/`
- Documentation updates if needed

## Forbidden Paths

- `ref_folder/`
- Scenes, prefabs, ScriptableObject assets, packages, project settings
- Full Excel import
- Full 168 contracts
- Cargo/mixed content beyond extensibility placeholders
- Unity UI

## Implementation Scope

- Ship bundle 1-3 fixture definitions.
- 2-3 deterministic contract fixtures.
- Representative AR/CR rule fixture definitions.
- Document bundle definitions for ship application, registration, hull inspection, and route declaration.
- In-memory repositories over fixture definitions.
- Duplicate ID validation.
- Missing reference validation.
- Contract turn/slot ID validation.
- Document submitted-state validation.
- Tests for validators and fixture lookup.

## Non-goals

- ScriptableObject asset creation unless explicitly approved in a future prompt.
- Final content pipeline.
- Cargo/mixed rows.
- All AR/CR rows.
- UI.

## Architecture Constraints

- Prefer in-memory fixtures first.
- Keep definitions separate from runtime state.
- Use repositories and validators around stable IDs.
- Do not invent final Excel rows.

## Algorithms/Data Structures To Consider

- Algorithms: duplicate detection, reference resolution, turn/slot-to-contract ID validation.
- Data structures: dictionaries keyed by stable IDs, validation result lists, fixture arrays.

## Design Patterns To Consider

- Repository.
- Validator.
- Result Object.

## Validation Requirements

- Tests for duplicate IDs, missing references, invalid turn/slot IDs, and missing document submitted state.
- Verify no ScriptableObject assets were created.
- Verify forbidden paths were untouched.

## Final Report Format

Use the standard milestone report from `docs/project/MILESTONE_CHECKLIST.md`, including algorithms, data structures, design patterns, and boundary confirmation.

## Human Review Triggers

- Need to create ScriptableObject assets.
- Need source spreadsheet access.
- Need to expand beyond 2-3 fixtures.

## Boundary Confirmation

M2 must confirm no scenes, prefabs, ScriptableObject assets, packages, project settings, or `ref_folder/` files were modified.

