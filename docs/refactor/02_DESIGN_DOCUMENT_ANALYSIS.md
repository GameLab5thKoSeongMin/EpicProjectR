# Design Document Analysis

## Sources

Local exported markdown design docs were not available. `docs/design/` did not exist during analysis.

Notion access succeeded. The following pages were fetched and used:

- `Epic`: https://app.notion.com/p/39307d58984f809d8090c1b0c2933288
- `Rissebra's Underwriter - main 24-turn plan`: https://app.notion.com/p/6db07d58984f822fa8d401075c05b909
- `Game structure and 24-turn difficulty curve`: https://app.notion.com/p/d5007d58984f8223be258122ea43cd99
- `Underwriting rule expansion - document bundles 4-7 and news timeline`: https://app.notion.com/p/48607d58984f820db968013880f4c556
- `Special contractors 15`: https://app.notion.com/p/74907d58984f82d0b59d017730755551
- `After-story system`: https://app.notion.com/p/23f07d58984f83e48a4881b9ce41e1a4
- `Economy diagnosis and improvement`: https://app.notion.com/p/f9707d58984f8371abdd811e66b74a3f
- `Data table guide`: https://app.notion.com/p/56407d58984f8366a4790129f613184e
- `Ship insurance vertical slice`: https://app.notion.com/p/36e07d58984f83f3b31381ea558c6318
- `Document bundle`: https://app.notion.com/p/b2407d58984f83eea0de01b9e372a036
- `Result notification system`: https://app.notion.com/p/9f407d58984f82d49f6c812f251bc2e7
- `Contract management system detail`: https://app.notion.com/p/25307d58984f8235966301c6a09ce721
- `Insurance claim detail`: https://app.notion.com/p/d9207d58984f8384b1f901747f7dde24

## Confirmed from Design Docs

### Core Game Concept

The game is a maritime insurance underwriting simulation. The player works as an underwriter in the fictional city of Rissebra, reviewing contract applications, document bundles, rules, news, subjective information, and later consequences.

The main game is planned as 24 monthly turns from 1599-01-15 to 1600-12-15. Each turn has 7 contracts, for 168 total contracts. The design separates special contractors from regular contracts and explicitly plans 15 special contractors across 37 appearances.

### Core Player Fantasy

The player fantasy is not only "find the correct rule." It is being responsible for underwriting decisions where objective rules, subjective information, compassion, pressure, and later consequences collide. The design repeatedly emphasizes dilemmas: compassion versus standards, rules versus life, subjective information versus documents, correct rejection versus emotional cruelty, and trust after betrayal.

### Core Gameplay Loop

Confirmed loop:

1. Start a workday/turn.
2. Read current news/rule changes.
3. Receive contract applications.
4. Inspect document bundle.
5. Compare fields and rules.
6. Consider subjective information where present.
7. Choose approve, conditional approval, or reject.
8. Track approved contracts.
9. Resolve accidents/outcomes later.
10. Receive settlement, claims, letters, board posts, gifts, and inspection results.
11. Advance to the next turn.

### Player Actions

Confirmed actions include:

- Inspect insurance applications and document bundles.
- Compare names, dates, ownership, route, weather, cargo value, certificates, and news-based criteria.
- Select/check rejection or consideration reasons.
- Approve, conditionally approve, or reject.
- Read news and result feedback.
- Review approved contracts on a map-like contract management screen.

### Underwriting / Review Flow

Ship insurance starts with document bundles 1-3:

- Bundle 1: ship insurance application plus ship registration certificate.
- Bundle 2: bundle 1 plus hull inspection certificate.
- Bundle 3: bundle 2 plus route declaration.

The main 24-turn design expands later:

- Bundle 4: cargo application plus cargo manifest.
- Bundle 5: adds loading certificate.
- Bundle 6: adds route declaration.
- Bundle 7: mixed ship plus cargo documents.
- Quarantine certificate appears temporarily in turns 19-22 for relevant contracts.

The rule system has absolute rejection (`AR`) and rejection consideration (`CR`). Absolute rejection covers missing documents, mismatches, expired documents, failed hull inspection, banned areas, contraband, and other must-reject risks. Consideration reasons are risks that can be priced or absorbed, such as old hull, accident history, repairs, defects, bad weather, dangerous cargo, or elevated routes.

### Approval and Rejection Flow

The design requires objective correctness and subjective responsibility to be separated:

- Objective score comes from checked criteria and correct action.
- Subjective deviations are not scored directly; they matter through later outcomes.
- If an approved or conditionally approved contract has `AccidentFlag = 1`, an accident occurs deterministically on the return date.
- Rejected contracts do not produce regular accident outcomes, though special contractors can still have after-story branches.

### UI / UX Flow

Confirmed UI surfaces:

- Workdesk/document review.
- Rule/checklist or standard table.
- News/newspaper.
- Contract management map for approved contracts.
- Insurance claim documents for approved contracts with accidents.
- Letters and gifts for special contractor feedback.
- Port board for public outcomes.
- Settlement/evaluation screen.

