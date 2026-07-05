# M8.8-B Main Scene UI Remap Plan

Date: 2026-07-06

Status: Completed. Continue to M8.8-C because no hard stop condition was reached.

## 1. Executive Summary

M8.8-B will move the first playable from a generated three-column QA workdesk toward the old `Main.unity` visual language without copying old scripts, scenes, prefabs, ProjectSettings, Packages, or gameplay logic.

The safe route is to emulate Main scene composition in runtime-generated uGUI:

- full-screen Main-like background layer,
- top date/status strip,
- central workbench/paper/document zone,
- right shelf/checklist zone,
- bottom-right decision/result paper,
- compact sprite-backed buttons,
- Kyobo/Galmuri-like Korean font priority,
- optional sprite fallbacks for every copied asset.

## 2. M8.8-A Finding Summary

M8.8-A found that current parity is approximately `46 / 100`.

Key confirmed Main scene traits:

- `Main.unity` uses one main Screen Space Overlay Canvas at `1920 x 1080`.
- The scene is authored with layered fixed RectTransforms, not responsive dashboard columns.
- The first viewport is dominated by `Main_Total.png`, workbench objects, a right-side shelf, a document/application panel, a bottom-right accept/reject panel, and paper document prefabs.
- Main document prefabs are script-bearing and should not be copied as active prefabs.
- Most visible font usage is TMP SDF Kyobo/Galmuri, but active first playable can safely emulate this using copied `.ttf` / `.otf` font files and uGUI `Text`.

## 3. Target UI Remap Strategy

Keep the existing generated UI architecture, but change its composition:

1. Build a full-screen background image panel first.
2. Add a top strip for turn/date/status similar to Main's date bar.
3. Add a large center workbench group instead of equal dashboard columns.
4. Place document cards on paper/prop-like surfaces in the center-left workbench.
5. Place contract docket and current case summary as a left-side paper stack, not a tall admin panel.
6. Place AR/CR rules in a right shelf/checklist panel with paper rows.
7. Place result and decision controls in a bottom-right paper panel.
8. Keep all interaction callbacks in the view and all state flow in the presenter.

This remains runtime-generated because scene/prefab authoring is not part of this milestone.

## 4. What To Emulate From Main Scene

Emulate:

- `Main_Total.png` full-screen background feeling.
- Top date/status strip.
- Workbench/card physical layout.
- Paper prop texture for documents and result panel.
- Right shelf/checklist paper rows inspired by `EssentialPanel` and `ConsideratePanel`.
- Compact accept/reject tab button style.
- Kyobo/Galmuri Korean font character.
- Decorative document seals, paper shadows, ship marker, and small prop hints.

## 5. What Not To Reuse

Do not reuse:

- `Main.unity` itself as an active scene.
- Old C# scripts such as `UnderwritingCaseApplicationView`, `MarineInsuranceRejectionReasonListView`, document view scripts, dialogue scripts, or toggle mover scripts.
- Main scene prefabs as runtime prefabs because they reference old scripts.
- ScriptableObject case data from `ref_folder/Assets/SO/Case`.
- Timeline/playable assets.
- ProjectSettings or Packages.
- TMP SDF assets or TMP settings.
- `.aseprite` files directly in this pass because they are not in the approved asset type list.

## 6. Asset Migration Plan For M8.8-C

Copy only runtime-safe files into:

- `EpicProjectR/Assets/ImportedReference/RuntimeSafe/MainSceneUI/`
- `EpicProjectR/Assets/Resources/FirstPlayable/ImportedReference/RuntimeSafe/MainSceneUI/`

Runtime Resources candidates:

| Source | Destination role | Why |
| --- | --- | --- |
| `ref_folder/Assets/Images/Main_Total.png` | background | Direct Main scene background dependency. |
| `ref_folder/Assets/Images/Old/paper.png` | paper prop/document panel | Direct Main scene paper image. |
| `ref_folder/Assets/Images/Paper texture 2.png` | document card texture | Direct Main scene paper texture. |
| `ref_folder/Assets/Images/Old/편지UI.png` | letter/document decoration | Direct Main scene letter prop. |
| `ref_folder/Assets/Assets/IndigoLay/PixelAdventureBookUI/Sprites/UI/Tabs/UI_tab_dim.png` | approve button sprite | Main scene accept-style tab dependency/equivalent. |
| `ref_folder/Assets/Assets/IndigoLay/PixelAdventureBookUI/Sprites/UI/Tabs/UI_tab_Off.png` | reject/neutral button sprite | Main scene reject-style tab dependency/equivalent. |
| `ref_folder/Assets/Assets/IndigoLay/PixelAdventureBookUI/Sprites/UI/Buttons/UI_BtnIcon_x.png` | small icon/button accent | Main scene small button treatment. |
| `ref_folder/Assets/TextMesh Pro/Resources/Fonts & Materials/KyoboHandwriting2022khn.ttf` | primary title/body font | Main scene TMP SDF source font equivalent for uGUI. |
| `ref_folder/Assets/TextMesh Pro/Resources/Fonts & Materials/Galmuri11-Bold.ttf` | accent/fallback font | Main scene Galmuri font equivalent for uGUI. |
| `ref_folder/Assets/TextMesh Pro/Resources/Fonts & Materials/KyoboHandwriting2020pdy.otf` | fallback/accent font | Main scene Kyobo fallback equivalent for uGUI. |

