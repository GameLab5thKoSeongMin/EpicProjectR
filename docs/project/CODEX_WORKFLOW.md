# Codex Workflow

Last updated: 2026-07-05.

## Required Reading Order

Before any future implementation or refactor task, Codex should read:

1. `AGENTS.md`
2. `EpicProjectR/AGENTS.md` if working inside the Unity project
3. `docs/design/10_IMPLEMENTATION_SOURCE_OF_TRUTH.md`
4. `docs/design/00_SOURCE_MAP.md`
5. `docs/project/REPOSITORY_BOUNDARIES.md`
6. `docs/project/VALIDATION_PLAN.md`
7. `docs/project/MILESTONE_CHECKLIST.md`
8. The design file relevant to the requested feature
9. `docs/refactor/OPEN_QUESTIONS.md`
10. Relevant previous milestone notes under `docs/refactor/`

## Default Working Rules

- Treat `docs/design/` as the local implementation-facing design package.
- Treat Notion as upstream evidence, not as a substitute for local versioned docs.
- Treat `docs/refactor/` as analysis and planning context.
- Treat `ref_folder/` as read-only reference material.
- Identify whether each task is documentation-only, code-only, or mixed.
- Explicitly list allowed and forbidden paths before editing.
- Keep changes scoped to the requested milestone.
- Update documentation whenever a source-of-truth decision changes.
- Mark assumptions explicitly when source spreadsheets are unavailable.

## When Notion Must Be Rechecked

Recheck Notion when:

- A user asks for the latest design.
- A local doc says a source is unresolved.
- A conflict appears between `docs/design/` and a current Notion page.
- Exact content rows, dialogue, economy values, or rule IDs are needed.

For routine implementation of already-documented behavior, read local docs first.

## Before Editing Code

Codex should confirm:

- Which milestone is being implemented.
- Which production folders are in scope.
- Whether Unity scenes, prefabs, ScriptableObjects, or data assets are allowed to change.
- Whether changes depend on inaccessible spreadsheets.

If the task is documentation-only, do not modify production C# code, Unity scenes, prefabs, ScriptableObject assets, or `ref_folder`.

## Reporting Format For Future Milestones

At the end of a milestone, report:

- Files created.
- Files updated.
- Production C# code touched, if any.
- Unity scenes touched, if any.
- Prefabs touched, if any.
- ScriptableObject assets touched, if any.
- Packages touched, if any.
- Project settings touched, if any.
- `ref_folder/` touched, if any.
- Algorithms used, when implementation occurred.
- Data structures used, when implementation occurred.
- Design patterns used, when implementation occurred.
- Validation performed.
- Validation skipped and why.
- Assumptions added.
- Open questions changed.
- Recommended next milestone.

## Handling Uncertainty

Use one of these labels in docs and task summaries:

| Label | Meaning |
| --- | --- |
| Confirmed | Directly supported by inspected Notion or local docs. |
| Inferred | Reasonable implementation conclusion from confirmed sources. |
| Assumed | Temporary default until a human or source file confirms. |
| Unresolved | Cannot be safely decided with available sources. |
| Obsolete | Old source exists but should not drive current implementation. |
