# Start To End Implementation Plan

Last updated: 2026-07-05.

## Current Baseline After M0

M0 established repository guardrails, source hierarchy, validation expectations, and Codex task structure. The active production Unity project is `EpicProjectR/`. The old project in `ref_folder/` is reference-only for the entire roadmap.

Confirmed local source hierarchy:

1. `AGENTS.md`
2. `EpicProjectR/AGENTS.md`
3. `docs/design/`
4. `docs/project/`
5. `docs/refactor/`
6. `docs/adr/`

## What M0.5 Produces

M0.5 produces a documentation-only START-to-END implementation package:

- Reconciled roadmap from M1 through M12.
- Prompt files for each future milestone.
- First playable definition.
- Dependency graph.
- Risk and gate map.

M0.5 does not implement gameplay, create production C# code, create tests, create Unity UI, or modify Unity assets.

## Terms

| Term | Definition |
| --- | --- |
| Technical slice | A small non-UI or minimal-UI path proving core data, rules, decisions, and outcomes work with fixtures. |
| First playable loop | A small end-to-end playable loop where the player reviews 2-3 ship contracts, checks AR/CR reasons, approves/rejects, receives audit/outcome feedback, and advances. |
| Vertical slice | A broader representative game segment, likely the 4-turn ship slice, with more UI, rule variety, news, outcomes, and QA polish. |
| Full 24-turn-ready architecture | Architecture that can support 24 turns, 168 contracts, cargo/mixed expansion, special contractors, after-stories, validation, and save/load without rewriting the foundations. |

## Overall Strategy

Build from stable domain concepts outward:

1. Value objects and model shapes.
2. Fixture content and validators.
3. Deterministic rule evaluation.
4. Decision audit and premium recommendation.
5. Turn/session flow.
6. Deterministic outcomes and minimal settlement.
7. Minimal UI and composition root, only after domain/application behavior is testable.
8. First playable loop.
9. Debug tools, save/load readiness, full content pipeline preparation, and extension planning.

## Reconciled Milestone List

| Milestone | Title | Type | Production C#? | Scene/prefab? | Reasoning |
| --- | --- | --- | --- | --- | --- |
| M1 | Domain Model Foundation | Implementation | Yes | No | Establish stable IDs, dates, contracts, documents, decisions, and result shapes. |
| M2 | Content Fixtures And Validation Foundation | Implementation | Yes | No | Add small in-memory fixtures and validators before a rule engine. |
| M3 | Underwriting Rule Engine | Implementation | Yes | No | Evaluate AR/CR rules deterministically over fixtures. |
| M4 | Decision Audit And Premium | Implementation | Yes | No | Separate rule results from player submission, scoring, and CR premium policy. |
| M5 | Turn And Case Flow | Implementation | Yes | No | Process a small sequence of contracts without UI. |
| M6 | Outcome And Settlement Foundation | Implementation | Yes | No | Resolve deterministic accidents and minimal traceable settlement/evaluation outputs. |
| M7 | Minimal Unity Review UI | Mixed, approval-gated | Maybe | Approval required | First passive UI layer and composition root only after core behavior exists. |
| M8 | First Playable Loop | Mixed, approval-gated | Yes | Approval required if assets change | Connect small playable flow over fixtures. |
| M9 | Debug And Validation Tools | Implementation or mixed | Yes | No by default | Make content/rules inspectable; editor tools require approval. |
| M10 | Save/Load Readiness | Implementation or docs | Maybe | No | Add snapshot design or implementation after runtime state shape exists. |
| M11 | Full Content Pipeline Prep | Documentation or implementation, data-gated | Maybe | No | Prepare import/schema strategy; final importer needs source files and approval. |
| M12 | Polish QA And Extension | Documentation planning | No by default | No by default | Break post-first-playable work into smaller extension tasks. |

## Numbering Changes From Previous Docs

`docs/design/10_IMPLEMENTATION_SOURCE_OF_TRUTH.md` uses broad M1-M3 guardrails. `docs/refactor/07_REFACTORING_MILESTONE_PROPOSAL.md` uses a more detailed M1-M9 sequence.

This plan keeps M1 as domain model, then splits broad "data validation" and "first UI" into smaller steps:

- M2 becomes in-memory content fixtures and validation foundation, not full import.
- M3 becomes underwriting rule engine, not UI.
- First Unity UI begins at M7, after model/rules/decisions/turn/outcome foundations exist.

## Why This Order

- UI depends on stable model, rule results, decisions, and session state.
- Rule evaluation depends on fixture content and document model shape.
- Decision audit depends on review results.
- Turn flow depends on decision submission and active contract creation.
- Outcomes and settlement depend on active contract state.
- Save/load depends on stable runtime state.
- Full content import depends on accessible source spreadsheets and schema decisions.

## Documentation-only Milestones

- M0 and M0.5 are documentation-only.
- M12 is planning-only by default.
- M11 may remain documentation-only if source spreadsheets are still inaccessible.
- M10 may become documentation-only if runtime state is not stable enough.

## Milestones That Must Not Touch Scenes/Prefabs

M1-M6, M9 by default, M10, and M11 must not touch scenes or prefabs unless a future prompt explicitly changes that. M7 and M8 require human approval before any scene, prefab, ScriptableObject asset, package, or project setting change.

## Milestones Depending On Unresolved Questions

| Question | Blocks |
| --- | --- |
| Conditional approval UX/scoring | Final M4/M7/M8 UI behavior, not M1 model reservation. |
| Premium adjustment method | Final M4/M7 behavior; default is rule-driven premium quote. |
| Exact AR/CR rows | Full M3/M11 content, not representative fixture engine. |
| Route coordinates | Contract map and polish, not M1-M8 small loop. |
| Claim detail fields | Claim sheet UI and polish, not minimal outcome feedback. |
| Excel source access | Full 168-contract import and final validation. |

## Human Review Gates

- Before M7 scene/prefab/UI asset work.
- Before any ScriptableObject asset creation.
- Before package or project setting changes.
- Before copying/importing anything from external or reference sources.
- Before implementing full content import.
- Before expanding to cargo/mixed contracts.
- Before save/load implementation if runtime state is not stable.

## Stop Conditions

Stop and ask for human review if:

- A prompt does not explicitly allow a path that needs modification.
- A required source spreadsheet is unavailable for the requested scope.
- Implementation would require scenes, prefabs, assets, packages, project settings, or `ref_folder/`.
- Design docs conflict with the requested behavior.
- Tests or validation reveal that a milestone cannot meet its acceptance criteria.

## Handoffs

Each milestone must hand off:

- Files changed.
- Algorithms, data structures, and design patterns used.
- Validation performed and skipped.
- Assumptions added.
- Risks and follow-up work.
- Whether the next milestone is unblocked.

