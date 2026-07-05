# Milestone M1: Domain Model Foundation

Recommended reasoning effort: High

## Goal

Implement pure C# domain model foundations for the underwriting refactor. Do not implement rule evaluation yet.

## Required Reading

1. `AGENTS.md`
2. `EpicProjectR/AGENTS.md`
3. `docs/design/10_IMPLEMENTATION_SOURCE_OF_TRUTH.md`
4. `docs/design/02_CORE_LOOP.md`
5. `docs/design/03_TURN_AND_CONTENT_SCOPE.md`
6. `docs/design/04_DOCUMENT_BUNDLES.md`
7. `docs/design/06_DATA_TABLES_AND_SCHEMA.md`
8. `docs/project/START_TO_END_IMPLEMENTATION_PLAN.md`
9. `docs/project/MILESTONE_CHECKLIST.md`

## Task Type

Implementation: pure C# domain and tests only.

## Allowed Paths

- `EpicProjectR/Assets/Scripts/Domain/`
- `EpicProjectR/Assets/Tests/EditMode/`
- `docs/project/` only for small report updates if needed
- `docs/refactor/OPEN_QUESTIONS.md` only if new questions are discovered

## Forbidden Paths

- `ref_folder/`
- Unity scenes (`*.unity`)
- Prefabs (`*.prefab`)
- ScriptableObject assets and Unity assets (`*.asset`)
- Unity metadata edits except new `.meta` files Unity may generate for approved new folders/files
- `EpicProjectR/Packages/`
- `EpicProjectR/ProjectSettings/`
- UI, content importers, runtime services, cargo/mixed implementation, full economy, full save/load

## Implementation Scope

- Stable ID wrappers for `ContractId`, `RuleId`, `DocumentId`, `BundleId`, `RouteId`, `NewsId`, `SpecialContractorId`, and `SubjectiveInfoId` where appropriate.
- `GameDate` and `TurnNumber`.
- Contract type and decision enums: approve/reject first, conditional reserved.
- Contract case model shape.
- Document bundle and document record model shape.
- Document field representation with stable field IDs.
- Rule category/severity model shape.
- `ReviewResult` shape only; no rule evaluator.
- `ReviewSubmission` shape.
- Accident flag/outcome identity concepts.
- Minimal `ActiveContractState`.
- Minimal settlement/economy value object placeholders only if needed for later boundaries.
- Immutable or readonly result-oriented APIs where practical.
- Top responsibility comment on every new C# file.

## Non-goals

- Actual rule evaluation.
- Fixture repositories.
- Excel import.
- Unity UI.
- Scene/prefab work.
- ScriptableObject asset creation.
- Cargo/mixed implementation.
- Full economy formulas.
- Save/load implementation.

## Architecture Constraints

- Domain code must not depend on `MonoBehaviour`.
- Keep runtime state in plain C# types.
- Use stable IDs, value objects, and result objects.
- Avoid Singleton-heavy architecture.
- Do not copy from `ref_folder/`.

## Algorithms/Data Structures To Consider

- Algorithms: ID parsing/validation, range validation, equality checks.
- Data structures: readonly structs/classes, immutable lists/arrays where practical, dictionaries only in tests if useful.

## Design Patterns To Consider

- Value Object.
- Result Object.
- Enumeration with explicit reserved values.

## Validation Requirements

- Run or attempt Unity edit-mode tests if configured.
- Add basic tests for ID validation, equality, decision enum, deterministic accident flag assumptions, and no Unity dependency in domain code.
- Verify forbidden paths were untouched.

## Final Report Format

1. Files created
2. Files updated
3. Files intentionally not modified
4. Validation performed
5. Validation failed/skipped and why
6. Algorithms used
7. Data structures used
8. Design patterns used
9. Risks and follow-up work
10. Recommended next milestone
11. Confirm forbidden paths were untouched

## Human Review Triggers

- Need to touch scenes, prefabs, assets, packages, project settings, or `ref_folder/`.
- Need to decide exact Excel schema.
- Need to implement rule evaluation before M3.

## Boundary Confirmation

M1 must end by confirming no Unity scenes, prefabs, ScriptableObject assets, packages, project settings, or `ref_folder/` files were modified.

