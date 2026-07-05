# Open Questions

Last updated: 2026-07-05 during the Notion design-document integration pass.

Status labels:

- `Resolved`: answered by the 2026-07-05 Notion pass or local docs.
- `Open`: still needs a human decision or source file.
- `Assumed`: safe temporary default for implementation.
- `Blocked`: cannot be resolved until an external/linked source is accessible.
- `Partially resolved`: high-level answer is known, exact content/data is not.

## Resolved Or Clarified

| Question | Status | Resolution / note | Follow-up |
| --- | --- | --- | --- |
| Is the 24-turn Notion plan the current source of truth over the vertical slice pages? | Resolved | Yes. The 24-turn master plan wins for full-game scope. The vertical slice remains prototype/first-playable context. | Read `docs/design/10_IMPLEMENTATION_SOURCE_OF_TRUTH.md` first. |
| Should "adventurer" terminology remain anywhere? | Resolved | No for production vocabulary. Use underwriter, contractor/applicant, contract case, route, and document bundle. | Keep adventurer only as obsolete/reference terminology. |
| Should local exports be added under `docs/design/`? | Resolved | Yes. Local implementation-facing design docs were created in this pass. | Keep them updated when upstream design changes. |
| Should future analysis cite Notion pages or exported/local docs? | Resolved | Prefer local `docs/design/` for implementation tasks; cite Notion as upstream evidence when rechecked. | Recheck Notion for latest design or unresolved content. |
| Is the old ship-prototype "bundle 4 removed" note still active? | Resolved | No. It is historical. Current full-game bundle 4 is cargo application + cargo manifest. | Do not let old bundle numbering drive implementation. |
| Is accident occurrence probabilistic? | Resolved | No. `AccidentFlag = 1` plus approval/conditional approval deterministically causes accident. | Add tests when domain code exists. |
| Does subjective information affect objective score? | Resolved | No. It affects accident/narrative consequences only, not objective scoring. | Keep rule and subjective systems separate. |
| Are the To-do list rough counts authoritative? | Resolved | No. The 24-turn master plan wins: 168 contracts, 15 special contractors, 37 special appearances, 131 general contracts. | Use To-do list only as supporting production/UI notes. |
| Is `보험금 청구서 상세` accessible? | Partially resolved | The Notion page was accessible but empty. | Claim detail design remains open. |

## Design Questions

| Question | Status | Why it matters | Suggested default assumption |
| --- | --- | --- | --- |
| Is conditional approval a final player action? | Open | It affects decision enum, premium UI, scoring, and contractor response. | Support approve/reject first; reserve conditional approval in the model. |
| Can the player manually change premium, or only through checked CR reasons? | Assumed | Changes pricing UI and validation. | Use rule-driven premium adjustment only until confirmed. |
| How should subjective information be presented and submitted? | Open | It affects UI, dialogue timing, and outcome explanation. | Store subjective info as display/event data tied to `SubjectiveID`; do not feed it into AR/CR rules. |
| Are all 15 special contractor arcs final at the data-row level? | Partially resolved | High-level arcs are confirmed, exact rows/branches are not. | Reserve model support for 15, implement first few as fixtures. |
| What is the first playable target: 2-3 contract fixture, 4-turn/10-contract vertical slice, or full 24-turn skeleton? | Open | Affects M1-M4 milestone size. | Start with 2-3 ship fixtures, then expand to 4-turn ship slice. |
| Should individual vertical-slice contract pages be exported locally, or should only final Excel rows be used? | Open | Determines whether prototype contract rows can become test fixtures. | Use them only as temporary fixtures unless final data tables are unavailable. |

## Technical Questions

| Question | Status | Why it matters | Suggested default assumption |
| --- | --- | --- | --- |
| Will final data arrive as Excel, CSV, JSON, or ScriptableObjects? | Open | Determines importer and validation pipeline. | Build repositories over in-memory definitions first; add import adapter later. |
| Should Unity Localization be used in the new project? | Open | Reference uses it; current active project may not need it yet. | Use plain strings for M1-M3, keep text source abstract. |
| What Unity test framework setup should be used? | Open | Determines test folders and assemblies. | Use Unity Test Framework edit-mode tests. |
| Are Addressables needed? | Open | Reference uses Addressables, but current first playable does not require them. | Do not add Addressables until content volume requires it. |
| Should generated constants be allowed? | Open | Helps prevent string-key drift once tables stabilize. | Start with typed ID wrappers and validators; generate constants later if schema stabilizes. |
| What namespace should production code use? | Open | Affects code organization and consistency. | Use `EpicProjectR` or a team-approved namespace before coding. |

## Unity Scene / Prefab Questions

| Question | Status | Why it matters | Suggested default assumption |
| --- | --- | --- | --- |
| Should any reference UI prefab be reused visually? | Open | Copying prefabs may import hidden dependencies. | Do not copy prefabs; rebuild minimal views. |
| Which scene should become the production gameplay scene? | Open | Active project scene strategy is not documented in the design pass. | Create a new gameplay scene in a later UI milestone, not during domain work. |
| Is the contract map required in first playable? | Assumed | It is important but depends on active contract state and route data. | Postpone full map until after core outcome state works. |
| Should Timeline sequences be preserved? | Open | Reference has timeline code/assets. | Postpone until dialogue/outcome systems are stable. |
| Which UI art direction should be followed? | Open | Affects first UI implementation. | Use simple functional underwriting UI first. |

## Data / Content Questions

| Question | Status | Why it matters | Suggested default assumption |
| --- | --- | --- | --- |
| Can Codex access the Google Drive `계약 관리 데이터 모음` folder? | Blocked | It appears to contain the exact source workbooks. | Not accessible in this task; build schema from Notion summaries only. |
| Are `01_Contracts.xlsx` through `08_AfterStories.xlsx` final? | Blocked | They define exact schema, IDs, and rows. | Treat workbook names and joins as intended shape, not final column list. |
| What are the exact 18 AR and 12 CR definitions? | Blocked | Needed for full rules. | Implement a small representative ship subset first. |
| What are exact cargo field names and value-ratio formulas? | Blocked | Needed for bundles 4-7. | Keep generic document fields and numeric ratio condition support ready. |
| How are route IDs and route map coordinates defined? | Blocked | Needed for contract map and route animation. | Model routes by ID first; add coordinates later. |
| Is `R0005` definitely Baleca in final route data? | Partially resolved | Quarantine logic references Baleca/Baleca-bound contracts. | Use only as a temporary fixture until route table is accessible. |
| What are exact insurance claim fields? | Open | The claim detail page is empty. | Use minimal accident summaries until claim content is provided. |
| Who owns rule/content validation sign-off? | Open | Many rows require designer review. | Human designer should review validation reports before content lock. |

## Repository / Process Questions

| Question | Status | Why it matters | Suggested default assumption |
| --- | --- | --- | --- |
| Is the old reference project expected to be deleted later? | Open | Affects repository cleanup. | Keep `ref_folder/` until explicit approval to archive/remove. |
| Should future milestones modify scenes/prefabs alongside domain code? | Assumed | Mixing domain and Unity asset work increases review risk. | Keep domain model and UI/scene milestones separate. |
| Should M0 prompt be generated next? | Resolved | M0 was executed as a guardrails and baseline-validation milestone. M0 note: future prompts should now use `AGENTS.md`, `EpicProjectR/AGENTS.md`, and `docs/project/CODEX_TASK_TEMPLATE.md`. | Proceed to M1 only after reviewing `docs/project/M0_COMPLETION_REPORT.md`. |
