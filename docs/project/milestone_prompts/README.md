# Milestone Prompt Package

Last updated: 2026-07-05.

This folder contains reusable Codex prompts for implementing the game step by step after M0.5.

## Recommended Execution Order

1. `M01_DOMAIN_MODEL_FOUNDATION.md`
2. `M02_CONTENT_FIXTURES_AND_VALIDATION_FOUNDATION.md`
3. `M03_UNDERWRITING_RULE_ENGINE.md`
4. `M04_DECISION_AUDIT_AND_PREMIUM.md`
5. `M05_TURN_AND_CASE_FLOW.md`
6. `M06_OUTCOME_AND_SETTLEMENT_FOUNDATION.md`
7. `M07_MINIMAL_UNITY_REVIEW_UI.md`
8. `M08_FIRST_PLAYABLE_LOOP.md`
9. `M09_DEBUG_AND_VALIDATION_TOOLS.md`
10. `M10_SAVE_LOAD_READINESS.md`
11. `M11_FULL_CONTENT_PIPELINE_PREP.md`
12. `M12_POLISH_QA_AND_EXTENSION.md`

## Safe To Run Immediately

These are the safest first implementation prompts, assuming the human wants to proceed to implementation:

- `M01_DOMAIN_MODEL_FOUNDATION.md`

Run later only after previous milestone handoff confirms readiness:

- `M02_CONTENT_FIXTURES_AND_VALIDATION_FOUNDATION.md`
- `M03_UNDERWRITING_RULE_ENGINE.md`
- `M04_DECISION_AUDIT_AND_PREMIUM.md`
- `M05_TURN_AND_CASE_FLOW.md`
- `M06_OUTCOME_AND_SETTLEMENT_FOUNDATION.md`

## Requires Human Review First

- `M07_MINIMAL_UNITY_REVIEW_UI.md`, if it will modify scenes, prefabs, ScriptableObject assets, packages, or project settings.
- `M08_FIRST_PLAYABLE_LOOP.md`, if it will modify scenes, prefabs, ScriptableObject assets, packages, or project settings.
- `M09_DEBUG_AND_VALIDATION_TOOLS.md`, if it will add editor windows, editor menus, or asset-backed tooling.
- `M10_SAVE_LOAD_READINESS.md`, if runtime state is not stable enough for implementation.
- `M11_FULL_CONTENT_PIPELINE_PREP.md`, if source spreadsheets are used or importer implementation is requested.
- `M12_POLISH_QA_AND_EXTENSION.md`, before turning any extension area into implementation.

## May Modify Unity Scenes/Prefabs

Only these prompts may lead to scene/prefab changes, and only with explicit human approval:

- `M07_MINIMAL_UNITY_REVIEW_UI.md`
- `M08_FIRST_PLAYABLE_LOOP.md`

All other prompts must avoid scene/prefab changes unless a later human-approved prompt explicitly changes the boundary.

## Documentation-only Prompts

By default:

- `M12_POLISH_QA_AND_EXTENSION.md`

Conditional documentation-only prompts:

- `M10_SAVE_LOAD_READINESS.md`, if save/load implementation is too early.
- `M11_FULL_CONTENT_PIPELINE_PREP.md`, if source spreadsheets are inaccessible.

## Implementation Prompts

- `M01_DOMAIN_MODEL_FOUNDATION.md`
- `M02_CONTENT_FIXTURES_AND_VALIDATION_FOUNDATION.md`
- `M03_UNDERWRITING_RULE_ENGINE.md`
- `M04_DECISION_AUDIT_AND_PREMIUM.md`
- `M05_TURN_AND_CASE_FLOW.md`
- `M06_OUTCOME_AND_SETTLEMENT_FOUNDATION.md`
- `M07_MINIMAL_UNITY_REVIEW_UI.md`, with approval gate
- `M08_FIRST_PLAYABLE_LOOP.md`, with approval gate
- `M09_DEBUG_AND_VALIDATION_TOOLS.md`
- `M10_SAVE_LOAD_READINESS.md`, if state is stable
- `M11_FULL_CONTENT_PIPELINE_PREP.md`, if data and approval exist

## Depends On Inaccessible Spreadsheets

Blocked or limited by inaccessible spreadsheets:

- `M11_FULL_CONTENT_PIPELINE_PREP.md`
- Full expansion beyond representative fixtures in M3/M4/M8.
- Cargo, mixed, full economy, dialogue, and after-story expansion.

Not blocked by inaccessible spreadsheets:

- `M01_DOMAIN_MODEL_FOUNDATION.md`
- `M02_CONTENT_FIXTURES_AND_VALIDATION_FOUNDATION.md`
- `M03_UNDERWRITING_RULE_ENGINE.md` for representative fixtures
- `M04_DECISION_AUDIT_AND_PREMIUM.md` for known CR policy
- `M05_TURN_AND_CASE_FLOW.md`
- `M06_OUTCOME_AND_SETTLEMENT_FOUNDATION.md` for minimal deterministic outcomes

## Blocked Until Source Data Exists

- Full 168-contract import.
- Exact 18 AR and 12 CR final rows.
- Full cargo/mixed content.
- Special contractor branch rows.
- Dialogue import.
- After-story event import.
- Final economy simulation values.

## How To Use These Prompts

1. Open the next prompt file.
2. Paste it into a new Codex task.
3. Confirm any required human approval gates.
4. Ensure allowed and forbidden paths are explicit.
5. Run the milestone.
6. Review the final report before proceeding to the next prompt.

## Next Prompt To Run

Run `M01_DOMAIN_MODEL_FOUNDATION.md` next.

M1 should not modify Unity scenes, prefabs, ScriptableObject assets, packages, project settings, or `ref_folder/`.

