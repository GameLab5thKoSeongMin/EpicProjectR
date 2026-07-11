# M8.14-F Main Asset Fidelity Report

Status: Implemented, compiled, Play Mode-driven, and visually reviewed twice.

## Overall Result

M8.14 verified Main's actual scene/object/GUID relationships, retained exact already-migrated binaries, corrected their active importer settings, removed non-build-safe external file loading, and tuned the first playable entry and review surfaces after two screenshot passes.

Final evidence-based parity score: `93 / 100`.

## Main Direction Confirmed

- Bright port office at `1920x1080` reference composition.
- Small physical paper at Main position `0,-331`, size `102x75`.
- Bell at Main position `-197,-305`, size `120x120`.
- Character hidden initially, then animated before review.
- Large central work surface and narrow right shelf/decision region.
- Physical paper/tab language instead of a generic dashboard.

## Assets Copied, Reused, And Skipped

- Copied in M8.14: none.
- Reused: exact Main background, contractor, paper prop, paper texture, letter, bell, tabs, small button, and three font binaries already present in canonical Resources.
- Fallback reuse: existing speech-bubble PNG because Main's exact bubble is `.aseprite`.
- Skipped: broad image folders, old scripts/scenes/prefabs/ScriptableObjects, animation assets, materials, TMP SDF assets, fantasy skill/crystal-ball art, map/news assets, and all unexportable `.aseprite` sources.

SHA-256 checks prove every selected active binary is byte-identical to its documented reference source. See `M8_14_A_MAIN_ASSET_FORENSICS.md` and `M8_14_B_ASSET_MIGRATION_MANIFEST.md`.

## Code And Importer Changes

- `FirstPlayableAssetCatalog`: Resources-only loading; Sprite first, Texture2D/generated Sprite compatibility fallback; no `System.IO` or `using_image` dependency.
- `FirstPlayableMainSceneRectSpec`: Main-measured entry paper and bell geometry; aspect-safe review props.
- `FirstPlayableView`: correct prop/surface sprite roles, preserved aspect, readable dialogue rows, visible HUD title, and improved reason contrast.
- `FirstPlayableUiTheme`: lighter reason-row normal/selected states.
- `FirstPlayableKoreanText`: corrected bell-first instruction and removed visible C001-C003 IDs from completion text.
- Ten active PNG `.meta` files: Sprite type, single mode, no mipmaps, transparency enabled, center pivot and PPU 100 retained.
- `M8_14PlayModeCapture`: editor-only reusable visual QA driver under the allowed tests path.

## Architecture Preservation

- Bootstrap remains composition root only.
- Presenter remains responsible for state and C001-C003 flow.
- View remains event-driven and passive; no underwriting/economy logic was added.
- Factory, theme, catalog, rect spec, state machine, and tween runner retain separate responsibilities.
- Domain, Application, and Content files were not changed.

## Validation

- Unity compile: passed with `Tundra build success`.
- Resource sprite reimport: passed for all ten PNGs.
- Automated SampleScene Play Mode: passed through C001, C002, C003, exit, next-wait, and completion.
- Screenshot capture: 9 final images plus 9 iteration-1 images.
- Static runtime editor API scan: no matches in `Assets/Scripts`.
- External file-loading scan: no `System.IO`, `File.ReadAllBytes`, or `using_image` in Presentation runtime.
- Hidden accident-field scan: no matches in Presentation.
- Forbidden path diff: no `ref_folder`, Packages, ProjectSettings, scene, or prefab changes.
- EditMode tests: attempted but 0 discovered; inconclusive.

## Source IDs And Hidden Data

Internal IDs remain exact, including C001-C003, AR01-AR04, CR01-CR02, and R0005. Presentation mapping switch cases still use exact IDs internally. Normal completion, document, and rule labels do not expose these IDs. `AccidentFlag` and `WillAccidentIfApproved` remain absent from player-facing Presentation code.

## Algorithms, Data Structures, And Patterns

- Algorithms: GUID-to-`.meta` tracing, SHA-256 deduplication, explicit UI state driving, smoothstep tween preservation, screenshot compare-and-correct loop.
- Data structures: GUID/path maps, hash maps, explicit screen states, existing rule dictionaries and once-only incentive `HashSet`.
- Patterns: composition root, presenter, passive view, factory, theme/catalog separation, explicit UI state machine, editor-only QA driver.

## Files Intentionally Not Modified

- `ref_folder/`
- `EpicProjectR/Packages/`
- `EpicProjectR/ProjectSettings/`
- Unity scenes and prefabs
- ScriptableObjects
- Domain, Application, and Content production code

## Remaining Risks

- Exact Main table/shelf `.aseprite` rendering remains unavailable without an approved exporter.
- Exact 1920x1080 and 1280x720 visual QA remains pending because the current Game View ignored requested resolution.
- C002 transition darkness and chat first-line vertical placement merit one manual Editor pass.
- The existing EditMode test organization should be repaired in a separate test-infrastructure milestone.

## Recommended Next Milestone

M8.15 should be a narrow manual Game View preset and remaining visual correction pass: capture exact 1920x1080 and 1280x720, correct C002 transition darkness/chat padding, and decide whether approved PNG exports of the Main shelf/table `.aseprite` sources are available.

