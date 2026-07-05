# ADR 0002: Codex Guardrails And Repository Boundaries

Date: 2026-07-05

Status: Accepted

## Context

The repository contains an active Unity project, an old reference project, local design docs, refactor analysis, workflow docs, and ADRs. Future Codex milestones need explicit guardrails before implementation begins so they do not accidentally copy old assets or modify Unity production files outside scope.

M0 is documentation, workflow setup, repository boundaries, and baseline validation only.

## Decision

Add root and project-specific guardrail files:

- `AGENTS.md`
- `EpicProjectR/AGENTS.md`

Add reusable project workflow docs:

- `docs/project/MILESTONE_CHECKLIST.md`
- `docs/project/CODEX_TASK_TEMPLATE.md`
- `docs/project/BASELINE_VALIDATION.md`
- `docs/project/M0_COMPLETION_REPORT.md`

Future milestones must declare allowed and forbidden paths before editing. If permission is not explicit, modification is forbidden.

## Consequences

- Codex tasks have a single root instruction file to read first.
- Unity-specific restrictions are visible near the active project.
- M1 can start from domain model work without opening scenes or importing reference assets.
- Future reports must state algorithms, data structures, design patterns, validation, files changed, risks, and follow-up work.

## Why Root `AGENTS.md` Is Needed

The repository-level file defines source hierarchy, hard restrictions, reporting expectations, and uncertainty handling before any task decides what to edit.

## Why `EpicProjectR/AGENTS.md` Is Needed

The active Unity project has special constraints around scenes, prefabs, ScriptableObjects, packages, project settings, tests, namespaces, and runtime state. Keeping a shorter project-specific file near the Unity project reduces the chance of treating Unity assets like ordinary text files.

## Why `ref_folder/` Is Read-only

`ref_folder/` is an old reference project with obsolete vocabulary and implementation patterns. It can inform analysis, but copying or importing it would risk dragging old assumptions, hidden dependencies, and unwanted Unity assets into production.

## Why Design Docs Win Over The Reference Prototype

`docs/design/` summarizes the current Notion design pass. It confirms the maritime underwriting direction, 24-turn scope, deterministic accidents, current document bundle meanings, and obsolete old fantasy/adventurer terminology. When it conflicts with `ref_folder/`, the current design docs are the safer source.

## Why M1 Starts With Domain Model

The underwriting loop depends on stable concepts: contract IDs, document bundles, AR/CR rules, review results, player decisions, deterministic outcomes, and scoring. Building these as pure C# first reduces scene/prefab churn and makes validation possible before UI work.

## Why Future Milestones Must Declare Paths

Unity projects contain many asset and generated file types. A milestone that does not explicitly allow scenes, prefabs, ScriptableObjects, packages, project settings, or `ref_folder/` should not touch them. Declaring paths up front makes review and rollback tractable.

## Alternatives Considered

| Alternative | Reason not chosen |
| --- | --- |
| Rely only on `docs/project/REPOSITORY_BOUNDARIES.md` | Future Codex tasks conventionally look for `AGENTS.md`; the rule needs to be discoverable immediately. |
| Put all rules only in root `AGENTS.md` | Unity-specific details are easier to miss without a project-local file. |
| Start M1 with UI or scene work | UI depends on domain concepts that are not yet implemented or tested. |
| Permit copying from `ref_folder/` with review | The reference project conflicts with current design vocabulary and can introduce hidden Unity dependencies. |

