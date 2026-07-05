# UI UX Flow

Last inspected: 2026-07-05.

## Primary Play Flow

1. Turn opens on the underwriter workday.
2. Player reads scheduled news and active rule updates.
3. Player selects one of the turn's contract cases.
4. Player reviews the active document bundle.
5. Player checks documents against underwriting criteria.
6. Player optionally reads subjective information/dialogue when available.
7. Player decides to approve, reject, or later conditionally approve.
8. Approved contracts enter contract management.
9. Accidents, letters, board posts, gifts, inspections, and settlement results appear on later turns.

The UI should make the player feel like a working underwriter, not a fantasy combat manager.

## Core Screens

| Screen / surface | Purpose | First playable priority | Notes |
| --- | --- | --- | --- |
| Workdesk / contract inbox | Select contracts for the current turn | Required | Should show turn/date and remaining cases. |
| News panel | Show scheduled rule updates and world events | Required for bundle 3/news tests | Rules may become active, stronger, or inactive through news. |
| Document viewer | Inspect applications, certificates, declarations, and manifests | Required | Should support document tabs or stacked documents by bundle. |
| Rule/checklist panel | Let player mark findings and understand AR/CR criteria | Required | Must separate objective checks from subjective information. |
| Decision panel | Approve/reject, later conditional approval | Required | M1 can ship with approve/reject only if model reserves conditional. |
| Active contract map | Track approved contracts on routes and accident states | Later | Important for result system, not required before active contract state exists. |
| Insurance claim sheet | Display accident claim details | Later / unresolved | Linked Notion page exists but is empty. |
| Settlement receipt | Show salary, commission, penalties, balance, rank score | Later | Economy data is partly confirmed but exact sheets are inaccessible. |
| Letter | Private special-contractor feedback | Later | After-story channel. |
| Port board | Public outcomes, notices, thanks, and announcements | Later | After-story/channel throttling applies. |
| Office gift/object | Persistent visible consequence | Later | After-story channel. |
| Inspection notice | Systemic punishment or formal review | Later | Used for bribe/fine/evaluation consequences. |

## Contract Management Map

Confirmed behavior from `계약 관리함 시스템 상세`:

- Approved contracts create ship icons on the map.
- Clicking a ship shows contract data.
- Routes connect Rissebra to destination port cities.
- Each contract has a `RouteID` and moves along that route.
- Accident contracts become red on the accident date and display accident date/reason.

Do not implement map animation before contract outcome state and route data are stable.

## Outcome Ordering

When multiple result surfaces compete for attention:

1. Accident reports should appear before settlement numbers.
2. Special contractor and after-story consequences should appear before purely numerical cleanup when they are tied to the same prior decision.
3. Board and letter output should be capped at roughly two items per turn, with overflow queued.

## First Playable UI Scope

Required:

- Turn/date header.
- Contract list for 2-3 ship cases.
- Bundle 1-3 document viewer.
- Checklist/rule findings for representative AR/CR cases.
- Approve/reject decision.
- Immediate result/debug summary for validation.

Deferred:

- Full 24-turn calendar UX.
- Cargo and mixed document views.
- Animated contract map.
- Insurance claim sheet.
- Full economy settlement UI.
- Dialogue presentation system.
- Letters, gifts, board, and inspection channels.

## UI Copy And Terminology

Use the current maritime vocabulary:

- Underwriter.
- Contractor or applicant.
- Contract case.
- Ship insurance, cargo insurance, mixed contract.
- Absolute rejection.
- Rejection consideration.
- Rissebra.

Avoid old reference terms such as adventurer, monster, location countermeasure, or quest unless a future design decision explicitly restores them.

