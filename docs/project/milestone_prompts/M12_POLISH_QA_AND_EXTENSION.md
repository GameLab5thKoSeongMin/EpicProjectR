# Milestone M12: Polish QA And Extension

Recommended reasoning effort: High

## Goal

Plan post-first-playable polish and expansion as smaller future tasks. Do not implement all extensions at once.

## Required Reading

1. `AGENTS.md`
2. `EpicProjectR/AGENTS.md`
3. `docs/project/FIRST_PLAYABLE_DEFINITION.md`
4. `docs/project/IMPLEMENTATION_ROADMAP.md`
5. `docs/project/IMPLEMENTATION_RISKS_AND_GATES.md`
6. `docs/design/07_UI_UX_FLOW.md`
7. `docs/design/08_OUTCOMES_ECONOMY_AND_AFTERSTORY.md`

## Task Type

Documentation planning by default.

## Allowed Paths

- `docs/project/`
- `docs/refactor/OPEN_QUESTIONS.md` if new questions are found

## Forbidden Paths

- `ref_folder/`
- Production code unless a smaller implementation prompt is created and approved
- Scenes, prefabs, ScriptableObject assets, packages, project settings
- Giant combined feature implementation

## Extension Areas

- UI polish.
- Contract map.
- News presentation.
- After-story channels.
- Special contractor expansion.
- Cargo insurance expansion.
- Mixed contract expansion.
- Claim sheet once fields exist.
- Economy polish.
- QA checklist.
- Regression test plan.

## Required Output

- Break extension work into smaller future tasks.
- Identify blockers and human review gates for each task.
- Identify validation needs for each task.
- Avoid combining unrelated extensions.

## Non-goals

- New gameplay implementation.
- Asset import.
- Full content import.
- Scene/prefab changes.

## Architecture Constraints

- Preserve domain/application boundaries.
- Keep UI passive.
- Keep content data-driven.
- Do not use `ref_folder/` as source.

## Algorithms/Data Structures To Consider

- Algorithms: regression test selection, QA matrix planning.
- Data structures: task list, risk matrix, coverage checklist.

## Design Patterns To Consider

- Planning decomposition.
- Risk gate.
- Regression checklist.

## Validation Requirements

- Documentation review.
- Confirm no production files changed.
- Confirm forbidden paths untouched.

## Final Report Format

Use standard milestone report. Algorithms/data structures/design patterns may be planning-level.

## Human Review Triggers

- Any request to implement an extension immediately.
- Any scene/prefab/asset/package/project setting change.
- Any source data dependency.

## Boundary Confirmation

M12 must confirm no production code, Unity assets, packages, project settings, or `ref_folder/` changes unless a separate approved implementation task exists.

