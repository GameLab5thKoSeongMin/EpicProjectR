# Codex Task Template

Use this template for future milestones. Keep tasks small and sequential.

```markdown
# Milestone [ID]: [Title]

Recommended reasoning effort: [Low/Medium/High]

## Goal

[One concrete outcome.]

## Required Reading

1. `AGENTS.md`
2. `EpicProjectR/AGENTS.md` if working inside the Unity project
3. `docs/design/10_IMPLEMENTATION_SOURCE_OF_TRUTH.md`
4. `docs/design/00_SOURCE_MAP.md`
5. `docs/project/REPOSITORY_BOUNDARIES.md`
6. `docs/project/CODEX_WORKFLOW.md`
7. `docs/project/VALIDATION_PLAN.md`
8. `docs/project/MILESTONE_CHECKLIST.md`
9. [Milestone-specific docs]

## Task Type

[Documentation-only / code-only / mixed]

## Allowed Files/Folders

- [Explicit paths]

## Forbidden Files/Folders

- `ref_folder/`
- Unity scenes unless explicitly allowed
- Prefabs unless explicitly allowed
- ScriptableObject assets unless explicitly allowed
- Packages unless explicitly allowed
- Project settings unless explicitly allowed
- [Other paths]

## Implementation Scope

- [Small sequential item 1]
- [Small sequential item 2]

## Non-goals

- [What must not be done]

## Architecture Constraints

- Prefer pure C# domain/application logic where possible.
- Keep runtime state out of ScriptableObject assets.
- Avoid Singleton-heavy architecture.
- Use explicit Composition Root when wiring is needed.
- Do not copy implementation from `ref_folder/`.

## Algorithms/Data Structures To Consider

- Algorithms: [e.g. deterministic rule evaluation, ID validation, state transition]
- Data structures: [e.g. dictionaries by stable ID, immutable result objects, queues]

## Design Patterns To Consider

- [e.g. value object, result object, policy, repository, validator, presenter]

## Validation Commands/Checks

- [Command or manual check]
- Verify no forbidden paths changed.
- Verify `ref_folder/` unchanged.

## Final Report Format

1. Files created
2. Files updated
3. Files intentionally not modified
4. Validation performed
5. Validation failed or skipped, with reasons
6. Algorithms used
7. Data structures used
8. Design patterns used
9. Risks and follow-up work
10. Recommended next milestone

## Human Review Triggers

- Design source conflict.
- Need to modify scenes/prefabs/assets/packages/project settings.
- Need to copy/import from `ref_folder/`.
- Source spreadsheet or Notion data is inaccessible but required.
```