Do not copy `.meta` files; Unity should regenerate active project metadata.

## 7. UI Implementation Plan For M8.8-D

Allowed code targets:

- `FirstPlayableAssetCatalog.cs`
- `FirstPlayableUiTheme.cs`
- `FirstPlayableUiFactory.cs`
- `FirstPlayableView.cs`
- `FirstPlayableScreenState.cs` only if needed
- `FirstPlayablePresenter.cs` only if needed
- `FirstPlayableKoreanText.cs` only if labels need adjustment

Implementation steps:

1. Add MainSceneUI resource paths and optional sprites/fonts to `FirstPlayableAssetCatalog`.
2. Prioritize Kyobo/Galmuri-like fonts in `FirstPlayableUiTheme` through the asset catalog while retaining fallback to existing Noto/OS/default fonts.
3. Add factory helpers for fixed/anchored panels and image fills so the view can build a Main-like layered composition without spreading low-level RectTransform setup everywhere.
4. Replace the three-column root with a full-screen layered background/workbench composition.
5. Keep the view passive: it receives state, renders text/cards/rows, exposes decision events, and does not evaluate gameplay.
6. Style documents as individual paper records with title, status, field lines, seal/letter accents, and missing-document emphasis.
7. Style rules as AR/CR paper rows in a right shelf.
8. Style the result/decision panel as a bottom-right paper panel with compact buttons.

## 8. Code Quality Plan For M8.8-E

Review after implementation:

- asset path centralization,
- optional asset fallback behavior,
- null guard consistency,
- event listener safety,
- layout helper responsibility,
- repeated string cleanup,
- RenderReview/RenderResult/RenderComplete consistency,
- source ID display consistency,
- Korean text catalog usage.

Only make small maintainability changes inside allowed Presentation files.

## 9. Validation Plan For M8.8-F

Run or attempt:

- git status before/after,
- Unity script compilation,
- EditMode tests if Unity runner can produce results,
- pure C# smoke tests,
- static search for `UnityEditor` / `AssetDatabase`,
- static search for `AccidentFlag` / `WillAccidentIfApproved` in Presentation,
- source ID preservation checks,
- copied asset extension/boundary checks,
- no Packages/ProjectSettings/scene changes,
- no copied C# scripts from `ref_folder`,
- Main-scene-only compliance check,
- QA guide update if UI instructions changed.

Manual Play Mode QA may be skipped if unavailable, but the final report must say so.

## 10. Parity Score Targets

Target after M8.8-F:

| Category | M8.8-A baseline | Target |
| --- | ---: | ---: |
| Main scene layout similarity | 28 | 58 |
| Main scene color/tone similarity | 50 | 68 |
| Main scene button similarity | 38 | 62 |
| Main scene paper/document similarity | 42 | 66 |
| Main scene font similarity | 45 | 68 |
| Main scene panel/background similarity | 34 | 68 |
| Korean text readability | 82 | 84 |
| First playable flow preservation | 100 | 100 |
| Source ID preservation | 100 | 100 |
| Runtime/build safety | 85 | 88 |
| Imported asset boundary safety | 100 | 100 |
| Maintainability after UI remap | 82 | 82 |
| Main-scene-only compliance | 100 | 100 |

Target overall parity: **68 / 100**.

## 11. Risk And Stop Conditions

Stop if:

- a Main-like implementation requires old C# scripts,
- a Main-like implementation requires ProjectSettings or Packages,
- source IDs would need to change,
- first playable flow breaks and cannot be restored within allowed paths,
- Unity compile errors cannot be fixed safely,
- another scene is required as a visual reference.

Do not stop merely because:

- optional sprites are unavailable,
- manual Play Mode QA is unavailable,
- `.aseprite` assets are skipped in favor of PNG/font equivalents.

## 12. Milestone Sequence

1. M8.8-B: write this plan.
2. M8.8-C: copy selected runtime-safe assets and write manifest.
3. M8.8-D: implement Main-like runtime UI remap.
4. M8.8-E: perform code quality pass and write report.
5. M8.8-F: validate, score parity, write final reports, update QA guide if needed.

## 13. Final Recommendation For Continuing To C

Continue to M8.8-C.

No hard stop condition was reached. The selected asset subset is made of standalone `.png`, `.ttf`, and `.otf` files or documented uGUI equivalents for Main's TMP SDF font usage.
