# AGENTS.md

Project: EpicProjectR, a Unity refactor project for a maritime insurance underwriting game set in Rissebra.

This file is the first instruction file future Codex tasks must read.

## Project Identity

| Area | Path | Role |
| --- | --- | --- |
| Repository root | `.` | Guardrails, docs, ADRs, and active/reference project folders. |
| Active Unity project | `EpicProjectR/` | Production Unity project. Production code lives here only. |
| Reference-only project | `ref_folder/` | Old project/reference material only. Never production. |
| Local design source | `docs/design/` | Current local design source of truth. |
| Refactor analysis | `docs/refactor/` | Analysis and recommendations, not direct implementation commands. |
| Workflow docs | `docs/project/` | Task process, validation, repository boundaries. |
| ADRs | `docs/adr/` | Decision history. |

## Required Reading Order

Before any milestone, read:

1. `AGENTS.md`
2. `EpicProjectR/AGENTS.md` if working inside the Unity project
3. `docs/design/10_IMPLEMENTATION_SOURCE_OF_TRUTH.md`
4. `docs/design/00_SOURCE_MAP.md`
5. `docs/project/REPOSITORY_BOUNDARIES.md`
6. `docs/project/CODEX_WORKFLOW.md`
7. `docs/project/VALIDATION_PLAN.md`
8. Relevant milestone docs under `docs/refactor/`
9. `docs/refactor/OPEN_QUESTIONS.md`
10. Relevant ADRs under `docs/adr/`

## Source Hierarchy

1. `docs/design/` is the local design source of truth.
2. Current Notion pages are upstream evidence when rechecked.
3. `docs/refactor/` is technical analysis and planning context.
4. `docs/project/` defines workflow and validation rules.
5. `docs/adr/` records durable decisions.
6. `ref_folder/` is read-only reference.

If design docs and the old reference project conflict, design docs win unless the human explicitly says otherwise.

## Hard Restrictions

Do not:

- Copy old scripts, prefabs, scenes, ScriptableObjects, or assets from `ref_folder/`.
- Move, rename, delete, import, or modify files in `ref_folder/`.
- Treat `ref_folder/` as production.
- Modify production C# code unless the milestone explicitly permits it.
- Modify Unity scenes, prefabs, ScriptableObject assets, packages, project settings, or generated Unity files unless the milestone explicitly permits it.
- Add Unity packages without explicit approval.
- Run destructive cleanup commands.

If permission is not explicitly stated, assume the path is forbidden.

## Coding Principles For Future Implementation

- Prefer pure C# domain logic for rules, scoring, outcomes, economy, and turn flow.
- Keep domain code independent from `MonoBehaviour`, scenes, prefabs, and Unity UI.
- Prefer ScriptableObjects for authorable content definitions, not runtime mutable state.
- Prefer explicit Composition Root wiring over hidden scene searches and global contexts.
- Avoid Singleton-heavy architecture.
- Use stable IDs and typed result objects for contracts, rules, routes, documents, and outcomes.
- Every C# file added in future implementation milestones must include a top comment explaining its responsibility.

## Documentation Principles

- Keep milestone docs concise, implementation-facing, and explicit.
- Mark facts as confirmed, inferred, assumed, unresolved, or obsolete when needed.
- Update `docs/refactor/OPEN_QUESTIONS.md` when a task reveals new uncertainty.
- Record durable architecture or process decisions in `docs/adr/`.
- Do not invent exact content rows, dialogue, economy values, or full rule tables while source spreadsheets are inaccessible.

## Unity-Specific Restrictions

- Production work belongs under `EpicProjectR/`.
- Future production code should generally live under `EpicProjectR/Assets/Scripts/`.
- Future tests should generally live under `EpicProjectR/Assets/Tests/EditMode/`.
- Scene and prefab changes should be separate from pure domain milestones.
- ScriptableObject assets may define content; runtime state should stay in plain C# state/snapshot objects.
- Package and project setting changes require explicit milestone permission.

## Refactor Rules

- Start with small, sequential milestones.
- M1 should begin with pure C# domain model foundations, not UI or scene work.
- Do not import old architecture wholesale from `ref_folder/`.
- Preserve unrelated user changes in the worktree.
- If old reference code is useful, read it for context and rebuild the needed behavior in the active project.

## Validation Expectations

Every future implementation milestone must report:

- Algorithms used.
- Data structures used.
- Design patterns used.
- Validation performed.
- Validation skipped and why.
- Files changed.
- Risks and follow-up work.

At minimum, check whether the task modified:

- Production C# code.
- Unity scenes.
- Prefabs.
- ScriptableObject assets.
- Packages.
- Project settings.
- `ref_folder/`.

## Final Report Format

Use this structure unless the user asks otherwise:

1. Files created
2. Files updated
3. Files intentionally not modified
4. Validation performed
5. Validation failed or skipped, with reasons
6. Algorithms/data structures/design patterns used, when implementation occurred
7. Repository boundaries confirmed
8. Remaining risks and follow-up work
9. Recommended next milestone

## When Uncertain

- Stop before touching forbidden paths.
- Prefer documenting an assumption over silently making one.
- Ask the human when a decision would change production assets, source data, or architecture scope.
- If a command fails due to environment, configuration, or permission issues, record the failure and use a safer fallback.

