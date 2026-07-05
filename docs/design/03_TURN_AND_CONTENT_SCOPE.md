# Turn and Content Scope

Last inspected: 2026-07-05.

## Full Game Scope

The current full-game design is 24 turns over 2 years. The main Notion page gives the period as 1599-01-15 through 1600-12-15, with one workday per month, 7 contracts per turn, and 168 total contracts.

Special contractor scope is confirmed at the summary level: 15 special contractors, 37 special appearances, and 131 general contracts.

## Phase Structure

| Phase | Turns | Insurance focus | Design purpose |
| --- | --- | --- | --- |
| Part 1 | 1-6 | Ship insurance only, bundles 1-3 | Teach document comparison, hull judgment, route/weather, subjective information. |
| Part 2 | 7-11 | Cargo insurance only, bundles 4-6 | Teach declared value versus market value, packing/loading, cargo-route risk. |
| Part 3 | 12-24 | Mixed ship + cargo, bundle 7 with bundles 3 and 6 still present | Combine all criteria, news changes, and special contractor arcs. |

## Scope Table

| Scope item | Confirmed value | Source | Implementation implication | Risk |
| --- | --- | --- | --- | --- |
| Full game date range | 1599-01-15 to 1600-12-15 | Main 24-turn plan | Use `GameDate`/turn definitions; avoid real current date dependencies. | Exact per-turn day values beyond month may need table confirmation. |
| Full game turn count | 24 turns | Main 24-turn plan | Turn schedule should be fixed and deterministic. | None for high-level scope. |
| Contracts per full turn | 7 contracts | Main 24-turn plan | Contract queue must support 7 slots per turn. | Exact rows inaccessible without Excel. |
| Total contracts | 168 contracts | Main 24-turn plan | `ContractId` range expected `C001`-`C168`. | Do not author all rows manually from summaries. |
| Special contractors | 15 contractors | Special contractors page | Model `SpecialContractorId` such as `SC01`. | Branch details still depend on Excel. |
| Special appearances | 37 appearances | Main plan and special contractors page | `SpecialID` links repeated contract rows. | Exact branch outcomes need external tables. |
| General contracts | 131 contracts | Main 24-turn plan | General templates and dialogue tones needed later. | Exact rows inaccessible. |
| News count | 13 news entries | Rule expansion page | `NewsTimeline` should activate/deactivate rules by turn. | Exact article copy/table rows missing. |
| Absolute rejection rules | 18 AR rules | Main plan / rule expansion page | Rule model must support ID, active window, conditions. | Not all exact AR rows visible. |
| Rejection consideration rules | 12 CR rules | Main plan / rule expansion page | CR pricing policy needed. | Not all exact CR rows visible. |
| Vertical slice | 4 turns / 10 contracts, 1598-03-18 to 1598-12-17 | Ship insurance vertical slice page | Useful first playable reference. | Prototype dates/scope should not override full-game dates. |
| First sample contract | Bundle 1 clean approval tutorial | `1턴 첫 번째 계약 · 산타 루지아` | Useful M0/M1 fixture only. | Individual pages should not replace final Excel rows. |

## First Playable Recommendation

Use the ship vertical slice as a first playable inspiration, but implement a smaller technical slice first:

1. One turn with 2-3 ship contracts.
2. Bundle 1 and bundle 2 first.
3. Add bundle 3 once date/name/hull checks are stable.
4. Include one clean approval, one absolute rejection, and one consideration reason.
5. Use deterministic outcome tests, not final UI-heavy consequence systems.

## Full Game Skeleton Recommendation

The full-game skeleton should be planned early but not fully content-filled:

- `TurnDefinition` for 1-24.
- `ContractId` conventions for `C001`-`C168`.
- Rule activation windows.
- News entries by turn.
- Placeholder repository support for special contractor IDs and after-story IDs.

## Features to Postpone

- Full cargo and mixed contract UI.
- Full 168-row content import.
- Final after-story branch writing.
- Final claim document UI.
- Persistent gifts/office decoration.
- Save/load UI.
- Imported reference project scene/prefab reuse.
