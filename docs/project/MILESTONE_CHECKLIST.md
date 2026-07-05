# Milestone Checklist

Last updated: 2026-07-05.

Use this checklist before and after every future Codex milestone.

## Before Starting

- Read root `AGENTS.md`.
- Read `EpicProjectR/AGENTS.md` if working inside the Unity project.
- Identify the milestone ID and title.
- Identify whether the task is documentation-only, code-only, or mixed.
- Check whether the task depends on inaccessible source files.
- Check current repository status if possible.

## Required Docs To Read

- `docs/design/10_IMPLEMENTATION_SOURCE_OF_TRUTH.md`
- `docs/design/00_SOURCE_MAP.md`
- `docs/project/REPOSITORY_BOUNDARIES.md`
- `docs/project/CODEX_WORKFLOW.md`
- `docs/project/VALIDATION_PLAN.md`
- `docs/refactor/OPEN_QUESTIONS.md`
- Relevant milestone docs under `docs/refactor/`
- Relevant ADRs under `docs/adr/`

## Scope Confirmation

Every M1+ implementation prompt must explicitly state whether the task may modify:

- Production C# code.
- Unity scenes.
- Prefabs.
- ScriptableObject assets.
- Packages.
- Project settings.
- `ref_folder/`.

If permission is not explicitly stated, assume modification is forbidden.

## Allowed Files And Folders

List the allowed paths before editing. For documentation-only work, this is normally:

- `AGENTS.md`
- `README.md`
- `EpicProjectR/AGENTS.md`
- `docs/design/`
- `docs/project/`
- `docs/refactor/`
- `docs/adr/`

For implementation milestones, allowed paths must be stated by the prompt.

## Forbidden Files And Folders

Unless explicitly approved, do not modify:

- `ref_folder/`
- Unity scenes (`*.unity`)
- Prefabs (`*.prefab`)
- ScriptableObject assets (`*.asset`)
- Unity metadata (`*.meta`)
- `EpicProjectR/Packages/`
- `EpicProjectR/ProjectSettings/`
- Generated Unity files such as `Library/`, `Temp/`, `Logs/`, `UserSettings/`, `.csproj`, or `.sln`

## Implementation Rules

- Start with pure C# domain/application logic when possible.
- Keep runtime state separate from ScriptableObject definitions.
- Avoid importing old reference implementation.
- Keep changes small and sequential.
- Add a short responsibility comment at the top of every new C# file.

## Architecture Rules

- Prefer explicit Composition Root over hidden scene searches.
- Avoid Singleton-heavy architecture.
- Prefer repositories and validators around authorable content.
- Use stable IDs and typed result objects.
- Keep UI passive where possible; route behavior through application/domain services.

## Validation Rules

- Run or attempt the validation named by the milestone.
- If no test command exists, say so and perform file/path inspection.
- Confirm whether production C# changed.
- Confirm whether scenes/prefabs/assets/packages/project settings changed.
- Confirm whether `ref_folder/` changed.
- Report validation skipped or failed with the reason.

## Final Report Format

Report:

1. Files created
2. Files updated
3. Files intentionally not modified
4. Validation performed
5. Validation failed or skipped, with reasons
6. Algorithms used
7. Data structures used
8. Design patterns used
9. Remaining risks and follow-up work
10. Recommended next milestone

Use `Not applicable` for algorithms/data structures/design patterns on documentation-only milestones.

## Human Approval Points

Ask for explicit approval before:

- Modifying scenes, prefabs, ScriptableObject assets, packages, or project settings.
- Importing or copying anything from `ref_folder/`.
- Adding Unity packages.
- Deleting, moving, or renaming files outside the approved scope.
- Resolving a design question that is not answered by `docs/design/`.

