# Start To End Dependency Graph

Last updated: 2026-07-05.

## Graph

```mermaid
flowchart TD
  A["Stable IDs and value objects"] --> B["GameDate / TurnNumber"]
  A --> C["ContractCase"]
  A --> D["DocumentBundle"]
  A --> E["DocumentRecord"]
  A --> F["DocumentField"]
  A --> G["RuleDefinition"]
  B --> C
  D --> C
  E --> C
  F --> E
  G --> H["RuleCondition"]
  G --> I["RuleActivationWindow"]
  C --> J["Content fixtures"]
  D --> J
  E --> J
  G --> J
  J --> K["Repositories"]
  J --> L["Validators"]
  K --> M["ReviewResult"]
  H --> M
  I --> M
  M --> N["ReviewSubmission"]
  N --> O["DecisionAuditResult"]
  M --> O
  M --> P["PremiumQuoteResult"]
  O --> Q["TurnSessionState"]
  C --> Q
  Q --> R["ActiveContractState"]
  R --> S["OutcomeResult"]
  S --> T["SettlementResult"]
  Q --> U["Application/session services"]
  O --> U
  S --> U
  T --> U
  U --> V["UI Presenters"]
  V --> W["Passive Views"]
  U --> X["Composition Root"]
  V --> X
  L --> Y["Debug/validation tools"]
  M --> Y
  Q --> Z["Save/load snapshot"]
  R --> Z
  S --> Z
```

## Must Exist Before UI Work Starts

M7 UI work should not begin until these exist and are tested:

- Stable IDs and value objects.
- `GameDate` and `TurnNumber`.
- `ContractCase`.
- `DocumentBundle`, `DocumentRecord`, and `DocumentField`.
- Representative content fixtures and repositories.
- `RuleDefinition`, `RuleCondition`, and `RuleActivationWindow`.
- `ReviewResult`.
- `ReviewSubmission`.
- `DecisionAuditResult`.
- `PremiumQuoteResult`.
- `TurnSessionState`.
- Minimal `ActiveContractState`.

`OutcomeResult` and minimal `SettlementResult` should exist before connecting the full first playable loop in M8.

## Dependency Notes

- Rule evaluation depends on both content fixtures and document field representation.
- Decision audit depends on review results, not raw documents.
- Premium quote depends on CR findings from review results.
- Active contract creation depends on decision submission and audit.
- Outcome resolution depends on active contract state and hidden `AccidentFlag`.
- UI presenters should consume view models from application/session services, not inspect repositories directly.
- Save/load should serialize stable IDs and primitive runtime state, then rehydrate through repositories.

