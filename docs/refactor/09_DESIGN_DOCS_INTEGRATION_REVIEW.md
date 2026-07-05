# Design Docs Integration Review

Date: 2026-07-05

## Purpose

This review compares the previous refactor analysis package with the current Notion documentation pass and the new local `docs/design/` baseline.

## Notion Sources Rechecked

The following pages were accessed during this pass:

- `에픽`
- `리세브라의 언더라이터 - 본편 24턴 기획`
- `1. 게임 구조 & 24턴 난이도 곡선`
- `2. 언더라이팅 기준 확장 - 서류 조합 4~7과 뉴스 타임라인`
- `3. 특수 계약자 15인`
- `4. 애프터 스토리 시스템`
- `5. 경제 시스템 진단과 개선안`
- `6. 데이터 테이블 가이드`
- `선박 보험 버티컬 슬라이스`
- `서류 묶음`
- `양식`
- `결과 고지 시스템`
- `계약 관리함 시스템 상세`
- `보험금 청구서 상세`
- `재윤 To-do list`
- `1턴 첫 번째 계약 - 산타 루지아`
- `Epic Project 관련`

The external Google Drive data folder and linked Excel workbooks were not accessed.

## Confirmed From Previous Analysis

| Previous finding | Current result |
| --- | --- |
| The reference project should be treated carefully and not imported wholesale. | Confirmed. `ref_folder/` is now explicitly reference-only. |
| The project needs a domain-first refactor path. | Confirmed. Rule, contract, outcome, and validation docs now support that direction. |
| Accident behavior should be deterministic from design data. | Confirmed. `AccidentFlag = 1` plus approval causes accident; rejection prevents it. |
| The design needs local source-of-truth docs. | Confirmed and completed under `docs/design/`. |
| UI should follow the new underwriting loop instead of old fantasy reference UI. | Confirmed. Current terminology and UI flow are maritime underwriting focused. |

## Major Corrections Or Clarifications

| Area | Previous ambiguity | Current clarification |
| --- | --- | --- |
| Source hierarchy | Notion versus local docs versus reference project was not fixed. | `docs/design/10_IMPLEMENTATION_SOURCE_OF_TRUTH.md` is the first local read; Notion remains upstream evidence. |
| Bundle 4 | Older `서류 묶음` page said a prior bundle 4 was removed. | Current full-game bundle 4 is cargo application + cargo manifest. The older note is historical. |
| Terminology | Prompt/reference history included adventurer-like terms. | Current production vocabulary is underwriter, contractor/applicant, contract case, route, document bundle. |
| Insurance claim detail | Result system implied claim documents. | The linked `보험금 청구서 상세` page exists but was empty, so claim detail is unresolved. |
| To-do list counts | Supporting To-do notes included rough content counts. | The 24-turn master plan wins: 168 contracts, 15 special contractors, 37 appearances, 131 general contracts. |

## New Design Confirmations

- Full game runs for 24 monthly turns from 1599-01-15 to 1600-12-15.
- Each turn has 7 contracts.
- The game has three phases: ship, cargo, then mixed.
- The first year teaches systems; the second year increases difficulty, narrative pressure, and money pressure.
- There are 18 absolute rejection rules and 12 consideration rules in the intended rule table.
- There are 13 scheduled news entries in the current rule/news plan.
- There are 8 named source workbooks, but they were inaccessible during this pass.
- Special contractor IDs are `SC01` to `SC15`.
- After-story output channels are board, letter, gift, and inspection.
- After-story events are throttled so only a small number surface per turn.

## Impact On Milestones

| Milestone | Impact |
| --- | --- |
| M0 | Generate an implementation prompt based on the new source hierarchy and repository boundaries. |
| M1 | Build a ship-only domain/rule/outcome model with representative bundle 1-3 fixtures. |
| M2 | Add data import/validation only after source spreadsheets or local exports are available. |
| M3 | Build minimal workdesk/document/decision UI over validated fixtures, not full result-map UI. |
| Later | Add cargo, mixed contracts, special contractors, economy, map, claims, and after-story once data shape is locked. |

## Retired Assumptions

- Do not assume the fantasy/adventurer reference domain is still active.
- Do not assume bundle numbering from the old ship-only prototype applies to the full game.
- Do not assume accident probability rolls.
- Do not assume subjective information changes scoring.
- Do not assume claim sheet details are specified.

## Remaining Risk

The linked Excel workbooks were not accessible. Exact contract rows, document fields, rule IDs, economy values, dialogue, and after-story events still require source-file access or explicit designer confirmation.

