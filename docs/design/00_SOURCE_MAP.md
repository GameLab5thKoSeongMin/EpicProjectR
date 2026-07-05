# Design Source Map

Last inspected: 2026-07-05.

This file records which design sources were inspected for the local design documentation pass. Local `docs/design/` should be treated as the implementation-facing source package for future Codex tasks, while the Notion pages remain upstream design evidence.

## Primary Design Sources

| Source name | Source type | Access status | Last inspected | Short purpose | Key design areas covered | Treatment | Notes |
| --- | --- | --- | --- | --- | --- | --- | --- |
| `에픽` | Notion page | Accessed | 2026-07-05 | Game design hub under `노션 정리` | Links to main 24-turn plan, vertical slice, document bundle, result system, To-do, Google Drive data folder | Source of truth index | Contains an inaccessible external Google Drive folder link for contract data. |
| `리세브라의 언더라이터 — 본편 24턴 기획` | Notion page | Accessed | 2026-07-05 | Main design master page | 24 turns, 168 contracts, accident rule, 8 Excel data files, Rissebra setting | Primary source of truth | This should win over older prototype pages when scope conflicts. |
| `1. 게임 구조 & 24턴 난이도 곡선` | Notion child page | Accessed | 2026-07-05 | Full-game progression | 3 phases, turn table, ship/cargo/mixed progression, difficulty curve, subjective info pattern | Primary source of truth | Exact per-contract rows still defer to `01_Contracts.xlsx`. |
| `2. 언더라이팅 기준 확장 — 서류 조합 4~7과 뉴스 타임라인` | Notion child page | Accessed | 2026-07-05 | Rule and document expansion | Cargo/mixed bundles, CR premium tiers, news activation, rule strengthening/deactivation | Primary source of truth | Confirms bundle 4 in the current plan is cargo, not the removed crew-list idea in the older document page. |
| `3. 특수 계약자 15인` | Notion child page | Accessed | 2026-07-05 | Special contractor arcs | 15 contractors, 37 appearances, dilemma types, SC IDs | Primary source of truth | Exact branch rows defer to `06_SpecialContractors.xlsx`, `07_Dialogues.xlsx`, and `08_AfterStories.xlsx`. |
| `4. 애프터 스토리 시스템` | Notion child page | Accessed | 2026-07-05 | Narrative consequence system | Channels, event syntax, feedback throttling, after-story categories | Primary source of truth | Confirms channel queueing and "story before numbers" ordering. |
| `5. 경제 시스템 진단과 개선안` | Notion child page | Accessed | 2026-07-05 | Economy design | Evaluation score vs money, income, penalties, settlement receipt style, bankruptcy pressure | Primary source of truth | Exact formulas and final balance depend on `05_Economy.xlsx`. |
| `6. 데이터 테이블 가이드` | Notion child page | Accessed | 2026-07-05 | Implementation schema guide | Header convention, ID joins, accident logic, scoring, active rules, Unity import notes | Primary source of truth | Strongest implementation-facing Notion page. |

## Supporting Design Sources

| Source name | Source type | Access status | Last inspected | Short purpose | Key design areas covered | Treatment | Notes |
| --- | --- | --- | --- | --- | --- | --- | --- |
| `서류 묶음` | Notion page | Accessed | 2026-07-05 | Ship-document bundle sketch | Ship bundles 1-3, ship AR/CR examples, removed old bundle 4 note | Supporting context | Use for ship bundle 1-3 details; do not use its removed bundle 4 note over the 24-turn plan. |
| `양식` | Notion child page | Accessed | 2026-07-05 | Document field examples | Ship application, registration, hull inspection, route declaration field examples | Supporting context | Useful for first playable document field fixtures. |
| `결과 고지 시스템` | Notion page | Accessed | 2026-07-05 | Outcome feedback surfaces | Contract management, claims, letters, port board | Supporting context | Confirms required surfaces but not final implementation details. |
| `계약 관리함 시스템 상세` | Notion child page | Accessed | 2026-07-05 | Approved contract map detail | Route map, approved contract icons, accident color/status behavior, RouteID movement | Supporting context | Includes embedded images not imported locally. |
| `재윤 To-do list` | Notion page | Accessed | 2026-07-05 | UI/task notes | Calendar, document UI tweaks, approval document, rough counts | Supporting context | Not a design source of truth; use only as UI production context. |

## Prototype / Vertical Slice Sources

