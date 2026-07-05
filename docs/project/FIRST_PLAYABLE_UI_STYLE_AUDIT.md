# First Playable UI Style Audit

Date: 2026-07-05

Milestone: M8.5 Existing UI Style Integration, Font Import, and Game Draft Polish Pass.

## Scope And Boundary

Allowed changes for this pass are limited to first playable presentation code, selected font files under the active Unity project, tests if needed, and project documentation. The reference project remains read-only except for copying selected font files.

## Current Scorecard

| Category | Current score | Target score | Changes planned | Final score | Remaining gap |
| --- | ---: | ---: | --- | ---: | --- |
| Existing UI style match | 20 | 70 | Replace raw debug stack with an underwriting workdesk layout inspired by the reference project's paper/book desk tone. | 68 | Generated colors/layout now match the tone, but no active project sprites are available. |
| Font consistency | 15 | 75 | Import Korean-capable reference fonts and centralize font choice in a presentation theme helper. | 80 | Uses imported Noto Serif KR in Editor; build fallback remains OS dynamic font. |
| Panel/background consistency | 25 | 70 | Use coordinated desk, ink, paper, border, and status colors instead of plain dark debug panels. | 70 | Consistent generated panels; no final art sprites. |
| Button consistency | 25 | 70 | Add deliberate approve/reject/continue button treatments and selectable transition colors. | 72 | Buttons have distinct tones and hover/click states; no icon sprites. |
| Document/card consistency | 20 | 75 | Render each submitted document as a paper-like card with title, status, fields, and missing-document emphasis. | 76 | Cards are readable and paper-like; texture/stamp art remains absent. |
| Checklist/rule row clarity | 30 | 80 | Separate AR and CR rows, show exact rule IDs, severity labels, trigger hints, and selected states. | 82 | AR/CR markers, exact IDs, indicated state, and selection feedback are now visible. |
| Result/settlement feedback clarity | 30 | 80 | Replace raw multiline dump with structured audit, premium, outcome, and settlement sections. | 78 | Much clearer than debug dump; final claim/receipt design is still unresolved. |
| Interaction feedback | 20 | 70 | Add hover/click color transitions, selected checklist row states, result highlight colors, and manual next/finish flow. | 72 | Manual next/finish and color feedback added; no animation/audio systems. |
| Source ID preservation | 90 | 100 | Keep exact IDs visible for contracts and rules; avoid renaming or normalizing source IDs. | 100 | Contract and rule IDs remain exact strings. |
| First playable functionality preservation | 85 | 100 | Preserve C001/C002/C003 fixture behavior and avoid changing application/domain rules. | 100 | Pure smoke test passed for the three required fixture flows. |

## Active Project UI / Asset Audit

### Assets Inspected

- `EpicProjectR/Assets/Scenes/SampleScene.unity`
- `EpicProjectR/Assets/Readme.asset`
- `EpicProjectR/Assets/TutorialInfo/`
- `EpicProjectR/Assets/Settings/`
- `EpicProjectR/Assets/Scripts/Presentation/FirstPlayableBootstrap.cs`
- `EpicProjectR/Assets/Scripts/Application/`
- `EpicProjectR/Assets/Scripts/Content/`
- `EpicProjectR/Assets/Scripts/Domain/`
- `EpicProjectR/Assets/Tests/EditMode/`

### Existing UI Conventions Found

Confirmed: the active Unity project currently has only the runtime-generated first playable UI plus Unity tutorial/readme assets. No active production UI prefab, panel sprite, document sprite, button sprite, or active project font asset was found.

Inferred: because active project UI assets are sparse, M8.5 should keep the runtime-generated approach and introduce a centralized generated style rather than edit scene/prefab YAML or import old reference UI art.

### Active Project Assets Selected

- `EpicProjectR/Assets/Scripts/Presentation/FirstPlayableBootstrap.cs`: selected as the existing first playable UI entry point.
- `EpicProjectR/Assets/Scenes/SampleScene.unity`: inspected only; not selected for modification in the planned pass.

### Assets Intentionally Not Used

- `EpicProjectR/Assets/TutorialInfo/`: Unity tutorial/readme support assets, not production UI.
- `EpicProjectR/Assets/Settings/`: render pipeline settings, not UI style assets.
- `EpicProjectR/Assets/Readme.asset`: Unity readme artifact, not production UI.

### Missing Active Assets

- No active project document/paper sprite.
- No active project panel or button sprites.
- No active project TextMeshPro font assets.
- No active project UI prefabs for the first playable.

## Reference Visual Audit

Reference inspection found an old UI direction with book/paper panels, paper textures, document prefabs, UI buttons, marine document/localization assets, and Korean-capable font assets. These are treated as visual reference only. No reference sprites, prefabs, scenes, scripts, ScriptableObjects, packages, or project settings are selected for import.

## Reference Font Audit

