# Risk Register

| Risk | Cause | Impact | Probability | Mitigation | When to Revisit |
| --- | --- | --- | --- | --- | --- |
| Reference prototype copied directly | `ref_folder` contains working-looking scenes, prefabs, and scripts | New project inherits tight coupling and obsolete logic | High | Treat reference as read-only; rebuild domain first | Before any implementation task |
| God controller recreated | `GameLoopController` centralizes many responsibilities | Hard to test, hard to extend, high regression risk | High | Split review, outcome, economy, turn flow, and UI presenters | M1-M4 |
| Data key drift | Reference uses string keys and has typos such as `insuracne.submit` | Rules silently fail or UI shows wrong data | High | Stable IDs, validators, generated constants if needed | M2 |
| Accident logic mismatch | Reference uses probability; design requires deterministic accident flag | Core game emotion and balance break | High | Implement deterministic `OutcomeResolver` | M3-M7 |
| Design source unavailable locally | `docs/design` does not exist and Excel files were not accessed | Missing exact schema and values | Medium | Record Notion sources; add open questions for Excel access | Immediately |
| Notion/Excel divergence | Notion summaries may not match final spreadsheets | Wrong content model | Medium | Validate against final Excel exports before implementation | M2 |
| Cargo/mixed insurance underestimated | Reference mostly covers ship insurance | Later rewrite of document/rule model | Medium | Include generic document bundle and field model from M1 | M1-M3 |
| UI scene/prefab fragility | Reference has large experimental UI scenes | Copying scenes causes hidden broken references | High | Build minimal new scene; use reference only visually | M5 |
| ScriptableObjects used as runtime state | Reference serializes mutable case objects | Save/load bugs and editor asset mutation | Medium | Runtime state uses POCOs and stable IDs | M1-M2 |
| Singleton/global context spread | Reference has `NewspaperRuntimeContext.Current` and scene searches | Hidden dependencies and order bugs | Medium | Use composition root and explicit dependencies | M4-M5 |
| Rule engine over-generalized | Desire to support every future rule at once | Slow implementation and hard debugging | Medium | Start with known operators and add when evidence appears | M3 |
| Economy formula uncertainty | Design references `05_Economy.xlsx`, not accessible locally | Settlement behavior may need rework | Medium | Isolate formulas in `EconomyService` and keep parameters data-driven | M7 |
| Conditional approval ambiguity | Design mentions pricing/consideration but exact action semantics unclear | UI/action flow may change | Medium | Keep decision enum minimal and document assumption | M3-M6 |
| Special contractor branching complexity | 15 contractors and 45 after-stories | Narrative event bugs and missed feedback | Medium | Build event scheduler with tests before content volume | M7-M8 |
| Schedule pressure | Full 24-turn design is larger than reference prototype | Incomplete core loop or brittle shortcuts | Medium | Use milestones; ship first vertical ship bundle loop | Each milestone review |
| Codex automation overreach | Large repo and tempting ref_folder import | Accidental code movement or prefab edits | Medium | Restrict prompts to one milestone and explicit non-goals | Every Codex task |
| Refactoring without tests | Unity scene testing alone is slow | Behavior regressions | High | Pure C# tests for rules/outcomes/economy | M1 onward |
| Localization workflow unclear | Reference uses Unity Localization; design tables may include text | Duplicate text or broken keys | Medium | Decide import/localization strategy before content import | M2 |
| Save/load postponed too far | Runtime state may later be hard to serialize | Rework of state model | Low | Use stable IDs and snapshot-friendly state from start | M1, M9 |
| Imported asset licensing/style risk | Reference contains third-party UI asset folders | Legal or visual inconsistency risk | Low | Human review before reuse | Before art/UI production |
