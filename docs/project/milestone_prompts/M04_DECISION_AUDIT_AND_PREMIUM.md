# Milestone M4: Decision Audit And Premium

Recommended reasoning effort: High

## Goal

Implement player decision audit and CR-based premium recommendation over `ReviewResult`.

## Required Reading

1. `AGENTS.md`
2. `EpicProjectR/AGENTS.md`
3. `docs/design/05_UNDERWRITING_RULES.md`
4. `docs/design/08_OUTCOMES_ECONOMY_AND_AFTERSTORY.md`
5. `docs/project/FIRST_PLAYABLE_DEFINITION.md`
6. `docs/refactor/OPEN_QUESTIONS.md`

## Task Type

Implementation: pure C# domain/application policies and tests.

## Allowed Paths

- `EpicProjectR/Assets/Scripts/Domain/`
- `EpicProjectR/Assets/Scripts/Application/`
- `EpicProjectR/Assets/Tests/EditMode/`
- Documentation updates if needed

## Forbidden Paths

- `ref_folder/`
- UI, scenes, prefabs, ScriptableObject assets, packages, project settings
- Full economy
- Settlement implementation

## Implementation Scope

- Compare `ReviewSubmission` against `ReviewResult`.
- Identify correct checked reasons.
- Identify missed reasons.
- Identify false positives.
- Correct action policy for approve/reject.
- Objective score calculation.
- Ensure subjective information has no score effect.
- CR premium recommendation:
  - 1 CR = 125 percent.
  - 2 CR = 150 percent.
  - 3+ CR = reject recommended, while preserving support for later risky underwriting.
- Reserve conditional approval in model; do not require UI behavior.

## Non-goals

- Full economy.
- Settlement line items.
- UI feedback screens.
- Conditional approval final UX.

## Architecture Constraints

- Decision audit consumes rule results; it must not re-evaluate documents directly.
- Premium policy is isolated and replaceable.
- Subjective information must not alter objective score.

## Algorithms/Data Structures To Consider

- Algorithms: set comparison, score calculation, CR count-to-multiplier mapping.
- Data structures: `HashSet<RuleId>`, audit result object, premium quote result.

## Design Patterns To Consider

- Policy.
- Result Object.
- Value Object.

## Validation Requirements

- Tests for correct reject.
- Tests for wrong approve.
- Tests for missed AR.
- Tests for false positive.
- Tests for CR pricing.
- Tests for subjective info non-effect.
- Forbidden path check.

## Final Report Format

Use standard milestone report and include algorithms, data structures, design patterns, validation, and untouched forbidden paths.

## Human Review Triggers

- Need final conditional approval semantics.
- Need freeform premium editing.
- Need UI changes.

## Boundary Confirmation

M4 must confirm no full economy, UI, scenes, prefabs, assets, packages, project settings, or `ref_folder/` changes.

