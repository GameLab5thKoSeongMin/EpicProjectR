# Reference Project Audit

## Scope

This audit treats `ref_folder/` as reference-only. No files were copied from it, and no production code was modified.

## Folder Structure Overview

`ref_folder/` is a complete Unity project with production-like prototype content:

| Area | Evidence | Notes |
| --- | --- | --- |
| Scenes | `ref_folder/Assets/Scenes/Title.unity`, `Main.unity`, `End.unity`, `NewUI.unity`, `NewUI2.unity`, `NewUIScene.unity`, `SampleScene.unity` | Build settings enable Title, Main, End. Several large UI scenes appear experimental. |
| Runtime scripts | `ref_folder/Assets/Scripts/Runtime/` | Game loop, UI, dialogue, tutorial, newspaper, timeline, approved contracts. |
| Core definitions | `ref_folder/Assets/Scripts/Core/Definitions/` | ScriptableObject definitions for insurance type, cases, rejection rules, localization helpers. |
| Core documents | `ref_folder/Assets/Scripts/Core/Documents/` | `UnderwritingCase`, document data entries, text/sprite sources, adventurer document remnants. |
| Editor tools | `ref_folder/Assets/Scripts/Editor/` | Case goal editors, node editor, rule drawers, localization generator. |
| Prefabs | `ref_folder/Assets/Prefabs/`, `ref_folder/Assets/Prefabs 1/`, `ref_folder/Assets/Prefabs_KSM/` | Document, book, map, monthly, rejection, end, conversation prefabs. |
| ScriptableObjects | `ref_folder/Assets/SO/` | Case assets and rejection reason assets. |
| Localization | `ref_folder/Assets/Localization/` | Marine names, document text, dialogue tables, Korean and English locales. |
| Third-party/UI asset folders | `ref_folder/Assets/Assets/FantasyGrimoireUI/`, `ref_folder/Assets/Assets/IndigoLay/` | Imported UI packs and demo scenes. |

Counts observed during analysis:

- 123 C# scripts under `ref_folder/Assets/Scripts`.
- 71 prefabs under `ref_folder/Assets`.
- 38 `.asset` files under `ref_folder/Assets/SO`.

## Important Scenes

- `ref_folder/ProjectSettings/EditorBuildSettings.asset` enables `Assets/Scenes/Title.unity`, `Assets/Scenes/Main.unity`, and `Assets/Scenes/End.unity`.
- `ref_folder/Assets/Scenes/NewUI.unity`, `NewUI2.unity`, `NewUIScene.unity`, and `SampleScene.unity` are large UI-heavy scenes but are not in build settings. Classify them as `Needs Human Review` before using them as layout references.
- `ref_folder/Assets/_Recovery/` contains recovery scenes. These should be `Do Not Reuse`.

## Important Scripts and Systems

| System | Classification | Evidence | Notes |
| --- | --- | --- | --- |
| ScriptableObject definition base | Preserve Concept | `ref_folder/Assets/Scripts/Core/Definitions/DefinitionBase.cs` | ID/display/icon/description shape is useful, but new IDs should be validated. |
| Insurance type definition | Refactor Before Reuse | `ref_folder/Assets/Scripts/Core/Definitions/MarineInsuranceTypeDefinition.cs` | Holds document prefabs and rejection reasons. Useful idea, but it mixes domain type with UI prefab references. |
| Case definitions | Refactor Before Reuse | `MarineInsuranceCaseDefinition.cs`, `FixedMarineInsuranceCaseDefinition.cs`, `RandomMarineInsuranceCaseDefinition.cs` | Fixed cases are conceptually useful. Random generation is less aligned with table-driven 168 contracts. |
| Runtime case data | Refactor Before Reuse | `ref_folder/Assets/Scripts/Core/Documents/UnderwritingCase.cs` | Useful as a prototype case container; too mutable and Unity-coupled for the new domain model. |
| Rejection rules | Preserve Concept | `ref_folder/Assets/Scripts/Core/Definitions/MarineInsuranceRejectionReasonDefinition.cs` | The absolute/consider distinction and condition operators are valuable. Move evaluation into pure C# policies. |
| Rule evaluator | Preserve Concept | `ref_folder/Assets/Scripts/Runtime/Evaluation/MarineInsuranceRejectionEvaluation.cs` | Deterministic rule list evaluation is a good base idea. Needs active-turn/news support. |
| Game loop | Refactor Before Reuse | `ref_folder/Assets/Scripts/Runtime/GameLoopController.cs` | Contains many needed ideas but is too broad and scene-coupled. |
| Document UI spawning | Refactor Before Reuse | `ref_folder/Assets/Scripts/Runtime/UI/UnderwritingCaseApplicationView.cs` | Good concept: document definitions drive visible document views. Must be presenter-driven in new project. |
| Rejection checklist UI | Preserve Concept | `MarineInsuranceRejectionReasonListView.cs`, `MarineInsuranceRejectionReasonItemView.cs` | Player checked reason IDs are useful. Keep as UI concept, not as source of truth. |
| Monthly settlement | Refactor Before Reuse | `MonthlySettlementView.cs`, `MonthlyEvaluationReportView.cs`, `GameLoopController.cs` | Good feedback loop but current economy differs from design docs. |
| Approved contract map | Preserve Concept | `ApprovedContractTracker.cs`, `ApprovedContractRecord.cs`, `ApprovedContractMapView.cs` | Matches design's contract management map concept. Needs deterministic outcome dates and accident states. |
| Newspaper | Refactor Before Reuse | `NewspaperGenerator.cs`, `NewspaperRuntimeContext.cs`, `NewspaperRuntimeState.cs` | Useful for news/rule effects, but current generator is random and global-context based. |
| Dialogue system | Refactor Before Reuse | `DialogueManager.cs`, `ContractorDialogueProfile.cs`, `DialogueSequenceBuilder.cs` | Useful presentation layer. Needs data-table integration and event routing. |
| Tutorial/timeline | Needs Human Review | `Runtime/Tutorial/`, `Runtime/TimelineSequence/` | Could be useful later, but not central to underwriting rules. |
| Old README countermeasure system | Do Not Reuse | `ref_folder/Assets/Scripts/README_사용법.md` | Describes fantasy monster/location/adventurer classes that are not present. Treat as obsolete history. |

