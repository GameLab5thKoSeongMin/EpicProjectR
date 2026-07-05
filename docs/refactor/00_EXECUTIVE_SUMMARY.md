# Refactor Executive Summary

## Scope

This analysis is documentation-only. No production code, Unity scenes, prefabs, reference files, or gameplay assets were modified.

## Intended Game

The current design direction is a maritime insurance underwriting decision game set in the fictional port city of Rissebra. The player reviews contracts, compares documents against underwriting rules, makes approval or rejection decisions, and later faces financial and narrative consequences. The main Notion design source describes a 24-turn main game from 1599-01-15 to 1600-12-15, with 7 contracts per turn for 168 total contracts, 15 special contractors appearing 37 times, deterministic accidents when `AccidentFlag = 1`, news-driven rule changes, economy pressure, and after-story feedback.

Primary design sources accessed:

- Notion: `Epic` page, https://app.notion.com/p/39307d58984f809d8090c1b0c2933288
- Notion: `Rissebra's Underwriter - main 24-turn plan`, https://app.notion.com/p/6db07d58984f822fa8d401075c05b909
- Notion: `Ship insurance vertical slice`, https://app.notion.com/p/36e07d58984f83f3b31381ea558c6318
- Notion: `Document bundle`, https://app.notion.com/p/b2407d58984f83eea0de01b9e372a036
- Notion: `Result notification system`, https://app.notion.com/p/9f407d58984f82d49f6c812f251bc2e7

No exported local design docs were found under `docs/design/`; the folder did not exist during analysis.

## Current Active Project State

The active Unity project is a clean production base, not a gameplay implementation. It contains a single SampleScene, URP settings, tutorial/readme assets, and only two tutorial scripts.

Evidence:

- `EpicProjectR/ProjectSettings/EditorBuildSettings.asset`
- `EpicProjectR/Assets/Scenes/SampleScene.unity`
- `EpicProjectR/Assets/TutorialInfo/Scripts/Readme.cs`
- `EpicProjectR/Packages/manifest.json`

This is good for a refactor because there is little production surface to preserve, but it means all actual gameplay architecture still needs to be built.

## Reference Project State

`ref_folder/` is a mature prototype/reference Unity project. It contains 123 C# scripts, 71 prefabs, 38 ScriptableObject assets under `Assets/SO`, multiple large UI scenes, localization, Addressables, Timeline, and many UI-specific MonoBehaviours.

Important reference systems:

- ScriptableObject case definitions: `ref_folder/Assets/Scripts/Core/Definitions/MarineInsuranceCaseDefinition.cs`
- Fixed/random case generation: `ref_folder/Assets/Scripts/Core/Definitions/FixedMarineInsuranceCaseDefinition.cs`, `ref_folder/Assets/Scripts/Core/Definitions/RandomMarineInsuranceCaseDefinition.cs`
- Case data object: `ref_folder/Assets/Scripts/Core/Documents/UnderwritingCase.cs`
- Rejection rules and conditions: `ref_folder/Assets/Scripts/Core/Definitions/MarineInsuranceRejectionReasonDefinition.cs`
- Rule evaluator: `ref_folder/Assets/Scripts/Runtime/Evaluation/MarineInsuranceRejectionEvaluation.cs`
- Runtime flow controller: `ref_folder/Assets/Scripts/Runtime/GameLoopController.cs`
- Document UI binding: `ref_folder/Assets/Scripts/Runtime/UI/UnderwritingCaseApplicationView.cs`
- Approved contract tracker: `ref_folder/Assets/Scripts/Runtime/ApprovedContractTracker.cs`
- Newspaper prototype: `ref_folder/Assets/Scripts/Runtime/Newspaper/NewspaperGenerator.cs`

## Match Against Current Design

The reference project matches the current design in broad concept: insurance applications, document bundles, rejection reasons, consideration reasons, checklist interaction, approval/rejection decisions, monthly settlement, approved contract tracking, newspaper effects, contractor dialogue, and editor tools.

It does not match the current design closely enough to copy directly. The reference implementation appears to target a smaller ship-insurance prototype or vertical slice. The current design requires a deterministic 24-turn content-driven structure, cargo and mixed insurance phases, news-based rule activation, special contractor arcs, after-story routing, economy separation, and Excel/table-driven content. The reference project still uses probabilistic accidents and a large scene-driven controller.

## Biggest Architectural Risks

- `GameLoopController` combines turn flow, visitor spawning, dialogue transitions, UI binding, economy, accident rolling, settlement, and newspaper refresh in one MonoBehaviour.
- `UnderwritingCase` stores runtime state, Unity object references, data keys, sprites, dialogue profiles, and mutable case data in one serializable object.
- Rules are stored as ScriptableObject conditions, but active rule scheduling, news activation, and deterministic content table joins are not modeled as first-class services.
- Several systems rely on scene discovery or static context, for example `Object.FindFirstObjectByType` in `GameLoopController` and `ApprovedContractTracker`, and `NewspaperRuntimeContext.Current`.
- Data keys are stringly typed and inconsistent. Examples include `insuracne.submit` in `ref_folder/Assets/Prefabs/ShipInsuranceDocument.prefab` and case assets, plus mixed key families such as `ship.name`, `insurance.ship.name`, and `registration.ship.name`.
- `ref_folder/Assets/Scripts/README_사용법.md` describes an older fantasy adventurer/countermeasure underwriting system whose referenced classes are not present. That document should be treated as obsolete concept history.

## Biggest Design and Implementation Gaps

- The active project has no domain model, rule engine, content loading, turn loop, UI presenters, or test harness yet.
- The reference project has ship-insurance concepts but not the full 24-turn main game structure.
- Cargo insurance, mixed contract bundles, temporary quarantine certificate rules, news-driven rule activation, after-story event queueing, and the two-track economy are missing or only partially represented.
- Deterministic accident logic from design docs conflicts with `Random.value`-based accident rolling in `GameLoopController`.
- The design calls for data-table joins by `ContractID`, while the reference project stores case data directly in ScriptableObjects and prefabs.

## Recommended Refactoring Direction

Build the new project around pure C# domain logic plus Unity-facing composition and presenters:

- ScriptableObjects for authorable definitions only: rule definitions, document definitions, contract definitions, news definitions, contractor profiles, after-story definitions.
- Plain C# models for runtime cases, decisions, review results, contracts, turns, economy results, and after-story events.
- A deterministic underwriting policy/rule engine that returns immutable review results.
- A repository/import layer that maps design table IDs into validated definitions.
- UI presenters that bind view models to Unity views without owning rules or persistence.
- A single scene composition root/bootstrap that wires services explicitly, avoiding global Singleton-heavy architecture.

## Recommended Next Step

Start with milestone M0, then M1:

- M0: Add project rules, documentation references, and baseline validation for the new production structure.
- M1: Implement the pure C# domain model foundation and first unit tests without touching gameplay scenes.

This order keeps future Codex tasks small, testable, and less likely to accidentally import prototype coupling from `ref_folder/`.
