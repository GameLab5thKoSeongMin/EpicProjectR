# Refactoring Milestone Proposal

## M0: Project Rules, Documentation, and Baseline Validation

- Goal: Establish guardrails before implementation.
- Scope: Add docs, architecture decision notes, coding conventions, test plan, and reference/source boundaries.
- Non-goals: Gameplay implementation, scene work, prefab work.
- Why now: Prevents accidental copying from `ref_folder`.
- Likely future folders: `EpicProjectR/Assets/Scripts`, `docs/refactor`, `docs/design`.
- Architecture decisions: Production code lives in active Unity project only; reference remains read-only.
- Data/algorithm considerations: Define stable ID conventions.
- Patterns: ADRs, validation checklist.
- Acceptance criteria: Team can identify source of truth and first implementation target.
- Validation method: Documentation review.
- Risks: Documentation becomes stale.
- Rollback strategy: Revert docs only.
- Suggested Codex prompt summary: "Create project implementation guardrails and baseline test/documentation structure for the underwriting refactor; do not implement gameplay."

## M1: Domain Model Foundation

- Goal: Add pure C# domain models and first tests.
- Scope: IDs, case, document field, rule, review result, player submission, turn, contract, outcome, economy value objects.
- Non-goals: Unity UI, scene wiring, importers.
- Why now: All later systems depend on stable concepts.
- Likely future folders: `EpicProjectR/Assets/Scripts/Domain`, `EpicProjectR/Assets/Tests`.
- Architecture decisions: Pure C# first; no MonoBehaviour dependencies.
- Data/algorithm considerations: immutable result objects, dictionaries by stable ID.
- Patterns: Value object, result object.
- Acceptance criteria: Models compile and unit tests cover basic creation/equality/serialization assumptions.
- Validation method: Unity edit mode tests or .NET-compatible tests.
- Risks: Over-modeling unconfirmed features.
- Rollback strategy: Remove new domain folder and tests.
- Suggested Codex prompt summary: "Implement a minimal pure C# underwriting domain model with stable IDs and result objects; no Unity scene or prefab changes."

## M2: Data Definitions and Content Validation

- Goal: Define authorable/imported content structures and validators.
- Scope: ScriptableObject definitions for rules, document bundles, contract cases, news, routes, contractors; validator reports duplicate IDs and missing references.
- Non-goals: Full Excel import unless source files are available and approved.
- Why now: Prevents string-key drift seen in `ref_folder`.
- Likely future folders: `EpicProjectR/Assets/Scripts/Content`, `EpicProjectR/Assets/Scripts/Editor`.
- Architecture decisions: SOs define content; runtime state remains separate.
- Data/algorithm considerations: `Dictionary<string, Definition>` validation.
- Patterns: Repository, validation result.
- Acceptance criteria: Validator catches duplicate IDs and missing document/rule references.
- Validation method: Editor validation command plus tests.
- Risks: Schema may change when Excel source is delivered.
- Rollback strategy: Keep definitions, replace importer/validator mapping.
- Suggested Codex prompt summary: "Create initial content definition ScriptableObjects and validation utilities for underwriting IDs and references."

## M3: Underwriting Rule / Policy Engine

- Goal: Evaluate active AR/CR rules deterministically.
- Scope: Rule conditions, active rule selection, review result, submission audit, premium policy.
- Non-goals: Complete UI.
- Why now: Core gameplay depends on reliable rule outputs.
- Likely future folders: `EpicProjectR/Assets/Scripts/Domain/Rules`, `EpicProjectR/Assets/Scripts/Application`.
- Architecture decisions: Rules are content; evaluator is pure service.
- Data/algorithm considerations: ordered rule lists, `HashSet` checked IDs, deterministic results.
- Patterns: Policy, result object.
- Acceptance criteria: Known sample cases produce expected triggered AR/CR results.
- Validation method: Unit tests using hand-authored fixtures.
- Risks: Rule condition set may be too small for later cargo rules.
- Rollback strategy: Extend condition types without changing public review result.
- Suggested Codex prompt summary: "Implement deterministic underwriting review and submission audit services with tests for missing, mismatch, numeric, and date conditions."

## M4: Application Case Flow

- Goal: Drive a fixed turn/contract review sequence without final UI polish.
- Scope: Turn schedule, current case selection, submit decision, create active contract, resolve deterministic outcomes.
- Non-goals: Full settlement UI, after-story UI.
- Why now: Connects rules to player flow.
- Likely future folders: `EpicProjectR/Assets/Scripts/Application`.
- Architecture decisions: Application service coordinates domain services; UI remains passive.
- Data/algorithm considerations: queues/index pointers by turn and slot.
- Patterns: State machine, command object if useful.
- Acceptance criteria: A test sequence can process multiple contracts and produce expected active contracts/outcomes.
- Validation method: Unit/integration tests without Unity scene.
- Risks: Conditional approval semantics unclear.
- Rollback strategy: Keep command/result boundary and adjust decision enum.
- Suggested Codex prompt summary: "Implement a pure/application-level turn and case progression service for fixed contract schedules."

