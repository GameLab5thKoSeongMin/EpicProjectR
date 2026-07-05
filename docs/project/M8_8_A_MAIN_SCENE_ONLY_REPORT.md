# M8.8-A Main Scene Only Report

Date: 2026-07-06

Status: Completed as a read-only visual/layout audit. No active UI, code, scene, prefab, asset, package, or project-setting implementation was performed.

## 1. Main Scene Hierarchy Summary

Confirmed scene reference:

- `ref_folder/Assets/Scenes/Main.unity`

The Main scene has one primary screen-space Canvas with:

- `Canvas`
  - `WorldShiftTargets`
    - `BackgroundImage`
    - character/dialogue objects
    - clickable desk props such as `Paper`, `board`, `letter`, bell-like button, and crystal-ball-like image
  - `MapBoard`
    - large map/board image stack
    - right-side scroll list
  - top date/header strip
  - `WorkBench`
    - right `Shelf`
      - `SkillCoreView`
      - `CheckView`
      - `EssentialPanel`
      - `ConsideratePanel`
    - `WorkBenchDisplay`
      - ship display layer
      - central underwriting/application panel
      - bottom-right decision panel with `Accept` and `Reject`
  - `NewsPanel`
  - fade/full-screen overlay images

Canvas settings found:

- `RenderMode`: Screen Space Overlay.
- `CanvasScaler`: Scale With Screen Size.
- `ReferenceResolution`: `1920 x 1080`.
- `MatchWidthOrHeight`: `0`.
- `PixelPerfect`: enabled on the primary Canvas.
- Layout style: mostly authored absolute RectTransform positions and fixed sprite dimensions, not layout-group-driven responsive columns.

## 2. Main Scene UI Style Summary

Main scene style is object-first rather than dashboard-first:

- Full-screen illustrated background image, not a flat generated color.
- Desk/workbench composition with a central ship/underwriting display.
- Right-side shelf panel with vertical paper/reason cards.
- Bottom-right paper-like decision panel containing result-like metadata and large `Accept` / `Reject` buttons.
- Document prefabs are small draggable paper cards with title, field rows, separator lines, ship image, and seal.
- Buttons use sprite art and tab/icon/button sheets, with a compact physical-object feel.
- Text uses TextMeshPro font assets, mostly `KyoboHandwriting2022khn SDF.asset`, with `Galmuri11 SDF.asset` and related fallback/accent usage.
- The tone is warm paper, dark overlay, aged UI art, and pixel/handwritten Korean-capable typography, with small bright accent colors for markers/outlines.

Important visual pattern:

- The old Main scene does not resemble a three-column admin workdesk. It layers interactable props, a workbench, document papers, a shelf/checklist area, and a decision plaque over a painted background.

## 3. Main Scene Asset Dependency Summary

Direct prefab references from `Main.unity`:

| GUID | Reference path | Role |
| --- | --- | --- |
| `ec0fd208461b3ea42bc6b1746a9fa398` | `ref_folder/Assets/Prefabs/ShipInsuranceDocument.prefab` | Ship insurance application paper document. |
| `cfcd26aab1b0a5546bff9354ca891958` | `ref_folder/Assets/Prefabs/ShipRegistDocument.prefab` | Ship registration paper document. |
| `ffa17623a3cdda24a9590bf953a83eb2` | `ref_folder/Assets/Prefabs/HullInspectionDocument.prefab` | Hull inspection paper document. |
| `04214045b956f9a4e93f5c9c999b6d4d` | `ref_folder/Assets/Prefabs/RouteDocument.prefab` | Route declaration paper document. |
| `c4d2a6b88eb1ff146b08a24770bfdad1` | `ref_folder/Assets/Prefabs_KSM/ConversationPanel.prefab` | Conversation/log panel. |

Visible or style-relevant scene assets identified:

