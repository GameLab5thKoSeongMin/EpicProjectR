# EpicProjectR

Unity refactor project for a maritime insurance underwriting game set in the fictional port city of Rissebra.

The player acts as an underwriter reviewing ship, cargo, and mixed insurance contracts. Current design emphasizes objective document/rule judgment, deterministic accident outcomes, and subjective human consequences.

## Current Status

Milestone M0 establishes repository guardrails, documentation workflow, and baseline validation. Gameplay implementation has not started in this milestone.

Recommended next milestone: M1, pure C# domain model foundation for ship-only underwriting fixtures and deterministic outcomes.

## Repository Layout

| Path | Role |
| --- | --- |
| `EpicProjectR/` | Active Unity production project. |
| `ref_folder/` | Reference-only old project. Do not copy, import, move, rename, delete, or modify files here. |
| `docs/design/` | Local design source of truth. Start here for product/design decisions. |
| `docs/refactor/` | Refactor analysis, milestone proposals, risk notes, and open questions. |
| `docs/project/` | Codex workflow, repository boundaries, validation plans, task templates, and milestone checklists. |
| `docs/adr/` | Architecture and process decision records. |
| `AGENTS.md` | Root Codex guardrails. Read first. |
| `EpicProjectR/AGENTS.md` | Active Unity project-specific Codex guardrails. |

## Source Of Truth

`docs/design/` is the local implementation-facing design source of truth. If it conflicts with the old reference project in `ref_folder/`, the design docs win unless a human explicitly says otherwise.

`docs/refactor/` is useful context and planning, but it is not a direct implementation command.

## How Future Codex Tasks Should Start

1. Read `AGENTS.md`.
2. Read `EpicProjectR/AGENTS.md` if working inside the Unity project.
3. Read `docs/design/10_IMPLEMENTATION_SOURCE_OF_TRUTH.md`.
4. Read `docs/design/00_SOURCE_MAP.md`.
5. Read `docs/project/REPOSITORY_BOUNDARIES.md`.
6. Read `docs/project/CODEX_WORKFLOW.md`.
7. Read `docs/project/MILESTONE_CHECKLIST.md`.
8. Confirm allowed and forbidden paths before editing.

## Guardrail Summary

- Production code lives only under `EpicProjectR/`.
- `ref_folder/` is read-only reference.
- Do not copy old scripts, prefabs, scenes, ScriptableObjects, or assets from `ref_folder/`.
- Do not modify scenes, prefabs, ScriptableObject assets, packages, or project settings unless a milestone explicitly permits it.
- Prefer pure C# domain logic before Unity UI or scene work.

