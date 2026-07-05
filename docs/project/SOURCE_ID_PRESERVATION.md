# Source ID Preservation

Last updated: 2026-07-05.

## Rule

Source IDs from Notion design docs and future spreadsheet rows must be preserved exactly.

Do not modify, rename, normalize, auto-generate, re-number, lowercase, uppercase, localize, enum-convert, or reinterpret source IDs.

Examples:

- `C001` remains `C001`.
- `AR01` remains `AR01`.
- `CR01` remains `CR01`.
- `SC01` remains `SC01`.
- `R0005` remains `R0005`.

## Runtime Implementation

The first playable implementation stores IDs in value objects under `EpicProjectR/Assets/Scripts/Domain/Identifiers.cs`.

These value objects:

- Store the exact provided string.
- Return the exact string from `ToString()`.
- Compare equality by exact string and ID kind.
- Validate known source forms but do not silently transform them.

## ID Kinds

| Kind | Meaning | Current use |
| --- | --- | --- |
| `SourceAuthored` | Comes from design/source data. | `C001`, `AR01`, `CR01`, `SC01`, `R0005`. |
| `Fixture` | Temporary first playable fixture ID or field key. | Document IDs such as `C001-APP`, bundle IDs such as `BUNDLE_1`, field IDs such as `ShipName`. |
| `RuntimeGenerated` | Created by runtime systems for transient output. | Outcome IDs such as `OUT-C001`. |
| `DisplayKey` | UI/localization/display key. | Reserved. |

## Fixture Warning

The first playable uses `C001`, `C002`, and `C003` as fixture contract IDs because they validate the source contract ID convention and turn/slot formula. Every fixture `ContractCase` is marked with `IsFixture = true`.

These rows are not final 168-contract data and must be replaced or validated against source workbooks when those files become available.

## Validation Expectations

Every implementation pass should confirm:

- No source ID was silently normalized.
- Fixture IDs are marked as fixtures.
- Importers, if added later, read code/source IDs exactly.
- UI may display labels, but debug/source ID fields preserve exact IDs.

