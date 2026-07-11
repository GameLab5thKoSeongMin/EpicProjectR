# M8.14-A Main Asset Forensics

Status: Confirmed from read-only YAML, `.meta`, byte hash, and RectTransform inspection.

## Scope And Precedence

- Domain and content behavior continues to follow `docs/design/`.
- `ref_folder/Assets/Scenes/Main.unity` is the only scene-level presentation reference.
- `SampleScene.unity` remains a runtime host and was not used as an art reference.
- No file under `ref_folder/` was modified.

## Main Composition Evidence

| Main hierarchy | Role | RectTransform evidence | Referenced source |
| --- | --- | --- | --- |
| `Canvas/WorldShiftTargets/BackgroundImage` | Full-screen office | position `0,0`, size `1920x1080` | `Images/Main_Total.png` |
| `.../BackgroundImage/Button (1)` | Entry bell | position `-197,-305`, size `120x120` | `Images/Old/Bell_Full.png` |
| `.../BackgroundImage/Paper` | Presented document | position `0,-331`, size `102x75` | `Images/Old/paper.png` |
| `.../BackgroundImage/Character` | Contractor | position `3,-78`, size `200x200` before runtime movement | `Images/임시_캐릭터 마법사.png` |
| `Canvas/WorkBench/WorkBenchDisplay/GameObject` | Main work surface | position `-36.4381,-40.527`, size `869.462x1001.5` | `.aseprite` table/layout sources |
| `Canvas/WorkBench/Shelf` | Right reason shelf | position `718.373,-39.626`, size `484.5942x1000.7685` | `.aseprite` shelf sources |

This confirms the intended direction: a bright physical office, a modest document beside a prominent bell, a character arrival before review, a large central work surface, and a narrow right decision area.

## Selected Runtime Candidates

All selected binary files already existed in active Resources and match their source bytes exactly.

| Main hierarchy / use | GUID | Source | Size | Source importer | Active decision |
| --- | --- | --- | ---: | --- | --- |
| `BackgroundImage` | `a03ee1cf9f1f8ad4e9606497cc8ee971` | `Images/Main_Total.png` | `1920x1080` | Sprite, PPU 100, center pivot, bilinear, alpha, compressed | Reuse exact file; restore Sprite/alpha import |
| `BackgroundImage/Button (1)` | `a784b67b61865d948abd83c5c974b61f` | `Images/Old/Bell_Full.png` | `128x128` | Multiple sprite, PPU 100, center pivot, alpha | Reuse exact file as a safe single Sprite |
| `BackgroundImage/Paper` | `de45691cea43e2744b122201560fa950` | `Images/Old/paper.png` | `102x75` | Multiple sprite, PPU 100, center pivot, alpha | Reuse exact file as a safe single Sprite |
| `BackgroundImage/Character` | `8880f784280d17e4bb705b2dac8d2e08` | `Images/임시_캐릭터 마법사.png` | `530x530` | Sprite, PPU 100, center pivot, alpha | Reuse exact file; preserve aspect |
| `WorkBenchDisplay/GameObject/Image` | `e841f4797d679f24897de78dee37de2e` | `Images/Paper texture 2.png` | `427x383` | Multiple sprite, PPU 100, alpha | Reuse exact file as a safe single Sprite |
| `BackgroundImage/letter` | `a528993b306d8c744a3435f534d9831d` | `Images/Old/편지UI.png` | `46x46` | Multiple sprite, PPU 100, alpha | Reuse exact file as a safe single Sprite |
| Decision accept | `b08924c552545c9449e191ce07988258` | `.../UI/Tabs/UI_tab_dim.png` | `159x61` | Sprite, PPU 100, uncompressed default | Reuse exact file; restore Sprite/alpha import |
| Decision reject | `a48a6213bf51c5849a37da92a610ab26` | `.../UI/Tabs/UI_tab_Off.png` | `159x61` | Sprite, PPU 100, uncompressed default | Reuse exact file; restore Sprite/alpha import |
| Main close buttons | `b199c7e3e923c864aaad2fe80448bc18` | `.../UI/Buttons/UI_BtnIcon_x.png` | `28x28` | Multiple sprite, PPU 100, alpha | Reuse exact file as a safe single Sprite |
| Dialogue fallback | Main uses `af1d...` `.aseprite` | `Images/Old/말풍선.png` | `736x301` | Sprite, PPU 100, alpha | Reuse exact PNG fallback; not claimed as exact `.aseprite` export |

Selected files use center pivots and zero borders. The active import reconstruction uses one Sprite per copied PNG because the runtime consumes each file as one complete visual and does not rely on old source slice IDs.

## Fonts

The active MainSceneUI copies of `Galmuri11-Bold.ttf`, `KyoboHandwriting2020pdy.otf`, and `KyoboHandwriting2022khn.ttf` match the reference font binaries by SHA-256. Main points to TMP SDF assets, but copying those generated TMP assets is outside scope; the current uGUI runtime uses the exact source font binaries with OS/default fallback.

## Excluded Main-Linked Candidates

| Candidate group | Main evidence | Decision |
| --- | --- | --- |
| Table/workbench art | `Table_Blue_2 (1).aseprite`, `Layout (2).aseprite`, `Layout_Left.aseprite` | Skip: no Aseprite CLI or package is available; generated work surface remains build-safe |
| Right shelf/reason paper art | `UI_Right_Under.aseprite`, `UI_Right_Middle_Line (1).aseprite`, `UI_Right_Middle_Paper*.aseprite` | Skip: no reliable frame export; current paper rows preserve the role without importing source format |
| Dialogue bubble/triangle | `SpeakBubble.aseprite`, `SpeakTriangle.aseprite` | Skip exact export; use the existing PNG bubble fallback |
| Map/ship/news art | board, route, sea, port, repair, ship, newspaper GUIDs | Skip: outside the C001-C003 underwriting loop |
| Crystal ball, feather, skill imagery | directly referenced by obsolete Main subpanels | Skip: obsolete fantasy direction conflicts with current maritime vocabulary |
| Direct document prefabs | ship insurance, registration, hull inspection, route document | Read-only evidence only; prefabs and old scripts are forbidden to copy |
| Animator, Timeline, materials, TMP SDF assets | direct external GUIDs | Skip: forbidden generated/runtime architecture dependencies |
| Missing GUIDs | `a86470...`, `311925...` | Skip: unresolved source and not required by the active first playable |

## Tool Availability

`aseprite`, ImageMagick `magick`, and `ffmpeg` were not found on PATH. No package or external converter was installed.

## Risks

- Exact shelf/table `.aseprite` frames remain the largest visual parity gap.
- uGUI text will not exactly match Main's TMP SDF rendering.
- Source multiple-sprite slicing is intentionally not reproduced because the active UI uses each selected PNG as one full visual.