| Candidate font | Original path | Likely usage | Korean support | TMP/SDF asset found | Copy necessary? | Selection |
| --- | --- | --- | --- | --- | --- | --- |
| `NotoSerifKR-Bold.ttf` | `ref_folder/Assets/Assets/FantasyGrimoireUI/Font/NotoSerifKR-Bold.ttf` | Titles/buttons with formal document tone. | Yes, inferable from `KR` family name. | Yes, `NotoSerifKR-Bold SDF.asset`. | Yes, for active UI title/button font. | Selected |
| `NotoSerifKR-SemiBold.ttf` | `ref_folder/Assets/Assets/FantasyGrimoireUI/Font/NotoSerifKR-SemiBold.ttf` | Body and document text with Korean support. | Yes, inferable from `KR` family name. | Yes, `NotoSerifKR-SemiBold SDF.asset`. | Yes, for active UI body font. | Selected |
| `Galmuri11-Bold.ttf` | `ref_folder/Assets/TextMesh Pro/Resources/Fonts & Materials/Galmuri11-Bold.ttf` | Pixel-styled emphasis. | Likely yes, Korean font family. | Yes. | No, too stylized for readable underwriting body copy. | Not selected |
| `KyoboHandwriting2020pdy.otf` | `ref_folder/Assets/TextMesh Pro/Resources/Fonts & Materials/KyoboHandwriting2020pdy.otf` | Handwritten accent. | Likely yes, Korean font family. | Yes. | No, decorative handwriting risks body readability. | Not selected |
| `KyoboHandwriting2022khn.ttf` | `ref_folder/Assets/TextMesh Pro/Resources/Fonts & Materials/KyoboHandwriting2022khn.ttf` | Handwritten accent. | Likely yes, Korean font family. | Yes. | No, decorative handwriting risks body readability. | Not selected |
| `LiberationSans.ttf` | `ref_folder/Assets/TextMesh Pro/Fonts/LiberationSans.ttf` | Default TMP Latin fallback. | No Korean support inferred. | Yes. | No, active UI needs Korean-capable fallback. | Not selected |
| `NotoSerif-Regular/Medium/Bold.ttf` | `ref_folder/Assets/Assets/IndigoLay/PixelAdventureBookUI/Font/` | Latin serif UI kit font. | No Korean support inferred from file name. | Yes. | No, Korean-capable KR variant is available. | Not selected |

## Selected Visual Direction

The generated UI will use a restrained maritime underwriting workdesk style: dark green-black desk background, parchment document cards, ink-colored body text, muted gold rule/status accents, red AR markers, blue CR markers, and a bottom decision/result band. This keeps the first playable in game-draft territory without importing reference art.

## Reference Fonts Copied

| Original path | New path | Why selected | Korean support | Used in first playable UI |
| --- | --- | --- | --- | --- |
| `ref_folder/Assets/Assets/FantasyGrimoireUI/Font/NotoSerifKR-Bold.ttf` | `EpicProjectR/Assets/Fonts/ImportedFromReference/NotoSerifKR-Bold.ttf` | Formal title/button face that matches the reference paper/book tone. | Yes, inferable from Noto Serif KR family and file name. | Yes, through `FirstPlayableUiTheme` in Editor. |
| `ref_folder/Assets/Assets/FantasyGrimoireUI/Font/NotoSerifKR-SemiBold.ttf` | `EpicProjectR/Assets/Fonts/ImportedFromReference/NotoSerifKR-SemiBold.ttf` | Readable body/document face with Korean coverage. | Yes, inferable from Noto Serif KR family and file name. | Yes, through `FirstPlayableUiTheme` in Editor. |

## Final Implementation Notes

- Added `FirstPlayableUiTheme` to centralize imported font lookup, palette, and selectable transitions.
- Reworked `FirstPlayableBootstrap` into a 1920x1080-scaled workdesk layout with header, docket, document viewer, AR/CR checklist, and bottom decision/result panel.
- Replaced timed auto-advance with explicit `Next Contract` / `Finish` acknowledgement.
- Kept first playable logic in the existing domain/application/content services.
- No non-font reference assets were copied.

## Risks

- Unity Editor has not yet generated `.meta` files for newly copied fonts in this pass.
- Runtime-generated uGUI can be less precise than authored prefab layout.
- Without active UI sprites, the style can only approximate the reference paper/book tone through colors, spacing, borders, and typography.

## M8.6 Addendum

Date: 2026-07-05

Status: Completed as a controlled reference asset migration and architecture hardening pass pending manual Unity visual QA.

Updates:

- Added a Resources-backed `FirstPlayableAssetCatalog` for build-safe runtime loading.
- Copied selected runtime-safe reference assets into `EpicProjectR/Assets/Resources/FirstPlayable/ImportedReference/RuntimeSafe/`.
- Added imported paper, panel, button, ribbon, ship, and seal sprite usage with generated color fallbacks.
- Removed Presentation dependency on `UnityEditor.AssetDatabase`.
- Split the former UI God Object into bootstrap, presenter, passive view, UI factory, theme, asset catalog, and screen state classes.

Revised style scores:

| Category | M8.5 final | M8.6 final | Note |
| --- | ---: | ---: | --- |
| Existing UI style match | 68 | 78 | Uses selected reference sprites while preserving workdesk tone. |
| Font consistency | 80 | 88 | Fonts now load from build-safe Resources path. |
| Panel/background consistency | 70 | 78 | Panel sprite is available with fallback colors. |
| Button consistency | 72 | 80 | Button sprite is available for decision controls. |
| Document/card consistency | 76 | 82 | Paper and seal sprites are available for document cards. |
| Source ID preservation | 100 | 100 | Exact IDs remain unchanged. |
| First playable functionality preservation | 100 | 100 | Pure C# smoke test passed. |
