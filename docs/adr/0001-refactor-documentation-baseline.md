# ADR 0001: Refactor Documentation Baseline

Date: 2026-07-05

Status: Accepted

## Context

The project has three overlapping knowledge sources:

- Current Notion design pages under the Epic hub.
- Existing local refactor analysis under `docs/refactor/`.
- Old reference material under `ref_folder/`.

The Notion pages contain the current maritime underwriting design, while the older reference project uses obsolete fantasy/adventurer vocabulary and should not drive implementation directly. Future Codex implementation tasks need a stable local source package that can be read without rediscovering the full Notion graph every time.

## Decision

Create `docs/design/` as the implementation-facing design baseline.

The source hierarchy is:

1. `docs/design/10_IMPLEMENTATION_SOURCE_OF_TRUTH.md`
2. Other `docs/design/` files by topic.
3. Current Notion pages as upstream evidence.
4. `docs/refactor/` as technical analysis.
5. `ref_folder/` as read-only reference.

The baseline also adds:

- `docs/project/CODEX_WORKFLOW.md`
- `docs/project/VALIDATION_PLAN.md`
- `docs/project/REPOSITORY_BOUNDARIES.md`
- `docs/refactor/09_DESIGN_DOCS_INTEGRATION_REVIEW.md`
- Updated `docs/refactor/OPEN_QUESTIONS.md`

## Consequences

Future implementation work can begin from local versioned docs instead of repeating the full Notion inspection.

Conflicts are now resolved in favor of the current 24-turn Notion plan over prototype or old reference sources. In particular:

- The game is a maritime underwriting simulation in Rissebra.
- The player is an underwriter, not an adventurer manager.
- Bundle 4 in the current full-game plan is cargo insurance, even though an older ship-prototype page notes a removed previous bundle 4.
- `AccidentFlag` is deterministic.
- Subjective information affects outcome/narrative, not objective score.

## Alternatives Considered

| Alternative | Reason not chosen |
| --- | --- |
| Use Notion directly for every task | Notion can change outside git history and is slower to re-inspect. |
| Use only existing `docs/refactor/` | Those docs predate this Notion consolidation pass and contain analysis rather than a clean source hierarchy. |
| Use `ref_folder` as production source | It reflects older vocabulary and reference implementation patterns that do not match the current design. |

## Follow-up

Generate an M0 implementation prompt next. It should instruct Codex to read the new source hierarchy first, preserve repository boundaries, and prepare the first domain-model milestone without touching production assets unless explicitly requested.

