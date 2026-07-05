# Implementation Risks And Gates

Last updated: 2026-07-05.

## Risk Table

| Risk or question | Affected milestone | Blocking? | Default assumption | Required human decision | Mitigation |
| --- | --- | --- | --- | --- | --- |
| Excel source files inaccessible | M11, full content | Blocks full import, not M1-M8 | Use in-memory fixtures | Provide files or approve alternate format | Keep importer later and data-driven. |
| Exact AR/CR rows unresolved | M3, M11 | Blocks full rule content, not representative engine | Use small ship subset | Confirm final rule rows | Build extensible condition model. |
| Conditional approval UX/scoring unresolved | M4, M7, M8 | Does not block M1-M3 | Reserve enum/model, omit UI action | Confirm semantics before UI | Tests cover approve/reject first. |
| Premium adjustment method assumed | M4, M7 | Partial blocker for final UI | Use CR-driven quote only | Confirm freeform vs rule-driven | Keep premium policy isolated. |
| Route coordinates unavailable | M7+, map polish | Blocks map animation, not first loop | Model route by ID only | Provide route table/art plan | Defer map animation. |
| Insurance claim detail fields unresolved | M6+, claim UI | Blocks claim sheet polish | Use minimal outcome summary | Define claim fields | Keep claim UI out of first loop. |
| Unity Localization need unknown | UI/content | Not M1 blocker | Plain strings for early fixtures | Decide before final content pipeline | Abstract display text keys. |
| Reference UI prefab reuse question | M7+ | Blocks reuse only | Do not reuse; rebuild simple UI | Explicit approval to reuse any reference asset | Keep `ref_folder/` read-only. |
| Scene/prefab modification gate | M7/M8 | Blocks asset changes | Presenter/view-model only if not approved | Approve specific scene/prefab paths | Stop before Unity asset edits. |
| ScriptableObject asset creation gate | M2/M11 | Blocks asset creation | In-memory fixtures first | Approve SO asset creation | Use code fixtures/repositories. |
| Full 168-contract import gate | M11 | Blocking | Do not import | Provide source files and schema decision | Validate fixtures first. |
| Cargo/mixed expansion gate | M11/M12+ | Blocking for expansion | Ship-only first | Confirm cargo/mixed data fields | Preserve extensible model. |
| Save/load implementation gate | M10 | Conditional | Design first if state unstable | Approve implementation timing | Serialize stable IDs only. |

## Does Not Block M1

- Excel source access.
- Exact full AR/CR rows.
- Conditional approval UI.
- Premium UI method.
- Route coordinates.
- Claim detail fields.
- Localization decision.
- Reference UI decision.

## Blocks UI Work

- Scene/prefab permission.
- Whether UI can create or modify ScriptableObject assets.
- Conditional approval UI semantics if conditional button is requested.
- UI art direction if beyond functional placeholder.

## Blocks Full 24-turn Content

- Source workbook access.
- Exact AR/CR rules.
- Route table and coordinates.
- Cargo/mixed document fields.
- Special contractor rows, dialogue rows, after-story rows.

## Blocks Economy/Settlement Polish

- `05_Economy.xlsx` access.
- Final rank thresholds, salary, costs, and simulation rows.
- Claim sheet detail fields.
- Final bribe/inspection and bankruptcy presentation rules.

