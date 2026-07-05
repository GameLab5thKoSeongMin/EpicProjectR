# Reference vs Design Gap Analysis

| Area | Reference Project State | Design Document Direction | Gap | Recommendation | Priority |
| --- | --- | --- | --- | --- | --- |
| Active project baseline | Active project only has SampleScene, URP settings, tutorial assets, and two tutorial scripts. Evidence: `EpicProjectR/Assets/Scenes/SampleScene.unity`, `EpicProjectR/Assets/TutorialInfo/Scripts/Readme.cs`. | New production project should host the refactored underwriting game. | No gameplay architecture exists yet. | Build new architecture from scratch in active project; use `ref_folder` only for reference. | Critical |
| Core underwriting concept | Reference has insurance cases, document views, rejection rules, approve/reject flow. Evidence: `MarineInsuranceCaseDefinition.cs`, `UnderwritingCaseApplicationView.cs`, `MarineInsuranceRejectionReasonDefinition.cs`. | Maritime underwriting decision game. | Concept match is strong. | Preserve concept, redesign implementation around pure domain services. | Critical |
| Turn structure | `GameLoopController` increments `currentTurn` and visitor count each month. | 24 fixed turns, 7 contracts per turn, 168 contracts. | Reference supports generic monthly flow but not fixed scheduled content. | Build `TurnSchedule` and `ContractQueue` from content tables. | Critical |
| Case content source | Fixed SOs and random generator. Evidence: `FixedMarineInsuranceCaseDefinition.cs`, `RandomMarineInsuranceCaseDefinition.cs`. | `01_Contracts.xlsx` and joined tables are source of truth. | Random generation is not aligned with main game. | Use fixed/imported definitions; keep random generation only for tests or debug. | Critical |
| Accident logic | `RollAccidentOccurrence()` uses probability and `Random.value`. | `AccidentFlag = 1` means approved contracts always accident; no probability. | Direct conflict. | Rebuild as deterministic `OutcomeResolver`. | Critical |
| Rejection rules | SO-based absolute and consideration reasons with condition operators. | 18 AR, 12 CR, active by turn/news. | Operators are useful, but no robust activation schedule. | Preserve rule definitions concept; add `RuleActivationPolicy`. | High |
| Document bundles | Type definition stores document prefabs. Cases and prefabs use data keys. | Bundles 1-7 plus temporary quarantine certificate. | Reference has ship bundles only and no cargo/mixed model. | Build document bundle definitions by ID; support optional documents by active rules. | High |
| Document data schema | `UnderwritingCase.data` stores key/value/code entries. | Table columns with stable English code names. | Stringly typed keys are inconsistent and unvalidated. | Add schema validation and generated constants or typed field IDs. | Critical |
| Player checklist | `MarineInsuranceRejectionReasonListView` tracks checked reason IDs. | Player should identify objective reasons and get score feedback. | Good UI concept, but currently UI owns checked state. | Move checked IDs into `ReviewSubmission`; UI presenter binds it. | High |
| Consideration reasons | Checking consideration reasons increases premium in controller. | CR checks can affect premium tiers: 1 = 125%, 2 = 150%, 3+ reject recommended, per design. | Old rate is serialized per reason and simpler. | Implement CR pricing policy from data parameters. | High |
| Approval decisions | Enum includes Approved, ConditionalApproved, Rejected, InvestigationRequired. | Approve, conditional approval, reject are implied; investigation unclear. | Enum has more states than confirmed design. | Keep only confirmed actions for M1; leave `InvestigationRequired` as open question. | Medium |
| Economy | Monthly settlement uses performance share, deductions, accident payout/performance loss; `CalculateMoneyEarned()` returns 0. | Evaluation score and money are separate tracks; accident responsibility is fee refund plus fixed penalty. | Old economy is incomplete and conflicts with design. | Rebuild economy from `05_Economy` design. | High |
| News | Random article generator and global published effects. Evidence: `NewspaperGenerator.cs`, `NewspaperRuntimeContext.cs`. | 13 scheduled news rows activate/strengthen/deactivate rules. | Current random newspaper is not deterministic schedule. | Rebuild as scheduled `NewsTimelineService`; reuse UI concept only. | High |
| Special contractors | Dialogue profiles and keyed responses exist. | 15 special contractors, 37 appearances, arcs, after-stories. | Reference has dialogue tooling but not full special contractor table logic. | Build `SpecialContractorDefinition` and `AfterStoryEventDefinition`. | High |
| After-stories | Some month-end document prefabs and dialogue responses exist. | 45 after-story events, channels: board, letter, gift, inspection. | No clear event queue/channel service in reference. | Rebuild as `AfterStoryScheduler` with channel queueing. | High |
| Contract management map | `ApprovedContractTracker` and map/list views exist. | Approved contracts appear on route map; accidents turn red by accident date. | Good concept, but old tracker only advances turns and arrival status. | Refactor with route IDs, outcome dates, accident status. | Medium |
| Dialogue | Dialogue manager and profile templates exist. | 221 dialogue rows in `07_Dialogues.xlsx`. | Reference can present lines, but needs table import and branch keys. | Keep presentation concepts; rebuild data loading. | Medium |
| UI scene composition | Large scene/prefab prototype in `ref_folder`. | New production UI not specified. | Copying scenes risks importing old coupling. | Recreate minimal scene composition root and views in active project later. | High |
| Editor tools | Node editor and rule utilities exist. | Designers need validation for large tables. | Old tools target SO workflow, not Excel/table import. | Preserve validation/editor-tool idea; rebuild validators for new schema. | Medium |
| Localization | Unity Localization assets exist. | Design has Korean content and possible export tables. | Localization approach not confirmed. | Decide early whether table importer outputs localization keys or direct strings. | Medium |
| Old fantasy underwriting README | README describes monster/location/adventurer countermeasure system. | Current Notion design is maritime insurance. | Obsolete terminology and missing classes. | Do not reuse except as historical note. | Low |
| Imported UI asset packs | FantasyGrimoireUI and PixelAdventureBookUI are present. | Visual direction not finalized in design docs. | License/style/use uncertain. | Needs human review before reuse. | Postpone |
| Save/load | Reference has runtime state but no clear save model. | Save/load readiness requested for architecture. | Stable IDs and serializable runtime snapshots missing. | Design domain state with save/load boundaries from the start. | Medium |

## Systems to Rebuild from Scratch

- Domain model for contracts, documents, rules, decisions, turns, economy, outcomes, and after-stories.
- Deterministic rule engine with active rule schedule.
- Content repository/import/validation layer.
- Outcome resolver using `AccidentFlag`.
- Economy result calculation from design parameters.
- New scene composition root and UI presenter layer.

## Systems Usable as Implementation Reference Only

- Document view spawning and binding.
- Rejection checklist item UI.
- Approved contract map/list/board views.
- Dialogue presentation flow.
- Newspaper visual concept.
- Editor node view idea for data/rule inspection.

## Systems That Can Be Postponed

- Full cargo/mixed insurance implementation after ship bundle foundation.
- Timeline/cinematic sequences.
- Gifts as persistent office decorations.
- Full save/load UI.
- Imported asset pack cleanup.
- Advanced debug visualizers beyond basic validators.
