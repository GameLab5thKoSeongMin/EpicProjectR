# Milestone Reconciliation Notes

Last updated: 2026-07-05.

| Source document | Original milestone summary | Issue or ambiguity | Final reconciled milestone | Reason |
| --- | --- | --- | --- | --- |
| `docs/design/10_IMPLEMENTATION_SOURCE_OF_TRUTH.md` | M1 domain model, M2 data validation, M3 first UI | Broad summary compresses several implementation steps. | Keep as guardrail direction, not exact numbering. | Design doc is product source of truth, but detailed execution needs smaller tasks. |
| `docs/refactor/07_REFACTORING_MILESTONE_PROPOSAL.md` | M2 content definitions/validation, M3 rule engine, M4 application flow, M5 UI | Better technical sequence, but M2 suggested ScriptableObjects before approval. | M2 becomes in-memory fixtures and validation foundation. | Avoid ScriptableObject asset creation until explicitly approved. |
| `docs/project/VALIDATION_PLAN.md` | M2 import/validation, M3 minimal UI | Validation summary is high-level. | Validation is distributed across M2, M3, M4, M5, and M6. | Each behavior should be testable before UI. |
| `docs/project/M0_COMPLETION_REPORT.md` | M1 can start with pure C# domain model. | Does not define full sequence. | M1 remains pure C# domain model. | Matches both design and refactor docs. |

## Specific Decisions

| Question | Final decision |
| --- | --- |
| Should M2 be content definitions, validation, or import pipeline? | M2 is in-memory content fixtures and validation foundation. Full import is M11 and is data-gated. ScriptableObject assets require explicit approval. |
| Should M3 be rule engine or first UI? | M3 is the underwriting rule engine. First UI begins at M7. |
| Where should player decision audit and premium policy live? | M4. It depends on `ReviewResult` from M3 and produces `DecisionAuditResult` and `PremiumQuoteResult`. |
| Where should deterministic outcomes and minimal settlement live? | M6. It depends on active contract state from M5. |
| Where should first Unity UI begin? | M7, after M1-M6 establish testable behavior. Scene/prefab changes require human approval. |
| Where does first playable loop live? | M8, after minimal UI and core application flow exist. |
| Where does save/load readiness live? | M10, after runtime state shape exists. |
| Where does full 24-turn content pipeline live? | M11, after fixture architecture is proven and source data is available or explicitly approved. |

## Numbering Summary

The reconciled roadmap is:

1. M1 Domain Model Foundation.
2. M2 Content Fixtures And Validation Foundation.
3. M3 Underwriting Rule Engine.
4. M4 Decision Audit And Premium.
5. M5 Turn And Case Flow.
6. M6 Outcome And Settlement Foundation.
7. M7 Minimal Unity Review UI.
8. M8 First Playable Loop.
9. M9 Debug And Validation Tools.
10. M10 Save/Load Readiness.
11. M11 Full Content Pipeline Prep.
12. M12 Polish QA And Extension.