| Asset | Observed role |
| --- | --- |
| `ref_folder/Assets/Images/Main_Total.png` | Main full-screen background. |
| `ref_folder/Assets/Images/Old/paper.png` | Paper prop. |
| `ref_folder/Assets/Images/UI_Right_Middle_Paper.aseprite` | Essential/checklist paper panel. |
| `ref_folder/Assets/Images/UI_Right_Middle_Paper_revert.aseprite` | Consideration/checklist paper panel. |
| `ref_folder/Assets/Images/UI_Right_Middle_Line (1).aseprite` | Shelf/right-side line art. |
| `ref_folder/Assets/Images/UI_Right_Under.aseprite` | Right-side lower UI piece. |
| `ref_folder/Assets/Images/Title.aseprite` | Button/title-like UI sprite. |
| `ref_folder/Assets/Images/BoardPanel.aseprite` | Board/panel visual. |
| `ref_folder/Assets/Images/Paper texture 2.png` | Paper texture/image. |
| `ref_folder/Assets/Images/Ship.aseprite` and `ref_folder/Assets/Images/Ship/*` | Ship/workbench maritime display pieces. |
| `ref_folder/Assets/Images/Seal/Guild Stamp.aseprite` | Stamp/seal treatment for documents. |
| `ref_folder/Assets/Assets/IndigoLay/PixelAdventureBookUI/Sprites/UI/Tabs/UI_tab_Off.png` | Reject button/tab sprite. |
| `ref_folder/Assets/Assets/IndigoLay/PixelAdventureBookUI/Sprites/UI/Tabs/UI_tab_dim.png` | Accept button/tab sprite. |
| `ref_folder/Assets/Assets/IndigoLay/PixelAdventureBookUI/Sprites/UI/Buttons/UI_BtnIcon_x.png` | Small close/icon button treatment. |

Font assets observed through TMP font references:

- `ref_folder/Assets/TextMesh Pro/Resources/Fonts & Materials/KyoboHandwriting2022khn SDF.asset` appears most frequently.
- `ref_folder/Assets/Fonts/Galmuri11 SDF.asset` appears as another visible UI font.
- `ref_folder/Assets/TextMesh Pro/Resources/Fonts & Materials/Galmuri11-Bold SDF.asset` appears in Main/prefab references.
- `ref_folder/Assets/TextMesh Pro/Resources/Fonts & Materials/KyoboHandwriting2020pdy SDF.asset` appears in Main/prefab references.

Risk-bearing dependencies also exist in Main or direct prefabs:

- Old runtime scripts such as `UnderwritingCaseApplicationView`, `MarineInsuranceRejectionReasonListView`, document view scripts, `GameObjectToggle`, `RectTransformToggleMover`, `PanelSwitcher`, and dialogue/timeline scripts.
- ScriptableObject case data under `ref_folder/Assets/SO/Case/...`.
- Timeline/playable assets.
- Materials such as `Stencil.mat` and `StencilHidden.mat`.

These are evidence for visual structure only. They should not be copied as implementation code in M8.8-B/C unless explicitly allowed by a later milestone, and old C# scripts remain forbidden.

## 4. Current First Playable UI Summary

Current active first playable Presentation remains M8.6/M8.7 runtime-generated uGUI:

- `FirstPlayableBootstrap` composes asset catalog, theme, factory, passive view, session, fixtures, and presenter.
- `FirstPlayablePresenter` owns state flow and session coordination.
- `FirstPlayableView` passively builds and renders a generated Canvas.
- `FirstPlayableUiFactory` creates panels, text, scroll views, buttons, sprite images, and dividers.
- `FirstPlayableUiTheme` centralizes colors, fonts, and selectable states.
- `FirstPlayableAssetCatalog` loads runtime-safe fonts/sprites from `Resources` with fallbacks.
- `FirstPlayableScreenState` carries data-only review/result/complete states.
- `FirstPlayableKoreanText` centralizes Korean player-facing UI text while preserving source IDs.

Current layout:

