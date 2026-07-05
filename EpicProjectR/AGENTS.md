# EpicProjectR Unity Project Rules

This file applies inside the active Unity project at `EpicProjectR/`.

Read root `AGENTS.md` before this file.

## Active Project Path

`EpicProjectR/` is the active production Unity project. `../ref_folder/` is outside production scope and is reference-only.

## Future Implementation Folders

Recommended future folders, when an implementation milestone explicitly permits code or tests:

- `EpicProjectR/Assets/Scripts/Domain/`
- `EpicProjectR/Assets/Scripts/Application/`
- `EpicProjectR/Assets/Scripts/Content/`
- `EpicProjectR/Assets/Scripts/Presentation/`
- `EpicProjectR/Assets/Scripts/Infrastructure/`
- `EpicProjectR/Assets/Scripts/Editor/`
- `EpicProjectR/Assets/Tests/EditMode/`

Do not create these folders during documentation-only milestones unless the milestone explicitly asks for placeholder folders.

## Unity Restrictions

- Do not modify scenes, prefabs, ScriptableObject assets, packages, or project settings unless the milestone explicitly permits it.
- Do not store runtime mutable state in ScriptableObject assets.
- Keep pure domain logic free of `MonoBehaviour` dependencies.
- Prefer explicit Composition Root wiring over scene searches and global contexts.
- Avoid Singleton-heavy architecture.
- Every new C# file must start with a short responsibility comment.

## Tests

Prefer Unity Test Framework edit-mode tests under `EpicProjectR/Assets/Tests/EditMode/` for pure domain and application behavior.

Recommended namespace for future code: `EpicProjectR`, unless a milestone or human decision chooses another namespace.

## Validation Command Placeholders

Use the exact commands only after the relevant test infrastructure exists:

```powershell
# Placeholder: run Unity edit-mode tests through the Unity Test Runner or a configured CI command.
# Placeholder: run content validators once editor validation tooling exists.
```

If no validation command exists yet, report that clearly and use file/path inspection.

