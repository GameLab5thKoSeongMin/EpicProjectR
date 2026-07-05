# M8.11-F Main UI Size Image Parity Report

## Overall Result

Completed Main UI forensic extraction, current-vs-Main delta analysis, exact PNG asset mirroring, and layout implementation. The first playable now uses measured Main-derived sizes and display areas for workstation, right shelf, document cards, decision drawer, and decision buttons.

Before score: `82 / 100`.

After score: `88 / 100`.

## Main UI Forensic Summary

- Main Canvas: Screen Space Overlay, Scale With Screen Size, `1920x1080`, match width `0`.
- Main background: `Main_Total.png`, `1920x1080`.
- Main shelf: `484.5942 x 1000.7685`, absolute center around `1678,-40`.
- Main workstation/document panel: approx `869 x 1001`, absolute center around `44,-39`.
- Main decision paper: `480.8658 x 398.97`, opens from lower off-screen to lower-right.
- Main decision buttons: `120 x 65`, reject left, accept right.
- Main document prefabs: roots around `252x300`, `319x333`, `415x449`.

## Current-Vs-Main Delta Summary

Corrected:

- Workbench was too short and left-biased; now Main-sized and Main-positioned.
- Shelf was too small; now Main-sized and Main-positioned.
- Decision drawer was too wide and shallow; now Main-sized and Main-positioned.
- Buttons were layout-sized; now fixed `120x65`.
- Document cards were too short; now use Main prefab root sizes.
- RectTransform magic numbers are centralized in `FirstPlayableMainSceneRectSpec`.

Remaining:

- Exact `.aseprite` right shelf/table sliced art is skipped.
- uGUI Text approximates Main TMP SDF rendering.
- Docket is a first-playable support panel, not a direct Main object.

## Assets Corrected

Exact PNG assets were mirrored into `EpicProjectR/Assets/ImportedReference/RuntimeSafe/MainSceneUI/` and remain loaded from Resources where runtime needs them:

- Main background,
- paper texture,
- paper prop,
- letter prop,
- approve/reject tabs,
- small icon button,
- contractor image,
- speech bubble,
- bell/full decision prop.

## Layout Changes

- Added `FirstPlayableMainSceneRectSpec`.
- Updated `FirstPlayableView` to use spec for:
  - reference resolution,
  - entry paper,
  - docket,
  - contractor/dialogue,
  - workstation,
  - document board,
  - shelf,
  - decision box,
  - decision drawer,
  - document card sizes,
  - decision button sizes.

## Interaction Preservation

Preserved:

- entry click,
- review slide-in,
- document drag/clamp,
- AR/CR row toggles,
- conditional approve label,
- selected CR premium display,
- reject/approve submission,
- result and next/finish flow.

## Validation Result

Static validation passed:

- no `UnityEditor` / `AssetDatabase` in runtime script paths,
- no hidden accident source fields in Presentation,
- source IDs remain exact,
- no old scripts copied,
- no packages/project settings modified.

Unity compile passed:

- `unity_m8_11_compile.log`
- `Tundra build success`
- no C# compiler errors
- no `NullReferenceException`

Unity EditMode Test Runner was attempted and produced a `0` testcase result XML on the second attempt, so fixture test execution remains inconclusive.

Manual Play Mode and screenshot validation remain pending.

## Repository Boundary Confirmation

Not modified:

- `ref_folder/`,
- `EpicProjectR/Packages/`,
- `EpicProjectR/ProjectSettings/`,
- active scenes,
- active prefabs,
- ScriptableObject assets.

## Remaining Mismatches

- Exact `.aseprite` UI art requires a safe export/import decision.
- TMP SDF visual parity is not exact.
- Document cards are generated UI, not old prefabs.
- Screenshot-based fine-tuning is still needed.

## Recommended Next Milestone

M8.12 should be a Unity Editor Play Mode screenshot pass: capture entry/review/AR-CR/drawer/result states at `1920x1080`, then adjust only the centralized `FirstPlayableMainSceneRectSpec` values and text sizing to remove any observed overlap.
