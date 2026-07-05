# M8.8-E Presentation Code Quality Report

Date: 2026-07-06

Status: Completed. Continue to M8.8-F because no hard stop condition was reached.

## 1. Before / After Responsibility Review

| Component | Before M8.8-D | After M8.8-D/E |
| --- | --- | --- |
| `FirstPlayableBootstrap` | Composition root only. | Unchanged; still composes assets, theme, factory, view, session, and presenter. |
| `FirstPlayablePresenter` | State flow and session coordination. | Unchanged; no gameplay or UI creation moved into presenter. |
| `FirstPlayableView` | Passive runtime uGUI rendering using a three-column generated layout. | Passive runtime uGUI rendering using Main-like layered background, paper stack, workbench, shelf, and decision paper. |
| `FirstPlayableUiFactory` | Shared panel/text/scroll/button creation. | Added anchored panel creation and Main-style button sprite selection. Still owns UI object creation details. |
| `FirstPlayableUiTheme` | Color/font/button/toggle styling. | Updated palette toward Main scene: warm paper, dark overlay, shelf colors, tab button tones. |
| `FirstPlayableAssetCatalog` | Build-safe Resources loader for M8.6/M8.7 assets. | Added centralized MainSceneUI resource paths and optional sprite/font fields. |
| `FirstPlayableScreenState` | Data-only screen states with minimal guards. | Added constructor null guards while remaining data-only. |
| `FirstPlayableKoreanText` | Korean text catalog. | Unchanged; existing labels remain centralized and source IDs remain exact. |

## 2. Data Structure Choices

- `Dictionary<RuleId, Toggle>` and `Dictionary<RuleId, Image>` remain in the view for rule row interaction and selected-state refresh.
- `IReadOnlyList<DocumentRecord>` and `IReadOnlyList<FirstPlayableRuleRowState>` remain the view-state collections for deterministic render order.
- Asset catalog fields are explicit properties for each optional sprite/font rather than ad hoc resource string lookups in the view.
- Screen state constructors convert incoming enumerables to read-only lists to stabilize render data.

## 3. Algorithm / Flow Choices

- Presenter flow remains:
  - `ShowCurrentCase`
  - `SubmitDecision`
  - `ShowResult`
  - `AdvanceAfterResult`
  - `ShowCompleted`
- No auto-advance was added.
- View rendering still clears and rebuilds dynamic document/rule rows during review render.
- Button listener wiring remains one-time during view build, avoiding duplicated submit/next listeners on re-render.
- Runtime asset loading still uses `Resources.Load<T>()` with generated color/default font fallback.

## 4. Design Patterns Used

- Composition Root: `FirstPlayableBootstrap`.
- MVP / Passive View: `FirstPlayablePresenter` and `FirstPlayableView`.
- Factory: `FirstPlayableUiFactory`.
- Asset Catalog / Resource Provider: `FirstPlayableAssetCatalog`.
- Theme Object: `FirstPlayableUiTheme`.
- DTO / ViewModel: `FirstPlayableScreenState` classes.

## 5. Code Quality Improvements Made

- Added MainSceneUI resource path centralization in `FirstPlayableAssetCatalog`.
- Added optional asset properties for Main background, paper, letter, and tab button sprites.
- Added `CreateAnchoredPanel` to `FirstPlayableUiFactory` so Main-like layout can be authored without scattering RectTransform boilerplate in every call.
- Added button sprite selection in `FirstPlayableUiFactory`, keeping button asset choice out of the view.
- Updated `FirstPlayableUiTheme` with a Main-like palette while keeping selectable transition logic centralized.
- Added null guards to all screen state constructors and view render entry points.
- Kept existing Korean text catalog usage; no source IDs were translated.

## 6. Remaining Technical Debt

- `FirstPlayableView` is still verbose because runtime-generated uGUI has to express an authored Main-like layout in code.
- The implementation emulates Main's object layout; it does not reproduce drag/drop, dialogue, timeline, or old toggle-mover behavior.
- TMP SDF visual fidelity is approximated through uGUI font files, not an actual TMP migration.
- `.aseprite` Main scene sprites were not copied in this pass, so some shelf/outline detail is approximated.
- Unity visual QA is still needed to tune exact positions and text overflow at target resolutions.

## 7. Intentionally Not Changed

- Domain, Application, and Content behavior.
- `FirstPlayablePresenter` flow logic.
- Source IDs.
- Hidden accident fixture data handling.
- Active scenes, prefabs, ScriptableObjects, Packages, ProjectSettings, and `ref_folder`.
- Old Main scene scripts, prefabs, timelines, and ScriptableObject case content.
