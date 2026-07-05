# Architecture Direction

## Summary

Use a small layered Unity architecture:

1. Content definitions: ScriptableObjects or imported data assets with stable IDs.
2. Domain logic: pure C# models, services, policies, and result objects.
3. Application flow: turn/session controller that coordinates services.
4. Presentation: Unity views plus presenters/view models.
5. Composition: one scene bootstrap/composition root wires dependencies.

## Pattern Direction Matrix

| Pattern | Where Used | Problem Solved | Risk / Complexity | Avoid |
| --- | --- | --- | --- | --- |
| Composition Root | Main gameplay scene bootstrap | Replaces hidden scene searches and scattered globals with explicit wiring | Inspector wiring can become manual busywork | Multiple competing bootstraps |
| Runtime Services | Review, rule activation, turn progression, outcomes, economy | Keeps gameplay logic testable outside Unity UI | Services can grow too broad | Unity object references in services |
| ScriptableObject Definitions | Rules, bundles, news, contractor profiles, after-story definitions | Lets designers author content while code reads stable definitions | SOs can be misused as runtime state | Mutating SOs during play |
| Pure C# Domain Logic | Rule evaluation, scoring, outcomes, settlement | Enables fast deterministic tests for 168 contracts | Requires adapter layer from Unity content | MonoBehaviour-dependent rules |
| UI Presenter | Review screen, documents, checklist, settlement, map | Keeps views passive and prevents UI from owning gameplay decisions | Presenters can become controllers | Views calling evaluators directly |
| Rule / Policy | AR/CR checks, premium, correct action, news activation | Encapsulates changing criteria without controller branching | Over-generic rule language | Scripting engine before evidence |
| Repository | Lookup by contract/rule/news/document IDs | Hides whether content is SO, JSON, CSV, or imported Excel | Premature abstraction | Database-like layer for small data |
| Result Object | Review, audit, premium quote, outcome, settlement, validation | Makes feedback explainable and reproducible | Bloated result payloads | Returning only booleans for UI flows |
| State Machine | Top-level turn/review/settlement phases | Prevents invalid UI actions in wrong phase | Over-modeling animations | State for every panel toggle |
| Command | Submit decision, advance turn, acknowledge event | Useful for save/load and debug replay | Command bus overhead | Global command framework early |
| Factory | Convert definitions to runtime models/view models | Keeps import details out of runtime logic | Hidden mutation during creation | Factories making gameplay decisions |
| Editor Validation | IDs, joins, active ranges, field references | Catches content drift before play mode | Validators can lag schema changes | Manual-only content QA |

## SOLID Principles

### Where to Use

- Underwriting rules as small policy objects or data-driven predicates.
- Economy calculation as a separate service from turn progression.
- UI views that expose bind/update methods and do not evaluate rules.

### Why It Helps

The design will expand from ship insurance to cargo and mixed insurance. Keeping rule evaluation, scoring, outcomes, and UI separate prevents each new document bundle from bloating one controller.

### Risk

Too many tiny abstractions can slow a small team.

### Avoid

Do not split every field into its own service. Start with a few clear services: review, rule activation, outcome, economy, turn progression.

## Unity Scene Composition

### Proposed Structure

- One production gameplay scene initially.
- A `GameBootstrap` or `CompositionRoot` MonoBehaviour references content repositories and view roots.
- Scene MonoBehaviours are view/presenter hosts, not domain owners.

### Why It Helps

The reference project relies on large scene wiring and `FindFirstObjectByType`. A composition root makes dependencies visible and testable.

### Risk

Manual inspector wiring can still drift.

### Avoid

Avoid hidden scene searches as the normal dependency path. Use them only in editor/debug helpers.

## Composition Root / Bootstrap

### Where to Use

In the main gameplay scene, create services:

- `UnderwritingReviewService`
- `RuleActivationService`
- `TurnProgressionService`
- `OutcomeResolver`
- `EconomyService`
- repositories

Then inject or assign them to presenters/controllers.

### Why It Helps

It replaces broad global state and makes save/load boundaries clearer.

### Risk

Unity inspector does not naturally construct pure C# graphs. Keep the bootstrap explicit and small.

### Avoid

Avoid `NewspaperRuntimeContext.Current` style global contexts as the default.

## Runtime Services

### Where to Use

Runtime services should own workflows:

- Review current case.
- Submit player decision.
- Advance turn.
- Resolve outcomes.
- Calculate settlement.

### Why It Helps

Services can be unit tested without Unity scenes.

### Risk

If services start knowing about Unity objects, the benefit disappears.

### Avoid

No `GameObject`, `Sprite`, `TMP_Text`, `Button`, or prefab references in domain services.

