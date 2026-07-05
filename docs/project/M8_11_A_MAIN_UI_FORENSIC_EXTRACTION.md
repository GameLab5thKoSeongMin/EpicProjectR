# M8.11-A Main UI Forensic Extraction

Date: 2026-07-06

Status: Completed using only `ref_folder/Assets/Scenes/Main.unity` as scene-based visual/layout reference. Direct document prefabs and `ConversationPanel.prefab` were inspected only because they are directly referenced by Main.

## Canvas And Scaling

| Setting | Main value |
| --- | --- |
| Canvas render mode | Screen Space Overlay |
| Canvas pixel perfect | Enabled |
| CanvasScaler mode | Scale With Screen Size (`m_UiScaleMode: 1`) |
| Reference resolution | `1920 x 1080` |
| Screen match mode | Match Width Or Height |
| Match width/height | `0` |
| Reference pixels/unit | `100` |

## Major UI Object Roles

| Role | Main path | Confirmed behavior |
| --- | --- | --- |
| Background | `Canvas/WorldShiftTargets/BackgroundImage` | Full-screen `1920x1080` image, animated by `RectTransformToggleMover`. |
| Initial clickable paper | `Canvas/WorldShiftTargets/BackgroundImage/Paper` | Active `102x75` button image using old `paper.png`. |
| Letter prop | `Canvas/WorldShiftTargets/BackgroundImage/letter` | Inactive letter button prop using `편지UI.png`. |
| Contractor/dialogue button | `Canvas/WorldShiftTargets/BackgroundImage/Character/Ask/Button` | Transparent/sliced button in lower-left character dialogue region. |
| Workbench root | `Canvas/WorkBench` | Starts at `x=1371`, opens to `x=0` via `RectTransformToggleMover`. |
| Workbench display | `Canvas/WorkBench/WorkBenchDisplay` | Large central authored area, `1115.6436 x 1082.5066`. |
| Document area | `Canvas/WorkBench/WorkBenchDisplay/GameObject/Image/MainPanel` | `UnderwritingCaseApplicationView` instantiates document prefabs under this panel. |
| Draggable documents | Main document prefabs | Each prefab has `DraggableImageObject` and paper image. |
| Right shelf | `Canvas/WorkBench/Shelf` | Main right-side paper/checklist shelf, `484.5942 x 1000.7685`. |
| AR root | `.../CheckView/Scroll View/Viewport/Content/EssentialPanel` | Reject item root for `MarineInsuranceRejectionReasonListView`. |
| CR root | `.../CheckView/Scroll View/Viewport/Content/ConsideratePanel` | Consider-reject item root for `MarineInsuranceRejectionReasonListView`. |
| Decision paper | `Canvas/WorkBench/WorkBenchDisplay/GameObject/Image` | Bottom-right paper image using `Paper texture 2.png`, moves from `675,-594` to `675,-301`. |
| Reject button | `.../Image/Reject` | Right-bottom child button, `120 x 65`, sprite `UI_tab_Off.png`. |
| Approve button | `.../Image/Accept` | Right-bottom child button, `120 x 65`, sprite `UI_tab_dim.png`. |

## Animation And Interaction

- `WorkBench` uses `RectTransformToggleMover` closed `1371,0`, opened `0,0`, duration `0.5`.
- Decision paper uses `RectTransformToggleMover` closed `675,-594`, opened `675,-301`, duration `0.25`.
- Documents use `DraggableImageObject` and clamp to canvas bounds in old Main.
- CR selection changes approval label and premium through `GameLoopController` and `MarineInsuranceRejectionReasonListView`.

## Main UI Region Boundaries

Coordinates are Main's `1920x1080` CanvasScaler reference coordinates.

| Region | Main-derived target |
| --- | --- |
| Background | Center `0,0`, size `1920x1080`, anchor center. |
| Workbench root open | Center `0,0`; authored child regions place central content near absolute `x=44`, `y=-39`. |
| Central workstation | Approx absolute center `44,-39`, size `869x1001` from `WorkBenchDisplay/GameObject`. |
| Document drag area | Approx central board inside workstation, `820x760` used as safe generated equivalent. |
| Right shelf | Right-anchored equivalent center `x=1678`, `y=500`, size `485x1001`; as right anchor this is `anchoredPosition -242,-40`. |
| AR area | Inside `CheckView`, first panel image approx `419x73` at top. |
| CR area | Inside `CheckView`, second panel image approx `419x73` below AR. |
| Decision paper open | Right-anchored equivalent `anchoredPosition -241,200`, size `481x399`. |
| Decision paper closed | Right-anchored equivalent `anchoredPosition -241,-94`, size `481x399`. |
| Decision buttons | `120x65`, reject left, approve right. |

## Verification

- Confirmed only `ref_folder/Assets/Scenes/Main.unity` was used as scene reference.
- Direct prefabs inspected: four document prefabs and `ConversationPanel.prefab`.
- Old scripts read only when directly attached to Main or direct prefabs.
- Phase A extraction itself did not require modifying `ref_folder` or active scenes/prefabs/packages/settings.
