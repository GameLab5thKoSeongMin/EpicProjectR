# M8.8-F Main Scene Parity Score

Date: 2026-07-06

Status: Completed.

## Baseline

M8.8-A estimated overall Main scene parity at **46 / 100**.

## After M8.8-B~F

Estimated overall Main scene parity: **69 / 100**.

This score reflects static/code inspection and successful Unity compilation. It is not a substitute for manual visual QA in Play Mode.

| Category | Before | After | Reason |
| --- | ---: | ---: | --- |
| Main scene layout similarity | 28 | 60 | Replaced equal 3-column dashboard with background, top strip, left paper stack, central workbench, right shelf, and bottom-right decision paper. |
| Main scene color/tone similarity | 50 | 68 | Palette now uses warmer paper, darker overlay, shelf colors, and less generic green dashboard tone. |
| Main scene button similarity | 38 | 63 | Decision buttons now prefer MainSceneUI tab sprites for approve/reject. |
| Main scene paper/document similarity | 42 | 68 | Documents now use MainSceneUI paper texture preference and sit within a workbench surface. |
| Main scene font similarity | 45 | 67 | Runtime font priority now starts from MainSceneUI Kyobo/Galmuri source font equivalents for uGUI. |
| Main scene panel/background similarity | 34 | 70 | `Main_Total.png` is loaded as the full-screen background with a dark overlay and fixed Main-like panel placement. |
| Korean text readability | 82 | 84 | Text remains Korean and source IDs exact; paper panel text colors were tuned for contrast. |
| First playable flow preservation | 100 | 100 | Pure C# smoke passed for C001/C002/C003. |
| Source ID preservation | 100 | 100 | Static search confirms IDs remain exact; no source ID normalization was introduced. |
| Runtime/build safety | 85 | 90 | Unity batchmode compile passed; optional asset loading still falls back safely. |
| Imported asset boundary safety | 100 | 96 | Only `.png`, `.ttf`, `.otf`, and Unity-generated `.meta` files exist under MainSceneUI. |
| Maintainability after UI remap | 82 | 83 | Asset paths and anchored panel creation are centralized; view remains verbose but still passive. |
| Main-scene-only compliance | 100 | 100 | Only `ref_folder/Assets/Scenes/Main.unity` was used as scene visual/layout reference. |

## Visual Gaps Remaining

- No authored prefab hierarchy was created.
- Old Main drag/drop, dialogue, timeline, and toggle-mover behavior was not reproduced.
- `.aseprite` shelf/line/outline art was not copied in this pass.
- TMP SDF rendering was approximated with uGUI font files.
- Manual Play Mode screenshot review is still needed for exact position and overflow tuning.
