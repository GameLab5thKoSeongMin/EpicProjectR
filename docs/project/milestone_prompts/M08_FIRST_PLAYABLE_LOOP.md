# Milestone M8: First Playable Loop

Recommended reasoning effort: High

## Goal

Connect the small ship-only first playable loop over fixture data.

## Required Reading

1. `AGENTS.md`
2. `EpicProjectR/AGENTS.md`
3. `docs/project/FIRST_PLAYABLE_DEFINITION.md`
4. `docs/project/START_TO_END_DEPENDENCY_GRAPH.md`
5. `docs/design/02_CORE_LOOP.md`
6. `docs/design/07_UI_UX_FLOW.md`

## Task Type

Mixed. Scene/prefab changes require explicit permission. If not approved, stop before Unity asset edits and wire only code-level flow.

## Allowed Paths

- Approved domain/application/presentation/test paths
- Approved scene/prefab paths only if explicitly stated
- Documentation updates if needed

## Forbidden Paths

- `ref_folder/`
- Full 168-contract data
- Cargo/mixed implementation
- Full special contractor/after-story branches
- Full save/load UI
- Unapproved scenes, prefabs, assets, packages, project settings

## Implementation Scope

- Start small turn/session.
- Show current case.
- Review bundle 1-3 documents.
- Check AR/CR reasons.
- Submit approve/reject decision.
- Audit decision.
- Create active contract if approved.
- Resolve deterministic outcome when due.
- Show minimal outcome/settlement feedback.
- Advance to next case or turn.
- Manual QA checklist.

## Non-goals

- Full 24-turn game.
- Cargo/mixed.
- Final art polish.
- Claim sheet detail.
- Full economy.
- Save/load UI.

## Architecture Constraints

- Keep first playable small.
- UI remains passive.
- Domain/application services remain testable without Unity scene.
- Do not expose hidden `AccidentFlag` in normal player UI.

## Algorithms/Data Structures To Consider

- Algorithms: session progression, state transition, deterministic outcome resolution.
- Data structures: turn session state, active contracts, view models, result line items.

## Design Patterns To Consider

- Application Service.
- State Machine.
- Presenter/ViewModel.
- Result Object.

## Validation Requirements

- Run existing tests.
- Manual QA using `docs/project/FIRST_PLAYABLE_DEFINITION.md`.
- Verify forbidden paths untouched.
- Verify any scene/prefab changes were explicitly approved and listed.

## Final Report Format

Use standard milestone report and include manual QA results.

## Human Review Triggers

- Any unapproved Unity asset change.
- Need to expand beyond 2-3 fixtures.
- Need conditional approval UI.

## Boundary Confirmation

M8 must confirm no unapproved production C# code, scenes, prefabs, ScriptableObject assets, packages, project settings, or `ref_folder/` files were modified.

