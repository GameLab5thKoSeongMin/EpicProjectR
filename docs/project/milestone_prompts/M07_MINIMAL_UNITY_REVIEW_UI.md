# Milestone M7: Minimal Unity Review UI

Recommended reasoning effort: High. Use XHigh only if scene/prefab work is explicitly approved.

## Goal

Create the first minimal Unity-facing review UI layer, or presenter/view-model code only if scene/prefab changes are not approved.

## Required Reading

1. `AGENTS.md`
2. `EpicProjectR/AGENTS.md`
3. `docs/design/07_UI_UX_FLOW.md`
4. `docs/project/FIRST_PLAYABLE_DEFINITION.md`
5. `docs/project/IMPLEMENTATION_RISKS_AND_GATES.md`
6. `docs/project/MILESTONE_CHECKLIST.md`

## Task Type

Mixed. Code is allowed only in approved production paths. Scene/prefab/asset changes require explicit human approval in the prompt.

## Allowed Paths

- `EpicProjectR/Assets/Scripts/Presentation/`
- `EpicProjectR/Assets/Scripts/Application/` only for UI-facing adapters if needed
- `EpicProjectR/Assets/Tests/EditMode/`
- Scene/prefab paths only if explicitly approved
- Documentation updates if needed

## Forbidden Paths

- `ref_folder/`
- Scene/prefab/ScriptableObject asset/package/project setting changes unless explicitly approved
- Imported reference UI scripts, scenes, prefabs, or assets
- New gameplay logic in views

## Implementation Scope If Approved

- Minimal Composition Root or bootstrap.
- Passive views.
- Presenter/ViewModel layer.
- Contract list display.
- Document display.
- AR/CR checklist display.
- Approve/reject buttons.
- Minimal feedback/debug summary display.
- Manual smoke test instructions.

If scene/prefab modification is not approved, create only presenter/view-model code and documentation for scene wiring.

## Non-goals

- Reference prefab import.
- Full visual polish.
- Cargo/mixed UI.
- Contract map animation.
- Claim sheet UI.
- Full settlement UI.

## Architecture Constraints

- Views are passive.
- Presenters call application services.
- Gameplay logic stays in domain/application services.
- Prefer explicit Composition Root.
- Avoid Singleton-heavy architecture and hidden scene searches.

## Algorithms/Data Structures To Consider

- Algorithms: view-model snapshot mapping, UI command forwarding.
- Data structures: view models, presenter state, immutable display snapshots.

## Design Patterns To Consider

- Presenter.
- ViewModel.
- Composition Root.

## Validation Requirements

- Presenter tests if feasible.
- Manual smoke test instructions.
- Verify no unapproved scene/prefab/asset changes.
- Verify `ref_folder/` untouched.

## Final Report Format

Use standard milestone report and explicitly list whether scenes/prefabs/assets were modified.

## Human Review Triggers

- Any scene/prefab/asset/package/project setting edit.
- Any request to reuse reference UI.
- Any need to add packages.

## Boundary Confirmation

M7 must confirm whether Unity assets were changed. If not explicitly approved, it must confirm no scenes, prefabs, ScriptableObject assets, packages, project settings, or `ref_folder/` files were modified.