The contract management detail page specifies that approved contracts appear as ship icons on a map route, with accident contracts changing red when the accident date arrives.

### Data and Content Requirements

The main design expects 8 programmer-facing Excel files:

- `01_Contracts.xlsx`: 168-contract master table.
- `02_Documents.xlsx`: 8 document sheets joined by `ContractID`.
- `03_SubjectiveInfo.xlsx`: 40 subjective information rows.
- `04_UnderwritingRules.xlsx`: 18 absolute rejection rules, 12 consideration rules, 13 news rows, parameters.
- `05_Economy.xlsx`: rank, income, expense, simulation.
- `06_SpecialContractors.xlsx`: 15 profiles and arcs.
- `07_Dialogues.xlsx`: dialogue rows.
- `08_AfterStories.xlsx`: 45 after-story events.

The data guide specifies a header convention: row 1 Korean name, row 2 type, row 3 English/code column name, row 4+ data. `ContractID` uses `C###`, and `C{(turn-1)*7+slot}`.

### Worldbuilding Terms

Important terms:

- Rissebra: fictional player city.
- Ducat: currency.
- Underwriter: player role.
- Ship insurance, cargo insurance, mixed contract.
- Special contractor.
- Absolute rejection.
- Rejection consideration.
- Accident potential / `AccidentFlag`.
- News-driven rules.
- Contract management map.
- After-story.

### Important Constraints

- Ports and names are fictional.
- Accident potential is not shown in player UI.
- Subjective information does not mechanically change other data; it connects only to that contract's accident/outcome.
- Rules can activate, strengthen, and deactivate through news.
- Accident outcome is deterministic when approved and flagged.
- Special contractor rejection routes should still preserve story feedback.
- News/letters/board posts should avoid feedback overload; the after-story page says board/letter exposure should be capped and queued.

### Current Design Priorities

- Build a reliable content-driven underwriting loop.
- Separate objective rules from subjective dilemmas.
- Support 24-turn deterministic progression.
- Support expanding document bundles and active rule schedules.
- Make outcomes legible before financial numbers.
- Keep economy painful but not spiral-prone.
- Make content validation strong enough for many rows and joins.

### Explicitly Postponed or Not Fully Detailed

- `Insurance claim detail` page is empty.
- The Notion page references Google Drive Excel files, but they were not accessed in this task.
- Full implementation details for save/load are not specified.
- Exact Unity scene/prefab layout for the new production project is not specified.
- Some result channels are conceptually defined but not fully specified.

## Inferred from Design Docs

- The new project should be table-driven rather than random-case-driven.
- Rule evaluation should be deterministic and ordered for reproducible tests.
- News should produce rule activation windows rather than directly mutating scattered UI state.
- `ContractID`, rule IDs, route IDs, special contractor IDs, and document IDs should be stable IDs, not display text.
- The first implementation should focus on ship insurance bundle 1-3, because both the vertical slice and main game begin there.
- Cargo and mixed insurance should be designed into the model early, but implemented after the ship loop is stable.
- The "adventurer" terminology in the prompt and old README is not the current design vocabulary. The current Notion docs use contractors, ships, cargo, and underwriting cases.

## Unclear / Needs Confirmation

- Whether the Google Drive Excel sheets are final source of truth and whether they will be imported into Unity directly.
- Whether the new Unity project should use Unity Localization, JSON, ScriptableObjects, or a hybrid import pipeline.
- Whether conditional approval is a final required action or a prototype term that maps to "approval with premium increase."
- Whether the player can adjust premium manually or only through checked consideration reasons.
- Whether all after-stories should be authored as data tables or some should be scene/timeline events.
- Whether the game should preserve the reference project's direct dialogue bubble/timeline implementation.
- Whether "adventurer grade/license/qualification" from the request should be renamed to contractor/license/ship/cargo qualifications in the current marine design.

## Conflicts with Reference Implementation

| Area | Design Direction | Reference Project State | Conflict |
| --- | --- | --- | --- |
| Accidents | Deterministic from `AccidentFlag` when approved/conditional | `GameLoopController.RollAccidentOccurrence()` uses probability and `Random.value` | Must rebuild outcome logic. |
| Content source | 168 fixed contracts joined by table IDs | Fixed SO cases plus random case generation | Use SOs as definitions, not primary content authoring unless imported. |
| Economy | Evaluation score and money separated | Old `GameLoopController` mixes performance-share income and has incomplete `CalculateMoneyEarned()` | Rebuild economy from design. |
| Scope | Ship, cargo, mixed, news, after-stories | Mostly ship insurance prototype | Cargo/mixed systems missing. |
| Rule activation | Start/end turns and news timeline | Rejection reasons attached to insurance type | Need active rule schedule service. |
| Subjective info | Outcome-only, no objective score penalty | Dialogue/profile system exists but no full subjective table flow | Need explicit subjective info model. |
| Document schema | Excel tables with stable column names | String keys in prefabs/SOs with inconsistencies | Need schema validation. |