| Source name | Source type | Access status | Last inspected | Short purpose | Key design areas covered | Treatment | Notes |
| --- | --- | --- | --- | --- | --- | --- | --- |
| `선박 보험 버티컬 슬라이스` | Notion page | Accessed | 2026-07-05 | Prototype scope | 4 turns, 10 contracts, 1598 dates, ship bundles 1-3, first news use | Prototype source | Useful for first playable scope; full 24-turn plan wins for final scope. |
| `1턴 첫 번째 계약 · 산타 루지아` | Notion child page | Accessed | 2026-07-05 | Sample contract row | Bundle 1 clean approval tutorial, document values, tutorial flow | Prototype example | Confirms individual contract pages are accessible; exact rows should still come from exported tables if available. |

## Inaccessible or Unresolved Sources

| Source name | Source type | Access status | Last inspected | Short purpose | Key design areas covered | Treatment | Notes |
| --- | --- | --- | --- | --- | --- | --- | --- |
| Google Drive `계약 관리 데이터 모음` folder | Linked document | Inaccessible / not fetched | 2026-07-05 | Upstream spreadsheet source folder | Excel workbooks and possibly route/data assets | Unresolved primary data source | External Google Drive link was visible in Notion, but files were not accessed in this task. |
| `01_Contracts.xlsx` | Linked/external spreadsheet | Inaccessible | 2026-07-05 | 168-contract master table | Turn, slot, correct action, accident, premium | Source of truth once available | Do not invent exact rows. |
| `02_Documents.xlsx` | Linked/external spreadsheet | Inaccessible | 2026-07-05 | Document sheets | 8 document sheets, injected defects | Source of truth once available | Required before full content import. |
| `03_SubjectiveInfo.xlsx` | Linked/external spreadsheet | Inaccessible | 2026-07-05 | Subjective info table | 40 subjective info entries | Source of truth once available | Only high-level behavior is confirmed locally. |
| `04_UnderwritingRules.xlsx` | Linked/external spreadsheet | Inaccessible | 2026-07-05 | Full AR/CR/news/params table | 18 AR, 12 CR, 13 news, parameters | Source of truth once available | Notion gives model and examples, not all exact rows. |
| `05_Economy.xlsx` | Linked/external spreadsheet | Inaccessible | 2026-07-05 | Economy values and simulation | Ranks, money rules, 24-turn simulation | Source of truth once available | Notion confirms formulas and sample values but not full table. |
| `06_SpecialContractors.xlsx` | Linked/external spreadsheet | Inaccessible | 2026-07-05 | Special contractor profiles | 15 profiles, arcs, branches | Source of truth once available | Notion confirms IDs and high-level arc summaries. |
| `07_Dialogues.xlsx` | Linked/external spreadsheet | Inaccessible | 2026-07-05 | Dialogue rows | 221 dialogue rows, templates, special lines | Source of truth once available | Do not write final dialogue content from summaries. |
| `08_AfterStories.xlsx` | Linked/external spreadsheet | Inaccessible | 2026-07-05 | After-story events | 45 events, channels, triggers | Source of truth once available | Notion confirms event grammar and channels. |
| `보험금 청구서 상세` | Notion page | Accessed but empty | 2026-07-05 | Claim detail page | Insurance claim document details | Unresolved | Page exists but has no content. |

## Obsolete or Historical Sources

| Source name | Source type | Access status | Last inspected | Short purpose | Key design areas covered | Treatment | Notes |
| --- | --- | --- | --- | --- | --- | --- | --- |
| `Epic Project 관련` | Notion page | Accessed | 2026-07-05 | General game planning notes | Production process / planning stages | Historical context | Not specific enough to drive implementation. |
| `ref_folder/Assets/Scripts/README_사용법.md` | Reference project document | Accessed in previous audit | 2026-07-05 | Older fantasy/adventurer underwriting concept | Monsters, locations, adventurers, countermeasures | Obsolete | Its referenced classes were not present in the current script tree. |
| Existing `docs/refactor/` package | Previous refactor analysis | Accessed | 2026-07-05 | Prior technical analysis | Reference audit, architecture proposal, milestone plan, risks | Supporting context | Use as analysis, not as design source of truth. |

## Conflict Policy

When sources conflict:

1. Local `docs/design/10_IMPLEMENTATION_SOURCE_OF_TRUTH.md` is the first implementation read.
2. Current Notion main 24-turn plan and its six child pages win over older prototype pages.
3. Vertical slice pages guide first playable scope, not full-game scope.
4. `docs/refactor/` informs architecture but does not override current design docs.
5. `ref_folder/` is reference-only and never production source of truth.
