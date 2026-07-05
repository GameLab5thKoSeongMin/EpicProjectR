# Milestone M11: Full Content Pipeline Prep

Recommended reasoning effort: High, or XHigh if source spreadsheets become accessible.

## Goal

Prepare the full 24-turn content pipeline and migration path from fixtures to source data.

## Required Reading

1. `AGENTS.md`
2. `EpicProjectR/AGENTS.md`
3. `docs/design/03_TURN_AND_CONTENT_SCOPE.md`
4. `docs/design/06_DATA_TABLES_AND_SCHEMA.md`
5. `docs/project/VALIDATION_PLAN.md`
6. `docs/project/IMPLEMENTATION_RISKS_AND_GATES.md`

## Task Type

Documentation or implementation, data-gated.

## Allowed Paths

- `docs/project/`
- Approved content/importer/validator code paths if implementation is explicitly allowed
- `EpicProjectR/Assets/Tests/EditMode/` if validators are implemented

## Forbidden Paths

- `ref_folder/`
- Final importer without source files and explicit approval
- Scenes, prefabs, ScriptableObject assets, packages, project settings unless explicitly approved
- Manual invention of full 168 rows

## Scope

- Contract table schema alignment.
- Document table schema alignment.
- Rule table schema alignment.
- Import strategy options:
  - Excel.
  - CSV.
  - JSON.
  - ScriptableObject-generated content.
  - Hybrid approach.
- Validation requirements.
- Human data ownership points.
- Migration path from fixtures to real data.

## Non-goals

- Final importer unless source files are accessible and approved.
- Full content authoring.
- Cargo/mixed gameplay implementation.

## Architecture Constraints

- Importers feed definitions; runtime state remains separate.
- Validators must reject duplicate IDs, missing references, invalid bundles, invalid rule windows, and bad accident logic.
- Do not bind implementation to Korean display headers; use code headers where available.

## Algorithms/Data Structures To Consider

- Algorithms: schema validation, row mapping, reference graph validation.
- Data structures: table row DTOs, dictionaries by ID, validation reports.

## Design Patterns To Consider

- Import Adapter.
- Repository.
- Validator.
- Migration Plan.

## Validation Requirements

- If source files exist, run schema/read checks.
- If not, document blocked status and keep design-only.
- Forbidden path check.

## Final Report Format

Use standard milestone report and state whether source data was accessible.

## Human Review Triggers

- Source files become available.
- Need to choose Excel/CSV/JSON/SO pipeline.
- Need ScriptableObject asset generation.

## Boundary Confirmation

M11 must confirm no final importer, assets, packages, project settings, or `ref_folder/` changes occurred unless explicitly approved.

