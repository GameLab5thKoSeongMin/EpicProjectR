# M8.9-F Main Runtime Loop Parity Score

## Score

Estimated parity moved from M8.8 `69/100` to M8.9 `82/100`.

## Scoring Notes

| Area | Score | Notes |
| --- | ---: | --- |
| Composition and first viewport | 14/15 | Main-style background, entry document, and workbench regions are present. |
| Runtime loop sequencing | 16/20 | Entry, review start, decision drawer, result/next flow implemented. |
| Motion | 10/12 | Panel tweens mimic Main toggle movement timing. |
| Documents | 12/15 | Generated cards are draggable and clamped; exact old prefab document art is not copied. |
| AR/CR interaction | 14/15 | Split sections, selection tracking, conditional approve label, selected-CR premium display. |
| Decision UX | 10/12 | Lower-right trigger and bottom drawer implemented; exact old art/layout is approximate. |
| Validation confidence | 6/11 | Static checks done; Unity Play Mode still blocked by batchmode startup failure. |

## Remaining Gaps

- Need in-editor Play Mode screenshot/video confirmation.
- Old Main ship/damage visualization is approximated by safe existing/generic visuals.
- Full old dialogue system is not recreated; only entry/intro presentation behavior is mirrored.
