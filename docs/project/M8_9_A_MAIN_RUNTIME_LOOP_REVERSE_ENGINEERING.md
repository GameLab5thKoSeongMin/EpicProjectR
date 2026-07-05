# M8.9-A Main Runtime Loop Reverse Engineering

Status: confirmed from `ref_folder/Assets/Scenes/Main.unity`, direct document prefabs, `ConversationPanel.prefab`, and scripts directly linked from those assets.

## Verified Runtime Loop

1. Main starts with an active visitor/current case managed by `GameLoopController`.
2. Submitted document prefabs are instantiated under the application document root by `UnderwritingCaseApplicationView`.
3. Document prefabs carry `DraggableImageObject`, so papers can be dragged and clamped to the canvas bounds.
4. The dialogue/visitor presentation and workstation panels enter by `RectTransformToggleMover`.
5. Reject reasons are bound into separate AR and CR roots by `MarineInsuranceRejectionReasonListView`.
6. Selecting CR reasons updates approval button text to conditional approval and recalculates premium.
7. A lower-right decision button toggles the decision paper upward.
8. Approve/reject buttons call `UnderwritingDecisionToggleMoveGate`, which delegates state resolution to `GameLoopController`.

## Directly Verified Script Responsibilities

| Script | Responsibility confirmed |
| --- | --- |
| `GameLoopController` | Owns active visitor, decision gating, CR premium adjustment, conditional approval label, result resolution. |
| `UnderwritingCaseApplicationView` | Instantiates submitted document prefabs only. |
| `DraggableImageObject` | Mouse drag, bring-to-front, clamp to canvas bounds. |
| `MarineInsuranceRejectionReasonListView` | Splits rejection reasons into reject and consider-reject roots, tracks checked IDs. |
| `RectTransformToggleMover` | Opens/closes UI panels by anchored-position tween. |
| `UnderwritingDecisionToggleMoveGate` | Calls approve/reject, then toggles configured movers after resolution. |
| `ShipPanelVisualizer` | Displays ship visual and damage overlays from case data. |

## Main Scene Hierarchy And Regions

- Background: `Main.unity` uses a full-screen desk/background image composition.
- Left: document/visitor dialogue objects and contract/application presentation slide in from off-screen.
- Center: document root contains document prefabs such as `ShipInsuranceDocument`, `ShipRegistDocument`, `HullInspectionDocument`, and `RouteDocument`.
- Right: rejection reason list has separate roots for reject and consider-reject reasons.
- Lower-right: a compact decision trigger opens a bottom decision paper.

## Images And Fonts

- Already migrated in M8.8: background, paper textures, letter, tab buttons, Korean fonts.
- Newly used in M8.9 where present: temporary contractor image, speech bubble image, bell image.
- Any missing imported sprite/font must be treated as optional; runtime UI should use generated colors/default fonts instead of throwing.

## Animation And Tween Style

`RectTransformToggleMover` stores a closed anchored position on `Awake`, then tweens to an `openedPosition`. Durations observed around `0.25` to `0.4` seconds. M8.9 implements the same style with generated runtime RectTransform tweens, not copied old scripts.

## Drag Behavior

Documents begin drag under pointer, move to front, follow mouse, and clamp to bounds. Old code clamps to canvas; M8.9 clamps to the central workstation so papers cannot leave the active review table.

## Approval, Conditional Approval, Premium

Confirmed in `GameLoopController`:

- `considerRejectPremiumIncreaseRate = 0.1f`.
- Each checked CR reason increases current case premium by `10%`.
- Approval label switches to conditional approval if at least one CR reason is checked.
- Conditional approval is still resolved through approved-contract flow.

Current first playable application audit still quotes fixture premium as `100/125/150` by triggered CR count. M8.9 keeps that application/domain behavior unchanged and adds Main-style selected-CR premium display in the decision drawer.

## User-Memory Verification Table

| Requested behavior | Main verification | M8.9 implementation target |
| --- | --- | --- |
| Initial contract/document entry | Main opens visitor/case before review | Add clickable entry document before review UI. |
| Dialogue enters left | Toggle mover positions confirm slide | Add generated left dialogue/contractor tween. |
| Workstation central | Main document root holds document prefabs | Keep central workstation and draggable generated cards. |
| AR/CR panels right | Separate reject/consider roots confirmed | Split right checklist into AR upper and CR lower. |
| Decision box lower-right | Button toggles mover | Add final decision box and drawer rise. |
| Reject left, approve right | Button wiring confirms both decisions | Drawer uses reject-left/approve-right order. |
| CR changes approve text | Confirmed in `UpdateApproveButtonLabel` | Update approve text to `조건부 승인`. |
| CR premium +10% each | Confirmed in `ApplyConsiderRejectPremiumAdjustment` | Show selected-CR Main premium in drawer. |

## Risks

- Unity batchmode did not reach compile/import in this environment, so visual verification is still manual.
- Generated UI approximates old prefab hierarchy; it does not copy legacy scene objects or scripts.
- Main application premium and first playable fixture premium use different models; this is documented and intentionally not reconciled in domain code for M8.9.
