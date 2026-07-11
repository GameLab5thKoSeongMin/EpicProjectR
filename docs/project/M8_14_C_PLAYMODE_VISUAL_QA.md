# M8.14-C Play Mode Visual QA

Status: Unity compile and automated Play Mode interaction passed. Two screenshot review iterations completed.

## Environment

- Unity: `6000.3.19f1`
- Runtime host: `Assets/Scenes/SampleScene.unity`
- Reference: `ref_folder/Assets/Scenes/Main.unity`
- Driver: `Assets/Tests/PlayMode/Editor/M8_14PlayModeCapture.cs`

The driver uses the actual runtime-generated uGUI buttons and toggles. It does not call presenter methods directly and does not modify the scene.

## Compile

Unity reimported all ten corrected MainSceneUI sprites and reported `Tundra build success`. There were no C# errors. The only warnings were the two pre-existing obsolete `FindObjectOfType` calls in `FirstPlayableBootstrap`.

## Automated Play Flow

| Case | Driven flow | Result |
| --- | --- | --- |
| C001 | bell -> arrival -> presented document -> review -> decision paper -> approve | Passed; next contract returned to bell waiting state |
| C002 | bell -> arrival -> document -> applicable AR selection -> reject | Passed; no interaction exception and next contract waited |
| C003 | bell -> arrival -> document -> both applicable CR selections -> decision paper -> conditional approve | Passed; button changed to `조건부 승인`, paper showed `120%`, completion reached |

The run completed without `NullReferenceException`, `MissingReferenceException`, missing button, or missing applicable-toggle errors.

## Screenshot Resolution Caveat

Unity's normal Editor Game View produced screenshots at `1849x1031`, despite calls requesting `1920x1080`. A later request for `1280x720` produced an identical `1849x1031` file with the same SHA-256 as the completion screenshot. Therefore:

- the images are valid Play Mode visual evidence,
- they are not exact `1920x1080` pixel-parity evidence,
- the `1280x720` responsive check remains unverified.

## Iteration 1

Evidence is preserved under `docs/project/screenshots/m8_14/iteration_1/`.

Observed gaps:

- HUD title was clipped off the left edge.
- The physical trapezoid `paper_prop` was stretched across dialogue, reason, and decision amount panels.
- Reason text had insufficient contrast.
- Decision amounts sat over a visibly unsuitable stretched prop.

## Iteration 2 Corrections

- Repositioned the HUD title inside its left region.
- Reserved `paper_prop` for presented physical documents.
- Used `paper_texture_2` for dialogue panel, reason rows, decision trigger, and decision amount area.
- Increased dialogue bubble row height.
- Lightened normal and selected reason-row tint.

Iteration 2 evidence:

- [Initial Main composition](screenshots/m8_14/01_initial_1920x1080.png)
- [Contractor arrived](screenshots/m8_14/02_contractor_arrived_1920x1080.png)
- [C001 review](screenshots/m8_14/03_c001_review_1920x1080.png)
- [C001 decision open](screenshots/m8_14/04_c001_decision_open_1920x1080.png)
- [Next contract waiting](screenshots/m8_14/05_next_contract_waiting_1920x1080.png)
- [C002 AR review](screenshots/m8_14/06_c002_ar_review_1920x1080.png)
- [C003 CR decision](screenshots/m8_14/07_c003_cr_decision_1920x1080.png)
- [Completed loop](screenshots/m8_14/08_complete_1920x1080.png)
- [Failed 1280 request evidence](screenshots/m8_14/09_complete_requested_1280x720_actual_1849x1031.png)

## Remaining Visual Risks

- C002 capture retains unusually dark transition regions even though the interaction flow completes.
- The first chat line remains close to the upper edge of its bubble.
- Exact Main shelf/table `.aseprite` frames are not available as runtime PNGs.
- uGUI font output differs from Main's TMP SDF output.
- Exact 1920x1080 and 1280x720 screenshots still require manual Game View size presets.

## EditMode Test Runner

The command-line EditMode runner completed successfully but discovered `0` tests because the existing `FirstPlayableDomainTests.cs` belongs to the predefined `Assembly-CSharp` assembly rather than a discoverable test assembly. This result is inconclusive and is not counted as a test pass.

