# Milestone M9: Debug And Validation Tools

Recommended reasoning effort: High

## Goal

Improve validation and debug reporting for fixtures, rules, and turn schedules without adding gameplay features.

## Required Reading

1. `AGENTS.md`
2. `EpicProjectR/AGENTS.md`
3. `docs/project/VALIDATION_PLAN.md`
4. `docs/project/IMPLEMENTATION_RISKS_AND_GATES.md`
5. `docs/design/06_DATA_TABLES_AND_SCHEMA.md`

## Task Type

Implementation or mixed. Editor-only tools require explicit approval.

## Allowed Paths

- `EpicProjectR/Assets/Scripts/Domain/`
- `EpicProjectR/Assets/Scripts/Content/`
- `EpicProjectR/Assets/Scripts/Application/`
- `EpicProjectR/Assets/Scripts/Editor/` only if approved
- `EpicProjectR/Assets/Tests/EditMode/`
- Documentation updates if needed

## Forbidden Paths

- `ref_folder/`
- New gameplay features
- Unapproved scenes, prefabs, ScriptableObject assets, packages, project settings

## Implementation Scope

- Rule debug report.
- Contract fixture validation.
- Document field reference validation.
- Turn schedule validation.
- Review result inspector/report.
- Editor-only tooling only if approved.
- Tests for validators.

## Non-goals

- New contract content beyond needed fixtures.
- UI polish.
- Full import.
- Save/load.

## Architecture Constraints

- Debug tools call validators and services.
- Editor-only code must stay editor-only.
- Runtime should not depend on editor tooling.

## Algorithms/Data Structures To Consider

- Algorithms: issue grouping by ID/source, validation traversal, report sorting.
- Data structures: validation issue list, grouped reports, dictionaries by stable ID.

## Design Patterns To Consider

- Validator.
- Report Object.
- Editor Tooling Adapter.

## Validation Requirements

- Tests for validators and report generation.
- Manual editor validation only if editor tooling is approved.
- Forbidden path check.

## Final Report Format

Use standard milestone report and list whether editor-only code was added.

## Human Review Triggers

- Need editor window/menu integration.
- Need asset creation.
- Need source spreadsheets.

## Boundary Confirmation

M9 must confirm no new gameplay, unapproved assets, packages, project settings, or `ref_folder/` changes.

