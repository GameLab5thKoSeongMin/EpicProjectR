# M8.6 Architecture And Asset Audit

Status: baseline audit created before asset migration. Final scores updated after implementation.

## Scope

Confirmed: M8.6 is a controlled asset migration and architecture hardening pass for the first playable Unity project. Production code changes are limited to the active Unity project under `EpicProjectR/Assets/Scripts/`. Reference assets may be inspected from `ref_folder/`, but copied assets must be selected, documented, and kept out of old code, settings, prefabs, scenes, and package configuration.

Confirmed preservation targets:

- `C001`: approve with no checked rules.
- `C002`: check `AR01` and reject.
- `C003`: check `CR01` and `CR02`, approve with 150% premium quote, and resolve deterministic accident.
- Rejected flagged contracts do not create regular accidents.
- Hidden `AccidentFlag` is not shown in normal Presentation UI.
- Source IDs remain exact strings.

## Baseline Implementation Findings

Confirmed:

- `FirstPlayableBootstrap` currently owns composition, runtime UI creation, view references, rendering, input handling, result acknowledgement, and UI helper methods.
- `FirstPlayableUiTheme` currently uses `UnityEditor.AssetDatabase.LoadAssetAtPath` in Editor and OS dynamic fonts as fallback outside Editor.
- Domain and Application code are pure C# and do not depend on Unity UI or scene assets.
- `DecisionAuditService` already uses `HashSet<RuleId>` for comparison.
- `RuleReviewService` evaluates rules deterministically in repository order.
- `InMemoryContractRepository.Get` currently scans a list by ID, which is acceptable for fixtures but weak for 168-contract scale.
- Existing UI is runtime generated and therefore easy to validate structurally, but it has limited use of reference visual assets.

## Baseline Scores

| Category | Current | Target | Reason | Planned improvement | Final | Remaining gap |
| --- | ---: | ---: | --- | --- | ---: | --- |
| Asset migration readiness | 35 | 80 | Reference assets are present, but most are unclassified and many carry old-project dependency risk. | Create explicit migration plan, copy only selected `.ttf` and `.png` assets, and create manifest. | 82 | Manual Unity import settings and visual QA remain pending. |
| Build runtime safety | 45 | 85 | Presentation font loading depends on Editor-only `AssetDatabase` path behavior. | Replace runtime asset loading with a Resources-backed asset catalog. | 88 | Unity build was not run because Unity CLI is unavailable. |
| Existing UI style match | 68 | 80 | M8.5 typography and palette improved the workdesk, but most reference visual treatment is absent. | Use safe imported font, paper, panel, button, decoration, and ship sprite assets. | 78 | Manual visual QA is deferred; imported sprites may need Unity import tuning. |
| Presentation responsibility separation | 30 | 80 | Bootstrap is still a God Object. | Split bootstrap, presenter, passive view, UI factory, theme, and asset catalog. | 82 | View is still runtime-generated and can later move to authored prefab/scene. |
| Domain/Application separation | 90 | 95 | Domain/Application are already pure; Presentation calls into session cleanly. | Keep Unity-only changes in Presentation and make a small repository data-structure improvement. | 94 | Unity EditMode runner not executed in this environment. |
| Data structure scalability | 55 | 75 | Fixture repository scans by ID; asset paths are embedded in theme. | Add dictionary-backed contract lookup and explicit asset catalog fields. | 76 | Rule repository filtering is still computed by ordered scan for now. |
| Algorithm determinism and extensibility | 78 | 85 | Ordered rule evaluation is deterministic, but condition handling is still switch-based. | Preserve ordered review and document strategy/specification as future path without changing behavior. | 84 | Rule conditions are not yet full Strategy/Specification objects. |
| Design pattern fitness | 45 | 80 | Composition root, MVP, factory, and asset catalog boundaries are not yet separated. | Introduce light Composition Root, Passive View, Presenter, Factory, ViewModel, and Asset Catalog. | 82 | No authored prefab/presenter integration tests yet. |
| Testability | 70 | 85 | Pure services are testable, but Presentation logic is tangled with UI construction. | Move screen state and result formatting into presenter/view model methods where possible. | 82 | Unity UI rendering still needs editor/playmode QA. |
| Source ID preservation | 100 | 100 | Existing typed IDs preserve exact strings. | Do not modify source IDs or normalize display values. | 100 | No remaining gap. |
| Full game extensibility toward 24 turns / 168 contracts | 50 | 75 | Fixture loop works, but presentation and lookup shape do not yet scale cleanly. | Separate presentation responsibilities and use ID dictionary lookup. | 76 | Full content import, pagination, filters, and 24-turn flow remain future work. |
| Maintainability | 45 | 80 | Single large Presentation file is hard to extend safely. | Create small focused Presentation classes with responsibility comments. | 81 | Runtime uGUI construction is still verbose. |

## Active Asset State Before Copying

Confirmed:

- Existing active imported fonts are under `EpicProjectR/Assets/Fonts/ImportedFromReference/`.
- No new M8.6 runtime asset folder exists yet.
- M8.6 will use `EpicProjectR/Assets/Resources/FirstPlayable/ImportedReference/RuntimeSafe/` for assets that must load in builds through `Resources.Load<T>()`.

## Risk Notes

- Inferred: old prefabs, ScriptableObjects, animation controllers, materials, Addressables settings, render pipeline settings, and scenes may carry references that are unsafe in the active project.
- Assumed: Unity will generate new `.meta` files for copied runtime-safe assets during the next editor import.
- Unresolved: final sprite slicing/nine-slice import settings cannot be verified without Unity import metadata. Runtime-created sprites will be used as simple sprites in this pass.

## Final Implementation Summary

Completed:

- Added `FirstPlayableAssetCatalog` with explicit Resources paths for selected fonts and sprites.
- Removed `UnityEditor` and `AssetDatabase` from Presentation runtime code.
- Split `FirstPlayableBootstrap` into a composition root plus `FirstPlayablePresenter`, `FirstPlayableView`, `FirstPlayableUiFactory`, `FirstPlayableUiTheme`, and data-only screen state classes.
- Copied nine runtime-safe reference assets: two `.ttf` fonts and seven `.png` sprites.
- Changed `InMemoryContractRepository` from list scan lookup to dictionary-backed ID lookup while preserving ordered `GetAll()`.
- Preserved C001/C002/C003 behavior in pure C# smoke validation.
