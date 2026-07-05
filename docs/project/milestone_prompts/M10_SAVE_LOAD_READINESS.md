# Milestone M10: Save/Load Readiness

Recommended reasoning effort: High

## Goal

Prepare save/load readiness through snapshot design or implementation, depending on runtime state stability.

## Required Reading

1. `AGENTS.md`
2. `EpicProjectR/AGENTS.md`
3. `docs/project/START_TO_END_DEPENDENCY_GRAPH.md`
4. `docs/project/IMPLEMENTATION_RISKS_AND_GATES.md`
5. Current runtime state code from M5-M8

## Task Type

Implementation or documentation-only. Decide based on existing runtime state.

## Allowed Paths

- `EpicProjectR/Assets/Scripts/Application/`
- `EpicProjectR/Assets/Scripts/Infrastructure/`
- `EpicProjectR/Assets/Tests/EditMode/`
- `docs/project/` if creating design-only output

## Forbidden Paths

- `ref_folder/`
- Full save UI
- Cloud save
- Multiple-slot UI
- Encryption
- Asset reference serialization as source of truth
- Unapproved scenes, prefabs, assets, packages, project settings

## Implementation Scope If Appropriate

- Snapshot DTOs.
- Stable ID serialization.
- Runtime primitive/value state serialization.
- Rehydration through repositories.
- Deterministic continuation tests.

If save/load is too early, create a save/load design doc and defer implementation.

## Non-goals

- UI.
- Cloud platform integration.
- Full persistence settings.
- Content import.

## Architecture Constraints

- Save stable IDs and primitive state only.
- Rehydrate definitions through repositories.
- Do not serialize Unity asset references as source of truth.

## Algorithms/Data Structures To Consider

- Algorithms: snapshot creation, rehydration, deterministic continuation.
- Data structures: snapshot DTOs, ID lists, primitive state records.

## Design Patterns To Consider

- Snapshot.
- Repository.
- Serializer Adapter.

## Validation Requirements

- If implemented, tests for save mid-session, load, and continue with same results.
- If deferred, validation is documentation review.
- Forbidden path check.

## Final Report Format

Use standard milestone report and state whether implementation or design-only path was chosen.

## Human Review Triggers

- Runtime state is unstable.
- Need external serialization package.
- Need save UI.

## Boundary Confirmation

M10 must confirm no unapproved UI/assets/packages/project settings or `ref_folder/` changes.

