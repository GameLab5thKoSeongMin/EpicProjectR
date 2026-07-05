# Implementation Source Of Truth

Last inspected: 2026-07-05.

This is the first file future Codex implementation tasks should read.

## Source Hierarchy

1. Read this file first.
2. Read the relevant `docs/design/` files for the feature area.
3. Use Notion pages as upstream evidence when clarification is required.
4. Use `docs/refactor/` as technical analysis and milestone background.
5. Use `ref_folder/` only as reference material. Do not copy, move, rename, delete, or import from it without a separate explicit request.

## Current Product Direction

The game is a maritime insurance underwriting simulation set in the fictional port city of Rissebra. The player is an underwriter reviewing ship, cargo, and mixed insurance contracts across 24 turns from 1599-01-15 to 1600-12-15.

The full design target is:

- 24 monthly turns.
- 7 contracts per turn.
- 168 total contracts.
- 15 special contractors.
- 37 special contractor appearances.
- 131 general contracts.
- Deterministic accident outcomes through hidden `AccidentFlag`.
- Objective underwriting rules plus subjective human consequences.

## Implementation Vocabulary

Use:

- Underwriter.
- Contractor or applicant.
- Contract case.
- Ship insurance.
- Cargo insurance.
- Mixed contract.
- Document bundle.
- Absolute rejection (`AR`).
- Rejection consideration (`CR`).
- Special contractor.
- After-story.

Do not use old reference-project vocabulary such as adventurer, monster, location countermeasure, or quest unless a future design decision explicitly restores it.

## First Playable Direction

Recommended first playable scope:

- Ship-only contracts.
- Bundle 1, then bundle 2, then bundle 3.
- 2-3 deterministic contract fixtures before expanding to a 4-turn vertical slice.
- Representative AR/CR rules.
- Hidden `AccidentFlag` outcome validation.
- Approve/reject decisions first.

Postpone:

- Cargo bundles 4-6.
- Mixed bundle 7.
- Full 24-turn content import.
- Contract map animation.
- Insurance claim detail.
- Full settlement economy.
- Special contractor dialogue and after-story branches.

## Non-negotiable Design Confirmations

- `AccidentFlag` is deterministic, not probabilistic.
- Rejected contracts do not produce accidents.
- Subjective information does not alter objective score.
- The 24-turn Notion plan wins over older prototype pages when scope conflicts.
- The older "bundle 4 removed" note does not remove current cargo bundle 4.
- The local `docs/design/` package is now the implementation-facing summary of the Notion pass.

## Current Unresolved Sources

The Google Drive data folder and these files were not accessed in this task:

- `01_Contracts.xlsx`
- `02_Documents.xlsx`
- `03_SubjectiveInfo.xlsx`
- `04_UnderwritingRules.xlsx`
- `05_Economy.xlsx`
- `06_SpecialContractors.xlsx`
- `07_Dialogues.xlsx`
- `08_AfterStories.xlsx`

Do not invent exact row data, dialogue text, or final economy simulation values until those files are available.

## M0-M3 Guardrails

| Milestone | Guardrail |
| --- | --- |
| M0 documentation/prompting | Use these docs and update unresolved questions, but do not implement gameplay. |
| M1 domain model | Keep Unity scene/prefab work out; build testable contract/rule/outcome domain first. |
| M2 data validation | Validate IDs, document bundle shape, rule windows, and deterministic accidents before content expansion. |
| M3 first UI | Build a minimal workdesk/document/decision loop over stable fixtures. |

