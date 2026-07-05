# Milestone M5: Turn And Case Flow

Recommended reasoning effort: High

## Goal

Implement pure/application-level turn and case progression for fixture contracts.

## Required Reading

1. `AGENTS.md`
2. `EpicProjectR/AGENTS.md`
3. `docs/design/02_CORE_LOOP.md`
4. `docs/design/03_TURN_AND_CONTENT_SCOPE.md`
5. `docs/project/START_TO_END_DEPENDENCY_GRAPH.md`
6. `docs/project/FIRST_PLAYABLE_DEFINITION.md`

## Task Type

Implementation: pure C# application/session services and tests.

## Allowed Paths

- `EpicProjectR/Assets/Scripts/Domain/`
- `EpicProjectR/Assets/Scripts/Application/`
- `EpicProjectR/Assets/Tests/EditMode/`
- Documentation updates if needed

## Forbidden Paths

- `ref_folder/`
- Unity UI, scenes, prefabs, ScriptableObject assets, packages, project settings
- Full 168-contract data
- Full economy and after-story systems

## Implementation Scope

- `TurnDefinition` or `TurnSchedule` over fixture contracts.
- Current contract queue or index pointer.
- Session state.
- Submit decision command or method.
- Active contract creation for approved/conditional decisions.
- Rejected contract handling.
- Application service coordination between repositories, rule review, decision audit, and session state.
- Tests for processing multiple contracts in order.

## Non-goals

- Unity UI.
- Full content import.
- Full settlement UI.
- Full after-story UI.
- Save/load.

## Architecture Constraints

- Application services coordinate domain services.
- UI remains passive and absent in this milestone.
- Session state uses stable IDs and primitive/value object state.

## Algorithms/Data Structures To Consider

- Algorithms: queue/index progression, state transition, command handling.
- Data structures: contract queue/list, session state object, active contract list.

## Design Patterns To Consider

- Application Service.
- State Machine.
- Command Object.
- Result Object.

## Validation Requirements

- Tests for fixture contract sequence order.
- Tests for approval creating active contract.
- Tests for rejection not creating active contract.
- Tests for end-of-sequence behavior.
- Forbidden path check.

## Final Report Format

Use standard milestone report and include algorithms, data structures, design patterns, validation, and untouched forbidden paths.

## Human Review Triggers

- Need scene/UI changes.
- Need full 24-turn data.
- Need final conditional approval semantics beyond reserved model support.

## Boundary Confirmation

M5 must confirm no Unity UI, scenes, prefabs, assets, packages, project settings, or `ref_folder/` changes.