- Top header.
- Left contract docket/current case panel.
- Center document viewer.
- Right AR/CR checklist.
- Bottom decision/result panel.

Current style:

- Generated workdesk/dashboard layout.
- Dark green/black panels, parchment document cards, imported panel/button/paper/seal/ribbon/ship sprites where available.
- uGUI `Text`, not TMP.
- Korean player-facing text and exact source IDs preserved.

## 5. Gap Analysis

Major gaps against Main scene:

| Area | Current first playable | Main scene target | Gap |
| --- | --- | --- | --- |
| Overall composition | Header + three columns + bottom result band. | Object-layered scene with background, workbench, shelf, document props, and bottom-right decision paper. | Large. Current UI still feels like generated QA dashboard. |
| Background | Solid/generated desk color with panels. | Full illustrated `Main_Total.png` background and prop layer. | Large. |
| Canvas structure | Single generated Canvas and layout groups. | Authored hierarchy with many fixed RectTransforms and nested object panels. | Large. |
| Document UI | Vertical generated document cards in scroll view. | Individual draggable paper prefab style with compact title/field rows, seal, separator lines, and image accents. | Large. |
| Checklist UI | Right scroll list with AR/CR rows. | Right shelf/check view with distinct essential/consideration paper cards. | Medium-large. |
| Decision UI | Bottom full-width band with approve/reject and result text. | Bottom-right paper/plaque with `Accept` and `Reject` buttons and compact stats. | Large. |
| Button style | Imported generic button sprite plus color tint. | Sprite/tab/icon art, smaller physical buttons. | Medium-large. |
| Font usage | uGUI Noto Serif KR preferred; Kyobo/Galmuri as fallback. | TMP Kyobo/Galmuri SDF assets dominate. | Medium. Need emulate with runtime-safe font files without TMP settings/package drift. |
| Colors | Dark green workdesk + parchment/gold. | Painted background, black overlay, tan papers, red/blue/green/orange markers. | Medium. |
| Interaction feel | Functional clicks/toggles, no scene motion. | Toggle movers, draggable documents, shelf panels, animated/physical UI behavior. | Large, but M8.8 should emulate only safe first-playable needs. |

Preserved strengths:

- C001/C002/C003 flow is already implemented and validated in previous milestones.
- Korean player-facing labels are centralized.
- Hidden accident data is already sanitized from Presentation.
- Source IDs remain exact.
- Presentation split is maintainable enough for remap work.

## 6. Risk Analysis

High-risk items:

- Main scene and direct prefabs rely on many old runtime scripts. Copying those scripts is forbidden and would collapse the M8.6/M8.7 architecture.
- Main scene document prefabs include old document view scripts and draggable behavior. Direct prefab import would likely create missing-script or architecture coupling risks.
- Main scene uses TMP SDF assets. Current first playable uses uGUI `Text`; migrating to TMP may require package/settings review and is outside M8.8-A.
- Many visible image assets are `.aseprite` or old image files. M8.8-C needs a strict manifest and runtime-safe copy strategy.
- Authored absolute layout may not directly fit the generated responsive layout without text overflow risks.

Medium-risk items:

- Some Main scene visual references are old/prototype names such as `GameObject`, `Image`, `Date`, and misspelled object names. The next plan should translate these into clear active-project object names without changing source IDs.
- Existing M8.6 imported FantasyGrimoire sprites improved polish but are not Main-scene-specific, so overusing them would continue visual drift.

Low-risk items:

- Background/paper/button/stamp visual emulation can be done with copied standalone textures and generated fallback colors.
- Presenter/application/domain flow can remain untouched for most UI remap work.

## 7. Main-Scene-Only Compliance Evidence

Compliant actions performed:

