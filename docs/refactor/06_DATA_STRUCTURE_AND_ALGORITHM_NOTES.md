# Data Structure and Algorithm Notes

## Practical Recommendations

### Tag Matching

- Recommended data structure: `HashSet<string>` or strongly typed `TagId` sets.
- Reasoning: fast lookup for document flags, contractor traits, route tags, cargo tags, and rule categories.
- Complexity: average `O(1)` contains.
- Memory tradeoff: modest extra hash overhead.
- Fit: useful for active news tags, route properties, cargo tags, or contractor traits.
- Simpler alternative: `List<string>` and linear search.
- When simpler is enough: early M1 prototypes with fewer than 20 tags.

### Score Calculation

- Recommended data structure: immutable `SubmissionAuditResult` with lists of correct, missed, and false checked rule IDs.
- Reasoning: the design separates objective evaluation from money. A result object makes UI feedback reproducible.
- Complexity: if checked IDs are a `HashSet`, comparing triggered rules is `O(r)` where `r` is active/triggered rule count.
- Memory tradeoff: result lists duplicate IDs for clarity.
- Fit: supports settlement and review explanation.
- Simpler alternative: return only an integer score.
- When simpler is enough: never for final review, because the UI needs explanation.

### Rule Evaluation

- Recommended data structure: ordered `List<RuleDefinition>` for active rules, each containing ordered `List<RuleCondition>`.
- Reasoning: deterministic evaluation and stable UI order matter.
- Complexity: `O(r*c)` where `r` is active rules and `c` is average condition count.
- Memory tradeoff: minimal.
- Fit: 168 contracts and a few dozen rules are small; deterministic clarity beats clever indexing.
- Simpler alternative: hard-coded `if` checks.
- When simpler is enough: tiny prototype for one or two rules only.

### Absolute Rejection Checks

- Recommended data structure: active AR list plus `ReviewResult.TriggeredAbsoluteReasons`.
- Reasoning: any triggered AR means rejection is required.
- Complexity: same as rule evaluation.
- Memory tradeoff: result list stores triggered definitions or IDs.
- Fit: mirrors design's AR category.
- Simpler alternative: boolean `mustReject`.
- When simpler is enough: internal scoring only, but not for UI feedback.

### Rejection Consideration Checks

- Recommended data structure: active CR list plus checked/triggered CR ID sets.
- Reasoning: CRs affect premium recommendation and scoring differently than ARs.
- Complexity: rule eval `O(r*c)`, premium calculation `O(k)` for triggered/checked CRs.
- Memory tradeoff: small.
- Fit: supports 125%, 150%, and 3+ reject-recommended behavior.
- Simpler alternative: count CRs only.
- When simpler is enough: if all CRs have identical weight and no special text.

### Content Lookup

- Recommended data structure: `Dictionary<string, Definition>` keyed by stable IDs.
- Reasoning: contract tables join by `ContractID`, `RouteID`, `SpecialID`, `SubjectiveID`, and rule IDs.
- Complexity: average `O(1)` lookup.
- Memory tradeoff: dictionary overhead is acceptable.
- Fit: essential for validation and save/load.
- Simpler alternative: list scan.
- When simpler is enough: editor prototype with tiny content.

### Data Validation

- Recommended data structure: `ValidationResult` with `List<ValidationIssue>`, grouped by source table/file and row.
- Algorithm:
  1. Build dictionaries for all ID-bearing definitions.
  2. Check duplicate IDs.
  3. Check foreign key references.
  4. Check document fields referenced by rules.
  5. Check turn contract counts.
  6. Check rule activation windows.
  7. Check all UI-visible text has a source or localization key.
- Complexity: mostly `O(n)` after dictionaries are built.
- Memory tradeoff: stores issue list only.
- Fit: prevents reference-project-style key drift.
- Simpler alternative: throw on first error.
- When simpler is enough: command-line import smoke test; not editor UX.

### UI State Updates

- Recommended data structure: view models with immutable snapshots per screen.
- Reasoning: UI should bind to prepared data, not calculate rules.
- Complexity: rebuild small view model on decision/check changes, usually `O(r + d)` for rules plus documents.
- Memory tradeoff: small duplicate display strings.
- Fit: Unity UI with presenter pattern.
- Simpler alternative: views read domain models directly.
- When simpler is enough: internal debug views only.

### Turn Progression

- Recommended data structure:
  - `List<TurnDefinition>` indexed by turn number.
  - `Queue<ContractId>` or index pointer for current turn's contracts.
  - `List<ActiveContractState>` for approved contracts.
- Reasoning: design is fixed 24 turns with 7 slots each.
- Complexity: next contract `O(1)`, active contract advancement `O(a)` per turn where `a` is active contracts.
- Memory tradeoff: active contracts list grows but remains small.
- Fit: deterministic 24-turn structure.
- Simpler alternative: a single current contract index into all contracts.
- When simpler is enough: if there are always exactly 7 contracts and no skipped slots.

### Future Save/Load Support

- Recommended data structure: serializable `GameSnapshot` with IDs and primitive state.
- Include:
  - Current turn/slot.
  - Player economy/career.
  - Decisions by `ContractId`.
  - Active contract statuses.
  - Queued after-story event IDs.
  - Seen news/event IDs.
- Complexity: save/load `O(state size)`.
- Memory tradeoff: small.
- Fit: avoids serializing Unity object references.
- Simpler alternative: Unity `JsonUtility` snapshot of runtime POCOs.
- When simpler is enough: early debug save before custom migrations.

## Optional Future Improvements

### Strongly Typed IDs

Use small readonly structs or generated constants for IDs once schema stabilizes. This reduces string mistakes but adds boilerplate.

### Rule Indexing by Field

If rule count grows significantly, index rules by referenced field IDs. Not needed for current expected size.

### Import Code Generation

Generate C# constants for document field IDs from design tables. Helpful after the Excel schema stabilizes.

### Diff-based UI Binding

Only update UI elements whose view model fields changed. Not needed until UI performance becomes a real issue.

## What Not to Optimize Prematurely

- Do not build a generic database layer.
- Do not introduce ECS.
- Do not add caching before rule evaluation is measured.
- Do not make the rule engine a scripting language.
- Do not solve large-scale localization import until content ownership is confirmed.
