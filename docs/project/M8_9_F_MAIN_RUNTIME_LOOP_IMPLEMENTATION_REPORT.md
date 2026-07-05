# M8.9-F Main Runtime Loop Implementation Report

## Files Created

- `EpicProjectR/Assets/Scripts/Presentation/FirstPlayableDragController.cs`
- `EpicProjectR/Assets/Scripts/Presentation/FirstPlayableMainLoopState.cs`
- `EpicProjectR/Assets/Scripts/Presentation/FirstPlayableTweenRunner.cs`
- `EpicProjectR/Assets/Resources/FirstPlayable/ImportedReference/RuntimeSafe/MainSceneUI/Characters/contractor_temp.png`
- `EpicProjectR/Assets/Resources/FirstPlayable/ImportedReference/RuntimeSafe/MainSceneUI/UI/speech_bubble.png`
- `EpicProjectR/Assets/Resources/FirstPlayable/ImportedReference/RuntimeSafe/MainSceneUI/UI/bell_full.png`

## Files Updated

- `EpicProjectR/Assets/Scripts/Presentation/FirstPlayableAssetCatalog.cs`
- `EpicProjectR/Assets/Scripts/Presentation/FirstPlayableKoreanText.cs`
- `EpicProjectR/Assets/Scripts/Presentation/FirstPlayablePresenter.cs`
- `EpicProjectR/Assets/Scripts/Presentation/FirstPlayableScreenState.cs`
- `EpicProjectR/Assets/Scripts/Presentation/FirstPlayableView.cs`
- `EpicProjectR/Assets/Tests/EditMode/FirstPlayableDomainTests.cs`
- `docs/project/FIRST_PLAYABLE_QA_GUIDE.md`

## Implemented Runtime Loop

1. Runtime starts on a clickable contract/document entry paper.
2. Clicking the entry paper raises `ReviewStarted`; presenter transitions into review.
3. Dialogue/contractor, docket, workstation, and AR/CR shelf tween into position.
4. Central generated documents appear as draggable paper cards.
5. Dragging is clamped to the workstation board.
6. Right shelf is split into AR and CR sections.
7. Lower-right final decision box opens a bottom decision drawer.
8. Drawer presents reject on the left and approve on the right.
9. Selecting any CR changes approve text to `조건부 승인` and shows Main-style `+10%` per selected CR premium.
10. Submitting still delegates to `FirstPlayableSession`; result and next/finish flow remains presenter-owned.

## Architecture Preservation

- Bootstrap remains composition root only.
- Presenter owns state flow and session coordination.
- View remains passive Unity UI rendering/event exposure.
- UI factory/theme/assets remain separated.
- Domain/application/content behavior, source IDs, and hidden accident behavior are unchanged.

## Validation

- Static no `UnityEditor`/`AssetDatabase` in runtime script paths.
- Static no `AccidentFlag`/`WillAccidentIfApproved` in Presentation.
- Added EditMode test for Main selected-CR premium helper and conditional approve label.
- Unity batch compile attempted but blocked before compiler output; Play Mode validation remains pending.

## Repository Boundaries

- `ref_folder` was read only.
- No packages or project settings were edited.
- No production scenes, prefabs, or ScriptableObject assets were edited.

## Remaining Risks

- Unity generated `.meta` files for new scripts/assets during the attempted batch run, but compile confirmation is still pending.
- Manual Play Mode is still required for visual overlap, drag feel, and C001/C002/C003 click flow.
- Application premium quote remains fixture model `125/150`; Main-style `+10%` premium is displayed in the decision drawer and documented as presentation parity.
