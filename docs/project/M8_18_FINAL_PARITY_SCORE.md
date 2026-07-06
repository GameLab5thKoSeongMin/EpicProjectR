# M8.18 Final Parity Score

Status: Static/build parity score after M8.14-M8.18 correction pass. Play Mode screenshot scoring remains pending.

## Score

Previous M8.13 static score: `91 / 100`.

Current M8.18 static/build score: `95 / 100`.

## Category Scores

| Category | Score | Notes |
| --- | ---: | --- |
| Required background | 5 / 5 | `using_image/배경화면.png` is now preferred by runtime loader and confirmed 1920x1080. |
| Background brightness | 4 / 5 | Overlay remains low alpha; screenshot pending. |
| Initial Main composition | 5 / 5 | Character hidden; bell and document center-bottom. |
| Bell click trigger | 5 / 5 | Bell starts arrival only. |
| Character arrival origin | 5 / 5 | Entry starts from center-bottom, not side. |
| Character arrival feel | 4 / 5 | Move/scale/fade/tint implemented; screenshot pending. |
| Presented document entry | 5 / 5 | Document starts review only after arrival. |
| Review transition | 4 / 5 | Tweened layout movement preserved; exact camera feel pending visual QA. |
| Left/chat split | 4 / 5 | Chat upper and character lower regions retuned. |
| Chat accumulation/scroll | 5 / 5 | Multi-bubble scroll log with click progression and wheel scroll. |
| Workstation drag/clamp | 5 / 5 | Drag controller and clamp bounds preserved. |
| Workstation click behavior | 5 / 5 | Workstation documents do not return to Main. |
| Right panel proportions | 4 / 5 | Static 1920 fit improved; screenshot pending. |
| Applicable reasons only | 5 / 5 | Presenter filters by current review triggers. |
| Empty sections hidden | 5 / 5 | Section active state depends on child count. |
| Source IDs hidden from normal rows | 5 / 5 | Reason row labels use display titles only. |
| Decision paper trigger | 5 / 5 | Paper-like trigger preserved. |
| Decision drawer toggle | 5 / 5 | Opens upward, closes downward, delayed hide guarded. |
| Decision paper content | 5 / 5 | Title, compensation, premium, reject, approve/conditional approve only. |
| CR premium behavior | 5 / 5 | +10% per selected CR preserved. |
| Incentive behavior | 5 / 5 | floor 1% once-only incentive preserved. |
| Post-decision exit loop | 5 / 5 | Character exits and next contract waits for bell. |
| HUD visibility | 4 / 5 | Built last and rects widened; screenshot pending. |
| 1920x1080 clipping safety | 4 / 5 | Static rect check passes; screenshot pending. |
| Runtime/build safety | 4 / 5 | `dotnet build` passes; Unity Test Runner and Play Mode pending. |
| Repository boundaries | 5 / 5 | No ref_folder, Packages, ProjectSettings, scene, prefab, or SO edits by this pass. |

## Score Rationale

The pass closes the major remaining static gaps from M8.13: mandatory background selection, center-origin arrival, wider HUD, tighter review proportions, safer right-panel row fit, and safer drawer close behavior. It remains below 100 because Unity Test Runner and 1920x1080 Play Mode screenshots are still required for visual proof.