- Read required project and milestone documents.
- Inspected current first playable Presentation code.
- Inspected `ref_folder/Assets/Scenes/Main.unity` as the only scene-based visual/layout reference.
- Inspected only assets and prefabs directly referenced by `Main.unity` for dependency classification.
- Did not inspect forbidden scene files as visual/layout references.
- Did not modify `ref_folder/`.
- Did not copy assets.
- Did not modify active C# code, active scenes, prefabs, ScriptableObjects, packages, or project settings.

Directly inspected Main-referenced prefabs:

- `ref_folder/Assets/Prefabs/ShipInsuranceDocument.prefab`
- `ref_folder/Assets/Prefabs/ShipRegistDocument.prefab`
- `ref_folder/Assets/Prefabs/HullInspectionDocument.prefab`
- `ref_folder/Assets/Prefabs/RouteDocument.prefab`
- `ref_folder/Assets/Prefabs_KSM/ConversationPanel.prefab`

No other scene was used for visual or layout comparison.

## 8. Initial Parity Scores

Scores are initial M8.8-A estimates against `ref_folder/Assets/Scenes/Main.unity`, not against generic UI polish.

| Category | Score / 100 | Reason |
| --- | ---: | --- |
| Main scene layout similarity | 28 | Current UI has header/columns/bottom band, while Main uses background/workbench/shelf/object panels. |
| Canvas structure similarity | 30 | Both are screen-space Canvas at 1920x1080, but hierarchy and layout method differ strongly. |
| Panel composition similarity | 34 | Current right checklist and document panel loosely map to shelf/check/paper ideas, but composition is generic. |
| Paper/document style similarity | 42 | Current paper cards exist, but Main uses compact prefab-like document papers with seal/fields/image. |
| Button style similarity | 38 | Buttons are functional and sprite-backed, but not Main's compact tab/icon treatment. |
| Font similarity | 45 | Current fallback catalog includes Kyobo/Galmuri font files, but primary styling uses Noto/uGUI rather than Main TMP SDF look. |
| Color/tone similarity | 50 | Parchment/dark tones overlap, but Main is image-driven and warmer/more illustrated. |
| Decorative placement similarity | 25 | Main has props, ship display, shelf, map, and background; current has small header decorations only. |
| Korean readability | 82 | Current Korean is readable and centralized. |
| Interaction feel | 35 | Current UI is stable but lacks Main's object/toggle/physical panel feel. |
| Runtime stability | 85 | M8.7 compile/import passed previously; M8.8-A made no runtime change. |
| Code maintainability | 82 | M8.6/M8.7 split is preserved and suitable for planned remap. |
| Main-scene-only compliance | 100 | Only Main scene was used as scene-based reference. |

Estimated overall Main scene parity: **46 / 100**.

## 9. Recommendation For M8.8-B

Proceed to `M8.8-B: Main Scene UI Remap Plan` before copying or implementing anything.

Recommended planning direction:

- Emulate Main scene structure, do not import old scene/prefab hierarchy wholesale.
- Use `Main_Total.png` or a documented Main-derived equivalent as the primary background candidate if runtime-safe.
- Replace the generic three-column feeling with a Main-like composition:
  - background layer,
  - central workbench/document area,
  - right shelf/checklist cards,
  - bottom-right decision/result paper,
  - top date/status strip.
- Convert first playable document cards toward the four Main document prefab patterns:
  - ship insurance application,
  - ship registration,
  - hull inspection,
  - route declaration.
- Represent AR/CR as Main-like `EssentialPanel` and `ConsideratePanel` paper rows while preserving exact IDs.
- Keep `FirstPlayableBootstrap` as composition root, `FirstPlayablePresenter` as state flow owner, and `FirstPlayableView` as passive rendering.
- Use only runtime-safe standalone images/fonts copied into the approved M8.8-C destinations.
- Do not copy Main scene C# scripts, old prefabs as active runtime prefabs, ProjectSettings, Packages, ScriptableObjects, timelines, or scenes.

Stop condition for M8.8-B:

- If the plan cannot identify a safe asset subset without old script or settings coupling, stop before M8.8-C and request human direction.
