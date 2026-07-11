# M8.14-B Asset Migration Manifest

Status: Selective reuse completed; no duplicate binary migration was necessary.

## Canonical Runtime Location

`EpicProjectR/Assets/Resources/FirstPlayable/ImportedReference/RuntimeSafe/MainSceneUI/`

## Reused Exact Assets

| Active asset | Exact reference source | SHA-256 status | Runtime role |
| --- | --- | --- | --- |
| `Background/main_total.png` | `Images/Main_Total.png` | Exact | Main office background |
| `Characters/contractor_temp.png` | `Images/임시_캐릭터 마법사.png` | Exact | Contractor arrival/exit |
| `Documents/letter_ui.png` | `Images/Old/편지UI.png` | Exact | Available Main letter prop |
| `Documents/paper_prop.png` | `Images/Old/paper.png` | Exact | Entry and presented document |
| `Documents/paper_texture_2.png` | `Images/Paper texture 2.png` | Exact | Decision/document paper texture |
| `UI/bell_full.png` | `Images/Old/Bell_Full.png` | Exact | Entry bell |
| `UI/ui_btn_icon_x.png` | IndigoLay `UI_BtnIcon_x.png` | Exact | Small Main-style UI control |
| `UI/ui_tab_dim.png` | IndigoLay `UI_tab_dim.png` | Exact | Approve tab |
| `UI/ui_tab_off.png` | IndigoLay `UI_tab_Off.png` | Exact | Reject/neutral tab |
| `UI/speech_bubble.png` | `Images/Old/말풍선.png` | Exact fallback | Chat bubble fallback |
| Three MainSceneUI font binaries | reference TMP font source binaries | Exact | Korean typography |

## Importer Reconstruction

The ten active PNG `.meta` files were adjusted without copying source `.meta` files:

- texture type: Sprite (`8`),
- sprite mode: single (`1`),
- mipmaps: disabled for screen UI,
- alpha interpreted as transparency,
- stable default Sprite ID,
- center pivot, PPU 100, zero border retained.

`FirstPlayableAssetCatalog` now tries `Resources.Load<Sprite>` first and retains `Texture2D` plus generated `Sprite.Create` as a compatibility fallback.

## Newly Copied Assets

None. Every selected PNG/font already matched the intended reference file exactly, so another copy would only create duplication and GUID churn.

## Skipped Assets

- Broad `ref_folder` image folders.
- `.aseprite` and `.ase` sources without a reliable local exporter.
- Old prefabs, scenes, scripts, ScriptableObjects, animation assets, materials, and packages.
- Map, news, skill, crystal-ball, and other non-first-playable visuals.
- Audio: Main provided no required first-playable audio candidate in the inspected flow.

## Build Safety Decision

The catalog no longer searches `using_image` or reads repository files with `System.IO`. All selected visuals load from Resources and remain included in a Player build. Missing sprites degrade to generated Image colors; font loading continues through imported font, OS font, then Unity built-in fallback.

## Repository Boundary

- `ref_folder/`: read only, unchanged.
- `Packages/`: unchanged.
- `ProjectSettings/`: unchanged.
- Scenes, prefabs, ScriptableObjects: unchanged.