## Important Prefabs, ScriptableObjects, Editor Tools, and Test Utilities

### Prefabs

- Document prefabs: `ref_folder/Assets/Prefabs/ShipInsuranceDocument.prefab`, `ShipRegistDocument.prefab`, `HullInspectionDocument.prefab`, `RouteDocument.prefab`.
- Result/contract prefabs: `ref_folder/Assets/Prefabs/ContractListPrefab.prefab`, `ref_folder/Assets/Prefabs/MonthlyView.prefab`, `ref_folder/Assets/Prefabs/EndDocument/`.
- Checklist/rejection prefabs: `ref_folder/Assets/Prefabs/RejectPrefab.prefab`, `ref_folder/Assets/Prefabs/RejectAnswer.prefab`.
- Old/archive prefabs: `ref_folder/Assets/Prefabs/Old/` should be treated as `Do Not Reuse` until a human confirms otherwise.

### ScriptableObjects and Data Assets

- Case assets live under `ref_folder/Assets/SO/Case/`, for example `ref_folder/Assets/SO/Case/1-1/1-1Case.asset`.
- Rejection definitions live under `ref_folder/Assets/SO/Reject/Reject/` and `ref_folder/Assets/SO/Reject/ConsiderReject/`.
- Localization assets live under `ref_folder/Assets/Localization/`.
- Timeline/tutorial assets live under `ref_folder/Assets/SO/Case/` and `ref_folder/Assets/Timeline/`.

### Editor Tools

- Rule/case node editor: `ref_folder/Assets/Scripts/Editor/FixedMarineInsuranceCaseNodeEditorWindow.cs`.
- Rule template drawer/editor: `ref_folder/Assets/Scripts/Editor/MarineInsuranceRejectionReasonDefinitionEditor.cs`, `MarineInsuranceRejectionConditionDrawer.cs`.
- Localization generator: `ref_folder/Assets/Scripts/Editor/MarineInsuranceLocalizationAssetGenerator.cs`.
- Case goal helpers: `ref_folder/Assets/Scripts/Editor/MarineInsuranceCaseGoalEditorUtility.cs`.

### Test Utilities

No current formal test runner was found in `ref_folder/Assets/Scripts` for the marine insurance implementation. `ref_folder/Assets/Scripts/README_사용법.md` references an `UnderwritingTestRunner`, but the referenced class names appear only in that README and not in the current script tree. Classify this as `Do Not Reuse` for implementation planning and `Needs Human Review` if the team has an external or deleted test package.

## Main Gameplay Runtime Flow Found

The old runtime flow is centered on `GameLoopController`:

1. `BeginMonth()` increments turn and sets the visitor count.
2. `RingBell()` opens the next visitor or transitions to settlement.
3. `OpenNextVisitor()` creates a case from `MarineInsuranceCaseDefinition`.
4. `MarineInsuranceRejectionEvaluator.Evaluate()` evaluates the current case.
5. UI views bind the case: application documents, rejection checklist, ship panel, dialogue.
6. Player approves or rejects through controller methods.
7. `ResolveCurrentVisitor()` creates a `MonthlySettlementRecord`, rolls accident, applies performance delta, emits resolved events.
8. End-of-month settlement aggregates premium, deductions, performance share, and month-end contract documents.

Evidence:

- `ref_folder/Assets/Scripts/Runtime/GameLoopController.cs`
- `ref_folder/Assets/Scripts/Runtime/Evaluation/MarineInsuranceRejectionEvaluation.cs`
- `ref_folder/Assets/Scripts/Runtime/UI/MonthlySettlementView.cs`