## ScriptableObject Data Definitions

### Where to Use

- Author rules, document bundles, news, contractors, after-story definitions.
- Optionally hold imported contract rows.

### Why It Helps

Designers can inspect and tune content in Unity.

### Risk

Mutable SOs can accidentally become runtime state.

### Avoid

Do not store current decision, current accident status, or player money in SOs.

## Pure C# Domain Logic

### Where to Use

- Rule condition evaluation.
- Score calculation.
- Accident/outcome calculation.
- Economy line item calculation.
- Turn schedule selection.

### Why It Helps

Fast tests and deterministic behavior are critical for 168 contracts.

### Risk

Mapping Unity content definitions into domain models adds a small adapter layer.

### Avoid

Do not make rule evaluation depend on MonoBehaviour lifecycle.

## UI Presenter Pattern

### Where to Use

- Contract review screen.
- Document bundle display.
- Checklist display.
- Contract map.
- Settlement report.
- News/after-story channels.

### Why It Helps

The reference views directly pull from `UnderwritingCase` by strings. Presenters can prepare view models and keep UI passive.

### Risk

Presenters can become mini-controllers if they own workflow.

### Avoid

Views should not call rule evaluators. Presenters should not mutate domain definitions.

## Rule / Policy Pattern

### Where to Use

- Absolute rejection checks.
- Consideration checks.
- Premium multiplier calculation.
- Correct action selection.
- Active rule schedule.

### Why It Helps

News changes and cargo expansion become data/policy changes, not scattered branches.

### Risk

A fully generic rule language may be overkill.

### Avoid

Do not build a complex scripting language. Use a small set of known condition operators.

## Repository Pattern

### Where to Use

- Content lookup by stable IDs.
- Contract definitions by `ContractId`.
- Rule definitions by `RuleId`.
- News by turn.

### Why It Helps

It hides whether content came from ScriptableObjects, JSON, CSV, or imported Excel.

### Risk

Repository interfaces can be too abstract early.

### Avoid

Avoid database-like layers until needed. In-memory dictionaries are enough for early milestones.

## Result Object Pattern

### Where to Use

- `ReviewResult`
- `SubmissionAuditResult`
- `PremiumQuoteResult`
- `OutcomeResult`
- `MonthlySettlementResult`
- `ValidationResult`

### Why It Helps

UI can show clear feedback without re-running logic.

### Risk

Result objects can become bloated.

### Avoid

Do not put service references or Unity objects inside result objects.

## State Machine

### Use If Useful

A small explicit state machine is useful for top-level flow:

- TurnIntro
- ReviewingCase
- DecisionSubmitted
- OutcomeFeedback
- Settlement
- TurnTransition

### Why It Helps

Prevents UI buttons from being valid in the wrong phase.

### Risk

Overly formal state machines can slow UI iteration.

### Avoid

Do not model every animation or panel toggle as domain state.

## Command Pattern

### Use If Useful

Player decisions can be represented as commands:

- `SubmitDecisionCommand`
- `AdvanceTurnCommand`
- `AcknowledgeEventCommand`

### Why It Helps

Commands are save/load and undo/debug friendly.

### Risk

May be unnecessary for M1-M3.

### Avoid

Do not introduce command buses unless needed.

## Factory Pattern

### Where to Use

- Convert content definitions into runtime case models.
- Create view models for document screens.
- Create outcome event instances.

### Why It Helps

Keeps content import formats away from runtime logic.

### Risk

Factories can hide too much if they mutate dependencies.

### Avoid

Do not use factories to perform gameplay decisions.

## Editor Validation Tools

### Where to Use

- Validate unique IDs.
- Validate contract joins.
- Validate document fields referenced by rules exist.
- Validate rule active ranges.
- Validate each turn has expected contract count.
- Validate accident flags and outcome dates.

### Why It Helps

The reference project already shows key-string drift. Validation is a top priority.

### Risk

Validators can lag behind schema changes.

### Avoid

Do not rely on manual inspection alone for 168 contracts.

## Testability

### Recommended Tests

- Rule condition tests.
- Rule activation by turn/news.
- Contract table join tests.
- Review result tests for known contracts.
- Premium calculation tests.
- Accident/outcome determinism tests.
- Economy monthly settlement tests.

### Avoid

Avoid making tests require loaded Unity scenes.

## Save/Load Readiness

### Direction

Runtime state should save stable IDs and primitive values:

- Current turn/slot.
- Decisions by contract ID.
- Active contracts by contract ID and status.
- Queued events by event ID.
- Player economy/career state.

### Avoid

Do not save ScriptableObject object references as the only state. They can be rehydrated by repositories.
