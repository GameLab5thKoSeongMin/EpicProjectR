# M8.6 Architecture Refactor Report

Date: 2026-07-05

## Overall Result

Completed. The first playable Presentation layer no longer has the original God Object shape. Runtime composition, presenter state, view rendering, UI control creation, theme values, and asset loading now live in separate classes. The Domain/Application/Content behavior for C001, C002, and C003 is preserved.

## Before

- `FirstPlayableBootstrap` created the canvas, built all controls, held all UI references, evaluated view state, handled button events, submitted decisions, formatted results, and owned all UI helper methods.
- `FirstPlayableUiTheme` loaded fonts through Editor-only `AssetDatabase` when in the Unity Editor.
- Contract lookup used a list scan by `ContractId`.

## After

| Component | Responsibility |
| --- | --- |
| `FirstPlayableBootstrap` | Runtime entry point and composition root. |
| `FirstPlayableAssetCatalog` | Build-safe `Resources.Load<T>()` font and sprite provider. |
| `FirstPlayableUiTheme` | Colors, fonts, and selectable transition styling only. |
| `FirstPlayableUiFactory` | Shared uGUI control creation. |
| `FirstPlayableView` | Passive view with Unity UI references and render methods. |
| `FirstPlayablePresenter` | Session/view coordination, state transitions, and result text formatting. |
| `FirstPlayableScreenState` | Data-only DTOs for review, result, complete, and rule row states. |
| `InMemoryContractRepository` | Ordered contract list plus dictionary lookup by exact `ContractId`. |

## Data Structures

- `Dictionary<ContractId, ContractCase>` for scalable contract lookup while retaining ordered list iteration.
- `HashSet<RuleId>` in presenter to mark triggered active rules efficiently for checklist rendering.
- Existing `HashSet<RuleId>` in `DecisionAuditService` remains the decision audit comparison structure.
- Explicit asset catalog fields instead of scattered runtime strings.

## Algorithms

- Rule evaluation remains deterministic and ordered by repository order.
- Active rule filtering remains window-based and deterministic.
- Premium quoting remains CR-count based: 0 = 100%, 1 = 125%, 2+ = 150%, 3+ reject recommended.
- Outcome text is sanitized in Presentation so hidden accident fixture data is not displayed directly.

## Design Patterns

- Composition Root: `FirstPlayableBootstrap`.
- MVP/Passive View: `FirstPlayablePresenter` and `FirstPlayableView`.
- Factory: `FirstPlayableUiFactory`.
- Asset Catalog/Resource Provider: `FirstPlayableAssetCatalog`.
- DTO/ViewModel: `FirstPlayableScreenState` classes.
- Repository: existing content repositories, now with dictionary-backed contract access.

## Remaining Risks

- Presentation code still creates uGUI controls at runtime rather than using an authored prefab.
- Rule condition evaluation is still switch-based; future full-game content may justify Strategy/Specification condition classes.
- Unity UI compile/import validation still needs the Unity Editor or CI runner.