Classification: `Refactor Before Reuse`.

## Main UI Flow Found

The UI flow is prefab and MonoBehaviour driven:

- `UnderwritingCaseApplicationView` instantiates document prefabs from the insurance type and binds them if submitted.
- Document view components read strings from `UnderwritingCase.GetText(key)`.
- `MarineInsuranceRejectionReasonListView` instantiates checklist items for absolute and consideration reasons.
- `GameLoopController` updates approval label, premium display, workbench movement, dialogue state, and settlement state.
- `ApprovedContractTracker` feeds map/list/board views through `ContractsChanged`.

Evidence:

- `ref_folder/Assets/Scripts/Runtime/UI/UnderwritingCaseApplicationView.cs`
- `ref_folder/Assets/Scripts/Runtime/UI/ShipInsuranceApplicationDocumentView.cs`
- `ref_folder/Assets/Scripts/Runtime/UI/MarineInsuranceRejectionReasonListView.cs`
- `ref_folder/Assets/Scripts/Runtime/UI/ApprovedContractMapView.cs`

Classification: `Refactor Before Reuse`.

## Main Data Flow Found

The old project stores most contract data in `UnderwritingCase.data`, a list of key/value/code entries. Rules inspect those string keys; document views also read those keys. ScriptableObject case assets serialize concrete key/value data, while random cases generate values.

Evidence:

- `ref_folder/Assets/Scripts/Core/Documents/UnderwritingCase.cs`
- `ref_folder/Assets/Scripts/Core/Documents/CaseDataEntry.cs`
- `ref_folder/Assets/Scripts/Core/Documents/DocumentTextSource.cs`
- `ref_folder/Assets/SO/Case/1-1/1-1Case.asset`

Classification: `Refactor Before Reuse`.

## Systems Worth Preserving Conceptually

- Deterministic ordered rule evaluation over explicit rejection reason definitions.
- Separate absolute rejection and rejection consideration categories.
- Player checklist comparison against triggered reasons.
- ScriptableObject authoring for content definitions.
- Document prefab/view concept tied to document type.
- Approved contract tracker and map/list feedback.
- Newspaper/news as a rule modifier concept.
- Editor tools that help connect document fields and rule conditions.
- Dialogue template formatting with case tokens.

## Systems That Should Not Be Copied Directly

- `GameLoopController` as a central owner of rules, state, UI, dialogue, settlement, and progression.
- `UnderwritingCase` as a mutable Unity-coupled runtime and content object.
- `RandomMarineInsuranceCaseDefinition` as the primary content source for the main game.
- `NewspaperRuntimeContext.Current` and similar global scene context patterns.
- Scene `FindFirstObjectByType` dependencies as composition strategy.
- Old fantasy/adventurer countermeasure README workflow.
- Recovery scenes and imported demo scenes.

## Code Smells and Tight Coupling

- `GameLoopController` has too many responsibilities and owns too many serialized dependencies.
- `CalculateMoneyEarned()` returns `0`, implying incomplete or abandoned economy logic in the reference flow.
- Accident occurrence is probabilistic in `RollAccidentOccurrence()` using `Random.value`; the design docs require deterministic accidents from `AccidentFlag`.
- `UnderwritingCase` directly references Unity `Sprite`, `GameObject`, `MarineInsuranceTypeDefinition`, `ContractorDialogueProfile`, and mutable lists.
- UI views read raw string keys from case data, so schema errors appear at runtime.
- Data keys are inconsistent: `ShipInsuranceDocument.prefab` uses `insuracne.submit`; assets use `insurance.ship.name`, `registration.ship.name`, `hull.ship.name`; random generation also writes `ship.name`.
- Several components locate dependencies through scene search.
- `NewspaperRuntimeContext.Current` is a global state holder.

## Prototype-only or Deprecated-looking Logic

- `ref_folder/Assets/Scripts/README_사용법.md` references classes such as `RiskTagDefinition`, `MonsterRiskDefinition`, and `ManualCheckAuditor`; only the README references them.
- `ref_folder/Assets/_Recovery/` contains recovered scenes, not production structure.
- `ref_folder/Assets/Prefabs/Old/` and `ref_folder/Assets/Images/Old/` should be treated as archival.
- `NewUI`, `NewUI2`, and `NewUIScene` are not in build settings and may represent layout experiments.
- `RandomMarineInsuranceCaseDefinition` uses generated modern/Korean fallback content and `DateTime.Today`, which does not match the historical fixed schedule.

## Known Risks If Copied Into the New Project

- The new project would inherit scene coupling before the domain model is stable.
- Future table imports would need to fight against handwritten serialized case values.
- Rule activation by turn/news would be difficult to add cleanly.
- Save/load would serialize Unity object references instead of stable IDs.
- Designers could accidentally create invalid key strings without editor validation.
- Testing would require Unity scene setup instead of fast pure C# unit tests.