## M5: UI Presenter Refactor

- Goal: Build first Unity UI presenters against the new domain/application services.
- Scope: Contract review presenter, document presenter, checklist presenter, decision buttons.
- Non-goals: Import old scenes or prefabs wholesale.
- Why now: Domain is stable enough to display.
- Likely future folders: `EpicProjectR/Assets/Scripts/Presentation`, `EpicProjectR/Assets/Scenes`.
- Architecture decisions: Views bind view models; presenters call application services.
- Data/algorithm considerations: view model snapshots.
- Patterns: Presenter, view model.
- Acceptance criteria: A simple scene can show one case, check reasons, and submit a decision.
- Validation method: Play mode smoke test and presenter unit tests.
- Risks: UI polish may distract from logic.
- Rollback strategy: Keep domain/application services; replace views.
- Suggested Codex prompt summary: "Create minimal Unity presenters and passive views for one underwriting case using the new services."

## M6: Player Approval / Rejection Decision Flow

- Goal: Complete objective decision feedback.
- Scope: decision submission, AR/CR checked IDs, score calculation, premium quote, reason feedback.
- Non-goals: Full economy and after-stories.
- Why now: It is the core repeated action.
- Likely future folders: `Domain/Scoring`, `Presentation/Review`.
- Architecture decisions: Submission audit result is the source for feedback.
- Data/algorithm considerations: compare checked set to triggered set.
- Patterns: Result object, policy.
- Acceptance criteria: Correct reject, wrong approve, CR pricing, and missed reason cases are tested.
- Validation method: Unit tests and manual scene smoke test.
- Risks: Subjective deviation scoring may be misunderstood.
- Rollback strategy: Adjust score policy while keeping result shape.
- Suggested Codex prompt summary: "Implement player decision audit and feedback for AR/CR checks and premium adjustment."

## M7: Turn Progression and Result Handling

- Goal: Resolve approved contracts, accidents, news, and monthly settlement.
- Scope: active contract status, deterministic accident resolution, settlement line items, result event queue.
- Non-goals: Final art for result screens.
- Why now: Decisions need consequences.
- Likely future folders: `Application/Turns`, `Domain/Outcomes`, `Domain/Economy`.
- Architecture decisions: outcomes and economy are services, not UI logic.
- Data/algorithm considerations: active contracts list, outcome date checks, line item results.
- Patterns: State machine, result object.
- Acceptance criteria: Approved flagged contract always accidents on due date; rejected flagged contract does not.
- Validation method: Unit tests.
- Risks: Economy formulas depend on external Excel values.
- Rollback strategy: Keep service boundaries and swap formulas.
- Suggested Codex prompt summary: "Implement deterministic outcome resolution and monthly settlement calculations from domain policies."

## M8: Debug Tools, QA Tools, and Polish

- Goal: Make content and flow inspectable for designers and testers.
- Scope: validation windows, rule debug view, contract schedule viewer, review result inspector.
- Non-goals: New gameplay systems.
- Why now: Once core loop exists, content errors become the bottleneck.
- Likely future folders: `EpicProjectR/Assets/Scripts/Editor`, `Presentation/Debug`.
- Architecture decisions: Debug tools call validators and services.
- Data/algorithm considerations: issue grouping by source ID/row.
- Patterns: Editor tooling, validation result.
- Acceptance criteria: A designer can see why a contract triggers each rule.
- Validation method: Manual editor tests and validator fixtures.
- Risks: Editor tools become required for runtime.
- Rollback strategy: Keep tools editor-only.
- Suggested Codex prompt summary: "Build editor validation and debugging tools for underwriting contracts, rules, and turn schedules."

## M9: Save/Load Readiness

- Goal: Add serializable runtime snapshot support if project scope requires it.
- Scope: snapshot model, save/load service, migration plan placeholder.
- Non-goals: Cloud save, multiple slots UI, encryption.
- Why now: After state shape is stable.
- Likely future folders: `Application/SaveLoad`.
- Architecture decisions: Save stable IDs and primitive values only.
- Data/algorithm considerations: snapshot DTOs, repository rehydration.
- Patterns: Repository, serializer adapter.
- Acceptance criteria: Save mid-turn, load, and continue with same results.
- Validation method: deterministic save/load tests.
- Risks: Implementing before state stabilizes causes churn.
- Rollback strategy: Disable save UI; retain snapshot tests for later.
- Suggested Codex prompt summary: "Add save/load-ready runtime snapshot DTOs and deterministic rehydration tests for the underwriting session."
