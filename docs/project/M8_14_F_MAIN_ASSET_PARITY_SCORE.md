# M8.14-F Main Asset Parity Score

Status: Evidence-based final score after two screenshot iterations.

## Score

| Category | Score | Evidence |
| --- | ---: | --- |
| Main direction and composition | 18 / 20 | Entry geometry and overall office/workbench/shelf direction match; exact shelf/table art remains |
| Asset accuracy and provenance | 15 / 15 | Main GUID tracing and exact SHA-256 matches documented |
| C001-C003 game loop | 15 / 15 | Automated real-button Play Mode driver completed all three cases |
| Visual similarity | 11 / 15 | HUD/decision/reason surfaces improved; chat, C002 darkness, and `.aseprite` gap remain |
| Architecture | 10 / 10 | Composition root, presenter, passive view, factory/theme/catalog/state separation preserved |
| Build and fallback safety | 10 / 10 | Unity compile passed; Resources-only loading and sprite/font fallbacks retained |
| Regression safety | 5 / 5 | IDs preserved, hidden accident fields absent, forbidden production areas unchanged |
| Validation evidence | 9 / 10 | Two screenshot sets and full Play flow; exact requested resolutions and EditMode tests remain incomplete |
| **Total** | **93 / 100** | Evidence supports the score without applying a cap |

## Score Evolution

| Stage | Score | Reason |
| --- | ---: | --- |
| M8.13 static baseline | 91 / 100 | Strong state/architecture result without Play Mode or screenshots |
| M8.14 iteration 1 | 88 / 100 | Runtime flow passed, but screenshots exposed clipped HUD and incorrect paper-prop stretching |
| M8.14 iteration 2 | 93 / 100 | HUD title, decision paper, reason surfaces, importer fidelity, and build safety corrected |

## Why The Score Is Below 95

- Main's table and right-shelf visuals are `.aseprite` files without an available approved exporter.
- The C002 screenshot retains large dark regions not seen in the settled C001/C003 captures.
- The dialogue first line sits too close to the bubble top.
- Requested 1920x1080 and 1280x720 sizes were not honored by the current Editor Game View.

These are visible, measurable gaps, so a 95+ score would not be justified yet.

