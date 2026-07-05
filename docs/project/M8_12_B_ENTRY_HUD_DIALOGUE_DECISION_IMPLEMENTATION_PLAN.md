# M8.12-B Entry, HUD, Dialogue, Decision Implementation Plan

Status: Executed.

## Scope

Allowed implementation paths used:

- `EpicProjectR/Assets/Scripts/Presentation/`
- `docs/project/`

Forbidden paths kept untouched:

- `ref_folder/`
- `EpicProjectR/Packages/`
- `EpicProjectR/ProjectSettings/`
- active scenes, prefabs, ScriptableObject assets

## Implementation Plan

| Item | Plan |
| --- | --- |
| Top HUD | Replace one cluttered meta text with left office title, centered Korean date, right ducats/reputation. |
| Office ledger | Store first playable ducats/reputation in presenter. Award floor `finalPremium / 100` once per approved/conditional contract. |
| Entry composition | Show centered bell, paper document, and dim contractor preview over the Main background. |
| Bell click | Route bell/document click to existing `ReviewStarted` event. |
| Arrival animation | Use `FirstPlayableTweenRunner` movement, scale, fade, and tint tweens. |
| Camera-like transition | Coordinate contractor, dialogue, workbench, shelf, and decision drawer rect movement; do not change Unity camera. |
| Dialogue | Render a large upper-left speech panel with fixture-safe Korean lines from `FirstPlayableKoreanText`. |
| Dialogue click | Let the passive view advance only its displayed dialogue line. |
| Remove contract dashboard | Stop building the left docket/current contract paper in normal UI. |
| Workbench proportions | Keep central workbench as the largest area; shrink right rule shelf; make left dialogue/character the second-largest area. |
| AR/CR cleanup | Hide source IDs in row text; use `절대 거절 사유` and `거절 고려 사유`. |
| Document cleanup | Show document kind and fields only; hide `DocumentId` and submitted status. |
| Decision box | Rename to `인수 결정서`; presenter toggles drawer open/closed. |
| Lower reason paper | Place selected reason/premium summary inside a white paper-like panel in the decision drawer. |
| Submit flow | Keep decision submission in presenter/session. View only emits selected IDs and player decision. |
| Exit/progression | Play reverse character tween on result; document board click acknowledges result and advances. |

## Architecture Preservation

- Bootstrap remains composition root only.
- Presenter owns session state, current screen mode, ledger state, and progression.
- View remains passive with UI events and local dialogue pagination only.
- Factory still creates controls.
- Theme still owns colors/fonts.
- AssetCatalog still loads runtime-safe Resources assets with fallbacks.
- ScreenState remains data-only.
- MainLoopState remains helper logic for Main-like labels.

## Risks

- Visual parity still needs Play Mode screenshot verification.
- `dotnet build` was blocked by local SDK permission; Unity batch run returned exit code 0 but did not emit a log file in this environment.
- Exact old character arrival timing was not fully encoded in inspected Main scripts, so fade/scale/tint is an approximation.
