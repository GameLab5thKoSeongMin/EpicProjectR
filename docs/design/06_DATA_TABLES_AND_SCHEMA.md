# Data Tables And Schema

Last inspected: 2026-07-05.

## Table Header Convention

The Notion data guide confirms this workbook layout:

- Row 1: Korean display column name.
- Row 2: type.
- Row 3: English/code column name.
- Row 4 onward: data.
- `#` marks localizable text fields.

Importers should read by the row-3 code name, not by visible Korean labels.

## Confirmed Source Workbooks

| Workbook | Primary key | Confirmed purpose | Important joins | Access status |
| --- | --- | --- | --- | --- |
| `01_Contracts.xlsx` | `ContractID` (`C001` to `C168`) | 168 master contract rows | `RouteID`, `SpecialID`, `SubjectiveID`; child rows in documents/dialogue/after-story | Not accessed |
| `02_Documents.xlsx` | likely document row IDs plus `ContractID` | 8 document sheets and document defects | `ContractID` back to contracts | Not accessed |
| `03_SubjectiveInfo.xlsx` | `SubjectiveID` | Contract-bound subjective information | `SubjectiveID` from contracts | Not accessed |
| `04_UnderwritingRules.xlsx` | `RuleID` (`AR##`, `CR##`) | 18 absolute rejection rules, 12 consideration rules, 13 news entries, parameters | Rule windows, news, routes, document fields | Not accessed |
| `05_Economy.xlsx` | rank/economy IDs | Salary, commission, penalties, rank thresholds, simulation | Contract outcomes and settlement | Not accessed |
| `06_SpecialContractors.xlsx` | `SpecialID` (`SC01` to `SC15`) | Special contractor profile and arc metadata | Contracts, dialogue, after-story | Not accessed |
| `07_Dialogues.xlsx` | dialogue row ID | 221 dialogue rows, templates, special lines | `RefID` values such as `C###`, `TPL_...`, `TURN##` | Not accessed |
| `08_AfterStories.xlsx` | after-story event ID | 45 consequence events | `SourceContractID`, trigger condition, channel | Not accessed |

## Contract ID Convention

Confirmed contract ID formula:

```text
ContractID = C{(turn - 1) * 7 + slot}
```

Example: turn 12 slot 4 is `C081`.

This formula should be used by validation tools to detect missing, duplicated, or misnumbered rows.

## Core Contract Shape

Minimum implementation-facing contract fields:

| Field | Meaning | Status |
| --- | --- | --- |
| `ContractID` | Stable ID, `C001` to `C168` | Confirmed |
| `Turn` | 1 to 24 | Confirmed |
| `Slot` | 1 to 7 within turn | Confirmed |
| `ContractType` | ship, cargo, or mixed | Confirmed concept |
| `BundleID` | document bundle 1-7 | Confirmed concept |
| `RouteID` | route lookup ID | Confirmed concept |
| `ApplicantName` / `ContractorName` | applicant display name | Confirmed concept |
| `Premium` | base premium | Confirmed concept |
| `Coverage` | insured amount | Confirmed concept |
| `CorrectAction` | expected objective decision | Confirmed concept |
| `AccidentFlag` | hidden deterministic accident marker | Confirmed |
| `SpecialID` | optional `SC##` | Confirmed |
| `SubjectiveID` | optional subjective info row | Confirmed |

Exact column names should be aligned with row-3 workbook headers once files are available.

## Document Data Shape

`02_Documents.xlsx` has one row only for documents active in the contract's bundle. Missing documents are represented by `Submitted = 0`; mismatches are injected into document fields.

Recommended local model:

| Concept | Recommended representation |
| --- | --- |
| Document type | Stable enum/string such as `ShipApplication`, `ShipRegistration`, `HullInspection`, `RouteDeclaration`, `CargoApplication`, `CargoManifest`, `LoadingCertificate`, `QuarantineCertificate` |
| Submitted state | Boolean field on document record |
| Display fields | Typed fields where known, generic key/value fallback where not final |
| Defects | Detected by rules from field comparisons, not by hard-coded UI labels |

## Rule Data Shape

Rules should support:

- `RuleID`: `AR##` or `CR##`.
- `Severity`: absolute rejection or consideration.
- `StartTurn` / `EndTurn`.
- Optional `NewsID`.
- Optional `BundleID`.
- Optional `RouteID` or route/destination filter.
- Optional document type and field conditions.
- Player-facing explanation key.
- Premium multiplier or recommendation for `CR` rules.

Rule `AR18` has at least two active windows: turns 12-13 and turns 23-24. Avoid a model that assumes each rule has only one continuous date range.

## Route IDs

`R0005` is confirmed as the Baleca route/destination filter for the quarantine certificate window. Other route IDs and map coordinates were not accessible in this task.

Model route data by ID first. Coordinates, curves, map art, and animated movement can be added after route tables are available.

## Validation Priorities

When source spreadsheets become available, validate:

1. `C001` to `C168` are complete and unique.
2. Every contract has exactly one expected turn and slot.
3. Every referenced `RouteID`, `SpecialID`, and `SubjectiveID` resolves or is explicitly empty.
4. Document rows exist only for active bundle documents.
5. Missing documents use `Submitted = 0`.
6. Active rule windows can handle discontinuous ranges.
7. Approved or conditional `AccidentFlag = 1` rows produce accidents; rejected rows do not.
8. Subjective info does not affect objective scoring.

