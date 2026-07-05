# Validation Plan

Last updated: 2026-07-05.

## Documentation Validation

For documentation-only milestones:

- Confirm created/updated files are under `docs/`.
- Confirm no production C# files, Unity scenes, prefabs, ScriptableObjects, or `ref_folder` files changed.
- Confirm every unresolved source is listed in `docs/design/00_SOURCE_MAP.md` or `docs/refactor/OPEN_QUESTIONS.md`.
- Confirm implementation-facing decisions are summarized in `docs/design/10_IMPLEMENTATION_SOURCE_OF_TRUTH.md`.

## Data Validation

When spreadsheets become available, validate:

1. `C001` to `C168` are complete and unique.
2. Turn/slot mapping matches `(turn - 1) * 7 + slot`.
3. Contract type and bundle progression match the 24-turn plan.
4. Every `RouteID`, `SpecialID`, and `SubjectiveID` resolves.
5. Document rows match active bundle requirements.
6. Missing documents use `Submitted = 0`.
7. Rule windows support discontinuous ranges.
8. News entries activate, strengthen, or deactivate expected rules.
9. Dialogue and after-story references point to existing contracts or templates.

## Domain Rule Validation

Minimum edit-mode test areas:

- `AR` findings force rejection as the objective answer.
- `CR` findings produce expected premium recommendation tiers.
- Three or more `CR` findings recommend rejection.
- Subjective information does not change objective score.
- Rejected contracts with `AccidentFlag = 1` do not create accidents.
- Approved or conditional contracts with `AccidentFlag = 1` create deterministic accidents.
- News windows activate and deactivate rule checks.

## Economy Validation

Once economy parameters exist, validate:

- Salary and commission are calculated by rank.
- Renewal and no-accident bonuses are capped correctly.
- Accident penalties use commission refund plus fixed rank penalty.
- Bribe income and later inspection fine/evaluation penalty are both applied.
- Negative balance for two consecutive turns triggers bankruptcy.
- Settlement line items trace to contracts, rules, or events.

## UI Validation

First playable manual checks:

- Turn/date display is correct.
- Contract list displays all fixtures for the turn.
- Bundle 1-3 documents render without missing required fields.
- Rule/checklist panel distinguishes AR, CR, and subjective information.
- Approve/reject decisions produce expected objective result.
- Debug/test view does not expose hidden values in normal player flow.

Later UI checks:

- Approved contract map shows route icons.
- Accident contracts turn red on accident date.
- Claim sheet displays accident details once defined.
- Board/letter events are capped and queued.
- Settlement receipt is readable and traceable.

## Milestone Validation Summary

| Milestone | Validation focus |
| --- | --- |
| M0 | Documentation, source hierarchy, boundaries, open questions. |
| M1 | Domain model, representative ship rules, deterministic outcome tests. |
| M2 | Import/validation pipeline over source tables. |
| M3 | Minimal workdesk/document/decision UI over fixtures. |
| M4 | 4-turn ship vertical slice. |
| M5+ | Cargo, mixed contracts, after-story, economy, map, polish. |

