# M8.11-F Main UI Size Image Parity Score

## Score

Before M8.11: **82 / 100** from M8.9.

After M8.11: **88 / 100** estimated static parity.

## Category Scores

| Category | Score | Notes |
| --- | ---: | --- |
| Main UI size similarity | 9/10 | Workbench, shelf, decision drawer, documents retuned to measured values. |
| Main UI position similarity | 8/10 | Main-derived right/center/bottom positions used; auxiliary docket remains first-playable-specific. |
| Anchor/pivot similarity | 8/10 | Major regions use Main-like center/right/bottom anchors. |
| Image correctness | 8/10 | Exact PNGs used; `.aseprite` shelf/table assets skipped. |
| Display region correctness | 9/10 | Main workbench/shelf/drawer regions now match closely. |
| Dialogue fidelity | 7/10 | Region close; old dialogue system not recreated. |
| Workstation fidelity | 9/10 | Size and area now match measured Main workstation. |
| Document drag bounds fidelity | 8/10 | Bounds now central workstation; old canvas clamp not copied. |
| AR section fidelity | 8/10 | AR region and row heights match; exact sliced sprite skipped. |
| CR section fidelity | 8/10 | CR region and row heights match; exact sliced sprite skipped. |
| Decision drawer fidelity | 9/10 | Drawer size, position, buttons retuned from Main values. |
| Button image/size fidelity | 10/10 | Exact tab images and `120x65` size. |
| Font/text readability | 7/10 | Runtime-safe Kyobo/Galmuri fonts, no TMP SDF parity. |
| Runtime loop stability | 8/10 | Interactions preserved by code review; Play Mode pending. |
| Source ID preservation | 10/10 | No source ID mutation. |
| Hidden accident data safety | 10/10 | No Presentation exposure. |
| Runtime/build safety | 8/10 | Runtime Resources fallback preserved; Unity compile pending. |
| Repository boundary safety | 10/10 | No forbidden paths modified. |
| Code maintainability | 9/10 | Rect specs centralized in one helper. |
