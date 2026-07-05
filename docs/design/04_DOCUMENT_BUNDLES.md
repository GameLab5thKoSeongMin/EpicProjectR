# Document Bundles

Last inspected: 2026-07-05.

## Bundle Progression Summary

The current full-game plan keeps ship bundles 1-3 from the vertical slice and adds cargo/mixed bundles 4-7. The older `서류 묶음` page says an older "bundle 4" was removed because the crew list moved to subjective judgment. That note is historical for the ship prototype and should not override the current full-game bundle 4 cargo plan.

## Bundle Table

| Bundle ID | Documents included | Turn range if known | Purpose | Required rule support | Implementation priority |
| --- | --- | --- | --- | --- | --- |
| Bundle 1 | Ship insurance application + ship registration certificate | Full game turn 1; vertical slice turn 1 | Teach identity and validity comparison | Missing registration, ship name mismatch, owner mismatch, registration expiration, old hull consideration | First |
| Bundle 2 | Bundle 1 + hull inspection certificate | Full game turns 2-3; vertical slice turn 1 | Add hull condition and inspection judgment | Missing hull inspection, failed inspection, accident history, repair history, hull defect | First |
| Bundle 3 | Bundle 2 + route declaration | Full game turns 4-6 and later mixed phase in parallel | Add route, departure date, return date, weather, map risk | Missing route declaration, departure date mismatch, bad weather, uncharted route, banned area when news active | First playable after bundle 1-2 |
| Bundle 4 | Cargo insurance application + cargo manifest | Full game turn 7 | Start cargo insurance | Cargo identity, declared value versus reference value, cargo type basics | Later |
| Bundle 5 | Bundle 4 + loading certificate | Full game turn 8 | Add packing/loading state | Missing loading certificate, packing/loading risks | Later |
| Bundle 6 | Bundle 5 + route declaration | Full game turn 10 | Add cargo-route interaction | Cargo x route risk, weather/route effects, high-value cargo news | Later |
| Bundle 7 | Ship + cargo full documents, shared route declaration | Full game turn 12 onward | Mixed-contract synthesis | Cross-domain rule combination, news-driven criteria, special contractor arcs | Later production |
| Temporary quarantine certificate | Quarantine certificate for Baleca-bound relevant contracts | Full game turns 19-22, Baleca route/cargo cases | News-created temporary required document | AR16 activation, route/destination filter, optional document visibility | Postpone until news/rule windows exist |

## Ship Insurance Documents

Confirmed ship document fields from `서류 묶음` and `양식`:

- Ship insurance application: ship name, owner name, captain name, departure date.
- Ship registration certificate: ship name, owner name, build age/year, expiration date.
- Hull inspection certificate: inspection result, accident history, repair history, hull defect/mechanic inspection.
- Route declaration: departure date, departure port, destination port, expected return date, weather forecast, route map.

## Cargo Insurance Documents

Confirmed at high level only:

- Cargo application.
- Cargo manifest.
- Loading certificate.
- Route declaration for bundle 6.
- Quarantine certificate for temporary route/news cases.

Exact cargo fields should come from `02_Documents.xlsx` sheets such as `CargoApplication`, `CargoManifest`, `LoadingCert`, and `QuarantineCert`. These files were not accessible in this task.

## Mixed Contract Documents

Bundle 7 combines ship and cargo documents. The route declaration is shared. This means implementation should not hard-code "one route document per domain"; route data should be reusable across ship and cargo rule checks.

## First Implementation Range

Implement bundle 1 first, then bundle 2, then bundle 3. Do not implement bundle 4-7 in M1. M1 should only model enough extensibility that bundle 4-7 can be added later without renaming core concepts.
