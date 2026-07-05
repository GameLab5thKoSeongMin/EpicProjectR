# Repository Boundaries

Last updated: 2026-07-05.

## Active Areas

| Area | Role | Default write policy |
| --- | --- | --- |
| `EpicProjectR/` | Active Unity production project | Modify only in implementation milestones that explicitly allow the relevant Unity paths. |
| `EpicProjectR/Assets/` | Active Unity project files | Modify only in implementation milestones that explicitly allow code/assets. |
| `EpicProjectR/ProjectSettings/` | Unity project settings | Modify only when a milestone requires Unity configuration changes. |
| `EpicProjectR/Packages/` | Unity package manifest/lock | Modify only when dependencies are explicitly required. |
| `docs/design/` | Local design source package | Safe to update for design/source-of-truth work. |
| `docs/project/` | Workflow, validation, and repository rules | Safe to update for workflow/boundary work. |
| `docs/refactor/` | Prior analysis and technical recommendation | Safe to update for planning reviews and open questions. |
| `docs/adr/` | Decision history | Safe to update when durable decisions are made. |

## Reference Area

`ref_folder/` is reference-only.

Do not:

- Move files from `ref_folder`.
- Rename files in `ref_folder`.
- Delete files in `ref_folder`.
- Copy old scripts/assets into the active project.
- Import old prefabs/scenes/ScriptableObjects into production.

Use it only to inspect old behavior, structure, or patterns when explicitly useful.

## Documentation-Only Task Boundary

For documentation-only tasks, allowed changes are limited to:

- `docs/design/`
- `docs/project/`
- `docs/refactor/`
- `docs/adr/`

Do not modify:

- Production C# code.
- Unity scenes.
- Prefabs.
- ScriptableObject assets.
- Package manifests.
- Project settings.
- `ref_folder/`.

## Implementation Task Boundary

For future implementation milestones:

- Implementation should occur only inside the active Unity project unless explicitly approved.
- Prefer new domain code over importing old reference code.
- Keep scene/prefab changes separate from domain model changes when possible.
- Add tests around rule, scoring, and accident logic before broad UI work.
- Keep source data importers behind validation so bad content fails visibly.
- Preserve unrelated user changes in the worktree.
